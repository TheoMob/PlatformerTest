using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip CoinSound;
    [SerializeField] int CoinValue = 100;

    bool WasAlreadyPicked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!WasAlreadyPicked)
        {
            AudioSource.PlayClipAtPoint(CoinSound, Camera.main.transform.position); // esse som é 3d, então botei p tocar na camera pra n ficar surround, n deu mto certo
            FindObjectOfType<GameSession>().ScoreManager(CoinValue);
            WasAlreadyPicked = true;    // isso é pq como o personagem tem duas colisões, as moedas tavam contando o dobro de pontos, desse jeito conta uma vez só cada moeda
        }


        Destroy(gameObject);
    }
}
