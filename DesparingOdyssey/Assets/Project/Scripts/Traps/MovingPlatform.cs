using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    public Transform[] waypoints;   // An array of game objects for the platform to move to
    public float speed = 2f;        // The speed at which the platform moves
    public bool loop = false;       // Whether or not the platform should loop back to the beginning
    [SerializeField]
    private int currentIndex = 0;   // The current waypoint index the platform is moving towards
    private bool movingForward = true;  // Whether or not the platform is moving forward

    private void Update()
    {
        // Get the position of the current waypoint
        Vector3 targetPosition = waypoints[currentIndex].position;

        // Move the platform towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the platform has reached the current waypoint
        if (transform.position == targetPosition)
        {
            // If the platform is moving forward, increment the current index
            // If the platform is moving backwards, decrement the current index
            currentIndex += movingForward ? 1 : -1;

            // Check if the platform has reached the last waypoint
            if (currentIndex == waypoints.Length - 1 || currentIndex == 0)
            {
                // If the platform is set to loop, reset the index and direction
                if (loop)
                {
                    currentIndex = currentIndex == 0 ? 1 : waypoints.Length - 2;
                    movingForward = !movingForward;
                }
                else
                {
                    // Otherwise, stop the platform from moving
                    speed = 0f;
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        col.transform.parent = transform;
    }

    private void OnTriggerExit(Collider col)
    {
        col.transform.parent = null;
    }
}
