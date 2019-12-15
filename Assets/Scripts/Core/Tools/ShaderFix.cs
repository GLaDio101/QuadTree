using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Core.Tools
{
    [RequireComponent(typeof(TextMeshPro))]
    public class ShaderFix : MonoBehaviour
    {

        [UsedImplicitly]
        private void Start()
        {
            var m = GetComponent<TextMeshPro>().fontSharedMaterial;
            var shaderName = m.shader.name;
            var newShader = Shader.Find(shaderName);
            if (newShader != null)
            {
                m.shader = newShader;
            }
        }
    }
}
