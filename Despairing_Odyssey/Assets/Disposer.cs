using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disposer : MonoBehaviour
{

    //[SerializeField] private bool dispose = true;

    public IEnumerator Dispose(float disposeTime)
    {
        yield return new WaitForSeconds(disposeTime);
        Destroy(gameObject);
    }

    public IEnumerator Deactivate(float disposeTime)
    {
        yield return new WaitForSeconds(disposeTime);
        gameObject.SetActive(false);

    }

    public IEnumerator DeactivateFromList(float disposeTime, bool removeFromList, List<GameObject> list)
    {
        yield return new WaitForSeconds(disposeTime);
        gameObject.SetActive(false);
        if (removeFromList)
            RemoveFromList(list);

    }

    public void RemoveFromList(List<GameObject> list)
    {
        list.Remove(gameObject);
    }

}
