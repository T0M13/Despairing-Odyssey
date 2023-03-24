using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EffectBehaviour", menuName = "Behaviours/EffectBehaviour")]
public class EffectComponent : ScriptableObject, EffectBehaviour
{
    public GameObject effectGameObject;
    public GameObject effectClone;
    public ParticleSystem particleSystemOfEffect;
    public float disposeTime;
    public bool startOnAwake = false;

    private void Awake()
    {
        if (effectGameObject != null)
            particleSystemOfEffect = effectGameObject.GetComponent<ParticleSystem>();
        effectClone = null;

    }

    public void SpawnEffectWithDispose(Vector3 position, float disposeTime)
    {
        GameObject effectClone = Instantiate(effectGameObject, position, Quaternion.identity);
        effectClone.AddComponent<Disposer>();
        Disposer disposer = effectClone.GetComponent<Disposer>();
        disposer.StartCoroutine(disposer.Dispose(disposeTime));

    }


    public void SpawnEffectClone(Vector3 position)
    {
        if (effectClone == null)
        {
            effectClone = Instantiate(effectGameObject, position, Quaternion.identity);
            effectClone.transform.rotation = effectGameObject.transform.rotation;
        }

    }

    public void DeactivateEffectClone()
    {
        effectClone.SetActive(false);
    }

    public void SpawnEffectWithPosition(Vector3 position)
    {
        //throw new System.NotImplementedException();
    }

    public void DestroyEffectClone()
    {
        //throw new System.NotImplementedException();
    }

    public void ActivateEffectClone()
    {
        effectClone.SetActive(true);
    }
}
