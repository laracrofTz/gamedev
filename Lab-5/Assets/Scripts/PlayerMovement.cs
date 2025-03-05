using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 15f;
    private Rigidbody2D marioBody;
    public float maxSpeed = 20f;

    public float upSpeed = 10f;
    public float downSpeed = 10f;
    private bool onGroundState = true;
    private bool canDoubleJump = false;

    // global variables
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // platform effectro
    public List<PlatformEffector2D> platformEffectors; 
    public float waitTime = 0.5f;

    // other variables
    public TextMeshProUGUI scoreText;
    public GameObject enemies;

    public List<Coin> collectCoins;

    public static bool isGameOver;
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverScoreText;

    private int score = 0;

    // bloopa
    private bool carriedByBloopa = false;
    private GameObject bloopa;

    // buff
    private bool isInvincible = false;
    //public MushroomTree mushroomTree;

    private void Awake()
    {
        isGameOver = false;
        GameManager.Instance.OnCoinCollected += CoinCollected;
        GameManager.Instance.OnPlayerDeath += HandleGameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        if (collectCoins == null)
        {
            collectCoins = new List<Coin>();
        }
        gameOverScreen.SetActive(false);

    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    public void CoinCollected(int coinValue)
    {
        score += coinValue;
        UpdateScoreText();
    }
    // restart
    public void RestartButtonCallback(int input) 
    {
        ResetGame();
        Time.timeScale = 1.0f;
    }
    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-1.79f, -1.22f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        score = 0;
        UpdateScoreText();
        foreach (var coin in collectCoins)
        {
            coin.ResetCoin();
        }
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            var enemyMovement = eachChild.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                eachChild.transform.position = enemyMovement.startPosition; 
            }
        }

        //if (mushroomTree != null)
        //{
        //    mushroomTree.ResetMushroomTree();
        //}

        isGameOver = false;
        gameOverScreen.SetActive(false);

    }

    void Update()
    {
        if (!carriedByBloopa)
        {
            if (Input.GetKeyDown("a") && faceRightState)
            {
                faceRightState = false;
                marioSprite.flipX = true;
            }

            if (Input.GetKeyDown("d") && !faceRightState)
            {
                faceRightState = true;
                marioSprite.flipX = false;
            }

            if (Input.GetKeyDown("s") && !onGroundState)
            {
                StartCoroutine(DropThroughPlatform());
            }

            if (Input.GetKeyDown("space") && transform.parent != null)
            {
                // Detach Mario from Bloopa and allow jumping
                transform.parent = null;
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGroundState = false;
                canDoubleJump = true;
                carriedByBloopa = false;
            }
        }

        if (isGameOver)
        {
            gameOverScreen.SetActive(true);
        }
    }

    IEnumerator DropThroughPlatform()
    {
        foreach (var effector in platformEffectors)
        {
            effector.rotationalOffset = 180f;
        }
        marioBody.linearVelocity = new Vector2(marioBody.linearVelocity.x, -downSpeed);
        yield return new WaitForSeconds(waitTime);
        foreach (var effector in platformEffectors)
        {
            effector.rotationalOffset = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGroundState = true;
            canDoubleJump = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                GameManager.Instance.PlayerDied();
            }
        }
    }


    void FixedUpdate()
    {
        if (!carriedByBloopa)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(moveHorizontal) > 0)
            {
                Vector2 movement = new Vector2(moveHorizontal, 0);
                // check if it doesn't go beyond maxSpeed
                if (marioBody.linearVelocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
            }

            // stop
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            {
                // stop
                marioBody.linearVelocity = Vector2.zero;
            }

            if (Input.GetKeyDown("space"))
            {
                if (onGroundState)
                {
                    marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                    onGroundState = false;
                    canDoubleJump = true; // allow s for double jump after the first jump
                }
                else if (canDoubleJump)
                {
                    marioBody.linearVelocity = new Vector2(marioBody.linearVelocity.x, 0); // reset vertical velocity
                    marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                    canDoubleJump = false; // disable double jump after using 
                }
            }
        }
    }

    public void SetBeingCarriedByBloopa(bool value, GameObject bloopa)
    {
        carriedByBloopa = value;
        if (value)
        {
            this.bloopa = bloopa;
            transform.parent = bloopa.transform;
        }
        else
        {
            this.bloopa = null;
            transform.parent = null;
        }
    }

    private void HandleGameOver()
    {
        isGameOver = true;
        gameOverScoreText.text = scoreText.text;
        Time.timeScale = 0.0f;
    }

    public void ActivateInvincibility(float duration)
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine(duration));
        }
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;

        //make Mario blink
        for (int i = 0; i < (int)(duration * 2); i++)
        {
            marioSprite.enabled = !marioSprite.enabled; // toggle visibility
            yield return new WaitForSeconds(0.25f); 
        }

        marioSprite.enabled = true; 
        isInvincible = false;
    }
}