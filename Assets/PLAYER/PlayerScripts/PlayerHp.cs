using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerHp : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        Debug.Log("Hasar alındı! Güncel HP: " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Oyuncu öldü!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Heal(float healAmount)
    {
        currentHp += healAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        Debug.Log("İyileştirildi! Güncel HP: " + currentHp);
    }
}
