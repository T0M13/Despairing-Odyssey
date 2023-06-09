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
    [SerializeField] Vector3 groundDetectionOffset_Left;
    [SerializeField] Vector3 groundDetectionOffset_Right;
    [SerializeField] Vector3 groundDetectionOffset_Forward;
    [SerializeField] Vector3 groundDetectionOffset_Backward;
    [SerializeField] RaycastHit rayHit;
    [Header("Force Mode")]
    [SerializeField] ForceMode jumpForceMode = ForceMode.Impulse;
    [Header("Animation Settings")]
    [SerializeField] AnimationClip jumpClip;
    private int jumpInput_A;
    private int fallingInput_A;
    [Header("Other Settings")]
    [SerializeField] bool showGizmos;


    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }

    private void Awake()
    {
        jumpInput_A = Animator.StringToHash("isJumping");
        fallingInput_A = Animator.StringToHash("isFalling");
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

    public void PlayJumpAnimation(Animator anim)
    {
        isJumping = true;
        anim.SetBool(jumpInput_A, isJumping);
    }

    private bool IsPlayerGrounded(Rigidbody rb)
    {
        if (RayCastWithOffset(rb, Vector3.zero) || RayCastWithOffset(rb, groundDetectionOffset_Left) || RayCastWithOffset(rb, groundDetectionOffset_Right) || RayCastWithOffset(rb, groundDetectionOffset_Forward) || RayCastWithOffset(rb, groundDetectionOffset_Backward))
            IsGrounded = true;
        else IsGrounded = false;

        return IsGrounded;
    }

    private bool RayCastWithOffset(Rigidbody rb, Vector3 offset)
    {
        return Physics.Raycast(rb.transform.position + groundDetectionOffset + offset, Vector2.down * groundDetectionRange, out rayHit, groundDetectionRange, groundDetectionLayer);
    }

    private void ResetJumps()
    {
        jumpsLeft = maxJumps;
    }

    public void DrawGizmos(Rigidbody rb)
    {
        if (!showGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(rb.transform.position + groundDetectionOffset, Vector2.down * groundDetectionRange);
        Gizmos.DrawRay(rb.transform.position + groundDetectionOffset + groundDetectionOffset_Left, Vector2.down * groundDetectionRange);
        Gizmos.DrawRay(rb.transform.position + groundDetectionOffset + groundDetectionOffset_Right, Vector2.down * groundDetectionRange);
        Gizmos.DrawRay(rb.transform.position + groundDetectionOffset + groundDetectionOffset_Forward, Vector2.down * groundDetectionRange);
        Gizmos.DrawRay(rb.transform.position + groundDetectionOffset + groundDetectionOffset_Backward, Vector2.down * groundDetectionRange);
    }


}
