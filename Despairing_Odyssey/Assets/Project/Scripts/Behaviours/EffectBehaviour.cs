using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EffectBehaviour
{
    public void SpawnEffectWithDispose(Vector3 position, float disposeTime);
    public void SpawnEffectWithPosition(Vector3 position);
    public void SpawnEffectClone(Vector3 position);
    public void DestroyEffectClone();
    public void DeactivateEffectClone();
    public void ActivateEffectClone();

}
