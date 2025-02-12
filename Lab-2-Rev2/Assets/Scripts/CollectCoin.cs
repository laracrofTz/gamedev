using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public PlayerMovement playerMovement;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;

        // was getting a type mismatch here, so if payer movement reference is not set, we try to find it
        if (playerMovement == null)
        {
            GameObject mario = GameObject.FindGameObjectWithTag("Player");
            if (mario != null)
            {
                playerMovement = mario.GetComponent<PlayerMovement>();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && playerMovement != null)
        {
            playerMovement.CoinCollected(coinValue);
            gameObject.SetActive(false);
        }
    }

    public void ResetCoin()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}
