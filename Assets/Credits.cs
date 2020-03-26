using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Splash");
    }

    public void Donate()
    {
        Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=QUWF5WHNGWHJL");
    }
}
