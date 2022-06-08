using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int StartSceneIndex;

    private void Awake()
    {

        int numScenePersistence = FindObjectsOfType<ScenePersist>().Length;

        if (numScenePersistence > 1) // basicamente isso serve para que apenas uma GameSession esteja ativa por vez, ter varias ao mesmo tempo é um problema pq ela é a unica coisa que "não reseta"
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
       StartSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (CurrentSceneIndex != StartSceneIndex)
        {
            Destroy(gameObject);
        }

    }
}
