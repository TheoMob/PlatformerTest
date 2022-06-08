using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] float RunSpeed = 6f;
    [SerializeField] float jumpSpeed = 9.5f;
    [SerializeField] float ClimbSpeed = 5f;
    [SerializeField] float PlayerGravity = 1f;
    [SerializeField] Vector2 DeathKick = new Vector2(15f, 10f); // isso é pra quando o player morrer ele sair voando (trocar depois pra uma animação mais burnita)
    [SerializeField] int DeathDelay = 3;
    [SerializeField] float BounceSpeed = 5f;
    [SerializeField] float LeafGravity = 0.1f;
    [SerializeField] float LeafSpeed = 10f;
    [SerializeField] float LeafyJump = 5f;
    [SerializeField] float JumpTime = 0.35f;

    public bool PlayerHasControls = true;

    float OldJumpSpeed;
    float JumpTimeAux;

    bool isAlive = true;

    bool PickLeaf = false;
    bool isFlying = false;
    bool isJumping = false;

    Rigidbody2D myRigidBody; // variavel pra salvar o rigidbody do personagem
    Animator myAnimator; // variavel para salvar a animação do personagem (fazer ele correr, rolar, essas coisas)
    CapsuleCollider2D myBodyCollider2D; // variavel feita p salvar o colider do personagem
    BoxCollider2D myFeetCollider;

    float xThrow;
    float yThrow;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();  // salva na variavel myRigidBody o rigidbody do personagem
        myAnimator = GetComponent<Animator>();      // referencia o Animator do personagem em código (pra poder trocar as animações e os karaleo)
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();  // salva na variavel myCollider2D o collider2D do player (meio obvio)
        myFeetCollider = GetComponent<BoxCollider2D>();         // pega só o collider do pé dele, pra fins de espinho, não pular na parede, etc
        myRigidBody.gravityScale = PlayerGravity;   // seta a gravidade inicial do personagem
        OldJumpSpeed = jumpSpeed;   // isso serve pra que eu tenha um backup da velocidade do pulo, caso eu vá alterar ela
        JumpTimeAux = JumpTime;     // isso serve pra eu ter uma variável sempre guardando o tempo de pulo que eu estabeleço, já que eu vou precisar modificar com o tempo, mas sem perder o valor original
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHasControls)
        {

            if (!isAlive) { return; }
            run();
            //jump();
            climb_ladders();
            playerAliveCheck();
            SpringJumpUp();
            pickLeaf();
            TestJump();
            invert_sprite();
        }
        JumpAnimation();
        DebugResetKey(); // reseta a fase, é só pra testes, muitas coisas tem bugado...
    }

    private void DebugResetKey()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var current_scene_index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(current_scene_index);
        }
    }

    private void run()
    {

        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(xThrow * this.RunSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool player_has_horizontal_speed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", player_has_horizontal_speed);


    }

    private void TestJump()     // um novo metodo de pulo com intensidade, diferente do anterior esse tem sua força baseada no tempo que a pessoa segura o botão de pulo
    {

        if (CrossPlatformInputManager.GetButtonDown("Jump") && (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))) // Se o botão de pular for pressionado e o personagem estiver tocando no chão com o pé = true
        {
            Vector2 JumpVelocity = new Vector2(myRigidBody.velocity.x, jumpSpeed);  // coloca a velocidade em um vetor
            myRigidBody.velocity = JumpVelocity;                                    // coloca o vetor velocidade no corpo do player
            isJumping = true;                                                       // variavel auxiliar que serve pra saber se o jogador está no meio do pulo ou não
            JumpTime = JumpTimeAux;                                                       // tempo que o player tem de pulo no meio do ar, pra que o pulo tenha pressão 
        }

        if (((isJumping) && (JumpTime > 0)) && (CrossPlatformInputManager.GetButton("Jump")))   // se o player estiver no meio do pulo e o tempo de pulo dele for maior que zero = true
        {
 
            Vector2 JumpVelocityInAir = new Vector2(myRigidBody.velocity.x, jumpSpeed);         // coloca denovo a velocidade do jogador em um vetor
            myRigidBody.velocity = JumpVelocityInAir;                                           // coloca esse vetor velocidade no jogador

            JumpTime -= Time.deltaTime;                                                         // diminui o tempo de pulo do jogador

            if (JumpTime == 0)                                                                  // quando o tempo de pulo acaba, o jogador não pode mais pular, o tempo se reseta e o "pulo no meio do ar" acaba
            {
                isJumping = false;
                JumpTime = JumpTimeAux;
            }
        }
        if (CrossPlatformInputManager.GetButtonUp("Jump"))      // caso o jogador solte e tecla de espaço, o pulo acaba, logo ele não está mais pulando
        {
            isJumping = false;
        }

    }
    private void jump() // this is not used anymore
    {

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; } // se o personagem não estiver tocando a camada chamada "Ground", saia do pulo (pra não dar p pular no ar)

        if (CrossPlatformInputManager.GetButtonDown("Jump"))  // jump no caso é o espaço do teclado por convenção do Unity, dava p ter posto qlqr tecla ali q ia dar no mesmo, mas com Jump ele tem CrossPlatform
        {
             Vector2 JumpVelocityAdd = new Vector2(0f, jumpSpeed); // parecido com mover o personagem, mas agora utilizando o metodo de pulo
             myRigidBody.velocity += JumpVelocityAdd;
                
        }
    }

    private void invert_sprite()
    {
        bool player_has_horizontal_speed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;   // Mathf.Abs serve pra retornar o valor absoluto de algo (tira o sinal basicamente)
                                                                                                // Mathf.Epsilon é o menor valor existente pra um float, ele ta ali basicamente pra testar que a variavel é maior que zero                              

        if (player_has_horizontal_speed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f); // isso serve pra virar o sprite enquanto ele caminha, Mathf.Sign pega só o sinal de um valor
        }
    }

    private void climb_ladders()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = 1f;
            return;
        }

        yThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 playerClimbVelocity = new Vector2(myRigidBody.velocity.x, yThrow * ClimbSpeed);
        myRigidBody.velocity = playerClimbVelocity;

        bool player_is_actually_climbing = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

        myAnimator.SetBool("Climbing", player_is_actually_climbing);
        myRigidBody.gravityScale = 0f;

    }

    private void playerAliveCheck()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            myAnimator.SetTrigger("Death");     // isso aqui é o trigger da animação de morte, só ligo ele
            myRigidBody.velocity = DeathKick;   // isso é pra ele ser jogado p cima quando ele morre, fica bonitin
            isAlive = false;                    // isso é pra inibir o jogador de se movimentar enquanto estiver morto


            Invoke("DeathAnimation", DeathDelay); // esse invoke ta aqui pq eu queria uma especie de delayzinho toda vez que o player morrer, invez de dar insta respawn


            //FindObjectOfType<GameSession>().ProcessPlayerDeath(); // isso é uma subrotina do GameSession, já que é ele que determina quando o jogo reseta, tira vidas e etc
        }
    }
    private void DeathAnimation()   // animaçãozinha de morre
    {
        FindObjectOfType<GameSession>().ProcessPlayerDeath(); // isso é uma subrotina do GameSession, já que é ele que determina quando o jogo reseta, tira vidas e etc
    }   

    private void SpringJumpUp() // é pra um bloco de pulo, quando o player toca com os pés nele ele o joga p cima com uma velocidade la (BounceSpeed)
    {

        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Bouncy Stuff")))
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, BounceSpeed);
        }

    }

   private void pickLeaf() // é uma mecanica pra planar, o personagem pega a folinha e enquanto ele estiver com ela ele fica com um pulo especial e uma animação de paraquedas
    {                      // todo: criar uma subrotina diferente para a animação da folinha, por titulo de organização

        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("LeafyGlide")))
        {
            PickLeaf = true;
        }
        if ((!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) && (PickLeaf)) // se os pés do personagem sairem do chão e ele estiver com a folinha
        {
            if (!isFlying) { OldJumpSpeed = jumpSpeed; } // guarda a velocidade de speed do personagem antiga, pra ele voltar p ela quando acabar o Glide
 
            jumpSpeed = LeafyJump; // isso serve pra que, mesmo com a mudança da gravidade por causa do "planar" da folha, o tamanho do pulo continue o mesmo

            myRigidBody.gravityScale = LeafGravity;
             
            myAnimator.SetBool("Flying", true);

            isFlying = true;
        }
        if ((myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) && (isFlying))   // isso é pra testar se, depois de voar, o personagem caiu no chão, p normalizar o movimento dele
        {
            jumpSpeed = OldJumpSpeed;

            myRigidBody.gravityScale = PlayerGravity;

            myAnimator.SetBool("Flying", false);

            isFlying = false;
        }
        if (myRigidBody.IsTouchingLayers(LayerMask.GetMask("LeafyGlideProhibited"))) // se o personagem passar na parte onde a leaf é proibida, desabilita ela (faz uns efeitos loucos dps)
            { PickLeaf = false; }
    }

    private void JumpAnimation()    // animação de pulo, bem cagada, mas é oq o sistema oferece por hora
    {
       if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) { return; } // caso ele esteja em uma escada, eu não quero que a animação de pulo toque


       if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))   // se não estiver tocando no chão liga a animação de pulo
        {
            myAnimator.SetBool("Jumping", true);
        }
       if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))    // se encostar no chão desliga a animação de pulo
        {
            myAnimator.SetBool("Jumping", false);
        }
    }
}
