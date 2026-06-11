using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;
    // offset vertical aplicado al spawn (metros sobre la base de la plataforma)
    public float verticalOffset = 1f;

    void OnTriggerEnter(Collider collision)
    {
        //Si el objeto que colisiona es el jugador, le damos las monedas y destruimos el objeto
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddCoins(1);
            Destroy(gameObject);

        }

    }
}
