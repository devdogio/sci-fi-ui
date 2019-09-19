using UnityEngine;
using System.Collections;
using Devdog.General;
using UnityEngine.Assertions;

namespace Devdog.SciFiDesign.UI
{
    [ExecuteInEditMode]
    public class HologramImageEffect : MonoBehaviour
    {
        [Required]
        public Shader hologramShader;

        [Required]
        public Texture2D hologramTiledImage;

        private Material _hologramMaterial;

        protected virtual void CheckResources()
        {
            if (hologramShader == null)
            {
                hologramShader = Shader.Find("Hidden/HologramImageEffect");
            }

            if (hologramShader.isSupported == false)
            {
                Debug.LogWarning("Shader " + hologramShader.name + " is not supported on this platform.", gameObject);
            }

            if (_hologramMaterial == null)
            {
                _hologramMaterial = new Material(hologramShader);
                _hologramMaterial.hideFlags = HideFlags.DontSave;

                _hologramMaterial.SetTexture("_HologramTexture", hologramTiledImage);
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            CheckResources();


//            int rtW = source.width;
//            int rtH = source.height;

            //            _hologramMaterial.SetFloat("_ChromaticAberration", chromaticAberration);
            //            _hologramMaterial.SetFloat("_AxialAberration", axialAberration);
            //            _hologramMaterial.SetVector("_BlurDistance", new Vector2(-blurDistance, blurDistance));
            //            _hologramMaterial.SetFloat("_Luminance", 1.0f / Mathf.Max(Mathf.Epsilon, luminanceDependency));

            //            source.wrapMode = TextureWrapMode.Clamp;

            Graphics.Blit(source, destination, _hologramMaterial);
        }
    }
}