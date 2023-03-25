using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapController : MonoBehaviour
{

    [SerializeField] private ObjectPool arrowPool;

    public List<GameObject> spawnedArrows;
    [SerializeField] private float delayAndSpawnRate = 2f;
    [SerializeField] private float arrowSpeed = 7f;
    [SerializeField] Vector3 spawnPosition = new Vector3();

    private void Start()
    {
        GetObjectPool();
        StartSpawning();
    }

    private void OnValidate()
    {
        GetObjectPool();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnObject(delayAndSpawnRate));
    }

    private IEnumerator SpawnObject(float firstDelay)
    {
        float spawnCountdown = firstDelay;

        while (true)
        {
            yield return null;

            spawnCountdown -= Time.deltaTime;

            if (spawnCountdown < 0)
            {
                spawnCountdown += delayAndSpawnRate;

                GameObject arrowClone = arrowPool.GetAPooledObject();
                if (arrowClone != null)
                {
                    spawnedArrows.Add(arrowClone);

                    arrowClone.transform.localPosition = spawnPosition;
                    arrowClone.transform.localRotation = new Quaternion(0,180,0,0);

                    Rigidbody arrowRigid = arrowClone.GetComponent<Rigidbody>();
                    arrowRigid.velocity = arrowRigid.transform.forward * arrowSpeed;

                    arrowClone.SetActive(true);

                    Disposer arrowDisposer = arrowClone.GetComponent<Disposer>();
                    arrowDisposer.StartCoroutine(arrowDisposer.DeactivateFromList(3f, true, spawnedArrows));
                }

            }
        }
    }

    private void GetObjectPool()
    {
        arrowPool = GetComponent<ObjectPool>();
    }
}
