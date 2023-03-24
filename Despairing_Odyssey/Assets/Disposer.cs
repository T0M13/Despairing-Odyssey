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

}
