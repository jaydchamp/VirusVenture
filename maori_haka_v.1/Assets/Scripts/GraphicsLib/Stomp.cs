using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    public float growthSpeed = 0.5f;
    private float maxSize = 2.0f;
    public event Action StompFinished;
    void Update()
    {
        if (gameObject.activeSelf)//(shouldMove)
        {
            transform.localScale += new Vector3(growthSpeed, growthSpeed, 0f) * Time.deltaTime;

            if (transform.localScale.x >= maxSize && transform.localScale.y >= maxSize)
            {
                StompFinished?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
