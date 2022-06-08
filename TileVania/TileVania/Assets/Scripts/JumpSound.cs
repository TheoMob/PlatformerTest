using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class JumpSound : MonoBehaviour
{

    AudioSource jump_sound;

    // Start is called before the first frame update
    void Start()
    {
        jump_sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
         do_jump_sound();
    }

    private void do_jump_sound()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if (!jump_sound.isPlaying)
                jump_sound.Play();

        }
    }
}
