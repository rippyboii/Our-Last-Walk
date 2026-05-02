using UnityEngine;
using UnityEngine.UI;

public class BalanceUI : MonoBehaviour
{
    public RectTransform needle;       // the moving indicator image
    public RectTransform safeZone;     // the center zone image
    public float barWidth = 400f;      // total width of your balance bar in pixels

    private BalanceQTE balanceQTE;

    void Start()
    {
        balanceQTE = FindObjectOfType<BalanceQTE>();
        // TODO: hide UI when QTE is not active
    }

    void Update()
    {
        // needlePosition is -1 to 1, map it to pixel position on the bar
        // TODO: add smooth lerp here if you want the needle to glide rather than snap
        float pixelPos = balanceQTE.needlePosition * (barWidth / 2f);
        needle.anchoredPosition = new Vector2(pixelPos, needle.anchoredPosition.y);
    }
}