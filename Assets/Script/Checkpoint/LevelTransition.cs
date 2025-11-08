using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string SahneIsmi;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            CheckpointManager.lastCheckpointPosition = Vector3.zero;
            SceneManager.LoadScene(SahneIsmi);
        }
    }
}
