using UnityEngine;

public class WallCrushTrap : MonoBehaviour
{
    public Transform leftWall;
    public Transform rightWall;
    public Transform detectorBox;
    public float speed = 1f;

    private Vector3 leftStart;
    private Vector3 rightStart;
    public bool activateTrap = false;

    public float damageAmount = 100.0f;
    public string enemyTag = "Enemy";

    void Start()
    {
        leftStart = leftWall.position;
        rightStart = rightWall.position;
    }


    void Update()
    {
        if (activateTrap)
        {
            leftWall.position += Vector3.right * speed * Time.deltaTime;
            rightWall.position += Vector3.left * speed * Time.deltaTime;

            CheckCrushPlayer();
        }
        else
        {
            leftWall.position = Vector3.MoveTowards(leftWall.position, leftStart, speed * Time.deltaTime);
            rightWall.position = Vector3.MoveTowards(rightWall.position, rightStart, speed * Time.deltaTime);
        }

        UpdateDetectorDepth();
    }

    void UpdateDetectorDepth()
    {
        float distance = Vector3.Distance(leftWall.position, rightWall.position);
        Vector3 scale = detectorBox.localScale;
        scale.z = distance;
        detectorBox.localScale = scale;
    }

    public void ContinueCrushing()
    {
        activateTrap = true;
    }

    public void StopCrushing()
    {
        activateTrap = false;
    }

    void CheckCrushPlayer()
    {
        // Duvarlar arasındaki mesafe
        float distance = Vector3.Distance(leftWall.position, rightWall.position);

        // Sıkışma eşiği (duvarlar birbirine çok yakınsa)
        if (distance < 0.1f)
        {
            // detectorBox alanındaki objeleri bul
            Collider[] hits = Physics.OverlapBox(detectorBox.position, detectorBox.localScale / 2);

            foreach (Collider hit in hits)
            {
                if (hit.CompareTag(enemyTag))
                {
                    PlayerHp playerHealth = hit.GetComponent<PlayerHp>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damageAmount); // Anında tam hasar
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            PlayerHp playerHealth = other.GetComponent<PlayerHp>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}