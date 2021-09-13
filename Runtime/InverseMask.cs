using UnityEngine;
using UnityEngine.UI;

namespace Zigurous.UI
{
    /// <summary>
    /// Applies materials to the image and its children to create an
    /// inverse mask.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("Zigurous/UI/Effects/Inverse Mask")]
    public sealed class InverseMask : MonoBehaviour
    {
        private static Material _cutoutMaterial;
        private static Material _contentMaterial;

        /// <summary>
        /// The shared material applied to an image that is masking another
        /// image. This image acts as a cutout, thus its contents are not visible.
        /// </summary>
        public static Material cutoutMaterial
        {
            get
            {
                if (_cutoutMaterial == null)
                {
                    _cutoutMaterial = new Material(Shader.Find("UI/Default"));
                    _cutoutMaterial.name = "UIInverseMask-Cutout";
                    _cutoutMaterial.color = new Color(1f, 1f, 1f, 1f / 256f);
                    _cutoutMaterial.shaderKeywords = new string[] { "UNITY_UI_ALPHACLIP" };
                    _cutoutMaterial.SetInt("_Stencil", 1);
                    _cutoutMaterial.SetInt("_StencilComp", 8);
                    _cutoutMaterial.SetInt("_StencilOp", 2);
                    _cutoutMaterial.SetInt("_StencilWriteMask", 255);
                    _cutoutMaterial.SetInt("_StencilReadMask", 255);
                    _cutoutMaterial.SetInt("_ColorMask", 0);
                    _cutoutMaterial.SetInt("_UseUIAlphaClip", 1);
                }

                return _cutoutMaterial;
            }
        }

        /// <summary>
        /// The shared material applied to the children images being masked.
        /// The content of these images will be visible in the shape of the
        /// parent cutout mask.
        /// </summary>
        public static Material contentMaterial
        {
            get
            {
                if (_contentMaterial == null)
                {
                    _contentMaterial = new Material(Shader.Find("UI/Default"));
                    _contentMaterial.name = "UIInverseMask-Content";
                    _contentMaterial.color = Color.white;
                    _contentMaterial.SetInt("_Stencil", 2);
                    _contentMaterial.SetInt("_StencilComp", 3);
                    _contentMaterial.SetInt("_StencilOp", 0);
                    _contentMaterial.SetInt("_StencilWriteMask", 0);
                    _contentMaterial.SetInt("_StencilReadMask", 1);
                    _contentMaterial.SetInt("_ColorMask", 15);
                    _contentMaterial.SetInt("_UseUIAlphaClip", 0);
                }

                return _contentMaterial;
            }
        }

        #if UNITY_EDITOR
        private bool _invalidated;

        private void OnValidate()
        {
            _invalidated = true;
        }

        private void Update()
        {
            if (_invalidated)
            {
                Apply();
                _invalidated = false;
            }
        }
        #endif

        private void OnEnable()
        {
            Apply();
        }

        private void OnDisable()
        {
            Remove();
        }

        private void Apply()
        {
            Image mask = GetComponent<Image>();
            mask.material = InverseMask.cutoutMaterial;

            Image[] images = GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                Image image = images[i];

                if (image.transform != this.transform) {
                    image.material = InverseMask.contentMaterial;
                }
            }
        }

        private void Remove()
        {
            Image mask = GetComponent<Image>();
            mask.material = null;

            Image[] images = GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                Image image = images[i];

                if (image.transform != this.transform) {
                    image.material = null;
                }
            }
        }

    }

}
