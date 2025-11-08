using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    public void Hit()
    {
        Debug.Log($"{gameObject.name} hedef alındı!");
        // Buraya vurulma animasyonu, ses efekti veya parçalanma gibi şeyler eklenebilir.
    }
}

public class Laser : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserDistance = 15f;
    [SerializeField] private LayerMask ignoreMask;

    [Header("Damage Settings")]
    [SerializeField] private float damageCooldown = 1f; // saniyede 1 kere hasar
    [SerializeField] private float damageAmount = 10f;  // hasar miktarı

    [Header("Events")]
    [SerializeField] private UnityEvent OnHitTarget;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioSource audioSource;

    private RaycastHit rayHit;
    private Ray ray;
    private float damageTimer;

    private void Awake()
    {
        lineRenderer.positionCount = 2;

        // Eğer sahnede AudioSource eklenmemişse otomatik eklenir
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        damageTimer -= Time.deltaTime;
        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out rayHit, laserDistance, ~ignoreMask))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, rayHit.point);

            // Düşmana çarptıysa
            if (rayHit.collider.CompareTag("Enemy"))
            {
                if (damageTimer <= 0f && rayHit.collider.TryGetComponent(out PlayerHp playerHp))
                {
                    playerHp.TakeDamage(damageAmount);
                    OnHitTarget?.Invoke();
                    PlayHitSound();
                    damageTimer = damageCooldown;
                }
            }

            // Diğer target objesi varsa (örnek: tahta hedef)
            if (rayHit.collider.TryGetComponent(out Target target))
            {
                target.Hit();
                OnHitTarget?.Invoke();
                PlayHitSound();
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * laserDistance);
        }
    }

    private void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, ray.direction * laserDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rayHit.point, 0.23f);
    }
}
