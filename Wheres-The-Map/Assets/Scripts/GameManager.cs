using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI powerDownText;

    public GameObject map;
    public GameObject powerDown;

    public SpriteRenderer doraImage;
    public SpriteRenderer backgroundImage;

    public Sprite[] doraStages;
    public Sprite[] backgroundScenes;

    private float timeLeft = 60f;
    private int score = 0;
    private bool gameOver = false;
    private int doraStage = 0;
    private int selectedBGIndex = 0;
    private int lastBGIndex = -1;

    public AudioSource powerDownAudio;
    public AudioSource mapClickAudio;

    public ParticleSystem explosionEffect;

    void OnEnable()
    {
        GameEvents.OnMapFound += MapFound;
        GameEvents.OnGameOver += GameOver;
        GameEvents.ClickPowerDown += ActivatePowerDown;
        //GameEvents.PlayPowerDownSound += playPowerDownSound;
    }

    void OnDisable()
    {
        GameEvents.OnMapFound -= MapFound;
        GameEvents.OnGameOver -= GameOver;
        GameEvents.ClickPowerDown -= ActivatePowerDown;
        //GameEvents.PlayPowerDownSound -= playPowerDownSound;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadNewBG();
        powerDown.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        timeLeft -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timeLeft) + "s";

        int newStage = Mathf.FloorToInt((60 - timeLeft) / 15);
        if (newStage > doraStage && newStage < doraStages.Length)
        {
            doraStage = newStage;
            doraImage.sprite = doraStages[doraStage];
        }

        if (timeLeft <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOver = true;


        // add animations and sound later


        PlayerPrefs.SetInt("CurrentScore", score);
        // is there alrdy a highscore?
        if (PlayerPrefs.HasKey("HighScore"))
        {
            // is new score higher?
            if (score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        else
        {
            // no highscore key so need to set it
            PlayerPrefs.SetInt("HighScore", score);
        }
        Debug.Log("High score now is: " + PlayerPrefs.GetInt("HighScore"));
        //SceneManager.LoadScene(2);
        StartCoroutine(PlayExplosionAndEndGame());
    }

    IEnumerator PlayExplosionAndEndGame()
    {
        if (explosionEffect != null)
        {
            explosionEffect.Play();
            AudioSource audio = explosionEffect.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(2);
    }

    void MapFound()
    {
        if (gameOver) return;


        score++;
        scoreText.text = "Score: " + score;

        if (mapClickAudio != null)
        {
            mapClickAudio.Play();
        }

        LoadNewBG();
    }

    void LoadNewBG()
    {
        do
        {
            selectedBGIndex = Random.Range(0, backgroundScenes.Length);
        }
        while (selectedBGIndex == lastBGIndex);
        lastBGIndex = selectedBGIndex;

        //selectedBGIndex = Random.Range(0, backgroundScenes.Length);
        backgroundImage.sprite = backgroundScenes[selectedBGIndex];
        float randX = Random.Range(-8.3f, 7.99f);
        float randY = Random.Range(-2.53f, 3.8f);
        Vector2 mapRandPos = new Vector2(randX, randY);
        map.transform.position = mapRandPos;
    }

    public void ActivatePowerDown()
    {
        if (gameOver) return;

        timeLeft -= 15f;

        doraStage = Mathf.Clamp(doraStage + 1, 0, doraStages.Length - 1);
        doraImage.sprite = doraStages[doraStage];

        StartCoroutine(ShowPowerDownText());
        StartCoroutine(DisablePowerDownAfterSound());

        //powerDown.SetActive(false);
    }

    IEnumerator ShowPowerDownText()
    {
        powerDownText.gameObject.SetActive(true);

        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        powerDownText.gameObject.SetActive(false);
    }

    IEnumerator DisablePowerDownAfterSound()
    {
        if (powerDownAudio != null)
        {
            powerDownAudio.Play();
            yield return new WaitForSeconds(powerDownAudio.clip.length); 
        }
        powerDown.SetActive(false); 
    }

}
