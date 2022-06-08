using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource CaveWorldMusic;
    [SerializeField] AudioSource WindWorldMusic;
    [SerializeField] AudioSource EndMusic;


    int CurrentSceneIndex;

    // All of this is a fucking mess, don't even bother trying to understand, i doesn't even work
    // well, now it's working, soo... that's it



    // Update is called once per frame
    void Update()
    {
        MusicSelector();

    }
    private void MusicSelector()
    {
        CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;   // pega a fase que o jogador ta, pra depois colocar a musica apropriada

        if (CurrentSceneIndex == 1 || CurrentSceneIndex == 2 || CurrentSceneIndex == 3 || CurrentSceneIndex == 4 || CurrentSceneIndex == 5) // do 1 ao 5 é o mundo das cavernas
        {
            if (!CaveWorldMusic.isPlaying) // isso é pra testar se a musica ja n ta tocando, pra ela p tocar uma por cima da outra
            {
                CaveWorldMusic.Play();     // toca a musica da caverna
            }
        }
        if (CurrentSceneIndex == 6)
        {
            if (!WindWorldMusic.isPlaying)
            {
                CaveWorldMusic.Stop();
                WindWorldMusic.Play();
            }
        }

        if (CurrentSceneIndex == 7)
        {
            if (!EndMusic.isPlaying)
            {
                WindWorldMusic.Stop();
                EndMusic.Play();
            }
        }
    }

}
