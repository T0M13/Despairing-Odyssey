using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public InventoryItemType type;
    public int amount;

    [Header("Hover Effect")]
    [SerializeField] private Vector3 hoverUpPosition;
    [SerializeField] private Vector3 hoverPositionsOffset;
    [SerializeField] private Vector3 hoverDownPosition;
    [SerializeField] private bool hover = false;
    [SerializeField] private float hoverSpeed = 0.4f;
    [SerializeField] private AnimationCurve hoverCurve;
    [SerializeField] private float timer = 2f;
    [SerializeField] private EffectComponent effectBehaviour;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.inventoryBehaviour.inventoryFull) return;
            effectBehaviour.SpawnEffectWithDispose(transform.position, effectBehaviour.disposeTime);
            player.inventoryBehaviour.AddItem(type, amount);
            player.UpdateInventory();
            DeactivateItem();
        }
    }

    private void DeactivateItem()
    {
        gameObject.SetActive(false);
    }
    private void ActivateItem()
    {
        gameObject.SetActive(true);
    }

    private void Start()
    {
        if (!hover) return;
        SetPositions();
        StartCoroutine(HoverUp());
    }

    private void SetPositions()
    {
        hoverUpPosition = transform.position + hoverPositionsOffset;
        hoverDownPosition = transform.position - hoverPositionsOffset;
    }


    private IEnumerator HoverUp()
    {
        float cooldown = 0;

        while (cooldown < timer)
        {
            float t = cooldown / timer;

            transform.position = Vector3.Slerp(hoverUpPosition, hoverDownPosition, hoverCurve.Evaluate(hoverSpeed * t));

            cooldown += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(HoverDown());
    }

    private IEnumerator HoverDown()
    {
        float cooldown = 0;

        while (cooldown < timer)
        {
            float t = cooldown / timer;

            transform.position = Vector3.Slerp(hoverDownPosition, hoverUpPosition, hoverCurve.Evaluate(hoverSpeed * t));

            cooldown += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(HoverUp());
    }

}
