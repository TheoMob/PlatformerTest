using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxColliders;
    CapsuleCollider2D myBodyCollider;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxColliders = GetComponent<BoxCollider2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        EnemyWalk();
        //EnemyDeath();
    }

    private void EnemyWalk()
    {
        if (IsFacingRight())
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f); // se ele ta indo pra direita = velocidade positiva
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f); // se ele ta indo pra esquerda = velovidade negativa
        }

    }
    
    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    

    private void EnemyInvertSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f); // nem to usando
    }

    private void OnTriggerExit2D(Collider2D collision) // um evento que acontece quando o collider que eu botei no pé dele sai do chão, ou seja, quando sai da colisão
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f); // faz ele inverter o lado dele
    }

    private void EnemyDeath() // isn't working yet...
    {
        if (myBoxColliders.IsTouchingLayers(LayerMask.GetMask("player")))
            {
                Destroy(gameObject);
            }
    }
}
