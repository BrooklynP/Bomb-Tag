using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBeepScript : MonoBehaviour {

    // Constants
    const float fMAX_BEEP_TIME = 3.0f;
    const float fBEEP_DECAY_TIME = 0.18f;

    // Components
    AudioSource myAudioSource;

    // Private variables
    float fBeepTimer = 0;
    float fBeepFrequency = 0;

    // Use this for initialization
    void Start () {
        myAudioSource = GetComponent<AudioSource>();	
	}
	
	// Update is called once per frame
	void Update () {

        //PlayerControls.bombTimer = 5.0f;

        if( fBeepTimer <= 0.0f )
        {
            if( myAudioSource.isPlaying )
            {
                myAudioSource.Stop();
            }

            myAudioSource.Play();

            fBeepTimer = (fMAX_BEEP_TIME - fBeepFrequency);

            //fBeepTimer = (fMAX_BEEP_TIME - fBeepFrequency);
            fBeepFrequency += fBEEP_DECAY_TIME;

            // Clamp this so we don't get anything ridiculous
            fBeepTimer = Mathf.Clamp(fBeepTimer, 0.1f, fMAX_BEEP_TIME);
        }
        else
        {
            fBeepTimer -= Time.deltaTime;
        }
    }
}
