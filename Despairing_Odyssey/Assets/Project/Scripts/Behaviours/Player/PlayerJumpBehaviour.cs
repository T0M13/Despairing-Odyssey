
using UnityEngine;
public interface PlayerJumpBehaviour
{
    public void Jump(Rigidbody rb, float jumpInput);
    public void JumpWithAnimation(Rigidbody rb, float jumpInput, Animator anim);
}
