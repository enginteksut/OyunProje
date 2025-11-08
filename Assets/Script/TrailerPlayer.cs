using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TrailerPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;

    private bool _hasPlayed = false;

    private void Start()
    {
        if (_videoPlayer == null)
        {
            Debug.LogError("VideoPlayer referansı atanmadı!");
            return;
        }

        _videoPlayer.loopPointReached += OnVideoEnd;
        _videoPlayer.Play();
    }



    private void OnVideoEnd(VideoPlayer vp)
    {
        if (!_hasPlayed)
        {
            _hasPlayed = true;
            SceneManager.LoadScene("Level1");
        }
    }
}
