
using UnityEngine;
public interface PlayerMoveBehaviour
{
    public void Move(Transform playerTransform, Rigidbody playerRigid, Transform followTransform, Vector2 moveInput, Vector3 angles);
}
