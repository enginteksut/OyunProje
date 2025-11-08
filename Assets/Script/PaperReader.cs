using UnityEngine;
using TMPro;

public class PaperReader : MonoBehaviour
{
    public float etkilesimMesafesi = 3f;
    public LayerMask paperLayer;
    public GameObject uiPanel;
    public TMP_Text okumaBilgiText;   // "Kağıdı okumak için: Sol Tık"
    public TMP_Text cikmaBilgiText;   // "Kapatmak için: ESC"
    public AudioSource audioSource;
    public AudioClip okumaSesi;
    public GameObject crosshair;       // Inspector’dan atayınız

    private GameObject currentPaper;
    private bool isReading = false;

    void Start()
    {
        // Başlangıçta imleci gizle ve kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Crosshair her zaman açık kalsın
        if (crosshair != null)
            crosshair.SetActive(true);
    }

    void Update()
    {
        if (!isReading)
        {
            if (Input.GetMouseButtonDown(0)) // Artık sol tıkla etkileşim
            {
                TryReadPaper();
            }
            else
            {
                // Kağıt önünde isek "Kağıdı okumak için: Sol Tık" göster
                RaycastHit hit;
                bool hitPaper = Physics.Raycast(transform.position, transform.forward, out hit, etkilesimMesafesi, paperLayer);
                if (hitPaper)
                {
                    okumaBilgiText.text = "Kağıdı okumak için: Sol Tık";
                    okumaBilgiText.gameObject.SetActive(true);
                }
                else
                {
                    okumaBilgiText.gameObject.SetActive(false);
                }

                cikmaBilgiText.gameObject.SetActive(false);
            }
        }
        else
        {
            // Okuma modundayken ESC ile kapatma
            okumaBilgiText.gameObject.SetActive(false);
            cikmaBilgiText.text = "Kapatmak için: ESC";
            cikmaBilgiText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePaper();
            }
        }
    }

    void TryReadPaper()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, etkilesimMesafesi, paperLayer))
        {
            currentPaper = hit.collider.gameObject;
            uiPanel.SetActive(true);

            if (audioSource != null && okumaSesi != null && !audioSource.isPlaying)
            {
                audioSource.clip = okumaSesi;
                audioSource.Play();
            }

            isReading = true;

            // İmleç HİÇ görünmesin
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void ClosePaper()
    {
        uiPanel.SetActive(false);
        okumaBilgiText.gameObject.SetActive(false);
        cikmaBilgiText.gameObject.SetActive(false);

        currentPaper = null;
        isReading = false;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // İmleç gizli ve kilitli kalmaya devam etsin
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
