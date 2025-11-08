using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;

    public Transform ground;
    public float distance = 0.3f;

    public float speed;
    public float jumpHeight;
    public float gravity;

    public float originalHeight;
    public float crouchHeight;

    public bool canMove = true;

    public LayerMask mask;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    private void Update()
    {



        #region Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        if (canMove)
        {
            controller.Move(move * speed * Time.deltaTime);
        }


        #endregion

        #region Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);

        }

        #endregion

        #region Gravity
        isGrounded = Physics.CheckSphere(ground.position, distance, mask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        #endregion

        #region Basic Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controller.height = crouchHeight;
            speed = 2.0f;
            jumpHeight = 0;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            controller.height = originalHeight;
            speed = 5.0f;
            jumpHeight = 1.0f;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            speed = 2.0f;
        }

        #endregion

        #region Basic Running

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 10.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5.0f;
        }
        #endregion
    }

}
