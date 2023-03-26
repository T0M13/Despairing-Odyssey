using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Rigidbody rb = null;

    [SerializeField] bool rotateEnabled = true;
    [SerializeField] float rotationSpeed = 20.0f;

    [SerializeField] bool moveEnabled = true;
    [SerializeField] float moveSpeed = 1.0f;
    Vector3 startPosition = Vector3.zero;
    Vector3 endPosition = Vector3.zero;

    Vector3 platformPositionLastFrame = Vector3.zero;
    float timeScale = 0.0f;

    Dictionary<Rigidbody, float> RBsOnPlatformAndTime = new Dictionary<Rigidbody, float>();
    [SerializeField] List<Rigidbody> RBsOnPlatform = new List<Rigidbody>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        startPosition = rb.position;
        endPosition = new Vector3(startPosition.x + 3.0f, startPosition.y + 3.0f, startPosition.z);

    }

    private void FixedUpdate()
    {
        if (rotateEnabled)
        {
            rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles.x,
                rb.rotation.eulerAngles.y + rotationSpeed,
                rb.rotation.eulerAngles.z);

        }

        if (RBsOnPlatform.Count != RBsOnPlatformAndTime.Count)
        {
            RBsOnPlatformAndTime.Clear();
            foreach (Rigidbody rigid in RBsOnPlatform)
            {
                RBsOnPlatformAndTime.Add(rigid, 1.0f);

            }
        }

        if (moveEnabled)
        {
            platformPositionLastFrame = rb.position;
            timeScale = moveSpeed / Vector3.Distance(startPosition, endPosition);
            rb.position = Vector3.Lerp(endPosition, startPosition, Mathf.Abs(Time.time * timeScale % 2 - 1));
        }

        foreach (Rigidbody rigid in RBsOnPlatform)
        {
            RBsOnPlatformAndTime.TryGetValue(rigid, out float timer);
            if (timer < 1.0f)
            {
                RBsOnPlatformAndTime[rigid] += Time.deltaTime * 4.0f;
            }
            else if (timer > 1.0f)
            {
                RBsOnPlatformAndTime[rigid] = 1.0f;
            }
            RotateAndMoveRBOnPlatform(rigid, timer);
        }
    }

    private void RotateAndMoveRBOnPlatform(Rigidbody rigid, float timer)
    {
        if (rotateEnabled)
        {
            float rotationAmount = rotationSpeed * timer * Time.deltaTime;

            Quaternion localAngleAxis = Quaternion.AngleAxis(rotationAmount, rb.transform.up);
            rigid.position = (localAngleAxis * (rigid.position - rb.position)) + rb.position;

            Quaternion globalAngleAxis = Quaternion.AngleAxis(rotationAmount, rigid.transform.InverseTransformDirection(rb.transform.up));
            rigid.rotation *= globalAngleAxis;

        }

        if (moveEnabled)
        {
            rigid.position += (rb.position - platformPositionLastFrame) * timer;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.attachedRigidbody == null) && !(other.attachedRigidbody.isKinematic))
        {
            if (!(RBsOnPlatform.Contains(other.attachedRigidbody)))
            {
                RBsOnPlatform.Add(other.attachedRigidbody);
                RBsOnPlatformAndTime.Add(other.attachedRigidbody, 0.0f);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!(other.attachedRigidbody == null))
        {
            if (RBsOnPlatform.Contains(other.attachedRigidbody))
            {
                RBsOnPlatform.Remove(other.attachedRigidbody);
                RBsOnPlatformAndTime.Remove(other.attachedRigidbody);
            }
        }
    }
}
