using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int coinCount = 0;
    public int scoreCount = 0;
    public float scoreIncreaseInterval = 0.6f; 
    public int scoreAmountPerInterval = 15;

    public CanvasGroup GameOverCanvasGroup;
    public static GameManager Instance { get; private set; }


    void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        GameOverCanvasGroup.interactable = false;
        GameOverCanvasGroup.alpha = 0f;
        StartCoroutine(IncreaseScoreOverTime());
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        UIManager.Instance.UpdateCoinCount(coinCount);
    }
    private IEnumerator IncreaseScoreOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(scoreIncreaseInterval);
            scoreCount += scoreAmountPerInterval;
            UIManager.Instance.UpdateScore(scoreCount);
        }
    }

    public IEnumerator StopScoreIncrease()
    {
        // Detener la corrutina de aumento de puntaje
        StopCoroutine(IncreaseScoreOverTime());
        yield break;
    }


}
