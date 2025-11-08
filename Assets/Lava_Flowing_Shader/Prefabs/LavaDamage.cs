using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    public float damagePerSecond = 10f;
    public AudioClip damageSound;  // Ses dosyasını buraya bağlayacağız

    private void OnTriggerStay(Collider other)
    {
        PlayerHp playerHp = other.GetComponent<PlayerHp>();
        if (playerHp != null)
        {
            playerHp.TakeDamage(damagePerSecond * Time.deltaTime);

            // Ses oynatma
            AudioSource audioSource = other.GetComponent<AudioSource>();
            if (audioSource != null && damageSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(damageSound);
            }
        }
    }
}
