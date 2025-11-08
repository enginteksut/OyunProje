using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public PlayerController playerScript;


    [Range(50, 500)]
    public float sens;

    public Transform body;

    float xRot = 0f;

    public Transform leaner;
    public float zRot;
    bool isRotating;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        #region Camera Movement
        float rotX = Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime;
        float rotY = Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime;

        xRot -= rotY;
        xRot = Mathf.Clamp(xRot, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        body.Rotate(Vector3.up * rotX);
        #endregion

        #region Camera Lean
        if (Input.GetKey(KeyCode.E))
        {
            zRot = Mathf.Lerp(zRot, -20.0f, 5f * Time.deltaTime);
            isRotating = true;
            playerScript.canMove = false;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            zRot = Mathf.Lerp(zRot, 20.0f, 5f * Time.deltaTime);
            isRotating = true;
            playerScript.canMove = false;
        }

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            isRotating = false;
            playerScript.canMove = true;
        }
        if (!isRotating)
        {
            zRot = Mathf.Lerp(zRot, 0.0f, 5f * Time.deltaTime);
        }

        leaner.localRotation = Quaternion.Euler(0, 0, zRot);
        #endregion
    }
}
