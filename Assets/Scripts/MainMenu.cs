using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{	
	public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
	}

    public void Quit()
    {
#if UNITY_EDITOR
        if (Application.isEditor)
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
