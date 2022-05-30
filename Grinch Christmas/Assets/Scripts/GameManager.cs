using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //gamemanager singleton
    public static GameManager instance { get; private set; }

    public void Awake() {
        // if instance doesnt exist create it
        if (instance == null)
        {
            instance = this;
            // when scene is loaded, restarted, dont destroy the object
            //DontDestroyOnLoad(gameObject);
        }
        // else destroy it
        else if (instance != null){
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
