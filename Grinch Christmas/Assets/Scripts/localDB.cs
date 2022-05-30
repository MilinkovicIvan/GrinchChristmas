using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite; //used for DB creation
using System.Data;  //used for data reader
using System.IO;    //used to check if file exist

public class localDB : MonoBehaviour
{
    // name of the db and its location
    private string localDBName = "URI=file:progress.db";
    //"URI=file:" + Application.persistentDataPath + "/Inventory.db";
    //private string localDBName = "URI=file:" + Application.persistentDataPath + "/progress.db";
    //private string localDBName = "URI=file:" + Application.persistentDataPath + "/progress.db";
    /*   private string localDBName;

       private void Awake()
       {
           localDBName = Application.persistentDataPath + "/progress.db";
       }*/

/*    private string localDBName;

    private void Awake()
    {
        localDBName = "URI=file:" + Application.persistentDataPath + "/progress.db";
    }*/

    // Start is called before the first frame update
    void Start()
    {
        //check if db exist
        //if (File.Exists(Application.persistentDataPath + "/progress.db"))
        if (File.Exists("progress.db"))
        {
            //Debug.Log("DB already exists");
        }
        //if not,create it
        else 
        {
            // creating local db
            createLocalDB();

            // adding values to progress table columns 
            addProgressValues(1, 5, 20, "2,1,1,0,0,0,0,0,0,0");

            //update gameStats values
            gameStats.firstTimer = true;
/*
            // displaying progress from db
            int currentLv = getCurrentLv();
            Debug.Log("Current LV: " + currentLv);
            int currentLife = getLifeAmount();
            Debug.Log("Current LIFE: " + currentLife);
            int currentGold = getGoldAmount();
            Debug.Log("Current GOLD: " + currentGold);
            string powerupsFromDB = getPowerups();
            Debug.Log("Powerups string from DB: " + powerupsFromDB);
            
            //tests for updates
            // updating progress
            updateCurrentLv(6);
            updateLifeAmount(3);
            updateGoldAmount(15);
            updatePowerups(0,"5");

            // displaying progress from db
            Debug.Log("AFTER UPDATES");

            int updatedLv = getCurrentLv();
            Debug.Log("Updated LV: " + updatedLv);
            int updatedLife = getLifeAmount();
            Debug.Log("Updated LIFE: " + updatedLife);
            int updatedGold = getGoldAmount();
            Debug.Log("Updated GOLD: " + updatedGold);
            string updatedPowerupsFromDB = getPowerups();
            Debug.Log("Updated powerups string from DB: " + updatedPowerupsFromDB);

            //test for update all values in progress table
            updateProgressValues(1, 5, 20, "2,1,1,0,0,0,0,0,0,0");

            // displaying progress from db
            updatedLv = getCurrentLv();
            Debug.Log("Updated LV: " + updatedLv);
            updatedLife = getLifeAmount();
            Debug.Log("Updated LIFE: " + updatedLife);
            updatedGold = getGoldAmount();
            Debug.Log("Updated GOLD: " + updatedGold);
            updatedPowerupsFromDB = getPowerups();
            Debug.Log("Updated powerups string from DB: " + updatedPowerupsFromDB);
*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // method for creating table if it doesnt exist already
    public void createLocalDB() {
        // db connection
        using (var connection = new SqliteConnection(localDBName)) {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand()) {
                // creating the table
                command.CommandText = "CREATE TABLE IF NOT EXISTS progress (currentLv INT, lifeAmount INT, goldAmount INT, powerups VARCHAR(50));";
                command.ExecuteNonQuery();
            }
            //close connection
            connection.Close();              
        }
    }

    // method which will insert values into the progress table
    public void addProgressValues(int lv, int life, int gold, string powerups) {
        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                // inserting into the table
                command.CommandText = "INSERT INTO progress (currentLv, lifeAmount, goldAmount, powerups) VALUES ('" + lv + "', '" + life + "', '" + gold + "', '" + powerups + "');";
                command.ExecuteNonQuery();
            }
            //close connection
            connection.Close();
        }
    }

    // method which will update values in the progress table
    public void updateProgressValues(int lv, int life, int gold, string powerups)
    {
        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                // updating progress table
                command.CommandText = "UPDATE progress SET currentLv = '" + lv + "', lifeAmount = '" + life + "', goldAmount = '" + gold + "', powerups = '" + powerups + "';";
                command.ExecuteNonQuery();
            }
            //close connection
            connection.Close();
        }
    }

    //convert string to list method,returns list of powerups values
    public List<string> convertStringToList(string powerupsString) {
        //empty list
        List<string> listToReturn = new List<string>();

        //loop through given string, and add chars to list if they are not ","
        for (int i = 0; i < powerupsString.Length; i++)
        {
            if (powerupsString[i].ToString() is not ","){
                //Debug.Log("Adding to List: " + powerupsString[i]);
                listToReturn.Add(powerupsString[i].ToString());
            }
        }

        return listToReturn;
    }

    //convert list to string
    public string convertListToString(List<string> list) {
        // new string for return
        string returnString = "";

        // loop through list and make a new string with list values
        for (int i = 0; i < list.Count; i++)
        {
            // last list value will be added without ","
            if (i == 9)
            {
                returnString = returnString + list[i].ToString();
            }
            else { 
                returnString = returnString + list[i].ToString() + ",";
            }          
        }
        return returnString;   
    }

    // method to gets powerups from progress table and returns them as string
    public string getPowerups()
    {
        string powerups = "empty";

        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                //selecting powerups from progress table
                command.CommandText = "SELECT powerups FROM progress;";

                //iterating through progress set
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        powerups = reader["powerups"].ToString();
                        //Debug.Log("Powerups string from DB: " + reader["powerups"]);
                    }
                    reader.Close();
                }
            }
            //close connection
            connection.Close();

            return powerups;
        }
    }

    //update powerups
    public void updatePowerups(int position, string value)
    {
        // getting current powerups from DB
        string currentStringFromDB = getPowerups();
        
        //converting that string to list
        List<string> powerupsList = convertStringToList(currentStringFromDB);

        /*
        // testing to see content of list
        for (int i = 0; i < powerupsList.Count; i++)
        {
            Debug.Log("Powerups from list: " + powerupsList[i]);
        }
        */

        //updating values in that list with new
        powerupsList[position] = value;

        //converting list to string for DB
        string powerupsStringForUpdate = convertListToString(powerupsList);
        //Debug.Log("Powerups string for DB update: " + powerupsStringForUpdate);

        //updating powerups string in DB
        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                // updating lifeAmount
                command.CommandText = "UPDATE progress SET powerups = '" + powerupsStringForUpdate + "';";
                command.ExecuteNonQuery();
            }
            //close connection
            connection.Close();
        }
    }

    // method that will update currentLv in progress table
    public void updateCurrentLv(int value)
    {
        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                // updating currentLv
                command.CommandText = "UPDATE progress SET currentLv = '" + value + "';";
                command.ExecuteNonQuery();
            }
            //close connection
            connection.Close();
        }
    }

    // method that will update lifeAmount in progress table
    public void updateLifeAmount(int value)
    {
        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                // updating lifeAmount
                command.CommandText = "UPDATE progress SET lifeAmount = '" + value + "';";
                command.ExecuteNonQuery();
            }
            //close connection
            connection.Close();
        }
    }

    // method that will update goldAmount in progress table
    public void updateGoldAmount(int value)
    {
        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                // updating goldAmount
                command.CommandText = "UPDATE progress SET goldAmount = '" + value + "';";
                command.ExecuteNonQuery();
            }
            //close connection
            connection.Close();
        }
    }

    // method which will show currentLv in progress table
    public int getCurrentLv() {
        int currentLv = -1;
        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                //selecting currentLv from progress table
                command.CommandText = "SELECT currentLv FROM progress;";

                //iterating through progress set
                using (IDataReader reader = command.ExecuteReader()) 
                {
                    if (reader.Read()) 
                    {
                        currentLv = (int) reader["currentLv"];
                        //Debug.Log("Current LV: " + reader["currentLv"]);
                    }                                                                                     
                    reader.Close();                                     
                }              
            }
            //close connection
            connection.Close();

            return currentLv;
        }
    }

    // method which will show lifeAmount in progress table
    public int getLifeAmount()
    {
        int currentLife = -1;
        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                //selecting lifeAmount from progress table
                command.CommandText = "SELECT lifeAmount FROM progress;";

                //iterating through progress set
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentLife = (int) reader["lifeAmount"];
                        //Debug.Log("Life: " + reader["lifeAmount"]);
                    }
                    reader.Close();
                }
            }
            //close connection
            connection.Close();

            return currentLife;
        }
    }

    // method which will show goldAmount in progress table
    public int getGoldAmount()
    {
        int currentGold = -1;
        // db connection
        using (var connection = new SqliteConnection(localDBName))
        {
            //open connection
            connection.Open();

            // command object for db control
            using (var command = connection.CreateCommand())
            {
                //selecting goldAmount from progress table
                command.CommandText = "SELECT goldAmount FROM progress;";

                //iterating through progress set
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentGold = (int) reader["goldAmount"];
                        //Debug.Log("Gold: " + reader["goldAmount"]);
                    }
                    reader.Close();
                }
            }
            //close connection
            connection.Close();

            return currentGold;
        }
    }
}
