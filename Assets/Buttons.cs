using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

    public static string moveDirection = "";

    void OnMouseDown()
    {
        //Debug.Log(name + " Button Pressed");
        moveDirection = name;
        GameCode.moved = false;
    }

}
