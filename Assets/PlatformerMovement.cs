using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 20f;

    private float horizontalMove = 0f;
    private bool jump = false;

    public ParticleSystem dust;


    public AudioSource soundPlayer;
    public AudioClip jumpSound;

    // Update is called once per frame
    void Update()
    { 

        // Get horizontal input
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Set the animator's speed parameter based on absolute horizontal movement
        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        // Check for jump input
        if (Input.GetButtonDown("Jump"))
        {
            //animator.SetBool("isJumping", true);
            if (controller.IsGrounded())
            {
                // If grounded, allow jump
                jump = true;
                dust.Play();
                soundPlayer.clip = jumpSound;
                soundPlayer.Play();
            }
        }

        if (controller.m_Rigidbody2D.velocity.y < 0 && !controller.IsGrounded())
        {
            animator.SetBool("postPeak", true);
        }
        if (controller.m_Rigidbody2D.velocity.y > 0 && !controller.IsGrounded())
        {
            animator.SetBool("isJumping", true);
        }


        Debug.Log(controller.IsGrounded());
        if (controller.IsGrounded())
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("postPeak", false);
        }




        // Move the character
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, Input.GetKeyDown(KeyCode.F));
        jump = false;
        //animator.SetBool("isJumping", false);

    }


    //void FixedUpdate()
    //{
    //    // Move the character
    //    controller.Move(horizontalMove * Time.fixedDeltaTime, jump, Input.GetKeyDown(KeyCode.F));
    //    jump = false;

    //}
}
