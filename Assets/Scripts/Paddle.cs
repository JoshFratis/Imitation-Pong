using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; 

public class Paddle : MonoBehaviour
{
    public int playerNumber;
    public int courtSide;
    public int score;
    public Text scoreboard;

    private float yExtent;

    public PlayerInput input;
    private InputAction move;

    private Transform tf;
    private Rigidbody2D rb2d;

    private float moveDir;
    [SerializeField] float moveSpeed;

    // Debug
        public enum PhysicsMode 
        {
            AddForcePong,
            AddForceReal,
            Velocity
        }
        [SerializeField] PhysicsMode physicsMode;
    // End Debug


    void Awake()
    {
        input = new PlayerInput();

        tf = gameObject.GetComponent<Transform>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        yExtent = Camera.main.GetComponent<Camera>().orthographicSize; 
    }

    void OnEnable()
    {
        switch (playerNumber)
        {
            case 1:
                courtSide = -1;
                tf.position = new Vector3(-8, 0, 0);
                move = input.PaddleOne.Move;
                Debug.Log("Player 1 enabled");
                break;

            case 2:
                courtSide = 1;
                tf.position = new Vector3(8, 0, 0);
                move = input.PaddleTwo.Move;
                Debug.Log("Player 2 enabled");
                break;

            default:
                Debug.Log("strange player enabled");
                break;
        }
        
        // Velocity
        rb2d.velocity = Vector2.zero;

        // Controls
        move.Enable();
    }

    void OnDisable()
    {
        move.Disable();
    }

    void Update()
    {
        moveDir = move.ReadValue<float>();
        //Debug.Log(playerNumber + ".moveDir: " + moveDir);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Border")
        {
            switch (physicsMode)
            {
                case PhysicsMode.AddForcePong:
                case PhysicsMode.AddForceReal:
                case PhysicsMode.Velocity:
                    rb2d.velocity = Vector2.zero;
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        switch (physicsMode)
        {
            case PhysicsMode.AddForcePong:

                float yDir = 0;
                if (rb2d.velocity.y != 0)
                {
                    yDir = Mathf.Sign(rb2d.velocity.y);
                }

                if (moveDir == 0)
                {
                    rb2d.velocity = Vector2.zero;
                    //rb2d.AddForce(-rb2d.velocity);
                }
                else if (yDir != moveDir)
                {
                    rb2d.AddForce(new Vector2(0, moveDir * moveSpeed * 50));
                }
                break;

            case PhysicsMode.AddForceReal:
                rb2d.AddForce(new Vector2(0, moveDir * moveSpeed));
                break;

            case PhysicsMode.Velocity:
                rb2d.velocity = new Vector2(0, moveDir * moveSpeed);
                break;
        }
        
    }

}
