using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public LayerMask layer;
    public Transform pointStartRayCast;
    public GameObject objectSun;
    public GameObject objectRain;

    private float speed = 7;                // Set the player max speed
    private float forceJump = 2.58f;        // Set the play jumping force
    private Rigidbody2D myBeautifulBody;
    private Vector2 startUpVelocity;        // Get the player velocity on the start to prevent weird Y axis behavior
    private bool jumping = false;           // Set the player jump at false so he can jump when grounded
    private bool isGrounded;                // Set the player grounded status at false so he can jump when grounded
    private WeatherManager weatherManager;     // Getting the WeatherManager from the weatherZone gameObject

	// Use this for initialization
	void Start () {
        myBeautifulBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        startUpVelocity = myBeautifulBody.velocity;
        isGrounded = Physics2D.OverlapCircle(pointStartRayCast.position, 0.2f, layer.value);

        MovingPlayerX();
        Jump();
	}

    // Input from the player to move itself to the right or left and a jumping function

    private void MovingPlayerX() {
        float direction = Input.GetAxis("Horizontal");
        myBeautifulBody.velocity = new Vector2(direction * speed, startUpVelocity.y);
        if (Input.GetKeyDown("x")) {
            myBeautifulBody.velocity = new Vector2(direction * speed * 10, startUpVelocity.y);
        }
    }

    private void Jump() {
        bool jump = Input.GetAxis("Jump") > 0;
		if (jump && !jumping) {
			jumping = true;
			myBeautifulBody.AddForce(Vector2.up * forceJump, ForceMode2D.Impulse);
		}
		else jumping &= !isGrounded;
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

    // Detect when the player enter in a weather zone and get its WeatherManager script
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "WeatherZone")
            weatherManager = collision.gameObject.GetComponent<WeatherManager>();
    }

    // Player keyboard input to lauch a weather type (f -> invoke sun, r -> invoke rain)

    private void Sun() {
        if (Input.GetKey("f"))
            weatherManager.set_sun();
    }

    private void Rain() {
        if (Input.GetKey("r"))
            weatherManager.set_rain();
    }
}
