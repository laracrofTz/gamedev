using System.Collections;
using UnityEngine;

public class MushroomTree : MonoBehaviour
{
    public Animator animator;
    public GameObject coinPrefab;
    public Transform dropPoint;
    private bool hasDroppedCoin = false;
    private AudioSource audioSource;

    void Start()
    {
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
            //Debug.Log("Collision detected with player");
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

        //Debug.Log("Starting sway animation");

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
                    coinScript.playerMovement = mario.GetComponent<PlayerMovement>();
                    coinScript.coinValue = 2;
                }
            }
        }

        // waiting for animation to complete
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}