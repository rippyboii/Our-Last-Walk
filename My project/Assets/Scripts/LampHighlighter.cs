using UnityEngine;
using System.Collections.Generic;

public class LampHighlighter : MonoBehaviour
{
    [Header("Settings")]
    public float highlightRadius = 2f;
    public LayerMask highlightableLayer;
    public string specialTag = "Ingredient";       // tag for the special object

    [Header("Colors")]
    public Color regularColor = Color.blue;
    public Color specialColor = Color.yellow;

    private bool isActive = false;
    private List<Highlight> currentlyHighlighted = new List<Highlight>();

    public void Activate()
    {
        Debug.Log("Activate called, found: " + currentlyHighlighted.Count + " objects");
        if (isActive) return;
        isActive = true;

        // find all highlightable objects in radius
        Collider[] inRange = Physics.OverlapSphere(
            transform.position,
            highlightRadius,
            highlightableLayer
        );

        foreach (Collider col in inRange)
        {
            Highlight highlight = col.GetComponent<Highlight>();
            if (highlight == null) continue;

            // use different color depending on tag
            if (col.CompareTag(specialTag))
                highlight.SetColor(specialColor);
            else
                highlight.SetColor(regularColor);

            highlight.ToggleHighlight(true);
            currentlyHighlighted.Add(highlight);
        }
    }

    public void Deactivate()
    {   
        Debug.Log("Deactivate called, isActive: " + isActive + " highlighted count: " + currentlyHighlighted.Count);
        if (!isActive) return;
        isActive = false;

        foreach (Highlight highlight in currentlyHighlighted)
        {
            if (highlight == null) continue;
            highlight.SetColor(Color.green);
            highlight.ToggleHighlight(false);
        }

        currentlyHighlighted.Clear();
    }

    // visualize radius in scene view for easier setup
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, highlightRadius);
    }
}