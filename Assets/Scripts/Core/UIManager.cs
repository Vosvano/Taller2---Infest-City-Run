using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI scoreText;
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

    public void UpdateCoinCount(int newCount)
    {
        coinText.text = "Monedas: " + newCount.ToString();
    }
    public void UpdateScore(int newScore)
    {
        scoreText.text = "Puntos: " + newScore.ToString();
    }
}
