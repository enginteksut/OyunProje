using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoFinishHandler : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Inspector'dan atayabilirsin
    public string anaMenuSahneAdi = "Main"; // Ana menü sahne ismi

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("Main");
    }
}