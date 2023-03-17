using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerMoveBehaviour", menuName = "Behaviours/PlayerMoveBehaviour")]
public class PlayerMoveComponent : ScriptableObject, PlayerMoveBehaviour
{
    [Header("Camera Inverting Settings")]
    public bool invertHorizontalRotation = false;
    public bool invertVerticalRotation = false;

    [Header("Camera Rotation Settings")]
    public float rotationPower = 0.2f;
    public float rotationLerp = 0.5f;

    [Header("Stats")]
    public Vector3 nextPosition;
    public Quaternion nextRotation;

    public float moveSpeed = 1f;


    public void Move(Transform playerTransform, Transform followTransform, Vector2 lookInput, Vector2 moveInput)
    {
        #region Player Based Rotation

        //Move the player based on the X input on the controller
        //transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        #endregion

        #region Follow Transform Rotation

        //Rotate the Follow Target transform based on the input
        if (invertHorizontalRotation)
            followTransform.transform.rotation *= Quaternion.AngleAxis(-lookInput.x * rotationPower, Vector3.up);
        else
            followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.x * rotationPower, Vector3.up);

        #endregion

        #region Vertical Rotation
        if (invertVerticalRotation)
            followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.y * rotationPower, Vector3.right);
        else
            followTransform.transform.rotation *= Quaternion.AngleAxis(-lookInput.y * rotationPower, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        followTransform.transform.localEulerAngles = angles;
        #endregion


        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

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

        playerTransform.position = nextPosition;
    }
}