using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

    public float splashDelay = 10.0f;

	// Use this for initialization
	void Start () {
        Invoke("Delay", splashDelay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Delay ()
    {
        SceneManager.LoadScene("Game");
        return;
    }
}
