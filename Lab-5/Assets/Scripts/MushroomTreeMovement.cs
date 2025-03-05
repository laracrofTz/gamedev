using System.Collections;
using UnityEngine;

public class MushroomTree : MonoBehaviour
{
    public Animator animator;
    public GameObject coinPrefab;
    public Transform dropPoint;
    private bool hasDroppedCoin = false;
    private AudioSource audioSource;
    // public Vector3 initialPosition;

    //private void Awake()
    //{
    //    GameManager.Instance.OnTreeCollision += OnCollisionEnter2D;
    //}

    void Start()
    {
        //initialPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasDroppedCoin)
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ActivateInvincibility(10f); // 10 sec of buff
            }
            StartCoroutine(SwayAndDropCoin());
        }
    }

    IEnumerator SwayAndDropCoin()
    {
        hasDroppedCoin = true;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        animator.SetTrigger("Sway");

        // waiting for sway animation
        yield return new WaitForSeconds(0.5f);

        // spawn coin
        if (coinPrefab != null && dropPoint != null)
        {
            GameObject coin = Instantiate(coinPrefab, dropPoint.position, Quaternion.identity);
            Coin coinScript = coin.GetComponent<Coin>();
            if (coinScript != null)
            {
                GameObject mario = GameObject.FindGameObjectWithTag("Player");
                if (mario != null)
                {
                    coinScript.coinValue = 2;
                }
            }
        }

        // waiting for animation to complete
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

    //public void ResetMushroomTree()
    //{
    //    hasDroppedCoin = false;
    //    transform.position = initialPosition;
    //    gameObject.SetActive(true); 
    //}
}