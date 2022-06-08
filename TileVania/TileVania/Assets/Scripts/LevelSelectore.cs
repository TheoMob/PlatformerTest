using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectore : MonoBehaviour
{
    [SerializeField] int ToGoScene = 1;

    BoxCollider2D ExitBoxCollider;

    void Start()
    {
        ExitBoxCollider = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W)) && (ExitBoxCollider.IsTouchingLayers(LayerMask.GetMask("player"))))
        {
            SceneManager.LoadScene(ToGoScene);
        }
    }
}
