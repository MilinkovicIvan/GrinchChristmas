using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStats : MonoBehaviour
{
    //ref to localDB script
    public localDB localDB;
    

    //flag for keeping track if user is logged in 
    public static bool userLoggedin = false;
    //flag for tracking if this is first time game is run by user
    public static bool firstTimer = false;

    //sound/music flags
    public static bool musicOn = false;
    public static bool soundOn = true;

    //progress variables
    public static string username = "Grinch";  //will be updated when user login
    public static int currentLv;
    public static int lifeAmount;
    public static int goldAmount;
    public static string powerups;
    //public static List<string> powerups = new List<string>() { "2","1","1","0","0","0","0","0","0","0" };
 

    // Start is called before the first frame update
    void Start()
    {
        // get progress values from localDB and store them for later use
        localDB = GameObject.Find("GameManager").GetComponent<localDB>();
        currentLv = localDB.getCurrentLv();
        lifeAmount = localDB.getLifeAmount();
        goldAmount = localDB.getGoldAmount();
        powerups = localDB.getPowerups();

        //Debug.Log(firstTimer + " from gameStats");

/*
        Debug.Log(currentLv);
        Debug.Log(lifeAmount);
        Debug.Log(goldAmount);
        Debug.Log(powerups[1]);
*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
