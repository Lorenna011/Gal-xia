using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class GalaxySpiral : MonoBehaviour
{
    [Header("Formato da galáxia")]
    public int arms = 3;
    public int starsPerArm = 600;
    public float maxRadius = 3.5f;
    public float turns = 1.2f;
    public float rotationSpeed = 0.15f;
    public float armWidth = 0.18f;

    [Header("Aparência das partículas")]
    public float minParticleSize = 0.015f;
    public float maxParticleSize = 0.045f;
    public Color innerColor = Color.white;
    public Color outerColor = Color.blue;

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private Vector3[] basePositions;
    private Color[] colors;
    private float[] sizes;
    private int totalStars;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        arms = Mathf.Max(1, arms);
        starsPerArm = Mathf.Max(1, starsPerArm);

        totalStars = Mathf.Min(arms * starsPerArm, 2000);

        particles = new ParticleSystem.Particle[totalStars];
        basePositions = new Vector3[totalStars];
        colors = new Color[totalStars];
        sizes = new float[totalStars];

        Random.InitState(42);

        for (int i = 0; i < totalStars; i++)
        {
            int arm = i % arms;

            float t = Random.value;

            float angle = t * turns * Mathf.PI * 2f
                        + arm * Mathf.PI * 2f / arms;

            float radius = 0.15f + t * maxRadius;

            radius += Random.Range(-armWidth, armWidth);
            radius = Mathf.Clamp(radius, 0.05f, maxRadius + armWidth);

            basePositions[i] = new Vector3(
                radius * Mathf.Cos(angle),
                radius * Mathf.Sin(angle),
                Random.Range(-0.02f, 0.02f)
            );

            float colorBlend = Random.value;

            colors[i] = Color.Lerp(
                outerColor,
                innerColor,
                colorBlend
            );

            sizes[i] = Random.Range(
                minParticleSize,
                maxParticleSize
            );
        }
    }

    void Update()
    {
        float rotation = Time.time * rotationSpeed;

        float cos = Mathf.Cos(rotation);
        float sin = Mathf.Sin(rotation);

        for (int i = 0; i < totalStars; i++)
        {
            Vector3 p = basePositions[i];

            float x = p.x * cos - p.y * sin;
            float y = p.x * sin + p.y * cos;

            particles[i].position = new Vector3(x, y, p.z);
            particles[i].startSize = sizes[i];
            particles[i].startColor = colors[i];
            particles[i].remainingLifetime = 10000f;
            particles[i].startLifetime = 10000f;
        }

        ps.SetParticles(particles, totalStars);
    }
}
