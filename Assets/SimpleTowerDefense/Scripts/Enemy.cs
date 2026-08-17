using System.Collections.Generic;
using UnityEngine;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Moves along waypoints, takes damage and rewards the player when destroyed.
    /// Different enemy prefabs simply use different values in the Inspector.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        private static readonly List<Enemy> activeEnemies = new List<Enemy>();
        public static IReadOnlyList<Enemy> ActiveEnemies => activeEnemies;

        [SerializeField] private int maxHealth = 40;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private int reward = 20;
        [SerializeField] private int baseDamage = 1;
        [SerializeField] private float turnSpeed = 8f;
        [SerializeField] private Transform targetPoint;

        private Transform[] waypoints;
        private WaveManager waveManager;
        private int waypointIndex;
        private int health;
        private bool finished;

        public Transform TargetPoint => targetPoint != null ? targetPoint : transform;

        private void OnEnable()
        {
            activeEnemies.Add(this);
            health = maxHealth;
            finished = false;
        }

        private void OnDisable()
        {
            activeEnemies.Remove(this);
        }

        public void BeginPath(Transform[] path, WaveManager owner)
        {
            waypoints = path;
            waveManager = owner;
            waypointIndex = 1;
        }

        private void Update()
        {
            if (finished || waypoints == null || waypointIndex >= waypoints.Length)
            {
                return;
            }

            Vector3 destination = waypoints[waypointIndex].position;
            Vector3 direction = destination - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.08f)
            {
                waypointIndex++;
                if (waypointIndex >= waypoints.Length)
                {
                    ReachBase();
                }
                return;
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                moveSpeed * Time.deltaTime);

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    lookRotation,
                    turnSpeed * Time.deltaTime);
            }
        }

        public void TakeDamage(float amount)
        {
            if (finished)
            {
                return;
            }

            health -= Mathf.RoundToInt(amount);
            if (health <= 0)
            {
                GameManager.Instance.RewardEnemyDefeat(reward);
                AudioManager.Instance?.PlayEnemyDeath();
                EffectsManager.Instance?.PlayEnemyDestruction(transform.position);
                Finish();
            }
        }

        private void ReachBase()
        {
            AudioManager.Instance?.PlayBaseDamage();
            GameManager.Instance.DamageBase(baseDamage);
            Finish();
        }

        private void Finish()
        {
            if (finished)
            {
                return;
            }

            finished = true;
            waveManager.EnemyFinished();
            Destroy(gameObject);
        }

        // Used only by the editor prefab builder.
        public void Configure(int healthValue, float speed, int killReward, int damage, Transform aimPoint)
        {
            maxHealth = healthValue;
            moveSpeed = speed;
            reward = killReward;
            baseDamage = damage;
            targetPoint = aimPoint;
        }
    }
}
