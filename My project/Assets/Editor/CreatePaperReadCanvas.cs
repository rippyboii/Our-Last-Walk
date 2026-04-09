using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CreatePaperReadCanvas
{
    public static void Execute()
    {
        // --- Root Canvas ---
        GameObject canvasGO = new GameObject("PaperReadCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // --- Background Panel (dark semi-transparent) ---
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(canvasGO.transform, false);
        Image panelImg = panel.AddComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.85f);
        RectTransform panelRT = panel.GetComponent<RectTransform>();
        panelRT.anchorMin = Vector2.zero;
        panelRT.anchorMax = Vector2.one;
        panelRT.offsetMin = Vector2.zero;
        panelRT.offsetMax = Vector2.zero;

        // --- Paper card (off-white centered card) ---
        GameObject card = new GameObject("PaperCard");
        card.transform.SetParent(panel.transform, false);
        Image cardImg = card.AddComponent<Image>();
        cardImg.color = new Color(0.96f, 0.93f, 0.82f, 1f); // parchment
        RectTransform cardRT = card.GetComponent<RectTransform>();
        cardRT.anchorMin = new Vector2(0.5f, 0.5f);
        cardRT.anchorMax = new Vector2(0.5f, 0.5f);
        cardRT.pivot = new Vector2(0.5f, 0.5f);
        cardRT.sizeDelta = new Vector2(500f, 300f);
        cardRT.anchoredPosition = Vector2.zero;

        // --- "LUCKY 7777" text ---
        GameObject luckyGO = new GameObject("LuckyText");
        luckyGO.transform.SetParent(card.transform, false);
        Text luckyText = luckyGO.AddComponent<Text>();
        luckyText.text = "LUCKY 7777";
        luckyText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        luckyText.fontSize = 52;
        luckyText.fontStyle = FontStyle.Bold;
        luckyText.alignment = TextAnchor.MiddleCenter;
        luckyText.color = new Color(0.15f, 0.1f, 0.05f, 1f);
        RectTransform luckyRT = luckyGO.GetComponent<RectTransform>();
        luckyRT.anchorMin = new Vector2(0f, 0.3f);
        luckyRT.anchorMax = new Vector2(1f, 0.85f);
        luckyRT.offsetMin = Vector2.zero;
        luckyRT.offsetMax = Vector2.zero;

        // --- "[E] Put down paper" hint ---
        GameObject hintGO = new GameObject("CloseHint");
        hintGO.transform.SetParent(card.transform, false);
        Text hintText = hintGO.AddComponent<Text>();
        hintText.text = "[E]  Put down paper";
        hintText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        hintText.fontSize = 20;
        hintText.alignment = TextAnchor.MiddleCenter;
        hintText.color = new Color(0.3f, 0.25f, 0.15f, 1f);
        RectTransform hintRT = hintGO.GetComponent<RectTransform>();
        hintRT.anchorMin = new Vector2(0f, 0.05f);
        hintRT.anchorMax = new Vector2(1f, 0.25f);
        hintRT.offsetMin = Vector2.zero;
        hintRT.offsetMax = Vector2.zero;

        // --- PaperReadController on the canvas root ---
        PaperReadController controller = canvasGO.AddComponent<PaperReadController>();
        controller.paperCanvas = canvasGO;

        // --- Deactivate canvas by default ---
        canvasGO.SetActive(false);

        // --- Also deactivate the player prompt by default ---
        GameObject playerPrompt = GameObject.Find("PlayerPaperPrompt");
        if (playerPrompt != null) playerPrompt.SetActive(false);

        // --- Save scene ---
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("PaperReadCanvas created and deactivated successfully.");
    }
}
