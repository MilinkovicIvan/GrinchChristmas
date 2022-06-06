using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;   //used for GetNumericValue
using Random=UnityEngine.Random;


public class GameplayController : MonoBehaviour
{
    //link to preLevel panel ui
    public GameObject preLevel;

    //link to level panels
    public GameObject level1;

    //link to postLevel
    public GameObject postLevel;

    //link to all gifts
    public Image gift0;
    public Image gift1;
    public Image gift2;
    public Image gift3;
    public Image gift4;
    public Image gift5;
    public Image gift6;
    public Image gift7;
    public Image gift8;
    public Image gift9;
    public Image gift10;
    public Image gift11;
    public Image gift12;
    public Image gift13;
    public Image gift14;
    public Image gift15;
    public Image gift16;
    public Image gift17;
    public Image gift18;
    public Image gift19;
    public Image gift20;
    public Image gift21;
    public Image gift22;
    public Image gift23;
    public Image gift24;

    //link to map btns
    public Image btn;
    public Image btn1;
    public Image btn2;
    public Image btn3;
    public Image btn4;
    public Image btn5;
    public Image btn6;

    //2d arrays for gifts and colors
    public string[,] colorsArray;
    public string[,] currentLVColorsArray;
    public Image[,] giftsArray;
    public Image[] mapBtnArray;
    //colors
    //new Color32(255, 255, 0, 255); YELLOW
    //new Color32(0, 185, 255, 255); BLUE
    //new Color32(255, 0, 255, 255); PINK
    //new Color32(255, 255, 255, 255); WHITE

    //level selected is assigned with value passed by levelClicked method 
    public static int levelSelected = 0;

    //var for levels
    public static int movesLeft = 0;
    public static int goalGiftsLeft = 0;
    public static string giftColorWanted = "";
    public static bool goalLimit;

    //ref to selected powerups
    public static bool hammerSelected = false;
    public static bool bombSelected = false;
    public static bool movesSelected = false;

    //link to selected powerups texts
    public TextMeshProUGUI hammerText;
    public TextMeshProUGUI bombText;
    public TextMeshProUGUI movesText;

    //Link to selected powerups buttons
    public Button hammerButton;
    public Button bombButton;
    public Button movesButton;

    //link to level moves text and gift image color and text
    public TextMeshProUGUI movesLeftText;
    public Image goalGiftImage;
    public TextMeshProUGUI goalGiftText;

    //Link to powerups buttons inside level bottom UI
    public Button hammerLevelButton;
    public Button bombLevelButton;
    public Button movesLevelButton;

    //user mobile resolution
    public static int maxX = Screen.width;
    public static int maxY = Screen.height;

    //grid starting point and cell size
    //grid is 750x750, cell is 150x150
    public static int gridStartX = (maxX - 750) /2;
    public static int gridStartY = (maxY - 750) /2 + 750;
    public static int cell = 150;

    //var for drag points
    public static int startX = -1;
    public static int startY = -1;
    public static int endX = -1;
    public static int endY = -1;

    //var for gift positions
    public int startingCol;
    public int startingRow;
    public int endingCol;
    public int endingRow;
    //var for direction
    public string dragDirection;

    //link to postLevel title and message
    public TextMeshProUGUI postLevelTitle;
    public TextMeshProUGUI postLevelMessage;
    //link to postLevel btns
    public GameObject nextLvButton;
    public GameObject retryLvButton;
    public Button closePostLevel;

    //link to inv powerups texts
    public TextMeshProUGUI invHammerText;
    public TextMeshProUGUI invBombText;
    public TextMeshProUGUI invMovesText;

    //var for powerups
    public static bool userUsedHammerPowerup = false;
    public static bool userUsedBombPowerup = false;

    //ref to localDB script and menu controller for corutines
    public localDB localDB;
    public MenuController myMenuController;


    // Start is called before the first frame update
    void Start()
    {
        //populate arrays
        //2d array for colors
        colorsArray = new string[,] { 
            {"yellow", "blue", "yellow", "yellow", "pink" },
            {"pink", "blue", "pink", "blue", "blue" },
            {"yellow", "pink", "yellow", "yellow", "pink" },
            {"yellow", "pink", "pink", "blue", "blue" },
            {"blue", "blue", "yellow", "pink", "blue" }
        };

        //2d array for gift images
        giftsArray = new Image[,] {
            {gift0, gift1, gift2, gift3, gift4 },
            {gift5, gift6, gift7, gift8, gift9 },
            {gift10, gift11, gift12, gift13, gift14 },
            {gift15, gift16, gift17, gift18, gift19 },
            {gift20, gift21, gift22, gift23, gift24 }
        };

        //this array will expand with development,hold images for button in map
        //used for updating from red to green color when user finish level
        mapBtnArray = new Image[] { btn, btn1, btn2, btn3, btn4, btn5, btn6 };
        //loop bellow is for when user progress gets pulled from database, will update map btn colors based on what currentLv from database will be
        for (int i = 1; i < gameStats.currentLv; i++) {
            mapBtnArray[i].GetComponent<Image>().color = new Color32(0, 255, 0, 255);  //green
        }

        //link to components that are doing database updates
        localDB = GameObject.Find("GameManager").GetComponent<localDB>();
        myMenuController = GameObject.Find("GameManager").GetComponent<MenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //method which is triggered when user clicks on level buttons on map
    public void levelClicked(int levelNumber) 
    {
        //check to see if user is trying to skip levels, we allow repeat previously done levels, but not triggering levels above currentLv
        if (levelNumber <= gameStats.currentLv)
        {
            //if sound on,play click sound
            if (gameStats.soundOn)
            {
                SoundController.playSound("click");
            }

            //turn on preLevel ui
            preLevel.SetActive(true);

            //set levelSelected
            levelSelected = levelNumber;

            //check to see if user have powerups in inv, if not, set the buttons inactive            
            //getting value for hammer powerup
            char valueHammer = gameStats.powerups[0];
            int tempHammer = (int)Char.GetNumericValue(valueHammer);
            //turn off if less than 1
            if (tempHammer < 1)
            {
                hammerButton.interactable = false;
                hammerText.text = "";
            }
            else {
                hammerButton.interactable = true;
            }

            //getting value for bomb powerup
            char valueBomb = gameStats.powerups[2];
            int tempBomb = (int)Char.GetNumericValue(valueBomb);
            //turn off if less than 1
            if (tempBomb < 1)
            {
                bombButton.interactable = false;
                bombText.text = "";
            }
            else
            {
                bombButton.interactable = true;
            }

            //getting value for moves powerup
            char valueMoves = gameStats.powerups[4];
            int tempMoves = (int)Char.GetNumericValue(valueMoves);
            //turn off if less than 1
            if (tempMoves < 1)
            {
                movesButton.interactable = false;
                movesText.text = "";
            }
            else
            {
                movesButton.interactable = true;
            }
        }
        else 
        {
            //play bad sound 
            if (gameStats.soundOn)
            {
                SoundController.playSound("badclick");
            }
        }

    }

    //func to toggle hammerSelected
    public void toggleHammerSelected() 
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //toggle variable and text
        hammerSelected = !hammerSelected;
        if (hammerSelected)
        {
            hammerText.text = "+";
        }
        else {
            hammerText.text = "";
        }
    }

    //func to toggle bombSelected
    public void toggleBombSelected()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //toggle variable and text
        bombSelected = !bombSelected;
        if (bombSelected)
        {
            bombText.text = "+";
        }
        else
        {
            bombText.text = "";
        }
    }

    //func to toggle movesSelected
    public void toggleMovesSelected()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //toggle variable and text
        movesSelected = !movesSelected;
        if (movesSelected)
        {
            movesText.text = "+";
        }
        else
        {
            movesText.text = "";
        }
    }

    //method which is triggered when user clicks on X button in preLevel
    public void closePreLevel()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //reset levelSelected, hammerSelected, bombSelected, movesSelected
        levelSelected = 0;

        hammerSelected = false;
        hammerText.text = "";

        bombSelected = false;
        bombText.text = "";

        movesSelected = false;
        movesText.text = "";

        //turn off preLevel ui
        preLevel.SetActive(false);
    }

    //method which is triggered when user clicks on X button in postLevel
    public void turnOffPostLevel()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //turn off postLevel ui
        postLevel.SetActive(false);
    }

    //function triggered with nextLv btn from postLevel UI, will trigger preLevel for next level
    public void nextLevelTrigger(int levelNumber) {
        //turn off postLevel ui
        postLevel.SetActive(false);
        
        levelSelected += 1;
        levelClicked(levelSelected);

        //Debug.Log(levelSelected);
        //Debug.Log(gameStats.currentLv);
    }

    //function triggered with retryLv btn from postLevel UI, will trigger preLevel for that level
    public void retryLevelTrigger()
    {
        //turn off postLevel ui
        postLevel.SetActive(false);

        levelClicked(levelSelected);
    }

    //method which is triggered when user clicks on start button in preLevel
    public void startLevel()
    {
        //turn off preLevel ui
        preLevel.SetActive(false);

        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //start level with levelSelected value
        if (levelSelected == 1) 
        {
            //set moves amount and gift color, amount for that lv, target gift color
            movesLeft = 5;
            movesLeftText.text = movesLeft.ToString();

            goalGiftsLeft = 6;
            goalGiftText.text = goalGiftsLeft.ToString();
            goalGiftImage.GetComponent<Image>().color = new Color32(255, 255, 0, 255);  //yellow

            giftColorWanted = "yellow";
        }
        else if (levelSelected == 2)
        {
            //set moves amount and gift color, amount for that lv, target gift color
            movesLeft = 1;
            movesLeftText.text = movesLeft.ToString();

            goalGiftsLeft = 15;
            goalGiftText.text = goalGiftsLeft.ToString();
            goalGiftImage.GetComponent<Image>().color = new Color32(0, 185, 255, 255);  //blue

            giftColorWanted = "blue";
        }

        else if (levelSelected == 3)
        {
            //set moves amount and gift color, amount for that lv, target gift color
            movesLeft = 10;
            movesLeftText.text = movesLeft.ToString();

            goalGiftsLeft = 21;
            goalGiftText.text = goalGiftsLeft.ToString();
            goalGiftImage.GetComponent<Image>().color = new Color32(255, 0, 255, 255);  //pink

            giftColorWanted = "pink";
        }

        //check to see which powerups buttons can be used, the one from where user selected in preLevel
        //hammer
        if (hammerSelected)
        {
            hammerLevelButton.interactable = true;
        }
        else {
            hammerLevelButton.interactable = false;
        }
        //bomb
        if (bombSelected)
        {
            bombLevelButton.interactable = true;
        }
        else
        {
            bombLevelButton.interactable = false;
        }
        //moves
        if (movesSelected)
        {
            movesLevelButton.interactable = true;
        }
        else
        {
            movesLevelButton.interactable = false;
        }

        //reset powerups selection
        hammerText.text = "";
        bombText.text = "";
        movesText.text = "";
        hammerSelected = false;
        bombSelected = false;
        movesSelected = false;

        //clone of colorsArray
        currentLVColorsArray = colorsArray.Clone() as string[,];
        //set gift colors on grid
        setGiftColors();

        //start level
        level1.SetActive(true);
      
    }

    //method which will go through 2d arrays and set color of gifts
    public void setGiftColors()
    {
        for (int col = 0; col < currentLVColorsArray.GetLength(0); col++)
        {
            for (int row = 0; row < currentLVColorsArray.GetLength(1); row++)
            {
                //set color of gift Image based on string value in color array
                if (currentLVColorsArray[col, row] == "yellow")
                {
                    giftsArray[col, row].GetComponent<Image>().color = new Color32(255, 255, 0, 255);
                }
                else if (currentLVColorsArray[col, row] == "blue")
                {
                    giftsArray[col, row].GetComponent<Image>().color = new Color32(0, 185, 255, 255);
                }
                else if (currentLVColorsArray[col, row] == "pink")
                {
                    giftsArray[col, row].GetComponent<Image>().color = new Color32(255, 0, 255, 255);
                }
                else if (currentLVColorsArray[col, row] == "white")
                {
                    giftsArray[col, row].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }

            }
        }
    }

    //method which is triggered when user drags on gift grid, will take starting coordinates
    public void userDragBegin() 
    {
        //when user touch screen with finger
        if (Input.touchCount > 0) {
            //get first touch point, and if its phase began, store those coordinates for starting gift
            startX = (int)Input.GetTouch(0).position.x;
            startY = (int)Input.GetTouch(0).position.y;
        }
    }

    //method which is triggered when user drags on gift grid, will take ending coordinates
    public void userDragEnd()
    {
        //limit for reducing gift goal amount,allowed only once per drag
        goalLimit = false;

        //when user touch screen with finger
        if (Input.touchCount > 0)
        {
            //get first touch point, and if its phase end, store those coordinates for ending gift
            endX = (int)Input.GetTouch(0).position.x;
            endY = (int)Input.GetTouch(0).position.y;
        }

        //now that we have starting and ending coordinates we can figure out which gift user is trying to swap
        calcWhichGifts();
        //calc drag direction
        calcWhichDirection();
        //if starting and ending points are the same just exit, dont want to punish user for most likely bad drag due to finger size
        if (startingRow == endingRow && startingCol == endingCol)
        {
            return;
        }
        //swap gift colors and update grid
        swapGiftColors();
        setGiftColors();
        //reduce movesLeft and update ui
        movesLeft -= 1;
        movesLeftText.text = movesLeft.ToString();
        //check for 3 same,change their colors and update grid
        checkForSameColor(endingRow, endingCol);
        setGiftColors();
        //loop throught color array and see if there are any whites that need to be pushed to top
        int flag = 5;
        while (flag>0) 
        {
            for (int col = 0; col < currentLVColorsArray.GetLength(0); col++)
            {
                for (int row = 0; row < currentLVColorsArray.GetLength(1); row++)
                {
                    try
                    {
                        if (currentLVColorsArray[col, row] == "white")
                        {
                            pushWhiteUp(col, row);
                            setGiftColors();
                        }                   
                    }
                    catch
                    {                       
                    }
                }
            }
            flag--;
        }

        //loop throught color array and see if there are any additional sets that need to be taken care off
        int flag2 = 5;
        while (flag2 > 0)
        {
            for (int col = 0; col < currentLVColorsArray.GetLength(0); col++)
            {
                for (int row = 0; row < currentLVColorsArray.GetLength(1); row++)
                {
                    try
                    {
                        checkForSameColor(col, row);
                        //replace white gifts with new gifts with random color, yellow,blue or pink
                        replaceWhiteWithNewColor();
                        setGiftColors();
                    }
                    catch
                    {
                    }
                }
            }
            flag2--;
        }

        //check to see if user reached win/lose state
        //win state
        if (movesLeft >= 0 && goalGiftsLeft <= 0) {
            //turn off level
            level1.SetActive(false);

            //turn on postLevel
            postLevel.SetActive(true);

            //turn off retry btn
            retryLvButton.SetActive(false);

            //update title and message of postLevel
            postLevelTitle.text = "Congratz !!!";
            postLevelMessage.text = "You managed to steal all the gifts ! \n\nLots of children will be crying in the morning! \n\nBuah ah aha!";

            //update currentLv and map btn color for that lv
            gameStats.currentLv = levelSelected+1;
            //Debug.Log(levelSelected);
            //Debug.Log(gameStats.currentLv);

            //since we have only 3 lvs for now,turn off next lv btn once lv 3 is done
            if (gameStats.currentLv > 3)
            {
                nextLvButton.SetActive(false);
            }

            for (int i = 1; i < gameStats.currentLv; i++)
            {
                mapBtnArray[i].GetComponent<Image>().color = new Color32(0, 255, 0, 255);  //green
            }

            //update databases
            if (gameStats.userLoggedin == true)
            {
                //update local db
                localDB.updateProgressValues(gameStats.currentLv, gameStats.lifeAmount, gameStats.goldAmount, gameStats.powerups);
                //update online database
                myMenuController.StartCoroutine(myMenuController.updateOnlineProgressRecord());
            }
            else
            {
                //update local db
                localDB.updateProgressValues(gameStats.currentLv, gameStats.lifeAmount, gameStats.goldAmount, gameStats.powerups);
            }
        }
        //lose state
        else if (movesLeft < 1 && goalGiftsLeft > 0)
        {
            //turn off level
            level1.SetActive(false);

            //turn on postLevel
            postLevel.SetActive(true);

            //turn on retry btn
            retryLvButton.SetActive(true);
            nextLvButton.SetActive(false);

            //update title and message of postLevel
            postLevelTitle.text = "Ohh Nooo !!!";
            postLevelMessage.text = "You failed at stealing the gifts ! \n\nYou will have to do better next time!";
          
        }

    }

    //function which converts coordinates into rows and columns position which we will use to target gifts in 2d arrays
    public void calcWhichGifts() {
        //Debug.Log(gridStartX);
        //Debug.Log(gridStartY);
        //Debug.Log(startY);
        //Debug.Log(startX);
        ///////////////////////////////////////
        //calc for starting gift
        //figuring out row by cheching y coordinates
        if (startY < gridStartY && startY > gridStartY - cell) 
        {
            startingRow = 0;
        }
        else if (startY <= gridStartY - cell && startY > gridStartY - cell * 2) 
        {
            startingRow = 1;        
        }
        else if(startY <= gridStartY - cell * 2 && startY > gridStartY - cell * 3)
        {
            startingRow = 2;
        }
        else if(startY <= gridStartY - cell * 3 && startY > gridStartY - cell * 4)
        {
            startingRow = 3;
        }
        else if(startY <= gridStartY - cell * 4 && startY > gridStartY - cell * 5)
        {
            startingRow = 4;
        }

        //figuring out column by cheching x coordinates
        if (startX >= gridStartX && startX < gridStartX + cell)
        {
            startingCol = 0;
        }
        else if(startX >= gridStartX + cell && startX < gridStartX + cell * 2)
        {
            startingCol = 1;
        }
        else if(startX >= gridStartX + cell * 2 && startX < gridStartX + cell * 3)
        {
            startingCol = 2;
        }
        else if(startX >= gridStartX + cell * 3 && startX < gridStartX + cell * 4)
        {
            startingCol = 3;
        }
        else if(startX >= gridStartX + cell * 4 && startX < gridStartX + cell * 5)
        {
            startingCol = 4;
        }
      

        ///////////////////////////////////////
        //calc for ending gift
        //figuring out row by cheching y coordinates
        if (endY < gridStartY && endY > gridStartY - cell)
        {
            endingRow = 0;
        }
        else if (endY <= gridStartY - cell && endY > gridStartY - cell * 2)
        {
            endingRow = 1;
        }
        else if (endY <= gridStartY - cell * 2 && endY > gridStartY - cell * 3)
        {
            endingRow = 2;
        }
        else if (endY <= gridStartY - cell * 3 && endY > gridStartY - cell * 4)
        {
            endingRow = 3;
        }
        else if (endY <= gridStartY - cell * 4 && endY > gridStartY - cell * 5)
        {
            endingRow = 4;
        }

        //figuring out column by cheching x coordinates
        if (endX >= gridStartX && endX < gridStartX + cell)
        {
            endingCol = 0;
        }
        else if (endX >= gridStartX + cell && endX < gridStartX + cell * 2)
        {
            endingCol = 1;
        }
        else if (endX >= gridStartX + cell * 2 && endX < gridStartX + cell * 3)
        {
            endingCol = 2;
        }
        else if (endX >= gridStartX + cell * 3 && endX < gridStartX + cell * 4)
        {
            endingCol = 3;
        }
        else if (endX >= gridStartX + cell * 4 && endX < gridStartX + cell * 5)
        {
            endingCol = 4;
        }

        //Debug.Log("Starting gift: " + startingRow + "," + startingCol);
        //Debug.Log("Ending gift: " + endingRow + "," + endingCol);

    }

    //function which convert coordinates from starting and ending points into directions of user drag on screen
    public void calcWhichDirection() {
        int differenceInX = System.Math.Abs(endX - startX);
        int differenceInY = System.Math.Abs(endY - startY);

        //if diff in x is larger than y that means drag is either left or right
        if (differenceInX > differenceInY)
        {
            //if end x is larger than start x, drag is on right
            if (endX > startX)
            {
                dragDirection = "right";
            }
            //else drag is to the left
            else
            {
                dragDirection = "left";
            }
        }
        //otherwise drag is up or down
        else {
            //if end y is larger than start y, drag is up
            if (endY > startY)
            {
                dragDirection = "up";
            }
            //else is down
            else {
                dragDirection = "down";
            }
        }

        //Debug.Log("Drag direction: " + dragDirection);
    }

    //function which will swap gift colors
    public void swapGiftColors() {
        //make temp holder for starting gift color
        string tempColor = currentLVColorsArray[startingRow, startingCol];

        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }
        try
        {
            if (dragDirection == "right")
            {
                //swap colors in current color 2d array
                currentLVColorsArray[startingRow, startingCol] = currentLVColorsArray[startingRow, startingCol + 1];
                currentLVColorsArray[startingRow, startingCol + 1] = tempColor;

                //update ending gift
                endingRow = startingRow;
                endingCol = startingCol + 1;
            }
            else if (dragDirection == "left")
            {
                //swap colors in current color 2d array
                currentLVColorsArray[startingRow, startingCol] = currentLVColorsArray[startingRow, startingCol - 1];
                currentLVColorsArray[startingRow, startingCol - 1] = tempColor;

                //update ending gift
                endingRow = startingRow;
                endingCol = startingCol - 1;
            }
            else if (dragDirection == "up")
            {
                //swap colors in current color 2d array
                currentLVColorsArray[startingRow, startingCol] = currentLVColorsArray[startingRow - 1, startingCol];
                currentLVColorsArray[startingRow - 1, startingCol] = tempColor;

                //update ending gift
                endingRow = startingRow - 1;
                endingCol = startingCol;
            }
            else if (dragDirection == "down")
            {
                //swap colors in current color 2d array
                currentLVColorsArray[startingRow, startingCol] = currentLVColorsArray[startingRow + 1, startingCol];
                currentLVColorsArray[startingRow + 1, startingCol] = tempColor;

                //update ending gift
                endingRow = startingRow + 1;
                endingCol = startingCol;
            }
        }
        catch
        {
        }
        
    }

    //function which checks if there is 3 same color gifts next to each other
    public void checkForSameColor(int row, int col) {
        string targetColor = currentLVColorsArray[row, col];
        //var for storing positions of other gift with same colors
        int secondGiftRow = -500;
        int secondGiftCol = -500;
        int thirdGiftRow = -500;
        int thirdGiftCol = -500;
        //flag for changing gift colors if matched
        bool goFlag = false;

        ////////////////////////////////////////////////
        //check right side of that position
        try
        {
            if (currentLVColorsArray[row, col + 1] == targetColor)
            {
                secondGiftRow = row;
                secondGiftCol = col + 1;
                //if second matched,check third,this will check another on the right side
                if (currentLVColorsArray[row, col + 2] == targetColor)
                {
                    thirdGiftRow = row;
                    thirdGiftCol = col + 2;

                    goFlag = true;
                }
                //this will check left side, in case our first gift is in the middle
                else if (currentLVColorsArray[row, col - 1] == targetColor) {
                    thirdGiftRow = row;
                    thirdGiftCol = col -1;

                    goFlag = true;
                }
            }
        }
        catch
        {
        }       
        //change colors
        if (goFlag) 
        {
            //if targetColor matches the giftColorWanted, user matched 3 gifts which match the goal, reduce them from goal amount
            if (targetColor == giftColorWanted && !goalLimit )
            {
                goalGiftsLeft -= 3;
                goalGiftText.text = goalGiftsLeft.ToString();
                goalLimit = true;
            }
            currentLVColorsArray[row, col] = "white";
            currentLVColorsArray[secondGiftRow, secondGiftCol] = "white";
            currentLVColorsArray[thirdGiftRow, thirdGiftCol] = "white";

            goFlag = false;
        }

        ////////////////////////////////////////////////
        //check left side of that position
        try
        {
            if (currentLVColorsArray[row, col - 1] == targetColor)
            {
                secondGiftRow = row;
                secondGiftCol = col - 1;
                //if second matched,check third, this checks both on left side
                if (currentLVColorsArray[row, col - 2] == targetColor)
                {
                    thirdGiftRow = row;
                    thirdGiftCol = col - 2;

                    goFlag = true;
                }
                //this will check right side, in case first gift is middle one
                else if (currentLVColorsArray[row, col+1] == targetColor) {
                    thirdGiftRow = row;
                    thirdGiftCol = col +1;

                    goFlag = true;
                }
            }
        }
        catch
        {
        }     
        //change colors
        if (goFlag)
        {
            //if targetColor matches the giftColorWanted, user matched 3 gifts which match the goal, reduce them from goal amount
            if (targetColor == giftColorWanted && !goalLimit)
            {
                goalGiftsLeft -= 3;
                goalGiftText.text = goalGiftsLeft.ToString();
                goalLimit = true;
            }

            currentLVColorsArray[row, col] = "white";
            currentLVColorsArray[secondGiftRow, secondGiftCol] = "white";
            currentLVColorsArray[thirdGiftRow, thirdGiftCol] = "white";

            goFlag = false;
        }

        ////////////////////////////////////////////////
        //check up of that position
        try
        {
            if (currentLVColorsArray[row - 1, col] == targetColor)
            {
                secondGiftRow = row - 1;
                secondGiftCol = col;
                //if second matched,check third
                if (currentLVColorsArray[row - 2, col] == targetColor)
                {
                    thirdGiftRow = row - 2;
                    thirdGiftCol = col;

                    goFlag = true;
                }
                //check down in case its middle one
                else if (currentLVColorsArray[row + 1, col] == targetColor) {
                    thirdGiftRow = row +1;
                    thirdGiftCol = col;

                    goFlag = true;
                }
            }
        }
        catch
        {
        }
        //change colors
        if (goFlag)
        {
            //if targetColor matches the giftColorWanted, user matched 3 gifts which match the goal, reduce them from goal amount
            if (targetColor == giftColorWanted && !goalLimit)
            {
                goalGiftsLeft -= 3;
                goalGiftText.text = goalGiftsLeft.ToString();
                goalLimit = true;
            }

            currentLVColorsArray[row, col] = "white";
            currentLVColorsArray[secondGiftRow, secondGiftCol] = "white";
            currentLVColorsArray[thirdGiftRow, thirdGiftCol] = "white";

            goFlag = false;
        }

        ////////////////////////////////////////////////
        //check down of that position
        try
        {
            if (currentLVColorsArray[row + 1, col] == targetColor)
            {
                secondGiftRow = row + 1;
                secondGiftCol = col;
                //if second matched,check third
                if (currentLVColorsArray[row + 2, col] == targetColor)
                {
                    thirdGiftRow = row + 2;
                    thirdGiftCol = col;

                    goFlag = true;
                }
                //check up in case its middle one
                else if (currentLVColorsArray[row - 1, col] == targetColor) {
                    thirdGiftRow = row -1;
                    thirdGiftCol = col;

                    goFlag = true;
                }
            }
        }
        catch
        {
        }
        //change colors
        if (goFlag)
        {
            //if targetColor matches the giftColorWanted, user matched 3 gifts which match the goal, reduce them from goal amount
            if (targetColor == giftColorWanted && !goalLimit)
            {
                goalGiftsLeft -= 3;
                goalGiftText.text = goalGiftsLeft.ToString();
                goalLimit = true;
            }
            currentLVColorsArray[row, col] = "white";
            currentLVColorsArray[secondGiftRow, secondGiftCol] = "white";
            currentLVColorsArray[thirdGiftRow, thirdGiftCol] = "white";

            goFlag = false;
        }



    }

    //function which will push white gift to top of grid, by swithcing colors with above gifts
    public void pushWhiteUp(int row, int col) {
        try
        {
            //swap colors
            currentLVColorsArray[row, col] = currentLVColorsArray[row - 1, col];
            currentLVColorsArray[row - 1, col] = "white";
        }
        catch
        {
        }
    }

    //method which replaces white gifts with new gifts with random color, yellow,blue or pink
    public void replaceWhiteWithNewColor()
    {
        //loop through color array
        for (int col = 0; col < currentLVColorsArray.GetLength(0); col++)
        {
            for (int row = 0; row < currentLVColorsArray.GetLength(1); row++)
            {
                try
                {
                    if (currentLVColorsArray[col, row] == "white")
                    {
                        //use random to gen nb between 1-3
                        int nb = Random.Range(1, 4);  // creates a number between 1 and 4

                        //if 1 make yellow gift
                        if (nb == 1)
                        {
                            currentLVColorsArray[col, row] = "yellow";
                        }
                        //if 2 make blue gift
                        else if (nb == 2)
                        {
                            currentLVColorsArray[col, row] = "blue";
                        }
                        //if 3 make pink gift
                        else if (nb == 3)
                        {
                            currentLVColorsArray[col, row] = "pink";
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }

    //function which will increase moves amount after user click extra moves btn in level bottom navigation
    public void useExtraMoves() {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //turn off btn and update moves amount
        movesLevelButton.interactable = false;
        movesLeft += 5;
        movesLeftText.text = movesLeft.ToString();

        //update powerups string in gameStats
        //value for moves powerup
        char value = gameStats.powerups[4];
        int temp = (int)Char.GetNumericValue(value);
        temp -= 1;

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

        //update ui
        invMovesText.text = gameStats.powerups[4].ToString();
    }

    //function which will be triggered by user clicking on hammer btn in level bottom
    public void useHammer()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //turn off btn
        hammerLevelButton.interactable = false;

        //update powerups string in gameStats
        //getting value for hammer powerup
        char value = gameStats.powerups[0];
        int temp = (int)Char.GetNumericValue(value);
        //updating value
        temp -= 1;

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

        //update ui
        invHammerText.text = gameStats.powerups[0].ToString();

        //set flag linked to user using powerups
        userUsedHammerPowerup = true;
    }

    //function which will be triggered by user clicking on bomb btn in level bottom
    public void useBomb()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //turn off btn
        bombLevelButton.interactable = false;

        //update powerups string in gameStats
        //getting value for hammer powerup
        char value = gameStats.powerups[2];
        int temp = (int)Char.GetNumericValue(value);
        temp -= 1;

        //getting value for hammer powerup
        char valueHammer = gameStats.powerups[0];
        int tempHammer = (int)Char.GetNumericValue(valueHammer);

        //getting value for moves powerup
        char valueMoves = gameStats.powerups[4];
        int tempMoves = (int)Char.GetNumericValue(valueMoves);

        //creating new powerups string
        string newPowerups = tempHammer + "," + temp + "," + tempMoves + ",0,0,0,0,0,0,0";

        //updating powerups string
        gameStats.powerups = newPowerups;

        //update ui
        invBombText.text = gameStats.powerups[2].ToString();

        //set flag linked to user using powerups
        userUsedBombPowerup = true;
    }

    //fun for taking coordinates once user use powerups
    public void processPowerupsAction() {
        //when user touch screen with finger and flags are active, take coordinates of touch
        if (userUsedHammerPowerup || userUsedBombPowerup) 
        { 
            if (Input.touchCount > 0)
            {
                //store coordinates once user touch screen
                startX = (int)Input.GetTouch(0).position.x;
                startY = (int)Input.GetTouch(0).position.y;
                endX = (int)Input.GetTouch(0).position.x;
                endY = (int)Input.GetTouch(0).position.y;
                //Debug.Log("Coordinates from takeXYForPowerups usage: " + startX + ", " + startY + ", " + endX + ", " + endY);

                //process coordinated to row and col which relate to grid
                calcWhichGifts();
                //Debug.Log("Powerups execution at: " + startingRow + "," + startingCol); 

                //if its hammer,then change color of that specific gift to represent it being destroyed
                if (userUsedHammerPowerup)
                {
                    currentLVColorsArray[startingRow, startingCol] = "white";
                    
                }
                //if its bomb, change colors for entire row
                else if (userUsedBombPowerup) {
                    currentLVColorsArray[startingRow, 0] = "white";
                    currentLVColorsArray[startingRow, 1] = "white";
                    currentLVColorsArray[startingRow, 2] = "white";
                    currentLVColorsArray[startingRow, 3] = "white";
                    currentLVColorsArray[startingRow, 4] = "white";
                }

                //update grid colors
                setGiftColors();
                //loop throught color array and see if there are any whites that need to be pushed to top
                int flag = 5;
                while (flag > 0)
                {
                    for (int col = 0; col < currentLVColorsArray.GetLength(0); col++)
                    {
                        for (int row = 0; row < currentLVColorsArray.GetLength(1); row++)
                        {
                            try
                            {
                                if (currentLVColorsArray[col, row] == "white")
                                {
                                    //push white gifts to top
                                    pushWhiteUp(col, row);
                                    //update grid colors
                                    setGiftColors();                                   
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    flag--;
                }

                //replace white gifts with new gifts with random color, yellow,blue or pink
                replaceWhiteWithNewColor();
                //update grid colors
                setGiftColors();

                //loop throught color array and see if there are any additional sets that need to be taken care off
                int flag2 = 5;
                while (flag2 > 0)
                {
                    for (int col = 0; col < currentLVColorsArray.GetLength(0); col++)
                    {
                        for (int row = 0; row < currentLVColorsArray.GetLength(1); row++)
                        {
                            try
                            {
                                checkForSameColor(col, row);
                                setGiftColors();
                            }
                            catch
                            {
                            }
                        }
                    }
                    //replace white gifts with new gifts with random color, yellow,blue or pink
                    replaceWhiteWithNewColor();
                    setGiftColors();

                    flag2--;
                }



                //reset var
                userUsedHammerPowerup = false;
                userUsedBombPowerup = false;
            }
        }  
    }
}
