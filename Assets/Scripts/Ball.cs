using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Transform tf;

    private float yExtent;

    private float xAngle;
    private float yAngle;
    private float xForce;
    private float yForce;

    private bool bounce;

    void Awake()
    {
        tf = gameObject.GetComponent<Transform>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        yExtent = Camera.main.GetComponent<Camera>().orthographicSize; 
        bounce = false;
    }

   void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collided with... ");
    
        if (other.gameObject.tag == "Paddle")
        {
            Debug.Log("paddle");

            // Use distance from paddle's center to calculate vertical speed of bounce
            yAngle = (tf.position.y - other.gameObject.GetComponent<Transform>().position.y) * 10;  
            xAngle = -rb2d.velocity.x;
        }
        else if (other.gameObject.tag == "Border")
        {
            Debug.Log("border");

            xAngle = rb2d.velocity.x;
            yAngle = -rb2d.velocity.y;
        }
        bounce = true;
    }

    void FixedUpdate()
    {
        // Bounce ball off paddle
        if (bounce == true)
        {
            rb2d.velocity = new Vector2(xAngle, yAngle);              // flagged not good physics?

            // xForce = xAngle - rb2d.velocity.x;                   // attempt at good physics that doesn't work?
            // yForce = yAngle - rb2d.velocity.y;
            // rb2d.AddForce(new Vector2(xForce, yForce));
        }
        bounce = false;
    }
}
