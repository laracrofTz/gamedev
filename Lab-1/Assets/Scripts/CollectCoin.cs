using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    public PlayerMovement playerMovement;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        //UpdateScoreText();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerMovement.CoinCollected(coinValue); 
            gameObject.SetActive(false);
        }
    }

    public void ResetCoin()
    {
        //score = 0;
        transform.position = initialPosition;
        gameObject.SetActive(true); 
    }
}
