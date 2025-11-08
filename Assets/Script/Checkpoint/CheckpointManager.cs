using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    public static Vector3 lastCheckpointPosition = Vector3.zero;

    private void Awake()
    {
        // Singleton kurulumu
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetActiveCheckpoint(Vector3 checkpointPos)
    {
        lastCheckpointPosition = checkpointPos;
        Debug.Log("Checkpoint pozisyonu güncellendi: " + checkpointPos);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Enemy");

        if (player != null)
        {
            if (lastCheckpointPosition != Vector3.zero)
                player.transform.position = lastCheckpointPosition;
            else
            {
                GameObject start = GameObject.Find("LevelStartPoint");
                if (start != null)
                    player.transform.position = start.transform.position;
                else
                    Debug.LogWarning("LevelStartPoint bulunamadı!");
            }
        }
    }
}
