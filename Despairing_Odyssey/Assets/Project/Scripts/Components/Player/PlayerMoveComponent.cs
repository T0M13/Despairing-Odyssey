using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerMoveBehaviour", menuName = "Behaviours/PlayerMoveBehaviour")]
public class PlayerMoveComponent : ScriptableObject, PlayerMoveBehaviour
{

    [Header("Stats")]
    [SerializeField] Vector3 nextPosition;

    public float moveSpeed = 700f;
    public float maxMagnitude = 10f;
    public bool hasMaxMagnitude = true;
    public Vector3 velocity;

    public float stepHeight = 0.3f;
    public float stepSmooth = 0.1f;
    public bool canSlide = false;


    public void Move(Transform playerTransform, Rigidbody playerRigid, Transform followTransform, Vector2 moveInput, Vector3 angles)
    {

        if (moveInput.x == 0 && moveInput.y == 0)
        {
            nextPosition = playerTransform.position;
            if (!canSlide)
            {
                velocity = Vector3.zero + (playerTransform.up * playerRigid.velocity.y);
                playerRigid.velocity = velocity;
            }
            return;
        }

        float moveSpeed = this.moveSpeed / 100f;
        Vector3 position = (playerTransform.forward * moveInput.y * moveSpeed) + (playerTransform.right * moveInput.x * moveSpeed);
        nextPosition = playerTransform.position + position;

        //Set the player rotation based on the look transform
        playerTransform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);

        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

        velocity = (playerTransform.forward * moveInput.y * moveSpeed) + (playerTransform.right * moveInput.x * moveSpeed) + (playerTransform.up * playerRigid.velocity.y);

        playerRigid.velocity = velocity;


        //playerRigid.AddForce(velocity, ForceMode.VelocityChange);

        //playerRigid.MovePosition(nextPosition);
        //playerTransform.position = nextPosition;

        //Maybe use velocity ? (jittering)

    }

    public void ClampMovementMagnitude(Rigidbody playerRigid)
    {
        if (!hasMaxMagnitude) return;

        if (playerRigid.velocity.magnitude > maxMagnitude)
        {
            playerRigid.velocity = Vector3.ClampMagnitude(playerRigid.velocity, maxMagnitude);
        }
    }

    public void SetStepUpHeight(GameObject stepUpper)
    {
        stepUpper.transform.position = new Vector3(stepUpper.transform.position.x, stepHeight, stepUpper.transform.position.z);
    }


    public void StepUp(Transform playerTransform, Rigidbody playerRigid, GameObject stepLower, GameObject stepUpper)
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepLower.transform.position, playerTransform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepUpper.transform.position, playerTransform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                playerRigid.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }


        RaycastHit hitLower45;
        if (Physics.Raycast(stepLower.transform.position, playerTransform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        {
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepUpper.transform.position, playerTransform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
            {
                playerRigid.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepLower.transform.position, playerTransform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
        {
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepUpper.transform.position, playerTransform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
            {
                playerRigid.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }


    }

}