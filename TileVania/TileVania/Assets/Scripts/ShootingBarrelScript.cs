using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ShootingBarrelScript : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D PlayerCollider;
    [SerializeField] BoxCollider2D PlayerFeetCollider;
    [SerializeField] Rigidbody2D PlayerRigidBody2D;
    [SerializeField] SpriteRenderer PlayerVisibility;
    [Tooltip("1.Free Barrel 2.Oriented AutoBarrel")] [SerializeField] int TypeOfBarrelIndex = 1; // isso é pra definir se o controle do barril vai ser automático ou se o player vai poder escolher a direção (não ta pronto)
    [Tooltip("Only available in Mode 2")] [SerializeField] float AutoRotationAngle = 0f; // 1 = 90 , -1 = 270, 0 = 0/360, 100000 = 180
    [Tooltip("Time that the auto rotation takes")] [SerializeField] float AutoRotationSpeed = 1f;
    [SerializeField] float BarrelLaunchSpeed = 20f; // a velocidade na qual o player é lançado pelo barril
    [Tooltip("Only for Index 1")][SerializeField] float RotateFactor = 250f;     // o quão rapido ele rotaciona
     float TimeAfterAutoLaunch = 0.55f;

    BoxCollider2D BarrelCollider;
    float TimeOffBarrel = 0f;
    bool InsideBarrel = false;  // bool pra definir se o player está ou não dentro do barril
    bool WasLauched = false;    // isso serve pro barril voltar pro jeito que ele tava no Index 2



    void Start()
    {
        BarrelCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TypeOfBarrelIndex == 1)
        {
            BarrelRotation();
            EnterBarrel();
        }
        if (TypeOfBarrelIndex == 2)
        {
            BarrelAutoRotation();
        }

        if ((PlayerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) || (PlayerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))) // isso serve pra devolver o controle do jogador depois dele encostar no chão
        {
            FindObjectOfType<PlayerTest>().PlayerHasControls = true;
            // provavelmente com o tempo seja necessário adicionar mais camadas aqui, leve em consideração que o controle só é devolvido quando ele toca no chão da maneira atual
        }

    }

    private void BarrelRotation()
    {
        if (InsideBarrel)
        {
            float RotationThisFrame = RotateFactor * Time.deltaTime;

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * RotationThisFrame);

            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(-Vector3.forward * RotationThisFrame);
            }
        }
    }

    private void EnterBarrel()
    {
        if ((BarrelCollider.IsTouchingLayers(LayerMask.GetMask("player"))) && (TimeOffBarrel <= 0))
        {
            InsideBarrel = true;
        }
        if (InsideBarrel)
        {
            Vector2 BarrelPosition = transform.position;

            PlayerRigidBody2D.position = BarrelPosition;    // prende o player na posição do barril
            PlayerRigidBody2D.gravityScale = 0f;            // tira a gravidade do corpo, senão ele fica tentando cair
            FindObjectOfType<PlayerTest>().PlayerHasControls = false;   // retira do player a possibilidade de andar com o personagem
            PlayerVisibility.enabled = false;                           // deixa o player invisivel enquanto no barril

            if (Input.GetKeyDown(KeyCode.E))
            {
                InsideBarrel = false;              // se apertar E sai do barril
                PlayerVisibility.enabled = true;    // deixa o player visivel dnv
                TimeOffBarrel = 0.5f;   // variavel necessária pra que o player não entre automaticamente denovo dentro do barril enquanto estiver tentando sair
                PlayerRigidBody2D.velocity = new Vector2(transform.up.x * BarrelLaunchSpeed, transform.up.y * BarrelLaunchSpeed);
                PlayerRigidBody2D.gravityScale = 1f;
            }
        }
        if (TimeOffBarrel > 0f)
        {
            TimeOffBarrel -= Time.deltaTime;
        }
    }

    private void BarrelAutoRotation()
    {


        if ((BarrelCollider.IsTouchingLayers(LayerMask.GetMask("player"))) && (TimeOffBarrel <= 0))
        {
            InsideBarrel = true;
        }
        if (InsideBarrel)
        {
            Vector2 BarrelPosition = transform.position;

            PlayerRigidBody2D.position = BarrelPosition;    // prende o player na posição do barril
            PlayerRigidBody2D.gravityScale = 0f;            // tira a gravidade do corpo, senão ele fica tentando cair
            FindObjectOfType<PlayerTest>().PlayerHasControls = false;   // retira do player a possibilidade de andar com o personagem
            PlayerVisibility.enabled = false;                           // deixa o player invisivel enquanto no barril


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, AutoRotationAngle), Time.deltaTime * (AutoRotationSpeed));

            TimeAfterAutoLaunch -= Time.deltaTime;

            if (TimeAfterAutoLaunch < 0) 
            {
                print("uhuhuhuhuhuhuhu");
                InsideBarrel = false;              // Sai do Barril
                PlayerVisibility.enabled = true;    // deixa o player visivel dnv
                TimeOffBarrel = 1f;   // variavel necessária pra que o player não entre automaticamente denovo dentro do barril enquanto estiver tentando sair


                PlayerRigidBody2D.velocity = new Vector2(transform.up.x * BarrelLaunchSpeed, transform.up.y * BarrelLaunchSpeed);
                PlayerRigidBody2D.gravityScale = 1f;

                WasLauched = true;

                TimeAfterAutoLaunch = 0.55f; // isso é pra ser o Tempo original, arruma saporra depois @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            }
        }
        if (TimeOffBarrel > 0f)
        {
            TimeOffBarrel -= Time.deltaTime;
        }



        if ((WasLauched) && (TimeOffBarrel <= 0))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, 0), Time.deltaTime * BarrelLaunchSpeed);

            if (transform.rotation == Quaternion.Euler(0f, 0f, 0))
            {
                WasLauched = false;
            }
        }
    }
}
