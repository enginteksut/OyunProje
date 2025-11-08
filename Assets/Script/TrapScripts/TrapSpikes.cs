using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    public Transform[] spears; // Inspector'dan mızrakları atayabilirsin
    public float raisedHeight = 2f;
    public float loweredHeight = 0f;
    public float speed = 2f;
    private bool playerOnTrap = false;

    private void Update()
    {
        foreach (Transform spear in spears)
        {
            Vector3 targetPosition = spear.localPosition;
            targetPosition.y = playerOnTrap ? raisedHeight : loweredHeight;
            spear.localPosition = Vector3.Lerp(spear.localPosition, targetPosition, Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            foreach (Transform spear in spears)
            {
                spear.transform.localPosition += new Vector3(0, 2f, 0);

                // Zararı aktif et
                SpearDamage dmg = spear.GetComponent<SpearDamage>();
                if (dmg != null)
                    dmg.EnableDamage();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            playerOnTrap = false;
        }
    }
}