using UnityEngine;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Creates four short particle bursts from one reusable setup method.
    /// </summary>
    public class EffectsManager : MonoBehaviour
    {
        public static EffectsManager Instance { get; private set; }

        [SerializeField] private Material particleMaterial;

        private void Awake()
        {
            Instance = this;
        }

        public void PlayBulletImpact(Vector3 position)
        {
            CreateBurst("Bullet Impact", position, new Color(1f, 0.8f, 0.2f), 0.22f, 8);
        }

        public void PlayRocketExplosion(Vector3 position)
        {
            CreateBurst("Rocket Explosion", position, new Color(1f, 0.25f, 0.05f), 1.3f, 28);
        }

        public void PlayEnemyDestruction(Vector3 position)
        {
            CreateBurst("Enemy Destruction", position, new Color(1f, 0.15f, 0.1f), 0.7f, 16);
        }

        public void PlayBuildEffect(Vector3 position)
        {
            CreateBurst("Build Effect", position, new Color(0.1f, 0.75f, 1f), 0.9f, 20);
        }

        private void CreateBurst(
            string effectName, Vector3 position, Color color, float size, int particleCount)
        {
            GameObject effectObject = new GameObject(effectName);
            effectObject.transform.position = position;

            ParticleSystem particles = effectObject.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ParticleSystem.MainModule main = particles.main;
            main.duration = 0.35f;
            main.loop = false;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.2f, 0.45f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(1.5f, 4.5f);
            main.startSize = new ParticleSystem.MinMaxCurve(size * 0.25f, size * 0.55f);
            main.startColor = color;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.stopAction = ParticleSystemStopAction.Destroy;
            main.maxParticles = particleCount;

            ParticleSystem.EmissionModule emission = particles.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[]
            {
                new ParticleSystem.Burst(0f, (short)particleCount)
            });

            ParticleSystem.ShapeModule shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = size * 0.25f;

            ParticleSystemRenderer particleRenderer =
                effectObject.GetComponent<ParticleSystemRenderer>();
            particleRenderer.sharedMaterial = particleMaterial;

            particles.Play();
        }

        // Used only by the editor scene builder.
        public void Configure(Material material)
        {
            particleMaterial = material;
        }
    }
}
