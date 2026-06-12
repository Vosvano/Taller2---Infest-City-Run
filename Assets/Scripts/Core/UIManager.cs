using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinTextGO;
    [SerializeField] private TextMeshProUGUI scoreTextGO;

    [SerializeField] private CanvasGroup GameOverCanvasGroup;
    [SerializeField] private TextMeshProUGUI TextGameOver;
    private Player PlayerDeath;

    public static UIManager Instance { get; private set; }

    void Awake()
    {   //Singleton para asegurar que solo haya una instancia de UIManager y que persista entre escenas
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {   //Crear el manager
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        PlayerDeath = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PlayerDeath.OnPlayerDeath += Player_OnPlayerDeath;
        GameOverCanvasGroup.interactable = false;
        GameOverCanvasGroup.alpha = 0f;
    }

    public void UpdateCoinCount(int newCount)
    {
        coinText.text = "Coins: " + newCount.ToString();
        coinTextGO.text = "Coins: " + newCount.ToString();
    }
    public void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore.ToString();
        scoreTextGO.text = "Score: " + newScore.ToString();
    }

    IEnumerator GameOverAnimation()
    {
        
        yield return new WaitForSeconds(0.8f);

        GameOverCanvasGroup.interactable = true;

        while (GameOverCanvasGroup.alpha < 0.8f)
        {
            GameOverCanvasGroup.alpha += Time.deltaTime / 2f;
            yield return null;
        }
    }
    void Player_OnPlayerDeath()
    {
        Time.timeScale = 0f;

        GameOverCanvasGroup.alpha = 1f;
        GameOverCanvasGroup.interactable = true;
        GameOverCanvasGroup.blocksRaycasts = true;
        
        StartCoroutine(GameOverAnimation());
    }

    public void OnClickRestart()
    {
        // Asegurar que el tiempo esté normalizado al reiniciar
        Time.timeScale = 1f;

        Destroy(gameObject);

        // Reiniciar la escena actual para empezar de nuevo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
