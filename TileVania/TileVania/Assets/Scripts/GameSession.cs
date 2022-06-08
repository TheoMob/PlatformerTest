using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int Score = 0;
    [SerializeField] int PlayerLives = 3;
    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numGameSessions > 1) // basicamente isso serve para que apenas uma GameSession esteja ativa por vez, ter varias ao mesmo tempo é um problema pq ela é a unica coisa que "não reseta"
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        livesText.text = PlayerLives.ToString();
        scoreText.text = Score.ToString();
    }

    public void ScoreManager(int pointsToAdd)
    {
        Score += pointsToAdd;
        scoreText.text = Score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (PlayerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void ResetGameSession() // caso o player não tenha mais nenhuma vida, isso faz ele voltar pro Menu
    {
        SceneManager.LoadScene(0);         
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        var current_scene_index = SceneManager.GetActiveScene().buildIndex; // guarda qual fase o player está
        PlayerLives--;                                                      // tira uma vida
        livesText.text = PlayerLives.ToString();                            // mostra quantas vidas o player tem na tela
        SceneManager.LoadScene(current_scene_index);                            // reseta o nivel
    }

}
