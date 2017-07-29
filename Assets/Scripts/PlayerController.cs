﻿﻿using System.Collections;﻿
﻿using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public LayerMask layer;
    public GameObject Lightning;

    private float speed;                        // Set the player max speed
    private Rigidbody2D playerRigidBody;            // Get the player RigidBody to change the player velocity
    private Vector2 startUpVelocity;                // Get the player velocity on the start to prevent weird Y axis behavior
    private WeatherManager weatherManager;          // Getting the WeatherManager from the weatherZone gameObject
    private Vector3 weatherZonePosition;            // Getting the WeatherZone position when there's a collision with the player
    private Camera cameraTranslate;                 // Getting the camera position to do a translation to the WeatherZone position
    private float cameraTransitionSpeed = 7;        // Setting the translation speed between the WeatherZone
    private bool danceMode;                         // Setting the player mode (normal mode and danse mode)   // Get the player SpriteRenderer to modify the sprite ingame
    private Animator playerAnimator;                // 
    private SequenceManager sequenceManager;        // 
    private ShoutManager shoutManager;
    private EatingNoiseManager eatingNoiseManager;
    private SpriteRenderer playerRenderer;
    private string resourcesMoveset = "Sprites/DanceMoveset/";
    private string resourcesStun = "Sprites/StunSet/";
    private Sprite dancePoseQ;
    private Sprite dancePoseW;
    private Sprite dancePoseE;
    private Sprite dancePoseA;
    private Sprite dancePoseS;
    private Sprite dancePoseD;
    private Sprite stunedPose1;
    private Sprite stunedPose2;
    private Sprite stunedPose3;
    private bool stun;
    private bool gameOver;

	// Use this for initialization
	void Start () {
        playerRigidBody = GetComponent<Rigidbody2D>();
        cameraTranslate = GameObject.Find("Main Camera").GetComponent<Camera>();
        sequenceManager = GetComponent<SequenceManager>();
        shoutManager = GetComponentInChildren<ShoutManager>();
        eatingNoiseManager = GetComponentInChildren<EatingNoiseManager>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerRenderer = GetComponentInChildren<SpriteRenderer>();
        dancePoseQ = Resources.Load<Sprite>(resourcesMoveset + "dance_4_928x1370");
        dancePoseW = Resources.Load<Sprite>(resourcesMoveset + "dance_1_928x1370");
        dancePoseE = Resources.Load<Sprite>(resourcesMoveset + "dance_2_928x1370");
        dancePoseA = Resources.Load<Sprite>(resourcesMoveset + "dance_3_928x1370");
        dancePoseS = Resources.Load<Sprite>(resourcesMoveset + "dance_5_928x1370");
        dancePoseD = Resources.Load<Sprite>(resourcesMoveset + "dance_6_928x1370");
        stunedPose1 = Resources.Load<Sprite>(resourcesStun + "stun_elec_1_928x1370");
        stunedPose2 = Resources.Load<Sprite>(resourcesStun + "stun_elec_2_928x1370");
        stunedPose3 = Resources.Load<Sprite>(resourcesStun + "stun_static_928x1370");

        danceMode = false;
        stun = false;
        gameOver = false;

        speed = 6;
	}
	
	// Update is called once per frame
	void Update () {
        startUpVelocity = playerRigidBody.velocity;

        if (!GameManager.gm.GetComponent<GameManager>().GameOnPause()){
            PlayerKeyboardInputs();
        }

        if (!gameOver)
            cameraTranslate.transform.position = Vector3.Lerp(cameraTranslate.transform.position, 
                                                 weatherZonePosition + new Vector3(0,0,-10), 
                                                 cameraTransitionSpeed * Time.deltaTime);
	}

    public void CompleteSequence() {
        weatherManager.change_weather();
    }

    /* Input from the player :
     * MovingPlayerX()  : move the player to the right or left
     * Jump()           : the function name itself is explanatory enough
     * DanseMode()      : the player switch to the dance mode to... dance and a normal mode in which he can do the two above
     */

    private void PlayerKeyboardInputs () {
        if (!danceMode) {
            MovePlayerX();
            playerAnimator.enabled = true;
        } else {
            WeatherControl();
            playerAnimator.enabled = false;
        }

        if (!stun)
            if (Input.GetKeyDown(KeyCode.Space)) {
                DanceMode();
            }
    }

    private void MovePlayerX() {
        float direction = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Abs(direction) > 0.001f;
        playerAnimator.SetBool("isMoving", isMoving);
        playerRigidBody.velocity = new Vector2(direction * speed, startUpVelocity.y);
        if (direction > 0) {
            playerRenderer.flipX = false;
        } else if (direction < 0) {
            playerRenderer.flipX = true;
        }
    }

	public void DanceMode() {
        danceMode = !danceMode;
        if (danceMode) {
            sequenceManager.reset();
            sequenceManager.activateOutput();
        } else {
            sequenceManager.disableOutput();
        }
        playerAnimator.SetBool("isDancing", danceMode);
	}

	// Flip the sprite of the player to a defined angle (0 for normal direction and 180 to turn to the other side)
	private void FlipSprite(float angle) {
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

    // Player keyboard input to lauch a weather action (f -> invoke sun, r -> invoke rain)
    private void WeatherControl() {
        int id = 0;

		if (!stun)
		{
            if (Input.GetKeyDown("q"))
            {
                id = 1;
                playerRenderer.sprite = dancePoseQ;
            }
            else if (Input.GetKeyDown("w"))
            {
                id = 2;
                playerRenderer.sprite = dancePoseW;
            }
            else if (Input.GetKeyDown("e"))
            {
                id = 3;
                playerRenderer.sprite = dancePoseE;
            }
            else if (Input.GetKeyDown("a"))
            {
                id = 4;
                playerRenderer.sprite = dancePoseA;
            }
            else if (Input.GetKeyDown("s"))
            {
                id = 5;
                playerRenderer.sprite = dancePoseS;
            }
            else if (Input.GetKeyDown("d"))
            {
                id = 6;
                playerRenderer.sprite = dancePoseD;
            }
        }

        if (id != 0) {
            bool rightSymb = sequenceManager.addSymbol(id);
            if (!rightSymb) {
                print(shoutManager);
				shoutManager.PlayThunder();
				shoutManager.PlayCries();
                sequenceManager.reset();
                StartCoroutine(StunTime());
            } else {
                shoutManager.PlayVoices();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag.Equals("WeatherZone")) {
            weatherManager = collision.gameObject.GetComponent<WeatherManager>();
            weatherZonePosition = collision.transform.position;
        }

        if (collision.tag.Equals("Plant") && collision.GetComponent<SeedManager>().isReady())
        {
            GameManager.gm.OnePlantWasCollected(collision.gameObject);
            collision.GetComponent<SeedManager>().haverest();
            eatingNoiseManager.PlayEatings();
            if(speed < 12)
            {
                speed += 0.2f;
            }
        }
    }

    public void SetGameOver(bool isGameOver) {
        gameOver = isGameOver;
    }

    IEnumerator StunTime () {
        stun = true;

        GameObject L = Instantiate(Lightning, gameObject.transform, true);
        L.transform.position = playerRenderer.gameObject.transform.position + new Vector3(0f, 6f, 0.1f);
        yield return new WaitForSeconds(0.15f);

        playerRenderer.sprite = stunedPose1;
        for (int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(0.06f);
            playerRenderer.sprite = stunedPose2;
            yield return new WaitForSeconds(0.06f);
            playerRenderer.sprite = stunedPose1;
        }
        yield return new WaitForSeconds(0.25f);
        playerRenderer.sprite = stunedPose3;
        stun = false;
        Destroy(L);
    }
}
