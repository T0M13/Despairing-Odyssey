
using UnityEngine;
public interface PlayerMoveBehaviour
{
    public void Move(Transform playerTransform, Transform followTransform, Vector2 lookInput, Vector2 moveInput);
}
