using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpTrigger : MonoBehaviour
{
    [SerializeField] int powerupNum;
    [SerializeField] bool powerupStatus = true;
    public GameObject PowerUpManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            PowerUpManager.GetComponent<TestManager>().SetPowerupStatus(powerupNum, powerupStatus);
        }
    }
}
