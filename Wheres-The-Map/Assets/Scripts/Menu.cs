using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI timesUpText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (highScoreText != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "High Score: " + highScore + " maps";
        }

        if (timesUpText != null)
        {
            int currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
            timesUpText.text = "oof time's up. you found " + currentScore + " maps...";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPlayButton()
    {
        SceneManager.LoadScene(1); // 0 is the first scene that will appear, 1 the next and so on, can also put the scene name
    }

    public void onReplayButton()
    {
        SceneManager.LoadScene(1); // go to game scene
    }

    public void onExitButton()
    {
        SceneManager.LoadScene(0); 
    }
}
