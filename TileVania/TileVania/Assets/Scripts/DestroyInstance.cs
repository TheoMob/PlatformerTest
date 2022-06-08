using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInstance : MonoBehaviour
{
    BoxCollider2D myBoxCollider;
    // Start is called before the first frame update
    void Start()
    {
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("player")))    // se o player encostar na folha = destroy a folha
        {
            Invoke("DestroyLeaf", 0.1f); // esse delayzinho é pro script do player computar q ele pegou msm a folha
        }
    }
    private void DestroyLeaf()
    {
            Destroy(gameObject);
    }
}
