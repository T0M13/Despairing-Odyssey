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
    [Header("Ground Check")]
    [SerializeField] bool checkGrounded = true;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundDetectionRange = 2;
    [SerializeField] LayerMask groundDetectionLayer;
    [SerializeField] Vector3 groundDetectionOffset = new Vector3(0, -0.3f, 0);
    [SerializeField] RaycastHit rayHit;
    [Header("Force Mode")]
    [SerializeField] ForceMode jumpForceMode = ForceMode.Impulse;

    public void Jump(Rigidbody rb, float jumpInput)
    {
        if (checkGrounded)
        {
            if (IsGrounded(rb) && rb.velocity.y <= 0)
            {
                ResetJumps();
            }
        }

        if (jumpInput == 1 && jumpsLeft > 0)
        {
            jumpsLeft -= 1;
            rb.AddForce(Vector2.up * jumpForce, jumpForceMode);
        }
    }

    private bool IsGrounded(Rigidbody rb)
    {
        if (Physics.Raycast(rb.transform.position + groundDetectionOffset, Vector2.down * groundDetectionRange, out rayHit, groundDetectionRange, groundDetectionLayer))
            isGrounded = true;
        else isGrounded = false;

        return isGrounded;
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
