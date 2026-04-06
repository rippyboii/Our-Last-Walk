using NUnit.Framework;
using UnityEngine;

public class SmellLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform target; // the character/camera

    public int pointCount = 10;
    public float waveStrength = 0.3f;
    public float waveSpeed = 1f;
    public KeyCode activationKey = KeyCode.F;
    public float maxDistance;
    public float maxWidth;
    public float minWidth;
    private bool isActive = false;
    private GameObject currentPlayer;
    public GameObject dog;


    void Start()
    {
        lineRenderer.positionCount = pointCount;
        lineRenderer.enabled = false;
    }

    void Update()
    {   
        currentPlayer = GameObject.Find("Player").GetComponent<Player>().activePlayer;
        if (currentPlayer!=dog)
        {
            isActive = false;
            lineRenderer.enabled = isActive;
        }
        else
        {
            isActive = true;
            lineRenderer.enabled = isActive;
        }
     

        if (!isActive) return;

        // TODO: drive width or color by distance here
        float distance = Vector3.Distance(transform.position, target.position);
        float t = Mathf.Clamp01(distance / maxDistance); // 0 = close, 1 = far
        lineRenderer.startWidth = Mathf.Lerp(maxWidth, minWidth, t); // thick when close
        lineRenderer.endWidth = 0f; 
        for (int i = 0; i < pointCount; i++)
        {
            float tt = i / (float)(pointCount - 1);

            // base position lerped between source and target
            Vector3 basePos = Vector3.Lerp(transform.position, target.position, tt);

            float noise = Mathf.PerlinNoise(tt * 3f, Time.time * waveSpeed) - 0.5f;
            Vector3 offset = new Vector3(noise, noise * 0.5f, 0f) * waveStrength;
            basePos += offset;

            lineRenderer.SetPosition(i, basePos);
        }
    }
}