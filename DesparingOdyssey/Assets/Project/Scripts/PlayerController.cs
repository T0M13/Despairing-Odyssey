using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Linq;

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
    [SerializeField] private PlayerJumpComponent jumpBehaviour;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 lookInput;
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
    [Header("Inventory Settings")]
    public PlayerInvetoryComponent inventoryBehaviour;
    [SerializeField] private GameObject[] visualInventoryHealthPoints;
    [SerializeField] private GameObject[] visualInventorySaveItems;
    [Header("Ragdoll Settings")]
    [SerializeField] private bool isRagdoll = false;
    [SerializeField] private bool canRagdoll = true;
    [SerializeField] private float ragdollInput;
    [SerializeField] private Rigidbody[] ragdollbodyParts;
    [SerializeField] private Collider[] ragdollColliders;
    [Header("Animation Settings")]
    private int moveInputY_A;
    private int moveInputX_A;
    private int isMoving_A;

    public Vector3 SavedPosition { get => savedPosition; set => savedPosition = value; }
    public bool SavedPositionSaved { get => savedPositionSaved; set => savedPositionSaved = value; }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
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

        LockHideCursor();
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
        if (!canUseLogic) return;
        Move(); //--> move into fixed and look here
    }

    private void FixedUpdate()
    {
        if (!canUseLogic) return;
        Jump();
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
    /// Gets the Animator Component
    /// </summary>
    private void GetAnimator()
    {
        playerAnim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
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

    /// <summary>
    /// Moves the Player and Camera
    /// </summary>
    private void Move()
    {
        if (canMove)
        {
            moveBehaviour.Move(transform, followTransform, lookInput, moveInput);
            if (moveInput.x != 0 || moveInput.y != 0)
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

    private void UpdateMoveAnimation()
    {
        playerAnim.SetBool(isMoving_A, isMoving);
        playerAnim.SetFloat(moveInputX_A, moveInput.x);
        playerAnim.SetFloat(moveInputY_A, moveInput.y);
    }

    /// <summary>
    /// Makes Player Jump
    /// </summary>
    private void Jump()
    {
        if (canJump)
        {
            jumpBehaviour.JumpWithAnimation(playerRigidbody, jumpInput, playerAnim);
            jumpInput = 0;
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
        }
        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = false;
        }

        playerAnim.enabled = true;
        isRagdoll = false;
        canUseLogic = true;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        playerAnim.transform.rotation = new Quaternion(0, 0, 0, 0);
        playerCollider.enabled = true;
        playerRigidbody.isKinematic = false;

        SwitchCameras();
    }

    public void RagdollOn()
    {
        if (!canRagdoll) return;
        foreach (Rigidbody rigidbody in ragdollbodyParts)
        {
            rigidbody.isKinematic = false;
        }
        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }

        playerAnim.enabled = false;
        isRagdoll = true;
        canUseLogic = false;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        playerCollider.enabled = false;
        playerRigidbody.isKinematic = true;

        SwitchCameras();
    }

    public void ToggleRagdoll()
    {

        if (isRagdoll)
            RagdollOff();
        else
            RagdollOn();
    }

    public void SetDead()
    {
        isDead = true;
            RagdollOn();
        if (HasHealthPoints())
        {
            StartCoroutine(ReviveWithTime(InventoryItemType.HealthPoint, 1, lastPosition, true));
        }
        else if (savedPositionSaved)
        {
            StartCoroutine(ReviveWithTime(InventoryItemType.SaveItem, 1, savedPosition, false));
        }
        else
        {
            if (GameManager.instance == null) return;
            GameManager.instance.GameOver();
        }
    }

    public IEnumerator ReviveWithTime(InventoryItemType itemType, int amount, Vector3 position, bool withHealthItem)
    {
        yield return new WaitForSeconds(reviveTime);
        if (withHealthItem)
            ReviveWithItem(itemType, amount, position);
        else
            ReviveWithPosition(position, true);
    }

    public void Revive()
    {
        RagdollOff();
        isDead = false;
    }

    public void ReviveWithPosition(Vector3 position, bool useSaveItem)
    {
        SpawnRagdollClone();
        UpdateInventory();
        RagdollOff();
        isDead = false;
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
        isDead = false;
        transform.position = position;
    }

    public void SpawnRagdollClone()
    {
        GameObject ragdollClone = Instantiate(playerAnim.gameObject, playerAnim.transform.position, playerAnim.transform.rotation);
        StartCoroutine(DespawnRagdollClone(ragdollClone));
    }

    private IEnumerator DespawnRagdollClone(GameObject ragdollClone)
    {
        yield return new WaitForSeconds(despawnRagdollCloneTime);
        Destroy(ragdollClone);
    }

    private void SwitchCameras()
    {
        if (isRagdoll || (isRagdoll && isDead))
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

    private void UpdateLastPosition()
    {
        if (isDead || !jumpBehaviour.IsGrounded) return;
        lastPositionTimer -= Time.deltaTime;
        if (lastPositionTimer <= 0)
        {
            lastPositionTimer = lastPositionUpdateTime;
            lastPosition = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        jumpBehaviour.DrawGizmos(playerRigidbody);
    }

}
