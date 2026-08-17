using UnityEngine;

namespace SimpleTowerDefense
{
    public enum TowerType
    {
        MachineGun,
        Laser,
        Rocket
    }

    /// <summary>
    /// Finds the nearest enemy, turns toward it and attacks. One class handles all three towers.
    /// </summary>
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerType towerType;
        [SerializeField] private string displayName = "Tower";
        [SerializeField] private float range = 7f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private float projectileSpeed = 18f;
        [SerializeField] private float splashRadius;
        [SerializeField] private int upgradeCost = 100;
        [SerializeField] private int sellValue = 60;
        [SerializeField] private Transform rotatingPart;
        [SerializeField] private Transform firePoint;
        [SerializeField] private LineRenderer laserLine;

        private BuildSpot owner;
        private Enemy target;
        private float attackTimer;
        private float targetSearchTimer;

        public string DisplayName => displayName;
        public int UpgradeCost => upgradeCost;
        public int SellValue => sellValue;
        public bool IsUpgraded { get; private set; }

        private void Update()
        {
            targetSearchTimer -= Time.deltaTime;
            if (targetSearchTimer <= 0f)
            {
                targetSearchTimer = 0.2f;
                target = FindNearestEnemy();
            }

            if (target == null)
            {
                return;
            }

            AimAtTarget();
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = 1f / attacksPerSecond;
            }
        }

        private Enemy FindNearestEnemy()
        {
            Enemy nearest = null;
            float nearestDistance = range * range;

            foreach (Enemy enemy in Enemy.ActiveEnemies)
            {
                if (enemy == null)
                {
                    continue;
                }

                float distance = (enemy.transform.position - transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = enemy;
                }
            }

            return nearest;
        }

        private void AimAtTarget()
        {
            Vector3 direction = target.TargetPoint.position - rotatingPart.position;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                rotatingPart.rotation = Quaternion.Slerp(
                    rotatingPart.rotation,
                    rotation,
                    10f * Time.deltaTime);
            }
        }

        private void Attack()
        {
            AudioManager.Instance?.PlayTowerShot(towerType);

            if (towerType == TowerType.Laser)
            {
                target.TakeDamage(damage);
                laserLine.SetPosition(0, firePoint.position);
                laserLine.SetPosition(1, target.TargetPoint.position);
                laserLine.enabled = true;
                CancelInvoke(nameof(HideLaser));
                Invoke(nameof(HideLaser), 0.08f);
                return;
            }

            Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.Launch(target, damage, projectileSpeed, splashRadius);
        }

        private void HideLaser()
        {
            if (laserLine != null)
            {
                laserLine.enabled = false;
            }
        }

        public void Upgrade()
        {
            if (IsUpgraded)
            {
                return;
            }

            IsUpgraded = true;
            damage *= 1.6f;
            range *= 1.15f;
            attacksPerSecond *= 1.2f;
            rotatingPart.localScale *= 1.12f;
        }

        public void SetOwner(BuildSpot buildSpot)
        {
            owner = buildSpot;
        }

        public void SelectOwner()
        {
            owner.Select();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }

        // Used only by the editor prefab builder.
        public void Configure(
            TowerType type, string towerName, float towerRange, float towerDamage,
            float fireRate, Projectile projectile, float shotSpeed, float blastRadius,
            int upgradePrice, int refund, Transform turret, Transform muzzle, LineRenderer beam)
        {
            towerType = type;
            displayName = towerName;
            range = towerRange;
            damage = towerDamage;
            attacksPerSecond = fireRate;
            projectilePrefab = projectile;
            projectileSpeed = shotSpeed;
            splashRadius = blastRadius;
            upgradeCost = upgradePrice;
            sellValue = refund;
            rotatingPart = turret;
            firePoint = muzzle;
            laserLine = beam;
        }
    }
}
