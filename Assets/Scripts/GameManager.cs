/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The GameManager class manages the systems in charge of running the game and handling data
public class GameManager : MonoBehaviour
{
    public GameObject elevator;
    public Tunnel tunnel;
    public Aim aim;
    public static float speed = .3f;
    private List<GameObject> mobiles = new List<GameObject>();
    private SceneManager scenes;
    private ElevatorHealth health;
    private ScoringPoints score;

    // Start is called before the first frame update
    void Start()
    {
        if (Screen.fullScreen) Screen.fullScreen = false;

        health = elevator.GetComponent<ElevatorHealth>();
        scenes = GetComponent<SceneManager>();
        score = GetComponent<ScoringPoints>();
    }

    //Starts a mining expedition (main game)
    public void LoadGame()
    {
        tunnel.NewTunnle();        
        FollowerSpawner.pause = false;
        FollowerSpawner.enemyCount = 0;
        DepthGuage.ZeroOut();
        aim.TogglePause(false);
        score.ResetScore();
        health.RepairElevator(health.GetMaxHealth());
    }

    //Pauses mining expidition
    private void PauseGame()
    {
        tunnel.TogglePause(true);
        aim.TogglePause(true);
        FollowerSpawner.pause = true;

        for (int i = 0; i < FindObjectsOfType<Health>().Length - 1; i++)
        {    
            mobiles.Add(FindObjectsOfType<Health>()[i].gameObject);            
        }
        for (int i = 0; i < mobiles.Count; i++)
            if (mobiles[i] != elevator && mobiles[i] != null) mobiles[i].SetActive(false);
    }

    //Ends mining expedition
    public void GameOver()
    {
        PauseGame();
        score.SetHighScore();
        for (int i = 0; i < mobiles.Count; i++)
        {
            GameObject instance = mobiles[i];
            mobiles.Remove(instance);
            Destroy(instance);
        }

        scenes.ChooseState(3);
    }
}
