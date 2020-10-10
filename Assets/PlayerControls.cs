using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    // Constants
    const float MAX_TIME_FOR_NO_BACKSIES = 0.5f;
    public const float fBOMB_TIMER_MAX = 60.0f;

    // Statics
    static int playerWithBomb = 0;
    static float noBacksiesTimer = MAX_TIME_FOR_NO_BACKSIES;
    public static float bombTimer = fBOMB_TIMER_MAX;

    // Public editor variables
    public AudioClip jump;
    public AudioClip powerUp;
    public int playerIndex;

    // Components
    Rigidbody2D myRigidBody2D;
    SpriteRenderer mySpriteRenderer;
    AudioSource myAudioSource;
    
    //Private Variables
    bool isPlayerJumping = false;

    // Enums
    enum CONTROLS
    {
        LEFT,
        RIGHT,
        JUMP
    }

	// Use this for initialization
	void Start () {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();

        playerWithBomb = Random.Range(1, 3);
    }
	
	// Update is called once per frame
	void Update () {

        // Bomb timer.
        if(bombTimer > 0.0f)
        {
            bombTimer -= Time.deltaTime;
        }
        else
        {
            if(playerWithBomb == playerIndex)
            {
                Destroy(gameObject);
            }
        }

        // Change colour if I have the bomb
        if(playerWithBomb == playerIndex)
        {
            noBacksiesTimer -= Time.deltaTime;

            mySpriteRenderer.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else
        {
            mySpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
        }

        // Get velocity.
        Vector2 playerVelocity = myRigidBody2D.velocity;

        if(Input.GetKey(GetPlayerKey(CONTROLS.RIGHT)))
        {
            // Move to the right.
            playerVelocity += Vector2.right;
        }
        else if(Input.GetKey(GetPlayerKey(CONTROLS.LEFT)))
        {
            // Move to the left.
            playerVelocity += Vector2.left;
        }

        if (Input.GetKeyDown(GetPlayerKey(CONTROLS.JUMP)) && !isPlayerJumping)
        {
            // Jump.
            playerVelocity += (Vector2.up * 8.0f);

            // Set the flag.
            isPlayerJumping = true;

            // Play jump sound.
            PlaySound(jump);
        }

        // Clamp our players velocity.
        playerVelocity.x = Mathf.Clamp(playerVelocity.x, -3, 3);
        playerVelocity.y = Mathf.Clamp(playerVelocity.y, -20, 20);

        // Set the velocity.
        myRigidBody2D.velocity = playerVelocity;
	}

    KeyCode GetPlayerKey(CONTROLS requestedControl)
    {
        switch(requestedControl)
        {
            case CONTROLS.LEFT:
                {
                    if(playerIndex == 1)
                    {
                        return KeyCode.A;
                    }
                    if(playerIndex == 2)
                    {
                        return KeyCode.LeftArrow;
                    }
                    break;
                }
            case CONTROLS.RIGHT:
                {
                    if (playerIndex == 1)
                    {
                        return KeyCode.D;
                    }
                    if (playerIndex == 2)
                    {
                        return KeyCode.RightArrow;
                    }
                    break;
                }
            case CONTROLS.JUMP:
                {
                    if (playerIndex == 1)
                    {
                        return KeyCode.Space;
                    }
                    if (playerIndex == 2)
                    {
                        return KeyCode.Keypad0;
                    }
                    break;
                }
        }

        // Default return.
        return KeyCode.RightWindows;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Floor")
        {
            isPlayerJumping = false;
        }
        else if(coll.gameObject.tag == "Player")
        {
            if(noBacksiesTimer <= 0.0f)
            {
                if(playerWithBomb == playerIndex)
                {
                    PlayerControls otherPlayersControls = coll.gameObject.GetComponent<PlayerControls>();
                    playerWithBomb = otherPlayersControls.playerIndex;
                    PlaySound(powerUp);
                }
                else
                {
                    playerWithBomb = playerIndex;
                    PlaySound(powerUp);

                }

                noBacksiesTimer = MAX_TIME_FOR_NO_BACKSIES;
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(900, 10, 100, 20), bombTimer.ToString());
    }

    void PlaySound(AudioClip clipToPlay)
    {        
        if(myAudioSource.isPlaying)
        {
            myAudioSource.Stop();
        }

        float vol = Random.Range(0.4f, 0.8f);
        myAudioSource.PlayOneShot(clipToPlay, vol);
    }
}
