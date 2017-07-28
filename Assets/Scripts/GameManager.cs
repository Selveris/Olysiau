﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI; // include UI namespace so can reference UI elements
using UnityEngine.SceneManagement; // include so we can manipulate SceneManager

public class GameManager : MonoBehaviour {

	// static reference to game manager so can be called from other scripts directly (not just through gameobject component)
	public static GameManager gm;

	// levels to move to on victory and lose
	public string levelAfterVictory;
	public string levelAfterGameOver;

    // UI element to control
    public GameObject UIGamePaused;
    public Text UIScore;
    public Text UIHighScore;


    private PlayerController playerController;

    // private variables
    GameObject _player;
	Scene _scene;
    public int _score = 0;
    int _highscore = 0;
    private GameObject cameraDeath;
    private GameObject plantDead;
    private bool moveCamera;
    private float cameraTransitionSpeed = 7;
    private bool onPause;

	// set things up here
	void Awake () {
        // setup reference to game manager
        UIGamePaused.SetActive(false);
        if (gm == null)
			gm = this.GetComponent<GameManager>();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

		// setup all the variables, the UI, and provide errors if things not setup properly.
		SetupDefaults();
        cameraDeath = GameObject.Find("Main Camera");
    }

    void Start() {
        _highscore = GetHighscore();
    }

	// game loop
	void Update() {
		// if ESC pressed then pause the game
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (Time.timeScale > 0f) {
				UIGamePaused.SetActive(true); // this brings up the pause UI
				Time.timeScale = 0f; // this pauses the game action
                onPause = true;
			} else {
				Time.timeScale = 1f; // this unpauses the game action (ie. back to normal)
				UIGamePaused.SetActive(false); // remove the pause UI
                onPause = false;
			}
		}

        RefreshGUI();
        

        if (moveCamera){
			cameraDeath.transform.position = Vector3.Lerp( cameraDeath.transform.position, new Vector3(
                plantDead.transform.position.x, cameraDeath.transform.position.y , cameraDeath.transform.position.z),
												           cameraTransitionSpeed * Time.deltaTime);
        }
	}

    public bool GameOnPause()
    {
        return onPause;
    }

	// setup all the variables, the UI, and provide errors if things not setup properly.
	void SetupDefaults() {
		// setup reference to player
		if (_player == null)
			_player = GameObject.FindGameObjectWithTag("Player");
		
		if (_player==null)
			Debug.LogError("Player not found in Game Manager");

		// get current scene
		_scene = SceneManager.GetActiveScene();

		// if levels not specified, default to current level
		if (levelAfterVictory=="") {
			Debug.LogWarning("levelAfterVictory not specified, defaulted to current level");
			levelAfterVictory = _scene.name;
		}
		
		if (levelAfterGameOver=="") {
			Debug.LogWarning("levelAfterGameOver not specified, defaulted to current level");
			levelAfterGameOver = _scene.name;
		}

		
		if (UIGamePaused==null)
			Debug.LogError ("Need to set UIGamePaused on Game Manager.");
        if (UIScore == null)
            Debug.LogError("Need to set UIScore on Game Manager.");

        RefreshGUI();

    }

    void RefreshGUI()
    {
        UIScore.text = "Score: " + _score.ToString();
        UIHighScore.text = "Meilleur score: " + _highscore.ToString();
    }

    public void OnePlantDied (GameObject plant)
    {
        plantDead = plant;
        playerController.SetGameOver(true);
        StartCoroutine(GameOver());
    }

    public void OnePlantWasCollected (GameObject plant)
    {
        _score++;
    }

    public static int GetHighscore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            return PlayerPrefs.GetInt("Highscore");
        }
        else
        {
            return 0;
        }
    }

    public static void SetHighscore(int highscore)
    {
        PlayerPrefs.SetInt("Highscore", highscore);
    }

    public static void SavePlayerState(int score, int highScore, int lives)
    {
        PlayerPrefs.SetInt("Highscore", highScore);
    }

    // reset stored player state and variables back to defaults
    public static void ResetPlayerState()
    {
            PlayerPrefs.SetInt("Highscore", 0);
    }

    IEnumerator GameOver() {
        if(_score > _highscore)
        {
            _highscore = _score;
            SetHighscore(_highscore);
        }
        moveCamera = true;
        yield return new WaitForSeconds(3.5f);
        moveCamera = false;
        // load GameOver Scene
		SceneManager.LoadScene(levelAfterGameOver);
	}

	// public function for level complete
	private void LevelCompete() {

		// use a coroutine to allow the player to get fanfare before moving to next level
		StartCoroutine(LoadNextLevel());
	}

	// load the nextLevel after delay
	IEnumerator LoadNextLevel() {
		yield return new WaitForSeconds(3.5f);
		SceneManager.LoadScene(levelAfterVictory);
	}
}
