using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField][Tooltip("This is only for the character to move alongside the platform while stepping on it, if the player don't touch the platform, there's no need to fill this")] Rigidbody2D PlayerRigidBody;
    [SerializeField][Tooltip("This is only for the character to move alongside the platform while stepping on it, if the player don't touch the platform, there's no need to fill this")] BoxCollider2D PlayerFeetCollider;
    [SerializeField][Tooltip("This is only for the character to move alongside the platform while stepping on it, if the player don't touch the platform, there's no need to fill this")] Animator PlayerAnimator;
    [SerializeField] float ThisPlatformSpeedX = 2f;
    [SerializeField] float ThisPlatformSpeedY = 2f;
    [SerializeField][Tooltip("This can't be changed while the game is running")] float TimeofCycle = 1f;
    [SerializeField] float PlatformCooldown = 0.25f;

    float TimeofCycleBackup;
    float PlatformCooldownBackup;

    bool GoingForward = true;
    bool PlayerIsMoving = false;

    BoxCollider2D ThisPlatfomCollider;
    Rigidbody2D ThisPlatFormRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        ThisPlatfomCollider = GetComponent<BoxCollider2D>();
        ThisPlatFormRigidBody = GetComponent<Rigidbody2D>();
        TimeofCycleBackup = TimeofCycle;
        PlatformCooldownBackup = PlatformCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        PlatformMovement();

        PlatformLandCheck();

        PlayerControl();

    }

    private void PlatformMovement()
    {
        if ((TimeofCycle > 0) && (GoingForward))
        {
            ThisPlatFormRigidBody.velocity = new Vector2(ThisPlatformSpeedX, ThisPlatformSpeedY);
            TimeofCycle -= Time.deltaTime;
        }
        if ((TimeofCycle > 0) && (!GoingForward))
        {
            ThisPlatFormRigidBody.velocity = new Vector2(-ThisPlatformSpeedX, -ThisPlatformSpeedY);
            TimeofCycle -= Time.deltaTime;
        }
        if (TimeofCycle <= 0)
        {
            TimeofCycle = TimeofCycleBackup;
            GoingForward = !GoingForward;
        }
    }

    private void PlatformLandCheck()
    {
        if ((ThisPlatfomCollider.IsTouching(PlayerFeetCollider)) && (!PlayerIsMoving))
        {
            PlayerRigidBody.velocity = ThisPlatFormRigidBody.velocity;
            FindObjectOfType<PlayerTest>().PlayerHasControls = false;
        }
    }

    private void PlayerControl()
    {

        if (((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.A))) || (Input.GetKeyDown(KeyCode.Space)))  // se o player apertar qualquer tecla de movimento
        {
                FindObjectOfType<PlayerTest>().PlayerHasControls = true;    // deixa ele se movimentar
                PlayerIsMoving = true;  // diz que ele está se movendo  
        }
        if (((!Input.GetKey(KeyCode.D)) && (!Input.GetKey(KeyCode.A))) && (!Input.GetKey(KeyCode.Space)))   // se o player não estiver se movimentando de nenhuma forma
        {
            if ((ThisPlatfomCollider.IsTouching(PlayerFeetCollider)))       // e estiver em cima da plataforma
            {
                PlayerIsMoving = false;     // diz que ele não está se movendo

                PlayerAnimator.SetBool("Running", false); // isso é p ele n ficar fazendo aquela animação tosca de corrida mesmo estando parado na plataforma
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        FindObjectOfType<PlayerTest>().PlayerHasControls = true;
    }
}
