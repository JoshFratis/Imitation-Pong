using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Camera
    private float xExtent;
    private float yExtent;
    private float leftBound;
    private float rightBound;
    private float upperBound;
    private float lowerBound;

    // Pause
    public PlayerInput input;
    private InputAction escape;

    // Game State
    public enum GameState 
    {
        Unstarted,
        Playing,
        Paused,
        Over
    }
    public GameState gameState;

    // Menu
    [SerializeField] Menu menuMain;
    [SerializeField] Menu pauseMenu;

    // Scoreboards
    [SerializeField] Text scoreboardLeft;
    [SerializeField] Text scoreboardRight;

    // Net
    [SerializeField] GameObject net;

    // Players
    [SerializeField] Paddle playerOne;
    [SerializeField] Paddle playerTwo;

    // Ball
    [SerializeField] GameObject ball;
    [SerializeField] float serveSpeed;
    private Transform ball_tf;
    private Rigidbody2D ball_rb2d;


// -------------------------------------------------------------------------------


    // private InputAction scoreLeft;
    // private InputAction scoreRight;

    // // Score for Player 1
    // private void ScoreLeft(InputAction.CallbackContext context)
    // {
    //     ball_tf.position = new Vector3(rightBound + 100, 0, 0); 
    // }

    // // Score for Player 2
    // private void ScoreRight(InputAction.CallbackContext context)
    // {
    //     ball_tf.position = new Vector3(leftBound - 100, 0, 0); 
    // }

// -------------------------------------------------------------------------------


    void Awake()
    {
        ball_tf = ball.GetComponent<Transform>();           // Identify Ball's Transform
        ball_rb2d = ball.GetComponent<Rigidbody2D>();       // Identify Ball's Rigidbody 2D

        input = new PlayerInput();  // debug
    }

    void Start()
    {
        // Get camera bounds (assumes camera starts at origin)
        yExtent = Camera.main.GetComponent<Camera>().orthographicSize; 
        xExtent = yExtent / Screen.height * Screen.width;
        leftBound = -xExtent;
        rightBound = xExtent;
        lowerBound = -yExtent;
        upperBound = yExtent;
    }

    void OnEnable()
    {
        // Pause Controls
        escape = input.Game.Pause;
        escape.Enable();
        escape.performed += Pause;

        // debug controls
        // scoreLeft = input.Debug.ScoreLeft;
        // scoreLeft.Enable();
        // scoreLeft.performed += ScoreLeft;
        // scoreRight = input.Debug.ScoreRight;
        // scoreRight.Enable();
        // scoreRight.performed += ScoreRight;

        StartGame();
    }

    void OnDisable()
    {
        // Pause Controls
        escape.Disable();

        // Debug Controls
        // scoreLeft.Disable();
        // scoreRight.Disable();
    }

    // Pause Input
    void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("Escape!");
        if (gameState == GameState.Paused)
        {
            Debug.Log("Gamestate Paused. Resuming Game.");
            ResumeGame();
        }
        else if (gameState == GameState.Playing)
        {
            Debug.Log("Gamestate Playing. Pausing Game.");
            PauseGame();
        }
    }

    void Update()
    {
        // Check for Goal
        Paddle scorer = null;
        if (ball_tf.position.x < leftBound) 
        {
            scorer = playerTwo;
        }
        else if (ball_tf.position.x > rightBound) 
        {
            scorer = playerOne;
        }

        // Carry out Goal
        if (scorer != null)
        {
            AwardPoint(scorer);
            UpdateScore(scorer);
            if (CheckWin(scorer))
            {
                EndGame(scorer);
            }
            else
            {
                StartCoroutine(Serve(scorer));
            }
        }
    }

    // GAME FUNCTIONS
    void AwardPoint(Paddle scorer)
    {
        scorer.score++;
    }

    void UpdateScore(Paddle scorer)
    {
        scorer.scoreboard.text = scorer.score.ToString();
    }

    bool CheckWin(Paddle scorer)
    {
        if (scorer.score < 11)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }

    IEnumerator Serve(Paddle server)
    {
        
        ball_rb2d.velocity = new Vector2(0, 0);                     // stop ball  
        ball_tf.position = new Vector3(0, 0, 0);                    // move ball to center
        ball.SetActive(false);
        yield return new WaitForSeconds(3);
        ball.SetActive(true);
        ball_rb2d.AddForce(new Vector2(server.courtSide * serveSpeed, 0));   // add force to ball
    }

    // GAME STATE FUNCTIONS
    private void StartGame()
    {
        // Activate Objects
        net.SetActive(true);
        scoreboardLeft.gameObject.SetActive(true);
        scoreboardRight.gameObject.SetActive(true);
        playerOne.gameObject.SetActive(true);
        playerTwo.gameObject.SetActive(true);
        ball.SetActive(true);

        // Initialize Score
        playerOne.score = 0;
        playerTwo.score = 0;
        UpdateScore(playerOne);
        UpdateScore(playerTwo);

        // Game State
        Time.timeScale = 1;
        gameState = GameState.Playing;

        // Serve Ball
        StartCoroutine(Serve(playerTwo));
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
        net.SetActive(false);
        gameState = GameState.Paused;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
        net.SetActive(true);
        gameState = GameState.Playing;
    }

    public void RestartGame()
    {
        DeactivateObjects();
        StartGame();
    }

    public void EndGame(Paddle winner)
    {        
        menuMain.gameObject.SetActive(true);    // Post-Game Menu 

        if (winner == null)                     // Return to title menu if no winner 
        {
            menuMain.menuTitle = "PONG";
            playerOne.gameObject.SetActive(false);
            playerTwo.gameObject.SetActive(false);
        }
        else                                    // Declare winner by menu's title
        {
            menuMain.menuTitle = "PLAYER " + winner.playerNumber + " WINS!";      
        }

        DeactivateObjects();                    
        gameObject.SetActive(false);            // Deactivate self to end game
    }

    void DeactivateObjects()
    {
        net.SetActive(false);
        playerOne.gameObject.SetActive(false);
        playerTwo.gameObject.SetActive(false);
        ball.SetActive(false);
    }
}
