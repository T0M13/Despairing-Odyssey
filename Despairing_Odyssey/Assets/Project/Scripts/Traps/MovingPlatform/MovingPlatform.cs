using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private WaypointPath _waypointPath;

    [SerializeField]
    private float _speed;

    private int _targetWaypointIndex;

    private Transform _previousWaypoint;
    private Transform _targetWaypoint;

    private float _timeToWaypoint;
    private float _elapsedTime;

    private PlayerController player;

    void Start()
    {
        TargetNextWaypoint();
    }

    private void TargetNextWaypoint()
    {
        _previousWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);
        _targetWaypointIndex = _waypointPath.GetNextWaypointIndex(_targetWaypointIndex);
        _targetWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _speed;
    }

    
    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }



        if (player == null) return;
        if (player.IsRagdoll || (player.IsRagdoll && player.IsDead))
        {
            Unparent(player.transform);
        }

    }

    private void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<PlayerController>())
        {
            player = col.GetComponent<PlayerController>();


            player.transform.parent = transform;
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<PlayerController>())
        {
            Unparent(col.transform);
        }
    }

    private void Unparent(Transform transform)
    {
        transform.parent = null;
    }
}
