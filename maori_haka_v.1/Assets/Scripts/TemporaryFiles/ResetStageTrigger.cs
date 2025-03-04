using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStageTrigger : MonoBehaviour
{
    [SerializeField] GameObject checkPoint;

    private void Start()
    {
        checkPoint = this.gameObject.transform.GetChild(0).gameObject;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {

            //collider.gameObject.GetComponent<PlayerReposition>().RepositionPlayer();
            //transform.localPosition = origPlayerPosition;
            Debug.Log("CHECKPOINT");
            collider.gameObject.transform.position = checkPoint.gameObject.transform.position; 
            //Debug.Log(checkPoint.gameObject.transform.localPosition);
            //Debug.Log(collider.gameObject.transform.localPosition);

        }
    }
}
