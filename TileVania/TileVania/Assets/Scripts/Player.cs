using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 9.5f;
    [SerializeField] float ClimbSpeed = 5f;
    [SerializeField] Vector2 DeathKick = new Vector2(15f, 10f); // isso é pra quando o player morrer ele sair voando (trocar depois pra uma animação mais burnita)
    [SerializeField] int DeathDelay = 3;
    [SerializeField] float BounceSpeed = 5f;

    bool isAlive = true;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        run();
        jump();
        invert_sprite();
        climb_ladders();
        playerAliveCheck();
        SpringJumpUp();


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
        Vector2 playerVelocity = new Vector2(xThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool player_has_horizontal_speed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", player_has_horizontal_speed);

        /* if (Mathf.Abs(xThrow) > Mathf.Epsilon)
             myAnimator.SetBool("Running", true);    // isso ta em comentário porque é uma outra maneira de chegar no mesmo resultado das linhas em cima, só que eu que fiz e provavelmente é a maneira burra
         else
             myAnimator.SetBool("Running", false); */


    }
   
    private void jump()
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


            Invoke("DeathAnimation",DeathDelay); // esse invoke ta aqui pq eu queria uma especie de delayzinho toda vez que o player morrer, invez de dar insta respawn


            //FindObjectOfType<GameSession>().ProcessPlayerDeath(); // isso é uma subrotina do GameSession, já que é ele que determina quando o jogo reseta, tira vidas e etc
        }     
    }
    private void DeathAnimation()
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
    private void run_test()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        //print(xThrow);
        float xOffset = xThrow * runSpeed * Time.deltaTime;
        print(xOffset);
        float rawXPos = transform.localPosition.x + xOffset;

        transform.localPosition = new Vector3(rawXPos, transform.localPosition.y, transform.localPosition.z);
    }
    // eu queria trazer um adendo especial para essa subrotina chamada Run. Ela trabalha diretamente com a biblioteca Standard da Unity que é a CrossPlatformInput. O que ela basicamente faz é que ela recebe o valor da movimentação de qualquer plataforma, seja teclado ou controle
    // a questão é que esse asset já foi descontinuado e algum tempo, e sua versão mais recente é datada de 2017, portanto é bom procurar futuramente por um método alternativo e mais moderno de realizar isso. Mesmo assim ele funciona muito bem até agora e segue essa receita com bastante rigor
    // caso algum dia queira fzr alguma movimentação rapidamente, acredito que essa subrotina seja bem útil
}
