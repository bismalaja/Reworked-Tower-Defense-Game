using System.Collections.Generic;
using UnityEngine;

namespace SimpleTowerDefense
{
    /// <summary>
    /// A deliberately simple homing projectile. Rockets use splashRadius; bullets use zero.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        private Enemy target;
        private float damage;
        private float speed;
        private float splashRadius;
        [SerializeField] private float collisionRadius = 0.12f;

        public void Launch(Enemy enemy, float damageValue, float speedValue, float radius)
        {
            target = enemy;
            damage = damageValue;
            speed = speedValue;
            splashRadius = radius;
            Destroy(gameObject, 6f);
        }

        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 destination = target.TargetPoint.position;
            Vector3 direction = destination - transform.position;
            float travelDistance = speed * Time.deltaTime;

            if (direction != Vector3.zero && Physics.SphereCast(
                transform.position,
                collisionRadius,
                direction.normalized,
                out RaycastHit hit,
                travelDistance))
            {
                Enemy hitEnemy = hit.collider.GetComponentInParent<Enemy>();
                if (hitEnemy != null)
                {
                    HitTarget(hitEnemy, hit.point);
                    return;
                }
            }

            if (direction.magnitude <= travelDistance)
            {
                HitTarget(target, destination);
                return;
            }

            transform.position += direction.normalized * travelDistance;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void HitTarget(Enemy hitEnemy, Vector3 impactPoint)
        {
            if (splashRadius <= 0f)
            {
                EffectsManager.Instance?.PlayBulletImpact(impactPoint);
                hitEnemy.TakeDamage(damage);
            }
            else
            {
                EffectsManager.Instance?.PlayRocketExplosion(impactPoint);
                Collider[] hits = Physics.OverlapSphere(impactPoint, splashRadius);
                HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();

                foreach (Collider hit in hits)
                {
                    Enemy enemy = hit.GetComponentInParent<Enemy>();
                    if (enemy != null && damagedEnemies.Add(enemy))
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
