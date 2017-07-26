using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public LayerMask layer;
    public Transform pointStartRayCast;
    public GameObject objectSun;
    public GameObject objectRain;

    private float speed = 7;
    private float forceJump = 2.58f;
    private Rigidbody2D myBeautifulBody;
    private Vector2 startUpVelocity;
    private bool jumping = false;
    private bool isGrounded;
    private GameObject[] spawners;
    private WeatherManager weatherZone;

	// Use this for initialization
	void Start () {
        myBeautifulBody = GetComponent<Rigidbody2D>();
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        for (int i = 0; i < spawners.Length; i++) {
            print(spawners[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        startUpVelocity = myBeautifulBody.velocity;

        isGrounded = Physics2D.OverlapCircle(pointStartRayCast.position, 0.2f, layer.value);

        MovingPlayerX();
        Jump();
	}

    private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Ground") {
			isGrounded = true;
			jumping = false;
		}
    }

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

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.tag == "Ground")
            isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "WeatherZone")
            weatherZone = collision.gameObject.GetComponent<WeatherManager>();
    }

    private void Sun() {
        weatherZone.set_sun();
    }

    private void Rain() {
        weatherZone.set_rain();
    }
}
