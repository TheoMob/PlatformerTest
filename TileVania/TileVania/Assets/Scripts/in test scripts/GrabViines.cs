using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GrabViines : MonoBehaviour
{
    [SerializeField] Rigidbody2D PlayerRigidBody;
    [SerializeField] CapsuleCollider2D PlayerBodyCollider;
    [SerializeField] float SwingSpeed = 3f;
    BoxCollider2D VineTipCollider;
    Rigidbody2D VineRigidBody;

    bool isHoldingVine = false;

    float xThrow;
    void Start()
    {
        VineRigidBody = GetComponent<Rigidbody2D>();
        VineTipCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GrabVine();
        VineSwing();
    }

    void VineSwing()
    {
        if (isHoldingVine)
        {
            if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D)))
            {
                xThrow = CrossPlatformInputManager.GetAxisRaw("Horizontal");

                Vector2 SwingVelocity = new Vector2(xThrow * SwingSpeed, VineRigidBody.velocity.y);
                VineRigidBody.velocity = SwingVelocity;
            }
        }
    }

    private void GrabVine()
    {
             if ((Input.GetKeyDown(KeyCode.E)) && (isHoldingVine))
            {
                isHoldingVine = false;
                PlayerRigidBody.velocity = new Vector2 (15f, 15f);
             }
            if ((VineTipCollider.IsTouchingLayers(LayerMask.GetMask("player"))) && Input.GetKeyDown(KeyCode.E))
            {
                isHoldingVine = true;
            }
            if (isHoldingVine)
            {
                Vector2 VineTipPosition = VineTipCollider.transform.position;
                PlayerBodyCollider.transform.position = new Vector2 (VineTipPosition.x, VineTipPosition.y - 1f);
            }
    }
}

