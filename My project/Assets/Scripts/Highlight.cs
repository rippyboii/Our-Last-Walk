using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Highlight : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> renderers;
    [SerializeField]
    private Color highlightColor = Color.green;
    private Color color = Color.white;
    private List<Material> materials;

    public void SetColor(Color newColor)
    {
        highlightColor = newColor;
    }

    private void Awake()
    {
        materials = new List<Material>();
        foreach (var r in renderers)
        {
                materials.AddRange(new List<Material>(r.materials));
        }
    }
    public void ToggleHighlight(bool value)
    {
        foreach (var m in materials)
        {
            if (value)
            {
                m.EnableKeyword("_EMISSION");
                m.SetColor("_EmissionColor", highlightColor);
                m.color = highlightColor;
            }
            else
            {
                m.DisableKeyword("_EMISSION");
                m.color = color;
            }
        }
    }
  
}
