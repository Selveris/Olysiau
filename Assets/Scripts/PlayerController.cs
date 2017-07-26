using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public LayerMask layer;

    private float speed = 7;                    // Set the player max speed
    private float forceJump = 2.58f;            // Set the play jumping force
    private Rigidbody2D playerRigidBody;
    private Vector2 startUpVelocity;            // Get the player velocity on the start to prevent weird Y axis behavior
    private bool jumping = false;               // Set the player jump at false so he can jump when grounded
    private bool isGrounded;                    // Set the player grounded status at false so he can jump when grounded
    private WeatherManager weatherManager;      // Getting the WeatherManager from the weatherZone gameObject
    private Vector3 weatherZonePosition;        // Getting the WeatherZone position when there's a collision with the player
    private Camera camera;                      // Getting the camera position to do a translation to the WeatherZone position
    private float cameraTransitionSpeed = 7;    // Setting the translation speed between the WeatherZone

	// Use this for initialization
	void Start () {
        playerRigidBody = GetComponent<Rigidbody2D>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        startUpVelocity = playerRigidBody.velocity;

        PlayerKeyboardInputs();

        camera.transform.position = Vector3.Lerp(camera.transform.position, weatherZonePosition + new Vector3(0,0,-10), cameraTransitionSpeed * Time.deltaTime);
	}

    // Input from the player to move itself to the right or left and a jumping function

    private void PlayerKeyboardInputs () {
        MovingPlayerX();
        Jump();
        Sun();
        Rain();
    }

    private void MovingPlayerX() {
        float direction = Input.GetAxis("Horizontal");
        playerRigidBody.velocity = new Vector2(direction * speed, startUpVelocity.y);
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

	// Player keyboard input to lauch a weather action (f -> invoke sun, r -> invoke rain)

	private void Sun() {
	    if (Input.GetKeyDown("f"))
	        weatherManager.set_sun();
	}

	private void Rain() {
	    if (Input.GetKeyDown("r"))
	        weatherManager.set_rain();
	}

    // Detect when the player is in contact with the ground (gameObject tagged with "Ground")
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Ground") {
			isGrounded = true;
			jumping = false;
		}
	}

    // Detect when the player is not in contact with ground anymore

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.tag == "Ground")
            isGrounded = false;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "WeatherZone") {
            weatherManager = collision.gameObject.GetComponent<WeatherManager>();
            weatherZonePosition = collision.transform.position;
        }
    }
}
