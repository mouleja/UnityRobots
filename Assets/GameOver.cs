using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    private Text levelText, scoreText, turnsText, teleportsText, killedText;

    // Use this for initialization
    void Start () {
        levelText = GameObject.Find("Level").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        turnsText = GameObject.Find("Turns").GetComponent<Text>();
        teleportsText = GameObject.Find("Teleports").GetComponent<Text>();
        killedText = GameObject.Find("Killed").GetComponent<Text>();

        levelText.text = "Final Level: " + GameCode.level.ToString();
        scoreText.text = "Score: " + GameCode.score.ToString();
        turnsText.text = "Turns Survived: " + GameCode.turns.ToString();
        teleportsText.text = "Teleports: " + GameCode.teleports.ToString();
        killedText.text = "Robots killed: " + GameCode.killed.ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Quit ()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Splash");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Donate()
    {Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=QUWF5WHNGWHJL");
    }
}
