using UnityEngine;

/// <summary>
/// Controls a projectile that follows a target and creates an explosion effect on impact
/// </summary>
namespace Mythmatic.TurretSystem
{
    public class HomingProjectile : MonoBehaviour
    {
        [Header("Target Settings")]
        public GameObject target;

        [Header("Movement Settings")]
        [Range(1f, 100f)] public float speed = 20f;
        [Range(0f, 720f)] public float rotationSpeed = 360f;
        [Range(0f, 10f)] public float homingStrength = 10f;

        [Header("Lifetime Settings")]
        [Range(0.1f, 10f)] public float maxLifetime = 3f;

        [Header("Effects")]
        [SerializeField] private GameObject _explosionPrefab;
        public GameObject explosionPrefab
        {
            get { return _explosionPrefab; }
            set { _explosionPrefab = value; }
        }

        [Range(1, 100)] public int particleCount = 30;

        private Rigidbody rb;
        private float creationTime;
        private float initialSpeed;

        private void Awake()
        {
            initialSpeed = speed;
        }

        public void Initialize(GameObject enemy, float? projectileSpeed = null)
        {
            target = enemy;
            if (projectileSpeed.HasValue)
                speed = projectileSpeed.Value;

            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            creationTime = Time.time;

            if (target != null)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                rb.velocity = direction * speed;
            }
        }

        private void FixedUpdate()
        {
            if (target == null || rb == null)
            {
                DestroyProjectile();
                return;
            }

            if (Time.time - creationTime > maxLifetime)
            {
                DestroyProjectile();
                return;
            }

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

            transform.rotation = rotation;

            Vector3 newVelocity = Vector3.Lerp(
                rb.velocity.normalized,
                directionToTarget,
                homingStrength * Time.fixedDeltaTime
            ).normalized * speed;

            rb.velocity = newVelocity;
        }

        private void SpawnExplosion(Vector3 position)
        {
            if (_explosionPrefab != null)
            {
                GameObject explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);

                ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    var emission = particleSystem.emission;
                    emission.enabled = false;
                    particleSystem.Emit(particleCount);
                }

                float lifetime = particleSystem != null ? particleSystem.main.duration : 1f;
                Destroy(explosion, lifetime);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == target)
            {
                Vector3 impactPosition = collision.contacts[0].point;
                SpawnExplosion(impactPosition);

                // HASAR VER
                PlayerHp playerHp = collision.gameObject.GetComponent<PlayerHp>();
                if (playerHp != null)
                {
                    playerHp.TakeDamage(4f); // İstediğin hasar miktarını gir
                }
            }

            DestroyProjectile();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == target)
            {
                SpawnExplosion(transform.position);

                // HASAR VER
                PlayerHp playerHp = other.GetComponent<PlayerHp>();
                if (playerHp != null)
                {
                    playerHp.TakeDamage(4f); // İstediğin hasar miktarını gir
                }
            }

            DestroyProjectile();
        }

        private void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }
}
