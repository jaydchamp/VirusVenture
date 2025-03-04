using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamingEnemy : MonoBehaviour
{
    public LayerMask pointerToGroundLayer;
    private float speed = 1.5f;
    private float spriteWidth;

    private PolygonCollider2D myPolygonCollider;
    private Rigidbody2D myRigidbody2D;
    Transform myTransform;
    public AudioSource enemyDeathSound;

    void Start()
    {
        myRigidbody2D = this. GetComponent<Rigidbody2D>();
        myTransform = this.transform;
        spriteWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
        myPolygonCollider = this.GetComponent<PolygonCollider2D>();
    }

    void FixedUpdate()
    {
        //if no movemement move forward quick
        if (myRigidbody2D.velocity.magnitude < 0.01f)
        {
            //MoveForwardSlightly();
        }

        //check to see if ground in front of us
        Vector2 lineCast = myTransform.position - myTransform.right * spriteWidth;
        //Debug.DrawLine(lineCast, lineCast + Vector2.down);

        bool onGroundStatus = Physics2D.Linecast(lineCast, lineCast + Vector2.down, pointerToGroundLayer);

        //if no ground in front, turn around
        if (!onGroundStatus)
        {
            //Debug.Log("Flip direction");
            transform.Rotate(0f, 180f, 0f);

        }

        //constant move forward
        Vector2 myVelocity = myRigidbody2D.velocity;
        myVelocity.x = -myTransform.right.x * speed;//-speed;
        myRigidbody2D.velocity = myVelocity;
    }
    private void MoveForwardSlightly()
    {
        Vector2 newPosition = myTransform.position + new Vector3(speed * Time.deltaTime, 0, 0);
        myRigidbody2D.MovePosition(newPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("stomp"))
        {
            enemyDeathSound.Play();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            myRigidbody2D.velocity = Vector2.zero;
            DisableColliderForSec(1.0f);
        }
    }

    private IEnumerator DisableColliderForSec(float duration)
    {
        myPolygonCollider.isTrigger = true;


        yield return new WaitForSeconds(duration);

        myPolygonCollider.isTrigger = false;
    }
}
