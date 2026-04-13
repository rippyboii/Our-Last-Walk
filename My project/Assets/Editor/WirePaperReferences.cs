using UnityEngine;
using UnityEditor;

public class WirePaperReferences
{
    public static void Execute()
    {
        // Find PlayerTriggerZone (active, child of bedroom_paper)
        GameObject paper = GameObject.Find("bedroom_paper");
        if (paper == null) { Debug.LogError("bedroom_paper not found."); return; }

        Transform triggerZone = paper.transform.Find("PlayerTriggerZone");
        if (triggerZone == null) { Debug.LogError("PlayerTriggerZone not found under bedroom_paper."); return; }

        PaperProximityTrigger trigger = triggerZone.GetComponent<PaperProximityTrigger>();
        if (trigger == null) { Debug.LogError("PaperProximityTrigger not found."); return; }

        // Find PlayerPaperPrompt (may be inactive) using FindObjectsOfTypeAll
        GameObject prompt = null;
        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.name == "PlayerPaperPrompt" && go.scene.IsValid())
            {
                prompt = go;
                break;
            }
        }
        if (prompt != null)
        {
            trigger.promptUI = prompt;
            Debug.Log("Wired promptUI -> PlayerPaperPrompt");
        }
        else Debug.LogWarning("PlayerPaperPrompt not found in scene.");

        // Find PaperReadCanvas (inactive) and its PaperReadController
        GameObject canvasGO = null;
        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.name == "PaperReadCanvas" && go.scene.IsValid())
            {
                canvasGO = go;
                break;
            }
        }
        if (canvasGO != null)
        {
            PaperReadController controller = canvasGO.GetComponent<PaperReadController>();
            if (controller != null)
            {
                trigger.paper = controller;
                Debug.Log("Wired paper -> PaperReadController on PaperReadCanvas");
            }
            else Debug.LogWarning("PaperReadController not found on PaperReadCanvas.");
        }
        else Debug.LogWarning("PaperReadCanvas not found in scene.");

        EditorUtility.SetDirty(triggerZone.gameObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("WirePaperReferences complete.");
    }
}
