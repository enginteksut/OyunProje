using UnityEngine;
using System.Collections; // Coroutine için değil ama düzenli kalsın diye eklenebilir

public class DragonPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform[] waypoints;
    public float speed = 20f;
    public float reachDistance = 1f;

    [Header("Animation Parameters")]
    public string flyingAnimBoolName = "FlyingFWD";
    public string roarAnimTriggerName = "Roar";      // Kükreme animasyonu için trigger parametre adı

    [Header("Sound Settings")]
    public AudioClip roarSound;               // Kükreme sesi (DragonSound.mp3 buraya atanacak)
    public float roarCooldown = 15f;          // Kükremeler arası bekleme süresi (saniye)
    private float nextRoarTime = 0f;

    private int currentWaypointIndex = 0;
    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // AudioSource bileşenini al

        // AudioSource kontrolü
        if (audioSource == null)
        {
            Debug.LogWarning("Bu GameObject üzerinde AudioSource bileşeni bulunamadı. Kükreme sesi çalınamayacak. Lütfen bir AudioSource ekleyin ve 'Roar Sound' alanına sesi atayın.");
        }
        else if (roarSound == null)
        {
            Debug.LogWarning("'Roar Sound' alanı boş. Kükreme sesi çalınamayacak. Lütfen sesi atayın.");
        }


        if (waypoints.Length == 0)
        {
            Debug.LogWarning("Waypoints boş! Lütfen inspector'dan waypointleri ata.");
            enabled = false; // Script'i devre dışı bırak
            return;
        }

        // Uçuş animasyonunu başlat
        if (anim != null && anim.HasParameter(flyingAnimBoolName))
        {
            anim.SetBool(flyingAnimBoolName, true);
        }
        else if (anim == null)
        {
            Debug.LogError("Animator bileşeni bulunamadı!");
        }
        else
        {
            Debug.LogWarning("Animator'da '" + flyingAnimBoolName + "' isimli bir Bool parametresi bulunamadı!");
        }

        // İlk kükreme için rastgele bir başlangıç zamanı (isteğe bağlı, daha doğal yapar)
        nextRoarTime = Time.time + Random.Range(roarCooldown * 0.5f, roarCooldown * 1.5f);
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        MoveToWaypoint();

        // Kükreme zamanı geldiyse
        if (Time.time >= nextRoarTime)
        {
            TryPerformRoar();
        }
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];

        // Pozisyonu güncelle
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Yönünü hedefe döndür
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Dönüşü biraz daha hızlı yaptım
        }

        // Hedefe ulaştıysa sonraki hedefe geç
        if (Vector3.Distance(transform.position, target.position) < reachDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void TryPerformRoar()
    {
        // Gerekli bileşenler ve ayarlar var mı kontrol et
        if (audioSource != null && roarSound != null)
        {
            Debug.Log("Ejderha kükrüyor!");

            // Kükreme animasyonunu tetikle (Animator'da "Roar" trigger parametresi olduğunu varsayıyoruz)
            // Ve bu parametrenin gerçekten bir Trigger olduğundan emin oluyoruz
            if (anim != null && anim.HasParameter(roarAnimTriggerName, AnimatorControllerParameterType.Trigger))
            {
                anim.SetTrigger(roarAnimTriggerName);
            }
            else if (anim != null && !string.IsNullOrEmpty(roarAnimTriggerName)) // Parametre adı girilmiş ama bulunamamışsa uyar
            {
                // Debug.LogWarning("Animator'da '" + roarAnimTriggerName + "' isimli bir Trigger parametresi bulunamadı veya parametre adı boş.");
                // Bu uyarıyı her kükreme denemesinde görmek istemeyebilirsin, Start'a taşınabilir veya kaldırılabilir.
            }


            // Kükreme sesini çal
            audioSource.PlayOneShot(roarSound);

            // Bir sonraki kükreme zamanını ayarla
            nextRoarTime = Time.time + roarCooldown + Random.Range(-roarCooldown * 0.2f, roarCooldown * 0.2f); // Biraz rastgelelik ekle
        }
    }
}

// AnimatorExtensions sınıfı aynı kalmalı, HasParameter metodu kullanılıyor.
// Projenizde zaten varsa tekrar eklemenize gerek yok.
public static class AnimatorExtensions
{
    public static bool HasParameter(this Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    // Trigger parametrelerini kontrol etmek için ek bir extension metodu (opsiyonel ama daha güvenli)
    public static bool HasParameter(this Animator animator, string paramName, AnimatorControllerParameterType paramType)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == paramType && param.name == paramName)
                return true;
        }
        return false;
    }
}