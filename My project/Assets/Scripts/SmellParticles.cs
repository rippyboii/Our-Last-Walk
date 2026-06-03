using UnityEngine;

public class SmellParticles : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem smellParticles;
    public Transform target;

    [Header("Settings")]
    public float maxDistance = 20f;
    public float minEmissionRate = 2f;
    public float maxEmissionRate = 40f;
    public float attractSpeed = 2f;

    private Player player;
    private ParticleSystem.Particle[] particles;
    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        player = FindObjectOfType<Player>();
        smellParticles = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[smellParticles.main.maxParticles];
        emission = smellParticles.emission;
        emission.enabled = false;
    }

    void Update()
    {
        if (!player.IsDog())
        {
            emission.enabled = false;
            smellParticles.Clear();
            return;
        }

        emission.enabled = true;

        // scale emission rate by distance
        float distance = Vector3.Distance(transform.position, target.position);
        float t = 1f - Mathf.Clamp01(distance / maxDistance);
        emission.rateOverTime = Mathf.Lerp(minEmissionRate, maxEmissionRate, t);

        // move particles toward target
        int count = smellParticles.GetParticles(particles);
        for (int i = 0; i < count; i++)
        {
            particles[i].position = Vector3.MoveTowards(
                particles[i].position,
                target.position,
                attractSpeed * Time.deltaTime
            );
        }
        smellParticles.SetParticles(particles, count);
    }
}