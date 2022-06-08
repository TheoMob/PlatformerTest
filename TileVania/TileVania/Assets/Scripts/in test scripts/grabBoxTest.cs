using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabBoxTest : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D PlayerBodyCollider;
    BoxCollider2D ThisBoxCollider;


    bool isHoldingBox = false;
    // Start is called before the first frame update
    void Start()
    {
        ThisBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((ThisBoxCollider.IsTouchingLayers(LayerMask.GetMask("player"))) && (Input.GetKeyDown(KeyCode.G)))
        {
            isHoldingBox = !isHoldingBox;
            ThisBoxCollider.isTrigger = !ThisBoxCollider.isTrigger;

        }
        if (isHoldingBox)
        {
            Vector2 BoxHoldingPosition = PlayerBodyCollider.transform.position;

            ThisBoxCollider.transform.position = new Vector2(BoxHoldingPosition.x + 0.5f, BoxHoldingPosition.y);
        }
    }
}
