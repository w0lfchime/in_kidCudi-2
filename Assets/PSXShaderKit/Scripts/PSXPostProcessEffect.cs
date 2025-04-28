using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSXShaderKit
{
    public class PSXPostProcessEffect : MonoBehaviour
    {
        private enum ColorEmulationMode
        {
            Off = 0,
            Fullscreen_Customizable = 1,
            Fullscreen_Accurate = 2,
            PerObject_Accurate = 3
        };

        private enum DitheringMatrixSize
        {
            Dither2x2 = 0,
            Dither4x4 = 1,
            Dither4x4_PS1Pattern = 2
        }

        [Header("Resolution")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        [Tooltip("Fakes a low-resolution look by changing how pixel values are sampled.")]
        private float _PixelationFactor = 1;

        [SerializeField]
        [Tooltip("Speed of pixelation oscillation between 0 and 1.")]
        private float _PixelationSpeed = 1f;

        [Header("Color")]
        [SerializeField]
        private ColorEmulationMode _ColorEmulationMode = ColorEmulationMode.PerObject_Accurate;
        [SerializeField]
        private Vector3 _FullscreenColorDepth = new Vector3(256, 256, 256);
        [SerializeField]
        private Vector3 _FullscreenDitherDepth = new Vector3(32, 32, 32);
        [SerializeField]
        private DitheringMatrixSize _DitheringMatrixSize = DitheringMatrixSize.Dither4x4_PS1Pattern;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _DitheringScale = 1;

        [Header("Interlacing")]
        [SerializeField]
        private int _InterlacingSize = 1;

        [Header("Shaders")]
        [SerializeField]
        private Shader _PostProcessShader;
        private Material _PostProcessMaterial;

        [SerializeField]
        private Shader _PostProcessShaderAccurate;
        private Material _PostProcessMaterialAccurate;

        [SerializeField]
        private Shader _PixelationShader;
        private Material _PixelationMaterial;

        [SerializeField]
        private Shader _InterlacingShader;
        private Material _InterlacingMaterial;
        private RenderTexture _PreviousFrame;

        private bool _IsFirstFrame = true;

        void Start()
        {
            if (_PostProcessShader != null && _PostProcessShader.isSupported)
                _PostProcessMaterial = new Material(_PostProcessShader);

            if (_PixelationShader != null && _PixelationShader.isSupported)
                _PixelationMaterial = new Material(_PixelationShader);

            if (_PostProcessShaderAccurate != null && _PostProcessShaderAccurate.isSupported)
                _PostProcessMaterialAccurate = new Material(_PostProcessShaderAccurate);

            if (_InterlacingShader != null && _InterlacingShader.isSupported)
                _InterlacingMaterial = new Material(_InterlacingShader);
            else
                _InterlacingSize = -1;

            UpdateValues();
        }

        void OnValidate()
        {
            UpdateValues();
        }

        void UpdateValues()
        {
            Shader.SetGlobalFloat("_PSX_ObjectDithering", _ColorEmulationMode == ColorEmulationMode.PerObject_Accurate ? 1 : 0);
        }

        void OnDisable()
        {
            _IsFirstFrame = true;
        }

        void Update()
        {
            // Smoothly oscillate the pixelation factor from 0 to 1
            _PixelationFactor = Mathf.Lerp(0.4f, 1f, Mathf.PingPong(Time.time * _PixelationSpeed, 1f));
        }

        void ApplyPixelationEffect(RenderTexture source, RenderTexture destination)
        {
            if (_PixelationFactor >= 1.0f)
            {
                Graphics.Blit(source, destination);
                return;
            }

            FilterMode sourceFilterMode = source.filterMode;
            source.filterMode = FilterMode.Point;

            _PixelationMaterial.SetFloat("_PixelationFactor", _PixelationFactor);
            Graphics.Blit(source, destination, _PixelationMaterial);

            source.filterMode = sourceFilterMode;
        }

        void ApplyDitheringEffect(RenderTexture source, RenderTexture destination)
        {
            switch (_ColorEmulationMode)
            {
                case ColorEmulationMode.Off:
                case ColorEmulationMode.PerObject_Accurate:
                    Graphics.Blit(source, destination);
                    return;
            }

            switch (_ColorEmulationMode)
            {
                case ColorEmulationMode.Fullscreen_Customizable:
                    _PostProcessMaterial.SetVector("_ColorResolution", _FullscreenColorDepth);
                    _PostProcessMaterial.SetVector("_DitherResolution", _FullscreenDitherDepth);
                    _PostProcessMaterial.SetFloat("_DitheringScale", _DitheringScale);

                    switch (_DitheringMatrixSize)
                    {
                        case DitheringMatrixSize.Dither2x2:
                            _PostProcessMaterial.SetFloat("_HighResDitherMatrix", 0);
                            break;
                        case DitheringMatrixSize.Dither4x4:
                            _PostProcessMaterial.SetFloat("_HighResDitherMatrix", 0.5f);
                            break;
                        case DitheringMatrixSize.Dither4x4_PS1Pattern:
                            _PostProcessMaterial.SetFloat("_HighResDitherMatrix", 1.0f);
                            break;
                    }

                    Graphics.Blit(source, destination, _PostProcessMaterial);
                    break;

                case ColorEmulationMode.Fullscreen_Accurate:
                    _PostProcessMaterialAccurate.SetFloat("_DitheringScale", _DitheringScale);
                    Graphics.Blit(source, destination, _PostProcessMaterialAccurate);
                    break;
            }
        }

        void ApplyInterlacingEffect(RenderTexture source, RenderTexture destination)
        {
            if (_InterlacingSize <= 0)
            {
                Graphics.Blit(source, destination);
                return;
            }

            _InterlacingMaterial.SetFloat("_InterlacedFrameIndex", Time.frameCount % 2);
            _InterlacingMaterial.SetFloat("_InterlacingSize", _InterlacingSize);
            _InterlacingMaterial.SetTexture("_PreviousFrame", _IsFirstFrame ? source : _PreviousFrame);
            _IsFirstFrame = false;

            Graphics.Blit(source, destination, _InterlacingMaterial);

            if (_PreviousFrame)
                RenderTexture.ReleaseTemporary(_PreviousFrame);

            _PreviousFrame = RenderTexture.GetTemporary(source.descriptor);
            Graphics.Blit(source, _PreviousFrame);
            RenderTexture.active = destination;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            RenderTexture pixelationRT = RenderTexture.GetTemporary(source.descriptor);
            pixelationRT.filterMode = FilterMode.Point;
            ApplyPixelationEffect(source, pixelationRT);

            RenderTexture ditheringRT = RenderTexture.GetTemporary(source.descriptor);
            ditheringRT.filterMode = FilterMode.Point;
            ApplyDitheringEffect(pixelationRT, ditheringRT);
            RenderTexture.ReleaseTemporary(pixelationRT);

            ApplyInterlacingEffect(ditheringRT, destination);
            RenderTexture.ReleaseTemporary(ditheringRT);
        }
    }
}
