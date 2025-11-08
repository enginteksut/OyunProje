using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeverController : MonoBehaviour
{
    public GameObject lever;
    public GameObject door;
    public TextMeshProUGUI interactionText;
    public float interactionDistance = 3f;
    public float openHeight = 3f;
    public float openSpeed = 2f;

    public AudioSource audioSource;       // ← Ekledik
    public AudioClip doorSound;           // ← Ekledik

    private bool isOpen = false;
    private Vector3 initialDoorPosition;
    private Vector3 targetDoorPosition;
    private Vector3 initialScale;
    private float minScaleY = 0.2f;
    private Camera playerCamera;
    private Collider doorCollider;

    void Start()
    {
        initialDoorPosition = door.transform.position;
        targetDoorPosition = initialDoorPosition + Vector3.up * openHeight;
        initialScale = door.transform.localScale;
        playerCamera = Camera.main;
        interactionText.gameObject.SetActive(false);
        doorCollider = door.GetComponent<Collider>();

        if (doorCollider != null)
            doorCollider.isTrigger = false;
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == lever)
            {
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F) && !isOpen)
                {
                    isOpen = true;
                    interactionText.gameObject.SetActive(false);

                    if (audioSource != null && doorSound != null)
                        audioSource.PlayOneShot(doorSound); // ← Ses çalma

                    if (doorCollider != null)
                        doorCollider.isTrigger = true;

                    Invoke("StartClosingDoor", 3f);
                }
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }

        if (isOpen)
        {
            door.transform.position = Vector3.MoveTowards(
                door.transform.position,
                targetDoorPosition,
                openSpeed * Time.deltaTime
            );

            float progress = Mathf.InverseLerp(initialDoorPosition.y, targetDoorPosition.y, door.transform.position.y);
            door.transform.localScale = new Vector3(
                initialScale.x,
                Mathf.Lerp(initialScale.y, minScaleY, progress),
                initialScale.z
            );
        }
    }

    void StartClosingDoor()
    {
        isOpen = false;
        StartCoroutine(CloseDoor());
    }

    IEnumerator CloseDoor()
    {
        while (door.transform.position.y > initialDoorPosition.y)
        {
            door.transform.position = Vector3.MoveTowards(
                door.transform.position,
                initialDoorPosition,
                openSpeed * Time.deltaTime
            );

            float progress = Mathf.InverseLerp(targetDoorPosition.y, initialDoorPosition.y, door.transform.position.y);
            door.transform.localScale = new Vector3(
                initialScale.x,
                Mathf.Lerp(minScaleY, initialScale.y, progress),
                initialScale.z
            );

            yield return null;
        }

        if (doorCollider != null)
            doorCollider.isTrigger = false;
    }
}
