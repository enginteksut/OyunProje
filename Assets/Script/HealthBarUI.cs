using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHp player;
    public Image fillImage;

    void Update()
    {
        if (player != null && fillImage != null)
        {
            fillImage.fillAmount = player.currentHp / player.maxHp;
        }
    }
}