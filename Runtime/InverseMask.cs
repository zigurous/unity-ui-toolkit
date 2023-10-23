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
    [HelpURL("https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI/InverseMask")]
    public sealed class InverseMask : MonoBehaviour
    {
        private static Material m_CutoutMaterial;
        private static Material m_ContentMaterial;

        /// <summary>
        /// The shared material applied to an image that is masking another
        /// image. This image acts as a cutout, thus its contents are not visible.
        /// </summary>
        public static Material CutoutMaterial
        {
            get
            {
                if (m_CutoutMaterial == null)
                {
                    m_CutoutMaterial = new Material(Shader.Find("UI/Default"));
                    m_CutoutMaterial.name = "UIInverseMask-Cutout";
                    m_CutoutMaterial.color = new Color(1f, 1f, 1f, 1f / 256f);
                    m_CutoutMaterial.shaderKeywords = new string[] { "UNITY_UI_ALPHACLIP" };
                    m_CutoutMaterial.SetInt("_Stencil", 1);
                    m_CutoutMaterial.SetInt("_StencilComp", 8);
                    m_CutoutMaterial.SetInt("_StencilOp", 2);
                    m_CutoutMaterial.SetInt("_StencilWriteMask", 255);
                    m_CutoutMaterial.SetInt("_StencilReadMask", 255);
                    m_CutoutMaterial.SetInt("_ColorMask", 0);
                    m_CutoutMaterial.SetInt("_UseUIAlphaClip", 1);
                }

                return m_CutoutMaterial;
            }
        }

        /// <summary>
        /// The shared material applied to the children images being masked.
        /// The content of these images will be visible in the shape of the
        /// parent cutout mask.
        /// </summary>
        public static Material ContentMaterial
        {
            get
            {
                if (m_ContentMaterial == null)
                {
                    m_ContentMaterial = new Material(Shader.Find("UI/Default"));
                    m_ContentMaterial.name = "UIInverseMask-Content";
                    m_ContentMaterial.color = Color.white;
                    m_ContentMaterial.SetInt("_Stencil", 2);
                    m_ContentMaterial.SetInt("_StencilComp", 3);
                    m_ContentMaterial.SetInt("_StencilOp", 0);
                    m_ContentMaterial.SetInt("_StencilWriteMask", 0);
                    m_ContentMaterial.SetInt("_StencilReadMask", 1);
                    m_ContentMaterial.SetInt("_ColorMask", 15);
                    m_ContentMaterial.SetInt("_UseUIAlphaClip", 0);
                }

                return m_ContentMaterial;
            }
        }

        #if UNITY_EDITOR
        private bool invalidated;

        private void OnValidate()
        {
            invalidated = true;
        }

        private void Update()
        {
            if (invalidated)
            {
                Apply();
                invalidated = false;
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
            mask.material = CutoutMaterial;

            Image[] images = GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                Image image = images[i];

                if (image.transform != transform) {
                    image.material = ContentMaterial;
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

                if (image.transform != transform) {
                    image.material = null;
                }
            }
        }

    }

}
