using UnityEngine;

public class SmellLine : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lineRenderer;
    public GameObject target;

    [Header("Line Settings")]
    public int pointCount = 20;
    public float maxWidth = 0.1f;
    public float minWidth = 0.01f;
    public float maxDistance = 20f;

    [Header("Noise Settings")]
    public float waveStrength = 0.15f;
    public float waveSpeed = 0.5f;
    public float noiseScale = 1.5f;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        lineRenderer.positionCount = pointCount;
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!player.IsDog())
        {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        float t = Mathf.Clamp01(distance / maxDistance);
        lineRenderer.startWidth = Mathf.Lerp(maxWidth, minWidth, t);
        lineRenderer.endWidth = 0f;

        Vector3 lineDirection = (target.transform.position - transform.position).normalized;
        Vector3 right = Vector3.Cross(lineDirection, Vector3.up).normalized;
        Vector3 up = Vector3.Cross(lineDirection, right).normalized;

        for (int i = 0; i < pointCount; i++)
        {
            float tt = i / (float)(pointCount - 1);

            Vector3 basePos = Vector3.Lerp(transform.position, target.transform.position, tt);

            // smooth noise - offset sample positions slightly so axes don't match
            float noiseX = Mathf.PerlinNoise(tt * noiseScale, Time.time * waveSpeed) - 0.5f;
            float noiseY = Mathf.PerlinNoise(tt * noiseScale + 17.3f, Time.time * waveSpeed + 5.1f) - 0.5f;

            // fade offset at both ends so line connects cleanly to source and target
            float edgeFade = Mathf.Sin(tt * Mathf.PI);

            Vector3 offset = (right * noiseX + up * noiseY) * waveStrength * edgeFade;
            basePos += offset;

            lineRenderer.SetPosition(i, basePos);
        }
    }
}