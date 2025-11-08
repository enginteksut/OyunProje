using UnityEngine;

public class GolemPatrol : MonoBehaviour
{
    public float speed = 2f;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Şu anda hareket etmiyor, ama animasyon durumu yine de sıfırlanıyor
        animator.SetBool("isWalking", false);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Eğer çarpılan obje başka bir "Golem" ise ve ses çalmıyorsa, sesi çal
        if (collision.gameObject.CompareTag("Golem") && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
