using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HakaMemberFollow : MonoBehaviour
{
    private Transform playerTransform;
    private float followSpeed = 3f; //speed to follow player
    private float stopDistance = -2f; //how far away to stop
    private bool shouldFollow = true;

    private Vector2 targetPosition;

    void Update()
    {
        if (playerTransform == null)
        {
            Debug.LogError("PLAYER NOT FOUND");
            playerTransform = GameObject.FindWithTag("Player").transform;
            return;
        }

        if (shouldFollow)
        {
            //target movement pos
            Vector2 playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);
            targetPosition = playerPosition - (Vector2)(playerTransform.right) * stopDistance;

            //distance between member and target pos ^
            float distance = Vector2.Distance(transform.position, targetPosition);

            if (distance > 0.1f)
            {
                transform.position = Vector2.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }
    public void SetFollowStatus(bool set)
    {
        shouldFollow = set;
    }
}
