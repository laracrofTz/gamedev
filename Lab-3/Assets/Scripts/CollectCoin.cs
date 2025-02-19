using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.CoinCollected(coinValue);
            gameObject.SetActive(false);
        }
    }

    public void ResetCoin()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}
