using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [Header("Setup")]
    public List<GameObject> vehiclePrefabs;
    public Transform startPoint;
    public Transform endPoint;
    public int numberOfLanes = 4;
    public float baseSpeed = 5f;
    public float laneOffset = 2f;
    public float spawnInterval = 2f;
    public float spawnScale = 1f; // <<< New: scale multiplier for spawned vehicles

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnVehicle();
            timer = 0f;
        }
    }

    private void SpawnVehicle()
    {
        if (vehiclePrefabs.Count == 0) return;

        int lane = Random.Range(0, numberOfLanes);
        GameObject prefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Count)];

        bool goForward = lane < numberOfLanes / 2;
        Transform spawnOrigin = goForward ? startPoint : endPoint;
        Transform destination = goForward ? endPoint : startPoint;

        // Calculate lane offset
        Vector3 laneOffsetVector = Vector3.right * ((lane - (numberOfLanes - 1) / 2f) * laneOffset);

        // Spawn position
        Vector3 spawnPos = spawnOrigin.position + laneOffsetVector;

        // Destination position including lane offset
        Vector3 destinationPos = destination.position + laneOffsetVector;

        GameObject vehicle = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Face towards destination + lane offset
        Vector3 lookDirection = (destinationPos - spawnPos).normalized;
        if (lookDirection != Vector3.zero)
            vehicle.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        // Scale vehicle
        vehicle.transform.localScale *= spawnScale;

        // Assign move target
        TrafficVehicle trafficVehicle = vehicle.AddComponent<TrafficVehicle>();
        trafficVehicle.Initialize(destinationPos, CalculateLaneSpeed(lane));
    }


    private float CalculateLaneSpeed(int lane)
    {
        float midLane = (numberOfLanes - 1) / 2f;
        float distanceFromMid = Mathf.Abs(lane - midLane);
        return baseSpeed + (midLane - distanceFromMid);
    }
}
