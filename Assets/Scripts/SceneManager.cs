/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

//The SceneManager class handles which scene is to be displayed and active
public class SceneManager : MonoBehaviour
{
    public GameObject[] gameState;
    public int state;
    public GameObject instance;
    public Vector3 origin;
    public Camera cam;

    // Use this for initialization
    void Start()
    {
        state = 0;
        ChooseState(0);
    }

    //Removes current state and loads the next state
    public void LoadState()
    {
        Vector2 statePos = gameState[state].gameObject.transform.position;
        cam.transform.position = new Vector3(statePos.x, statePos.y, -10);
    }


    //Assign state to a specified state (UI Button Press)
    public void ChooseState()
    {
        string selectState = EventSystem.current.currentSelectedGameObject.name;//Takes the name of button pressed to call this function 
        string index = selectState.Substring(selectState.IndexOf("e") + 1);//Finds the assigned number of the button in the name
        int.TryParse(index, out state);//Turns string number into int number
        LoadState(); //Loads specific state
    }

    //Assign state to a specified state (Scripting)
    public void ChooseState(int state)
    {
        this.state = state;
        LoadState();
    }
}
