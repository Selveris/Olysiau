using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public LayerMask layer;
	public Sprite walkingSprite;
	public Sprite dancingSprite;

    private float speed = 7;                        // Set the player max speed
    private float forceJump = 2.58f;                // Set the play jumping force
    private Rigidbody2D playerRigidBody;            // Get the player RigidBody to change the player velocity
    private Vector2 startUpVelocity;                // Get the player velocity on the start to prevent weird Y axis behavior
    private bool jumping;                           // Set the player jump at false so he can jump when grounded
    private bool isGrounded;                        // Set the player grounded status at false so he can jump when grounded
    private WeatherManager weatherManager;          // Getting the WeatherManager from the weatherZone gameObject
    private Vector3 weatherZonePosition;            // Getting the WeatherZone position when there's a collision with the player
    private Camera camera;                          // Getting the camera position to do a translation to the WeatherZone position
    private float cameraTransitionSpeed = 7;        // Setting the translation speed between the WeatherZone
    private bool danceMode;                         // Setting the player mode (normal mode and danse mode)
    private SpriteRenderer playerSpriteRenderer;    // Get the player SpriteRenderer to modify the sprite ingame

	// Use this for initialization
	void Start () {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        danceMode = false;
        // transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
	}
	
	// Update is called once per frame
	void Update () {
        startUpVelocity = playerRigidBody.velocity;

        PlayerKeyboardInputs();

        camera.transform.position = Vector3.Lerp(camera.transform.position, 
                                                 weatherZonePosition + new Vector3(0,0,-10), 
                                                 cameraTransitionSpeed * Time.deltaTime);
	}

	/* Input from the player :
     * MovingPlayerX()  : move the player to the right or left
     * Jump()           : the function name itself is explanatory enough
     * DanseMode()      : the player switch to the dance mode to... dance and a normal mode in which he can do the two above
     */

	private void PlayerKeyboardInputs () {
        if (!danceMode) {
            MovePlayerX();
            Jump();
        } else {
            WeatherControl();
        }

        DanceMode();
    }

    private void MovePlayerX() {
        float direction = Input.GetAxis("Horizontal");
        playerRigidBody.velocity = new Vector2(direction * speed, startUpVelocity.y);
        playerSpriteRenderer.sprite = walkingSprite;
        if (direction > 0) {
            FlipSprite(0);
        } else if (direction < 0) {
            FlipSprite(180);
        }
        // Beginning of the function Dash
        /*if (Input.GetKeyDown("x")) {
            playerRigidBody.velocity = new Vector2(direction * speed * 10, startUpVelocity.y);
        }*/
    }

    private void Jump() {
        bool jump = Input.GetAxis("Jump") > 0;
		if (jump && !jumping) {
			jumping = true;
			playerRigidBody.AddForce(Vector2.up * forceJump, ForceMode2D.Impulse);
		}
		else jumping &= !isGrounded;
    }

	private void DanceMode() {
		if (Input.GetKeyDown(KeyCode.LeftControl)) {
            playerSpriteRenderer.sprite = dancingSprite;
			if (!danceMode)
				danceMode = true;
			else
				danceMode = false;
		}
	}

	// Flip the sprite of the player to a defined angle (0 for normal direction and 180 to turn to the other side)
	private void FlipSprite(float angle) {
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

	// Player keyboard input to lauch a weather action (f -> invoke sun, r -> invoke rain)

	private void WeatherControl() {
	    if (Input.GetKeyDown("f"))
            weatherManager.change_weather();
	}

    // Detect when the player is in contact with the ground (gameObject tagged with "Ground")
	private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag.Equals("Ground")) {
			isGrounded = true;
			jumping = false;
		}
	}

    // Detect when the player is not in contact with ground anymore

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded &= !collision.transform.tag.Equals("Ground");
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag.Equals("WeatherZone")) {
            weatherManager = collision.gameObject.GetComponent<WeatherManager>();
            weatherZonePosition = collision.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals("Plant")) {
            GameManager.gm.OnePlantWasCollected(collision.gameObject);
            collision.GetComponent<SeedManager>().haverest();
        }
    }
}
