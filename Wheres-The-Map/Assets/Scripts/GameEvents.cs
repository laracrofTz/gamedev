using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnMapFound;
    public static event Action OnGameOver;
    public static event Action ClickPowerDown;
    //public static event Action PlayPowerDownSound;

    public static void MapFound()
    {
        OnMapFound?.Invoke();
    }

    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void PowerDownClicked()
    {
        ClickPowerDown?.Invoke();
    }

    //public static void OnPlayPowerDownSound()
    //{
    //    PlayPowerDownSound?.Invoke();
    //}
}
