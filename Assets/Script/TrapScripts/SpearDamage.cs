using UnityEngine;

public class SpearDamage : MonoBehaviour
{
    public int damageAmount = 100; 
    private bool canDamage = false;

    public void EnableDamage()
    {
        canDamage = true;
    }

    public void DisableDamage()
    {
        canDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canDamage && other.CompareTag("Enemy"))
        {
            PlayerHp playerHp = other.GetComponent<PlayerHp>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(damageAmount);
            }
        }
    }
}