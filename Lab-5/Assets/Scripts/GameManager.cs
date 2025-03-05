using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<int> OnCoinCollected;
    public event Action OnPlayerDeath;
    //public event Action<Collision2D> OnTreeCollision;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CoinCollected(int coinValue)
    {
        OnCoinCollected?.Invoke(coinValue);
    }

    public void PlayerDied()
    {
        OnPlayerDeath?.Invoke();
    }

    //public void ActivateMushroomTree(Collision2D collision)
    //{
    //    OnTreeCollision?.Invoke(collision);
    //}
}
