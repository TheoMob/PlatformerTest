using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] float SlowMowFactor = 0.2f;
    [SerializeField] AudioClip SucessSound;
    BoxCollider2D ExitBoxCollider;


    private void Start()
    {
        ExitBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W)) && (ExitBoxCollider.IsTouchingLayers(LayerMask.GetMask("player"))))
        {
            StartCoroutine(LoadNextLevel());
            AudioSource.PlayClipAtPoint(SucessSound, Camera.main.transform.position);
            print("OK");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //StartCoroutine(LoadNextLevel());
        //AudioSource.PlayClipAtPoint(SucessSound, Camera.main.transform.position);
        // Invoke("LoadNextScene", levelLoadDelay);
    }

    IEnumerator LoadNextLevel()
    {
        Time.timeScale = SlowMowFactor;   // isso seria pra dar um efeito de slow motion quando a fase termina, mas eu achei meio ruim então vou "afrufruzar" de outro jeito dps
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        Time.timeScale = 1f;


        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }



    // as coisas marcadas aqui foram o meu jeito de normalmente lidar com a passagem de levels, entretanto eu vou utilizar uma tecnica nova e deixar essa como comentário
    // btw, eu já testei ela e funciona normalmente

   // private void LoadNextScene()
   // {
   //     int current_scene_index = SceneManager.GetActiveScene().buildIndex;
   //      int next_scene_index = current_scene_index + 1;

   //     SceneManager.LoadScene(next_scene_index);
   //}


}
