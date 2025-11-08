using NavKeypad;
using UnityEngine;

public class KeypadActivator : MonoBehaviour
{
    [SerializeField] private GameObject keypadUIPanel;
    [SerializeField] private Transform player;
    [SerializeField] private float activationDistance = 3f;
    [SerializeField] private Keypad keypad;

    private bool isPlayerNear = false;
    private bool isUIOpen = false;

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        isPlayerNear = distance <= activationDistance;

        if (isPlayerNear && !isUIOpen && Input.GetKeyDown(KeyCode.F))
        {
            OpenKeypadUI();
        }
        else if (isUIOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypadUI();
        }
    }

    private void OpenKeypadUI()
    {
        isUIOpen = true;
        keypadUIPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseKeypadUI()
    {
        isUIOpen = false;
        keypadUIPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnAccessGranted()
    {
        CloseKeypadUI();
    }
}