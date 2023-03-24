using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerMoveBehaviour", menuName = "Behaviours/PlayerMoveBehaviour")]
public class PlayerMoveComponent : ScriptableObject, PlayerMoveBehaviour
{

    [Header("Stats")]
    [SerializeField] Vector3 nextPosition;

    public float moveSpeed = 1f;
    //public AnimationCurve moveCurve;


    public void Move(Transform playerTransform, Rigidbody playerRigid, Transform followTransform, Vector2 moveInput, Vector3 angles)
    {

        if (moveInput.x == 0 && moveInput.y == 0)
        {
            nextPosition = playerTransform.position;
            return;
        }

        float moveSpeed = this.moveSpeed / 100f;
        Vector3 position = (playerTransform.forward * moveInput.y * moveSpeed) + (playerTransform.right * moveInput.x * moveSpeed);
        nextPosition = playerTransform.position + position;

        //Set the player rotation based on the look transform
        playerTransform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);

        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);


        playerRigid.MovePosition(nextPosition);
        //playerTransform.position = nextPosition;

        //Maybe use velocity ? (jittering)
        //playerTransform.GetComponent<Rigidbody>().velocity = nextPosition * moveSpeed;
    }

}