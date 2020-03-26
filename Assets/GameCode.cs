using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameCode : MonoBehaviour {

    public GameObject playerPrefab, robotPrefab, crashPrefab;
    public AudioClip crashSound, gameOverSound, smashSound, teleportSound;
    public static bool moved = true;
    public static int level = 0, score = 0, turns = 0, killed = 0, teleports = 0;
    private GameObject robotParent, crashParent;
    private AudioSource audioSource;
    
    private static int  xSpaces = 40,
                ySpaces = 30,
                initRobots = 20,
                perLevelAdd = 10;

    public string[,] currentState = new string[xSpaces, ySpaces];

    // Use this for initialization
    void Start () {
        level = 0; score = 0; turns = 0; killed = 0; teleports = 0;

        robotParent = GameObject.Find("Robots");
        crashParent = GameObject.Find("Crashes");
        if (!robotParent)
        {
            robotParent = new GameObject("Robots");
        }
        if (!crashParent)
        {
            crashParent = new GameObject("Crashes");
        }

        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogError("No AudioSource found");
        }

        currentState = BlankLevel(currentState);
        PopulateLevel(level);
        DrawLevel();
        CheckForInput();
    }

    void CheckForInput()
    {
            if (!moved)
            {
                string direction = Buttons.moveDirection;
                if (direction == "")
                {
                    Debug.LogError("Direction not defined");
                }
                MovePlayer(direction);
            }
    }

    // Update is called once per frame
       void Update () {
        CheckForInput();
    }

    void CheckWin()
    {
        for (int i = 0; i < xSpaces; i++)
        {
            for (int j = 0; j < ySpaces; j++)
            {
                if (currentState[i, j] == "r")
                {
                    return;
                }
            }
        }
        level += 1;
        PopulateLevel(level);
        DrawLevel();
        return;
    }

    void MoveRobots ()
    {
        int playerX = PlayerPosition()[0];
        int playerY = PlayerPosition()[1];
        string[,] newState = new string[xSpaces, ySpaces];
        newState = BlankLevel(newState);
        for (int i = 0; i < xSpaces; i++)
        {
            for (int j = 0; j < ySpaces; j++)
            {
                if (currentState[i, j] == "p" || currentState[i, j] == "c" || currentState[i, j] == "0")
                {
                    newState[i, j] = currentState[i, j]; // copy player, junkpiles and blanks
                }
            }
        }

        for (int i = 0; i < xSpaces; i++)
        {
            for (int j = 0; j < ySpaces; j++)
            {
                if (currentState[i,j] == "r")  // move the robots towards player
                {
                    int moveX = 0;
                    int moveY = 0;
                    if (playerX > i)
                    {
                        moveX = 1;
                    }
                    else if (playerX < i)
                    {
                        moveX = -1;
                    }

                    if (playerY > j)
                    {
                        moveY = 1;
                    }
                    else if (playerY < j)
                    {
                        moveY = -1;
                    }
                    int destX = i + moveX;
                    int destY = j + moveY;
                    string dest = newState[destX, destY]; // what is in robot's destination square?
                    //Debug.Log("dest of " + destX + ", " + destY + " = " + dest);

                    if (dest == "p")  // Robot crashes into player = game over
                    {
                        //    Debug.Log("Robot crashes into Player at " + destX + ", " + destY);
                        audioSource.clip = gameOverSound;
                        audioSource.Play();
                        Invoke("GameOver", 4.0f);
                    }
                    else if (dest == "c")  // Robot crashes into junkpile and is eliminanted
                    {
                    //    Debug.Log("Robot crashes into junkpile at " + destX + ", " + destY);
                        newState[i, j] = "0";
                        score += 1;
                        killed += 1;
                        audioSource.clip = smashSound;
                        audioSource.Play();
                    }
                    else if (dest == "r")  // Robots crashes into another robot, creating junkpile
                    {
                    //    Debug.Log("Robot crashes into robot at " + destX + ", " + destY);
                        newState[i, j] = "0";
                        newState[destX, destY] = "c";
                        score += 2;
                        killed += 1;
                        audioSource.clip = crashSound;
                        audioSource.Play();
                    }
                    else if (dest == "0") // Destination clear, robots moves there
                    {
                    //    Debug.Log("Robot moves into " + destX + ", " + destY);
                        newState[i, j] = "0";
                        newState[destX, destY] = "r";
                    }
                    else
                    {
                    //    Debug.LogError("newState destination " + destX + " , " + destY + " = " + dest);
                    //    Debug.LogError("i = " + i + ", j = " + j + ", moveX = " + moveX + ", moveY = " + moveY);
                    }
                }
            }
        }
        //print("newState");
        //ReportState(newState);
        currentState = newState;
        turns += 1;
        DrawLevel();
        CheckWin();
        return;
    }

    void ReportState(string [,] state)
    {
        for (int i = 0; i < xSpaces; i++)
        {
            for (int j = 0; j < ySpaces; j++)
            {
                if (state[i, j] == "r")
                {
                    print("Robot at : " + i + ", " + j);
                }
                else if (state[i, j] == "p")
                {
                    print("Player at : " + i + ", " + j);
                }
                else if (state[i, j] == "c")
                {
                    print("Crash at : " + i + ", " + j);
                }
            }
        }
    }

    int[] PlayerPosition ()
    {
        int[] playerPos = new int[2];
        for (int i = 0; i < xSpaces; i++)
        {
            for (int j = 0; j < ySpaces; j++)
            {
                if (currentState[i, j] == "p")
                {
                    playerPos[0] = i;
                    playerPos[1] = j;
                }
            }
        }
        return playerPos;
    }

    void MovePlayer (string direction) {
        moved = true;

        int playerX = PlayerPosition()[0];
        int playerY = PlayerPosition()[1];

        int movePlayerX = 0, movePlayerY = 0;
        if (direction == "UL")
        {
            movePlayerX = -1;
            movePlayerY = 1;
        }
        else if (direction == "U")
        {
            movePlayerX = 0;
            movePlayerY = 1;
        }
        else if (direction == "UR")
        {
            movePlayerX = 1;
            movePlayerY = 1;
        }
        else if (direction == "R")
        {
            movePlayerX = 1;
            movePlayerY = 0;
        }
        else if (direction == "DR")
        {
            movePlayerX = 1;
            movePlayerY = -1;
        }
        else if (direction == "D")
        {
            movePlayerX = 0;
            movePlayerY = -1;
        }
        else if (direction == "DL")
        {
            movePlayerX = -1;
            movePlayerY = -1;
        }
        else if (direction == "L")
        {
            movePlayerX = -1;
            movePlayerY = 0;
        }
        else if (direction == "Hold")
        {
            movePlayerX = 0;
            movePlayerY = 0;
        }
        else if (direction == "Teleport")
        {
            bool empty = false;
            while (!empty)
            {
                int randX = Random.Range(0, xSpaces - 1);
                int randY = Random.Range(0, ySpaces - 1);
                if (currentState[randX, randY] == "0")
                {
                    currentState[playerX, playerY] = "0";
                    currentState[randX, randY] = "p";
                    score -= 5;
                    turns += 1;
                    teleports += 1;
                    audioSource.clip = teleportSound;
                    audioSource.Play();
                    DrawLevel();
                    return;
                }

            }
        }
        else if (direction == "StartOver")
        {
            SceneManager.LoadScene("Splash");
        }
        else if (direction == "Quit")
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            Debug.LogError("Direction = " + direction);
        }

        int newX = playerX + movePlayerX;
        int newY = playerY + movePlayerY;
        if (newX < 0 || newX >= xSpaces || newY < 0 || newY >= ySpaces)
        {
            return;
        }
        if (currentState[newX, newY] == "r" || currentState[newX, newY] == "c")
        {
            GameOver();
        }
        currentState[playerX, playerY] = "0";
        currentState[newX, newY] = "p";
        //ReportState(currentState);
        MoveRobots();
        return;
    }

    void GameOver()
    {
        //print("Game Over");
        SceneManager.LoadScene("GameOver");
    }

    void DrawLevel()
    {
        ClearScreen();
        for (int i = 0; i < xSpaces; i++)
        {
            for (int j = 0; j < ySpaces; j++)
            {
                Vector2 position = new Vector2(i, j);
                if (currentState[i,j] == "r") {
                    GameObject newGuy = Instantiate(robotPrefab, position, Quaternion.identity) as GameObject;
                    newGuy.transform.parent = robotParent.transform;
                }
                if (currentState[i,j] == "p") {
                    GameObject player = Instantiate (playerPrefab, position, Quaternion.identity) as GameObject;
                    player.name = "Player";
                }

                if (currentState[i,j] == "c") {
                    GameObject newCrash = Instantiate(crashPrefab, position, Quaternion.identity) as GameObject;
                    newCrash.transform.parent = crashParent.transform;
                }

            }
        }
    }

    void ClearScreen()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("pieces");
        foreach (Object item in pieces)
        {
            Destroy(item);
        }
    }

    string[,] BlankLevel(string [,] level)
    {
        for (int i = 0; i < xSpaces; i++)
        {
            for (int j = 0; j < ySpaces; j++)
            {
                level[i, j] = "0";
            }
        }
        return level;
    }

    void PopulateLevel(int level) {
        currentState = BlankLevel(currentState);
        int population = initRobots + (perLevelAdd * level);
        for (int p = 0; p < population; p++)
        {
            bool empty = false;
            while (!empty)
            {
                int x = Random.Range(0, xSpaces);
                int y = Random.Range(0, ySpaces);
                if (currentState[x,y] == "0")
                {
                    currentState[x, y] = "r";
                    empty = true;
                }
            }
        }

        bool empty2 = false;
        while (!empty2)
        {
            int x = Random.Range(0, xSpaces);
            int y = Random.Range(0, ySpaces);
            if (currentState[x, y] == "0")
            {
                currentState[x, y] = "p";
                empty2 = true;
            }
        }
        //ReportState(currentState);
        return;
    }
}
