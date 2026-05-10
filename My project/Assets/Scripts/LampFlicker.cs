using UnityEngine;
public class LampFlicker : MonoBehaviour
{
    public Light lampLight;
    public float flickerRadius = 4f;
    public Transform ghost;
    private float baseIntensity;

    void Start()
    {
        baseIntensity = lampLight.intensity;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, ghost.position);
        
        if (distance < flickerRadius)
        {
            // subtle flicker using perlin noise
            float flicker = Mathf.PerlinNoise(Time.time * 3f, 0f);
            lampLight.intensity = Mathf.Lerp(baseIntensity, baseIntensity * 1.3f, flicker);
        }
        else
        {
            lampLight.intensity = baseIntensity;
        }
    }
}