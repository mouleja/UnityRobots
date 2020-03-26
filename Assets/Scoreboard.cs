using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

    private Text levelText, scoreText;

	// Use this for initialization
	void Start () {
        levelText = GameObject.Find("Level").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        levelText.text = "Level: " + GameCode.level.ToString();
        scoreText.text = " Score: " + GameCode.score.ToString();
	}
}
