using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerLookBehaviour", menuName = "Behaviours/PlayerLookBehaviour")]
public class PlayerLookComponent : ScriptableObject, PlayerLookBehaviour
{
    [Header("Camera Inverting Settings")]
    [SerializeField] bool invertHorizontalRotation = false;
    [SerializeField] bool invertVerticalRotation = false;

    [Header("Camera Rotation Settings")]
    [SerializeField] float rotationPower = 0.2f;
    [SerializeField] float rotationLerp = 0.5f;

    [SerializeField] Quaternion nextRotation;
    public Vector3 angles;


    public void Look(Transform playerTransform, Transform followTransform, Vector2 lookInput)
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

        angles = followTransform.transform.localEulerAngles;
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


    }
}