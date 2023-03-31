using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("References")]
    [SerializeField] private Transform followTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Transform playerMeshRoot;
    [SerializeField] private SkinnedMeshRenderer playerMesh;
    [SerializeField] private CinemachineVirtualCamera playerThirdPersonCamera;
    [SerializeField] private CinemachineVirtualCamera playerRigidbodyCamera;
    [Header("Move Settings")]
    [SerializeField] private PlayerMoveComponent moveBehaviour;
    [SerializeField] private PlayerLookComponent lookBehaviour;
    [SerializeField] private PlayerJumpComponent jumpBehaviour;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 lookInput;
    [SerializeField] private GameObject StepRayUpper;
    [SerializeField] private GameObject StepRayLower;
    [Header("Jump Settings")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private float jumpInput;
    [Header("Other Settings")]
    [SerializeField] private bool canUseLogic = true;
    [SerializeField] private bool isDead = false;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 savedPosition;
    [SerializeField] private bool savedPositionSaved = false;
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private float lastPositionUpdateTime = 3f;
    [SerializeField] private float lastPositionTimer = 3f;
    [SerializeField] private float reviveTime = 2f;
    [SerializeField] private float despawnRagdollCloneTime = 15f;
    [SerializeField] private float restartInput;
    private Quaternion zeroQuaternion = new Quaternion(0, 0, 0, 0);
    [Header("Inventory Settings")]
    public PlayerInvetoryComponent inventoryBehaviour;
    [SerializeField] private GameObject[] visualInventoryHealthPoints;
    [SerializeField] private GameObject[] visualInventorySaveItems;
    [Header("Ragdoll Settings")]
    [SerializeField] private bool isRagdoll = false;
    [SerializeField] private bool canRagdoll = true;
    [SerializeField] private float ragdollInput;
    [SerializeField] private float impactForce = -10f;
    [SerializeField] private bool canHaveImpact = false;
    [SerializeField] private Rigidbody[] ragdollbodyParts;
    [SerializeField] private Collider[] ragdollColliders;
    [Header("Animation Settings")]
    private int moveInputY_A;
    private int moveInputX_A;
    private int isMoving_A;

    public Vector3 SavedPosition { get => savedPosition; set => savedPosition = value; }
    public bool SavedPositionSaved { get => savedPositionSaved; set => savedPositionSaved = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsRagdoll { get => isRagdoll; set => isRagdoll = value; }
    public Vector2 MoveInput { get => moveInput; set => moveInput = value; }
    public PlayerJumpComponent JumpBehaviour { get => jumpBehaviour; set => jumpBehaviour = value; }
    public Animator PlayerAnim { get => playerAnim; set => playerAnim = value; }
    public Rigidbody PlayerRigidbody { get => playerRigidbody; set => playerRigidbody = value; }

    public event Action OnDeath;

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jumpInput = value.Get<float>();
    }

    public void OnRagdoll(InputValue value)
    {
        ragdollInput = value.Get<float>();
    }

    public void OnRestart(InputValue value)
    {
        restartInput = value.Get<float>();
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

        GetRigidbody();
        GetCapsuleCollider();
        GetAnimator();
    }
    private void Start()
    {
        SetAnimationHash();

        GetRagdoll();
        RagdollOff();

        UpdateLastPosition();

        InitiateInventory();

        SetStartPosition();

        SetStepUpHeight();

    }


    private void OnValidate()
    {
        GetRigidbody();
        GetCapsuleCollider();
        GetAnimator();
    }

    private void Update()
    {
        UpdateLastPosition();
        Ragdoll();
        Restart();

        CheckMeshRotation();
        CheckInAir();
        CheckImpact();
        CheckMovementMagnitude();

        PlayerAnim.transform.localPosition = Vector3.zero;
        if (!canUseLogic) return;
        Look();

    }

    private void SetStepUpHeight()
    {
        moveBehaviour.SetStepUpHeight(StepRayUpper);
    }

    private void CheckMovementMagnitude()
    {
        moveBehaviour.ClampMovementMagnitude(PlayerRigidbody);
    }

    private void CheckImpact()
    {
        if (!canHaveImpact) return;
        if (PlayerRigidbody.velocity.y <= impactForce && JumpBehaviour.IsGrounded)
        {
            RagdollOn();
        }
    }

    private void CheckMeshRotation()
    {
        if (PlayerAnim.transform.rotation != zeroQuaternion)
        {
            PlayerAnim.transform.rotation = zeroQuaternion;
        }
    }

    private void SetStartPosition()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        StepUp();
        if (!canUseLogic) return;
        Move();
        Jump();

    }


    /// <summary>
    /// Gets the Rigidbody Component
    /// </summary>
    private void GetRigidbody()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Gets the Animator Component
    /// </summary>
    private void GetAnimator()
    {
        PlayerAnim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    /// <summary>
    /// Gets the CapsuleCollider Component
    /// </summary>
    private void GetCapsuleCollider()
    {
        playerCollider = GetComponent<CapsuleCollider>();
    }

    private void SetAnimationHash()
    {
        moveInputY_A = Animator.StringToHash("moveInputY");
        moveInputX_A = Animator.StringToHash("moveInputX");
        isMoving_A = Animator.StringToHash("isMoving");
    }


    private void StepUp()
    {
        moveBehaviour.StepUp(transform, PlayerRigidbody, StepRayLower, StepRayUpper);
    }

    /// <summary>
    /// Moves the Player and Camera
    /// </summary>
    private void Move()
    {
        if (canMove)
        {
            moveBehaviour.Move(transform, PlayerRigidbody, followTransform, MoveInput, lookBehaviour.angles);
            if (MoveInput.x != 0 || MoveInput.y != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            UpdateMoveAnimation();
        }
    }

    private void Look()
    {
        lookBehaviour.Look(transform, followTransform, lookInput);
    }

    private void UpdateMoveAnimation()
    {
        PlayerAnim.SetBool(isMoving_A, isMoving);
        PlayerAnim.SetFloat(moveInputX_A, MoveInput.x);
        PlayerAnim.SetFloat(moveInputY_A, MoveInput.y);
    }

    /// <summary>
    /// Makes Player Jump
    /// </summary>
    private void Jump()
    {
        if (canJump)
        {
            JumpBehaviour.JumpWithAnimation(PlayerRigidbody, jumpInput, PlayerAnim);
            jumpInput = 0;
        }
    }

    private void CheckInAir()
    {
        if (!JumpBehaviour.IsGrounded)
        {
            //moveBehaviour.moveSpeed = moveBehaviour.MoveSpeedInAir;
            //playerRigidbody.interpolation = RigidbodyInterpolation.None;
        }
        else
        {
            //moveBehaviour.moveSpeed = moveBehaviour.MoveSpeedNormal;
            //playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    private void Ragdoll()
    {
        if (ragdollInput > 0)
        {
            ToggleRagdoll();
            ragdollInput = 0;
        }
    }

    private void Restart()
    {
        if (restartInput > 0)
        {
            ReviveWithPosition(startPosition, false);
            restartInput = 0;
        }
    }

    private void GetRagdollbodyParts()
    {
        ragdollbodyParts = playerMeshRoot.GetComponentsInChildren<Rigidbody>();
    }

    private void GetRagdollColliders()
    {
        ragdollColliders = playerMeshRoot.GetComponentsInChildren<Collider>();
    }

    private void GetRagdoll()
    {
        GetRagdollbodyParts();
        GetRagdollColliders();
    }

    public void RagdollOff()
    {
        if (!canRagdoll) return;
        foreach (Rigidbody rigidbody in ragdollbodyParts)
        {
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        }
        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = false;
        }

        PlayerAnim.enabled = true;
        IsRagdoll = false;
        canUseLogic = true;
        PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        playerCollider.enabled = true;
        PlayerRigidbody.isKinematic = false;

        SwitchCameras();
    }

    public void RagdollOn()
    {
        if (!canRagdoll) return;
        foreach (Rigidbody rigidbody in ragdollbodyParts)
        {
            rigidbody.isKinematic = false;
            rigidbody.interpolation = RigidbodyInterpolation.None;
        }
        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }

        PlayerAnim.enabled = false;
        IsRagdoll = true;
        canUseLogic = false;
        PlayerRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        playerCollider.enabled = false;
        PlayerRigidbody.isKinematic = true;

        SwitchCameras();
    }

    public void ToggleRagdoll()
    {

        if (IsRagdoll)
            RagdollOff();
        else
            RagdollOn();
    }

    public void SetDead()
    {
        IsDead = true;
        OnDeath.Invoke();
        RagdollOn();
        if (HasHealthPoints())
        {
            StartCoroutine(ReviveWithTimeAndItem(InventoryItemType.HealthPoint, 1, lastPosition, true));
        }
        else if (savedPositionSaved)
        {
            StartCoroutine(ReviveWithTimeAndItem(InventoryItemType.SaveItem, 1, savedPosition, false));
        }
        else
        {
            if (GameManager.instance == null) return;
            GameManager.instance.GameOver();
        }
    }

    public IEnumerator ReviveWithTimeAndItem(InventoryItemType itemType, int amount, Vector3 position, bool withHealthItem)
    {
        yield return new WaitForSeconds(reviveTime);
        if (withHealthItem)
            ReviveWithItem(itemType, amount, position);
        else
            ReviveWithPosition(position, true);
    }

    public IEnumerator ReviveWithTime(Vector3 position)
    {
        yield return new WaitForSeconds(reviveTime);
        ReviveWithPosition(position, false);
    }

    public void Revive()
    {
        RagdollOff();
        IsDead = false;
    }

    public void ReviveWithPosition(Vector3 position, bool useSaveItem)
    {
        SpawnRagdollClone();
        UpdateInventory();
        RagdollOff();
        IsDead = false;
        transform.position = position;
        if (useSaveItem)
            savedPositionSaved = false;
    }

    public void ReviveWithItem(InventoryItemType itemType, int amount, Vector3 position)
    {
        SpawnRagdollClone();
        inventoryBehaviour.RemoveItem(itemType, amount);
        UpdateInventory();
        RagdollOff();
        IsDead = false;
        transform.position = position;
    }

    public void SpawnRagdollClone()
    {
        GameObject ragdollClone = Instantiate(PlayerAnim.gameObject, PlayerAnim.transform.position, PlayerAnim.transform.rotation);
        StartCoroutine(DespawnRagdollClone(ragdollClone));

    }

    private IEnumerator DespawnRagdollClone(GameObject ragdollClone)
    {
        yield return new WaitForSeconds(despawnRagdollCloneTime);
        Destroy(ragdollClone);
    }

    private void SwitchCameras()
    {
        if (IsRagdoll || (IsRagdoll && IsDead))
        {
            playerThirdPersonCamera.enabled = false;
            playerRigidbodyCamera.enabled = true;
        }
        else
        {
            playerThirdPersonCamera.enabled = true;
            playerRigidbodyCamera.enabled = false;
        }
    }

    private bool HasHealthPoints()
    {
        if (inventoryBehaviour.inventoryItemSlots.Contains(InventoryItemType.HealthPoint))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void UpdateInventory()
    {
        for (int i = 0; i < inventoryBehaviour.inventoryItemSlots.Length; i++)
        {
            switch (inventoryBehaviour.inventoryItemSlots[i])
            {
                case InventoryItemType.None:
                    visualInventoryHealthPoints[i].SetActive(false);
                    visualInventorySaveItems[i].SetActive(false);
                    break;
                case InventoryItemType.HealthPoint:
                    visualInventoryHealthPoints[i].SetActive(true);
                    visualInventorySaveItems[i].SetActive(false);
                    break;
                case InventoryItemType.SaveItem:
                    visualInventoryHealthPoints[i].SetActive(false);
                    visualInventorySaveItems[i].SetActive(true);
                    break;
                default:
                    break;
            }
        }

    }

    public void InitiateInventory()
    {
        inventoryBehaviour.InitiateInventory();
        UpdateInventory();
    }

    public void ResetInventory()
    {
        inventoryBehaviour.RemoveAll();
        UpdateInventory();
    }

    public void ResetPlayer()
    {
        GetRagdoll();
        StartCoroutine(ReviveWithTime(startPosition));
        InitiateInventory();

    }

    private void UpdateLastPosition()
    {
        if (IsDead || !JumpBehaviour.IsGrounded) return;
        lastPositionTimer -= Time.unscaledDeltaTime;
        if (lastPositionTimer <= 0)
        {
            lastPositionTimer = lastPositionUpdateTime;
            lastPosition = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        JumpBehaviour.DrawGizmos(PlayerRigidbody);
    }

}
