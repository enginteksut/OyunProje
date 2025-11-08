using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;


    public CharacterController controller;

    void Update()
    {


        bool isJumping = Input.GetKey(KeyCode.Space) && !controller.isGrounded;
        animator.SetBool("IsJumping", isJumping);




        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);
        animator.SetBool("IsSprinting", isSprinting);

        // W, A, D tuşlarına basınca koşma animasyonu
        bool isRunningForward = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isSprinting;
        animator.SetBool("IsRunningForward", isRunningForward);


        

        // S tuşuna basınca geri koşma animasyonu
        bool isRunningBackward = Input.GetKey(KeyCode.S) && !isSprinting;
        animator.SetBool("IsRunningBackward", isRunningBackward);
    }
}