using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private int amountToPool;
    [SerializeField] private List<GameObject> pooledObjects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject clone = Instantiate(prefab, transform);
            clone.SetActive(false);
            pooledObjects.Add(clone);
        }
    }

    public GameObject GetAPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

}
