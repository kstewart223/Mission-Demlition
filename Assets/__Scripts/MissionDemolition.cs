using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameMode {
        idle,
        playing,
        levelEnd
    }
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitShots;
    public Vector3 castlePos;
    public Vector3 blockPos;
    public GameObject[] castles;
    public GameObject[] blocks;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameObject block;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        if(castle != null)
        {
            Destroy(castle);
            Destroy(block);
        }
        Projectile.DESTROY_PROJECTILES();

        castle  = Instantiate<GameObject>(castles[level]);
        block   = Instantiate<GameObject>(blocks[level]);
        castle.transform.position = castlePos;
        block.transform.position = blockPos;

        Goal.goalMet = false;
        
        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW(FollowCam.eView.both);

    }

    void UpdateGUI()
    {
        uitLevel.text = "Level:" +(level+1) +" of "+levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();
        if((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
            shotsTaken = 0;
            SceneManager.LoadScene("_Scene_1");
        }
        StartLevel();
    }

    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }
    
    // static public GameObject GET_BLOCK();
    // {
    //     return S.block;
    // }
    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }

}
