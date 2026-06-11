using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int coinCount = 0;
    public int scoreCount = 0;
    public float scoreIncreaseInterval = 0.6f; 
    public int scoreAmountPerInterval = 15;

    public static GameManager Instance { get; private set; }

    void Awake()
    {   //Singleton para asegurar que solo haya una instancia
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


}
