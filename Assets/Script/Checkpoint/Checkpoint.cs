using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform spawnPoint; // Bu nesne, oyuncunun doğacağı yerdir

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Checkpoint tetiklendi: " + other.name);
            CheckpointManager.Instance.SetActiveCheckpoint(spawnPoint.position);
        }
    }
}
