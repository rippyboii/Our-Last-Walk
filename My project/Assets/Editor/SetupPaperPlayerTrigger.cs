using UnityEngine;
using UnityEditor;

public class SetupPaperPlayerTrigger
{
    public static void Execute()
    {
        // Find bedroom_paper
        GameObject paper = GameObject.Find("bedroom_paper");
        if (paper == null)
        {
            Debug.LogError("Could not find 'bedroom_paper' in scene.");
            return;
        }

        // Create child trigger zone
        GameObject triggerZone = new GameObject("PlayerTriggerZone");
        triggerZone.transform.SetParent(paper.transform, false);
        triggerZone.transform.localPosition = Vector3.zero;

        // Add trigger collider - sized for player proximity
        // bedroom_paper has scale 100, so 0.015 local = 1.5 world units (comfortable reach)
        BoxCollider box = triggerZone.AddComponent<BoxCollider>();
        box.isTrigger = true;
        box.size = new Vector3(0.015f, 0.015f, 0.015f);
        box.center = Vector3.zero;

        // Add PaperProximityTrigger
        PaperProximityTrigger trigger = triggerZone.AddComponent<PaperProximityTrigger>();

        // Wire promptUI -> PlayerPaperPrompt
        GameObject prompt = GameObject.Find("PlayerPaperPrompt");
        if (prompt != null)
            trigger.promptUI = prompt;
        else
            Debug.LogWarning("Could not find 'PlayerPaperPrompt' - assign manually.");

        // Wire paper controller -> PaperReadCanvas
        GameObject canvasGO = GameObject.Find("PaperReadCanvas");
        if (canvasGO != null)
        {
            // PaperReadCanvas is inactive, Find won't locate it - use Resources or search inactive
            PaperReadController controller = canvasGO.GetComponent<PaperReadController>();
            if (controller != null)
                trigger.paper = controller;
            else
                Debug.LogWarning("PaperReadController not found on PaperReadCanvas.");
        }
        else
        {
            Debug.LogWarning("Could not find 'PaperReadCanvas' - it may be inactive. Assign manually.");
        }

        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("PlayerTriggerZone created on bedroom_paper.");
    }
}
