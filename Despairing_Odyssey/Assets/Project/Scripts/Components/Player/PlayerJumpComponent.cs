using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerJumpBehaviour", menuName = "Behaviours/PlayerJumpBehaviour")]
public class PlayerJumpComponent : ScriptableObject, PlayerJumpBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] float jumpForce;
    [SerializeField] int maxJumps = 1;
    [SerializeField] int jumpsLeft;
    [SerializeField] bool isJumping;
    [Header("Ground Check")]
    [SerializeField] bool checkGrounded = true;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isFalling;
    [SerializeField] float groundDetectionRange = 2;
    [SerializeField] LayerMask groundDetectionLayer;
    [SerializeField] Vector3 groundDetectionOffset = new Vector3(0, -0.3f, 0);
    [SerializeField] RaycastHit rayHit;
    [Header("Force Mode")]
    [SerializeField] ForceMode jumpForceMode = ForceMode.Impulse;
    [Header("Animation Settings")]
    [SerializeField] AnimationClip jumpClip;
    private int jumpInput_A;
    private int fallingInput_A;

    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }

    private void Awake()
    {
        jumpInput_A = Animator.StringToHash("isJumping");
        fallingInput_A = Animator.StringToHash("isFalling");
    }
    public void Jump(Rigidbody rb, float jumpInput)
    {
        //    if (checkGrounded)
        //    {
        //        if (IsPlayerGrounded(rb) && rb.velocity.y <= 0)
        //        {
        //            ResetJumps();
        //        }
        //    }

        //    if (jumpInput == 1 && jumpsLeft > 0)
        //    {
        //        jumpsLeft -= 1;
        //        rb.AddForce(Vector2.up * jumpForce, jumpForceMode);
        //    }
    }

public void JumpWithAnimation(Rigidbody rb, float jumpInput, Animator anim)
    {
        if (checkGrounded)
        {
            if (IsPlayerGrounded(rb) && rb.velocity.y <= 0)
            {
                isFalling = false;
                isJumping = false;
                ResetJumps();
                anim.SetBool(jumpInput_A, isJumping);
                anim.SetBool(fallingInput_A, isFalling);
            }
            if (!IsPlayerGrounded(rb) && rb.velocity.y < 0)
            {
                isFalling = true;
                isJumping = false;
                anim.SetBool(jumpInput_A, isJumping);
                anim.SetBool(fallingInput_A, isFalling);
            }
        }

        if (jumpInput == 1 && jumpsLeft > 0)
        {

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            jumpsLeft -= 1;
            rb.AddForce(Vector2.up * jumpForce, jumpForceMode);
            if (isJumping)
                anim.Play(jumpClip.name, -1, 0f);
            isJumping = true;
            anim.SetBool(jumpInput_A, isJumping);
        }
    }

    private bool IsPlayerGrounded(Rigidbody rb)
    {
        if (Physics.Raycast(rb.transform.position + groundDetectionOffset, Vector2.down * groundDetectionRange, out rayHit, groundDetectionRange, groundDetectionLayer))
            IsGrounded = true;
        else IsGrounded = false;

        return IsGrounded;
    }

    private void ResetJumps()
    {
        jumpsLeft = maxJumps;
    }

    public void DrawGizmos(Rigidbody rb)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(rb.transform.position + groundDetectionOffset, Vector2.down * groundDetectionRange);
    }


}
