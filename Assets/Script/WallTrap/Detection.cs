using UnityEngine;

public class Detection : MonoBehaviour
{
    public WallCrushTrap wallTrap;
    public float damagePerSecond = 50f;

    void OnTriggerStay(Collider other)
    {
        // "Enemy" tag'ini oyuncuya verdiysen kullan
        if (other.CompareTag("Enemy"))
        {
            wallTrap.ContinueCrushing();

            // Hasar verme işlemi
            PlayerHp playerHealth = other.GetComponent<PlayerHp>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            wallTrap.StopCrushing();
        }
    }
}