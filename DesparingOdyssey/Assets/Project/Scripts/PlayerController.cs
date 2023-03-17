using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("References")]
    [SerializeField] private Transform followTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private CapsuleCollider playerCollider;
    [Header("Move Settings")]
    [SerializeField] private PlayerMoveComponent moveBehaviour;
    [SerializeField] private bool canMove = true;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 lookInput;
    [Header("Other Settings")]
    [SerializeField] private bool canUseLogic = true;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LockHideCursor();
        GetRigidbody();
        GetCapsuleCollider();
    }

    private void OnValidate()
    {
        GetRigidbody();
        GetCapsuleCollider();
    }

    private void Update()
    {
        if (!canUseLogic) return;
        Move();
    }

    /// <summary>
    /// Locks and Hides the Cursor
    /// </summary>
    private void LockHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Unlocks and Shows the Cursor
    /// </summary>
    private void UnlockShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Gets the Rigidbody Component
    /// </summary>
    private void GetRigidbody()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Gets the CapsuleCollider Component
    /// </summary>
    private void GetCapsuleCollider()
    {
        playerCollider = GetComponent<CapsuleCollider>();
    }

    /// <summary>
    /// Moves the Player and Camera
    /// </summary>
    private void Move()
    {
        if (canMove)
            moveBehaviour.Move(transform, followTransform, lookInput, moveInput);
    }
}
