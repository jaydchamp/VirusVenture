using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperWave : MonoBehaviour
{
    public GameObject wavePrefab;
    Transform spawnLocation;
    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnWave()
    {
        //GameObject laser = Instantiate(laserPrefab, laserSpawn.position, laserSpawn.rotation) as GameObject;
        GameObject wave = Instantiate(wavePrefab,gameObject.transform.position, gameObject.transform.rotation);

    }
}
