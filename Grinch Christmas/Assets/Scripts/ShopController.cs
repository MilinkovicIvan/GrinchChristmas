using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;   //used for GetNumericValue

public class ShopController : MonoBehaviour
{
    //link to shop alert message
    public TextMeshProUGUI shopAlertMessage;
    //link to life and gold text
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI goldText;
    //link to inv powerups texts
    public TextMeshProUGUI hammerText;
    public TextMeshProUGUI bombText;
    public TextMeshProUGUI movesText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function triggered with user clicking on buy life btn in shop
    public void buyLife() {
        //check to see if user have enough gold
        if (gameStats.goldAmount >= 10)
        {   //10 is price for life 
            //if yes,increase life amount and decrease gold
            gameStats.goldAmount -= 10;
            gameStats.lifeAmount += 1;

            //give user message of successfull purchase 
            shopAlertMessage.text = "Life bought successfully!";
            //update ui
            lifeText.text = gameStats.lifeAmount.ToString();
            goldText.text = gameStats.goldAmount.ToString();
        }
        else {
            //give user message to watch add to earn gold
            shopAlertMessage.text = "Out of gold, watch add?";
        }
    }

    // NOTE !!!, big flaw with the way im storing the powerups, current way is storing with string, which requires extra complicated work with updating parts of that string
    // This section needs improvement but due to time limits ill keep it this way, better way would be string powerups in a array, updates and coding would be cleaner and simpler 

    //function triggered with user clicking on buy hammer btn in shop
    public void buyHammer()
    {
        //check to see if user have enough gold
        if (gameStats.goldAmount >= 5)
        {   //5 is price for hammer 
            //if yes,increase hammer amount in powerups string,position 0, and decrease gold
            gameStats.goldAmount -= 5;

            //getting value for hammer powerup
            char value = gameStats.powerups[0];
            int temp = (int)Char.GetNumericValue(value);
            //updating value
            temp += 1;

            //getting value for bomb powerup
            char valueBomb = gameStats.powerups[2];
            int tempBomb = (int)Char.GetNumericValue(valueBomb);

            //getting value for moves powerup
            char valueMoves = gameStats.powerups[4];
            int tempMoves = (int)Char.GetNumericValue(valueMoves);

            //creating new powerups string
            string newPowerups = temp + "," + tempBomb + "," + tempMoves + ",0,0,0,0,0,0,0";

            //updating powerups string
            gameStats.powerups = newPowerups;

            //give user message of successfull purchase 
            shopAlertMessage.text = "Hammer bought!";
            //update ui
            hammerText.text = gameStats.powerups[0].ToString();
            goldText.text = gameStats.goldAmount.ToString();
        }
        else
        {
            //give user message to watch add to earn gold
            shopAlertMessage.text = "Out of gold, watch add?";
        }
    }

    //function triggered with user clicking on buy bomb btn in shop
    public void buyBomb()
    {
        //check to see if user have enough gold
        if (gameStats.goldAmount >= 20)
        {   //20 is price for bomb 
            //if yes,increase bomb amount in powerups string,position 2, and decrease gold
            gameStats.goldAmount -= 20;
            char value = gameStats.powerups[2];
            int temp = (int)Char.GetNumericValue(value);
            temp += 1;

            //getting value for hammer powerup
            char valueHammer = gameStats.powerups[0];
            int tempHammer = (int)Char.GetNumericValue(valueHammer);

            //getting value for moves powerup
            char valueMoves = gameStats.powerups[4];
            int tempMoves = (int)Char.GetNumericValue(valueMoves);

            //creating new powerups string
            string newPowerups = tempHammer + "," + temp + "," + tempMoves +",0,0,0,0,0,0,0";

            //updating powerups string
            gameStats.powerups = newPowerups;

            //give user message of successfull purchase 
            shopAlertMessage.text = "Bomb bought!";
            //update ui
            bombText.text = gameStats.powerups[2].ToString();
            goldText.text = gameStats.goldAmount.ToString();
        }
        else
        {
            //give user message to watch add to earn gold
            shopAlertMessage.text = "Out of gold, watch add?";
        }
    }

    //function triggered with user clicking on buy moves btn in shop
    public void buyMoves()
    {
        //check to see if user have enough gold
        if (gameStats.goldAmount >= 10)
        {   //10 is price for moves 
            //if yes,increase moves amount in powerups string,position 4, and decrease gold
            gameStats.goldAmount -= 10;
            char value = gameStats.powerups[4];
            int temp = (int)Char.GetNumericValue(value);
            temp += 1;

            //getting value for hammer powerup
            char valueHammer = gameStats.powerups[0];
            int tempHammer = (int)Char.GetNumericValue(valueHammer);

            //getting value for bomb powerup
            char valueBomb = gameStats.powerups[2];
            int tempBomb = (int)Char.GetNumericValue(valueBomb);

            //creating new powerups string
            string newPowerups = tempHammer + "," + tempBomb + "," + temp + ",0,0,0,0,0,0,0";

            //updating powerups string
            gameStats.powerups = newPowerups;

            //give user message of successfull purchase 
            shopAlertMessage.text = "Moves bought!";
            //update ui
            movesText.text = gameStats.powerups[4].ToString();
            goldText.text = gameStats.goldAmount.ToString();
        }
        else
        {
            //give user message to watch add to earn gold
            shopAlertMessage.text = "Out of gold, watch add?";
        }
    }

}
