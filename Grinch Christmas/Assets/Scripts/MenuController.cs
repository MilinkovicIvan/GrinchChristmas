using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking; //used for sending web requests
using System.Text.RegularExpressions;   //used for regex pattern

public class MenuController : MonoBehaviour
{
    //regex pattern, minimum 1 lowercase and uppercase letter, 1 number,size from 4-24
    private const string REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{4,24})";   

    //link to login panel ui
    public GameObject loginUI;

    //link to settings panel ui
    public GameObject settingsUI;
    //links to guide panels and btn
    public GameObject guideBtn;
    public GameObject guidePanel1;
    public GameObject guidePanel2;
    public GameObject guidePanel3;

    //link to shop
    public GameObject shop;

    //link to inv
    public GameObject inv;

    //link to shop alert message
    public TextMeshProUGUI shopAlertMessage;
    //link to life and gold text
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI goldText;
    //link to inv powerups texts
    public TextMeshProUGUI hammerText;
    public TextMeshProUGUI bombText;
    public TextMeshProUGUI movesText;

    //link to alertMessage and submit button
    [SerializeField] private TextMeshProUGUI alertMessage;
    [SerializeField] private Button createButton;
    [SerializeField] private Button loginButton;

    //links to user inputs
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;

    //authentication uri's
    //[SerializeField] private string createURI = "http://localhost:22222/authentication/create";
    //[SerializeField] private string loginURI = "http://localhost:22222/authentication/login";
    [SerializeField] private string createURI = "https://grinch-christmas.herokuapp.com/authentication/create";
    [SerializeField] private string loginURI = "https://grinch-christmas.herokuapp.com/authentication/login";

    //progress uri's
    //[SerializeField] private string createProgressURI = "http://localhost:22222/progress/create";
    //[SerializeField] private string fetchProgressURI = "http://localhost:22222/progress/fetch";
    //[SerializeField] private string updateProgressURI = "http://localhost:22222/progress/update";
    [SerializeField] private string createProgressURI = "https://grinch-christmas.herokuapp.com/progress/create";
    [SerializeField] private string fetchProgressURI = "https://grinch-christmas.herokuapp.com/progress/fetch";
    [SerializeField] private string updateProgressURI = "https://grinch-christmas.herokuapp.com/progress/update";

    //links to settings buttons
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI soundText;

    //link to rewardedAdsButton
    [SerializeField] RewardedAdsButton rewardedAdsButton;

    void Start() 
    {
        // set life and gold text to match whats in gameStats,also set powerups amount to match gameStats
        if (gameStats.gameplayStarted == true) { 
            lifeText.text = gameStats.lifeAmount.ToString();
            goldText.text = gameStats.goldAmount.ToString();
            hammerText.text = gameStats.powerups[0].ToString();
            bombText.text = gameStats.powerups[2].ToString();
            movesText.text = gameStats.powerups[4].ToString();
            //flip flag back to false
            gameStats.gameplayStarted = false;
        }

    }

    // function for triggering scenes
    public void LoadScene(string name) 
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }
        SceneManager.LoadScene(name);
        
    }

    // function to open login window when cloud backup btn is clicked
    public void openLoginWindow() 
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        loginUI.SetActive(true);
    }

    // function to close login window when user click x button 
    public void closeLoginWindow()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //if this is first timer, and is logged in
        if (gameStats.userLoggedin == true && gameStats.firstTimer == true)
        {
            //check to see if user maybe had account before with more advanced progress
            StartCoroutine(fetchOnlineProgressRecord());

            //make a online progress record
            StartCoroutine(createOnlineProgressRecord());
        }
        else if (gameStats.userLoggedin == true) {
            //make a online progress record
            StartCoroutine(createOnlineProgressRecord());
        }
        
        //turn off login ui
        loginUI.SetActive(false);

    }

    // function to open settings window when settings btn is clicked
    public void openSettingsWindow()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        settingsUI.SetActive(true);
        guideBtn.SetActive(true);
        guidePanel1.SetActive(false);
        guidePanel2.SetActive(false);
        guidePanel3.SetActive(false);
    }

    //function which will show guide panel
    public void showGuide() {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }
        guideBtn.SetActive(false);
        guidePanel1.SetActive(true);
        guidePanel2.SetActive(false);
        guidePanel3.SetActive(false);

    }


    //function which will display different panels when next btn is clicked in guide
    public void nextGuidePanel(int fromPanel) {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        if (fromPanel == 1) {
            guideBtn.SetActive(false);
            guidePanel1.SetActive(false);
            guidePanel2.SetActive(true);
            guidePanel3.SetActive(false);
        }

        if (fromPanel == 2)
        {
            guideBtn.SetActive(false);
            guidePanel1.SetActive(false);
            guidePanel2.SetActive(false);
            guidePanel3.SetActive(true);
        }

        if (fromPanel == 3)
        {
            guideBtn.SetActive(true);
            guidePanel1.SetActive(false);
            guidePanel2.SetActive(false);
            guidePanel3.SetActive(false);
        }
    }

    // function to open shop window when shop btn is clicked
    public void openShopWindow()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }
        //reset shop message
        shopAlertMessage.text = "";

        inv.SetActive(false);
        shop.SetActive(true);

        //loading ad
        rewardedAdsButton.LoadAd();
    }

    // function to focus map when map btn is clicked
    public void focusMapWindow()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //turning off shop,inv 
        shop.SetActive(false);
        inv.SetActive(false);
    }

    // function to focus map when map btn is clicked
    public void openInventoryWindow()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //atm just turning off shop
        shop.SetActive(false);
        inv.SetActive(true);
    }

    // function to close login window when user click x button 
    public void closeSettingsWindow()
    {        
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //turn off login ui
        settingsUI.SetActive(false);
    }

    //function to toggle music
    public void toggleMusic() 
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        //toggle music
        gameStats.musicOn = !gameStats.musicOn;

        //toggle text ontop of icon
        if (!gameStats.musicOn) {
            musicText.text = "/";
            SoundController.stopMusic();
        }
        else {
            musicText.text = "";
            SoundController.playMusic();
        }
    }

    //function to toggle sound
    public void toggleSound() 
    {
        //if sound on,play click sound
        if (gameStats.soundOn) {
            SoundController.playSound("click");
        }

        //toggle sound
        gameStats.soundOn = !gameStats.soundOn;

        //toggle text ontop of icon
        if (!gameStats.soundOn)
        {
            soundText.text = "/";
        }
        else
        {
            soundText.text = "";
        }
    }


    // function which is triggered when user click create button from sign up/sign in window
    public void onCreateClicked()
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        // give user a message and make buttons non interactable
        alertMessage.text = "Doing sign up...";
        createButton.interactable = false;
        loginButton.interactable = false;

        // trigger doSignUpSignIn coroutine
        StartCoroutine(doSignUp());
    }

    // function which is triggered when user click login button from sign up/sign in window
    public void onLoginClicked() 
    {
        //if sound on,play click sound
        if (gameStats.soundOn)
        {
            SoundController.playSound("click");
        }

        // give user a message and make buttons non interactable
        alertMessage.text = "Doing sign in...";
        createButton.interactable = false;
        loginButton.interactable = false;

        // trigger doSignUpSignIn coroutine
        StartCoroutine(doSignIn());
    }

    // coroutine which will send web request to our server and try doing sign up functionality
    private IEnumerator doSignUp()
    {
        // get values from inputs
        string username = usernameInput.text;
        string password = passwordInput.text;

        // doing some validation on user input, min length 4, max length 24
        if (username.Length < 4 || username.Length > 24)
        {
            alertMessage.text = "Username not valid.";
            createButton.interactable = true;
            loginButton.interactable = true;
            yield break;
        }
        //also doing regex patter when creating account for password, to increase security somewhat
        if (!Regex.IsMatch(password, REGEX))
        {
            // will not tell user why exactly password is not valid, to not reveal regex pattern,
            // let them figure out what stronger password would be 
            alertMessage.text = "Password not valid.";
            createButton.interactable = true;
            loginButton.interactable = true;
            yield break;
        }

        //creating form that will be send within the request, will hold values needed for making user
        WWWForm form = new WWWForm();
        form.AddField("reqUsername", username);
        form.AddField("reqPassword", password);

        // make request with authentication uri and form values
        UnityWebRequest request = UnityWebRequest.Post(createURI, form);
        // handler for sending request
        var handler = request.SendWebRequest();

        // start time for safety reasons to prevent infinitive loop
        float startTime = 0.0f;
        //while handler is not done
        while (!handler.isDone)
        {
            //increase time
            startTime += Time.deltaTime;

            //if time is more than 5sec, break the loop
            if (startTime > 5.0f)
            {
                break;
            }

            yield return null;
        }

        // when handler is done and request is success
        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(request.downloadHandler.text);
            createResponse response = JsonUtility.FromJson<createResponse>(request.downloadHandler.text);
            //on success
            if (response.code == 0) // 0 means success
            {
                alertMessage.text = "User account created!";
            }
            else
            {
                switch (response.code)
                {
                    case 1:
                        //if code is 1 give user message credentials not valid
                        alertMessage.text = "Credentials not valid.";
                        break;
                    case 2:
                        //if 2, username taken
                        alertMessage.text = "Username already taken!";
                        break;
                    default:
                        //in case someone tries to breach the request
                        alertMessage.text = "Breach detected!";
                        createButton.interactable = false;
                        loginButton.interactable = false;
                        break;
                }              
            }
        }
        // else there is problem with connection to server
        else
        {
            alertMessage.text = "Connection problem with the server!";
        }

        //activate buttons
        createButton.interactable = true;
        loginButton.interactable = true;

        yield return null;
    }

    // coroutine which will send web request to our server and try doing sign in functionality
    private IEnumerator doSignIn() 
    {
        // get values from inputs
        string username = usernameInput.text;
        string password = passwordInput.text;

        // doing some validation on user input, min length 4, max length 24
        if (username.Length < 4 || username.Length > 24) 
        {
            alertMessage.text = "Username not valid.";
            createButton.interactable = true;
            loginButton.interactable = true;
            yield break;
        }
        if (password.Length < 4 || password.Length > 24)
        {
            alertMessage.text = "Password not valid.";
            createButton.interactable = true;
            loginButton.interactable = true;
            yield break;
        }

        //creating form that will be send within the request, will hold values needed for making user
        WWWForm form = new WWWForm();
        form.AddField("reqUsername", username);
        form.AddField("reqPassword", password);

        // make request with authentication uri and form values
        UnityWebRequest request = UnityWebRequest.Post(loginURI, form);
        // handler for sending request
        var handler = request.SendWebRequest();

        // start time for safety reasons to prevent infinitive loop
        float startTime = 0.0f;
        //while handler is not done
        while (!handler.isDone) 
        {
            //increase time
            startTime += Time.deltaTime;

            //if time is more than 10sec, break the loop
            if (startTime > 10.0f) 
            {
                break;
            }

            yield return null;
        }

        // when handler is done and request is success
        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(request.downloadHandler.text);
            loginResponse response = JsonUtility.FromJson<loginResponse>(request.downloadHandler.text);
            //on success
            if (response.code == 0)  // 0 means success
            {
                //update gameStats variables
                gameStats.userLoggedin = true;
                gameStats.username = response.data.username;
                //give message to user and deactivate buttons
                alertMessage.text = "Welcome " + response.data.username + "!";
                createButton.interactable = false;
                loginButton.interactable = false;

                //Debug.Log(gameStats.userLoggedin + " from menuController, userLoggedIn");
                //Debug.Log(gameStats.username);
            }
            else 
            {
                switch (response.code) 
                {
                    case 1:
                        //if code is 1 give user message and activate buttons
                        alertMessage.text = "Credentials not valid.";
                        createButton.interactable = true;
                        loginButton.interactable = true;
                        break;
                    default:
                        //in case someone tries to breach the request
                        alertMessage.text = "Breach detected!";
                        createButton.interactable = false;
                        loginButton.interactable = false;
                        break;
                }
                
            }
        }
        // else there is problem with connection to server
        else 
        {
            alertMessage.text = "Connection problem with the server!";
            createButton.interactable = true;
            loginButton.interactable = true;
        }  

        yield return null;
    }

    // coroutine which will send web request to our server and try making progress record
    private IEnumerator createOnlineProgressRecord()
    {
        //creating form that will be send within the request, will hold values needed for making progress record
        WWWForm form = new WWWForm();
        form.AddField("reqUsername", gameStats.username);
        form.AddField("reqCurrentLv", gameStats.currentLv);
        form.AddField("reqLifeAmount", gameStats.lifeAmount);
        form.AddField("reqGoldAmount", gameStats.goldAmount);
        form.AddField("reqPowerups", gameStats.powerups);

        // make request with progress uri and form values
        UnityWebRequest request = UnityWebRequest.Post(createProgressURI, form);
        // handler for sending request
        var handler = request.SendWebRequest();

        // start time for safety reasons to prevent infinitive loop
        float startTime = 0.0f;
        //while handler is not done
        while (!handler.isDone)
        {
            //increase time
            startTime += Time.deltaTime;

            //if time is more than 10sec, break the loop
            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        // when handler is done and request is success
        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(request.downloadHandler.text);
            progressCreateResponse response = JsonUtility.FromJson<progressCreateResponse>(request.downloadHandler.text);        
            //Debug.Log("code from create: " + response.code);
        }
        // else there is problem with connection to server
        else
        {
            //Debug.Log("Connection problem with the server!");
        }

        yield return null;
    }

    // coroutine which will send web request to our server and try fetching progress record
    // if values from online record are more advanced then in gameStats, gameStats will be updated with record values
    private IEnumerator fetchOnlineProgressRecord()
    {
        //creating form that will be send within the request, will hold values needed for making progress record
        WWWForm form = new WWWForm();
        form.AddField("reqUsername", gameStats.username);

        // make request with progress uri and form values
        UnityWebRequest request = UnityWebRequest.Post(fetchProgressURI, form);
        // handler for sending request
        var handler = request.SendWebRequest();

        // start time for safety reasons to prevent infinitive loop
        float startTime = 0.0f;
        //while handler is not done
        while (!handler.isDone)
        {
            //increase time
            startTime += Time.deltaTime;

            //if time is more than 10sec, break the loop
            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        // when handler is done and request is success
        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(request.downloadHandler.text);
            progressFetchResponse response = JsonUtility.FromJson<progressFetchResponse>(request.downloadHandler.text);
            //Debug.Log("code from fetch: " + response.code);

            if (response.code == 3) {   //means record fetched  
                //Debug.Log("level from fetch: " + response.data.currentLv);
                //Debug.Log("level from gamestats: " + gameStats.currentLv);

                if (response.data.currentLv > gameStats.currentLv) {     //means we have user which had account before and have advanced progress stored online
                    //update gameStats to reflect info from online database, basically recovering user progress
                    gameStats.currentLv = response.data.currentLv;
                    gameStats.lifeAmount = response.data.lifeAmount;
                    gameStats.goldAmount = response.data.goldAmount;
                    gameStats.powerups = response.data.powerups;
/*
                    Debug.Log(gameStats.currentLv);
                    Debug.Log(gameStats.lifeAmount);
                    Debug.Log(gameStats.goldAmount);
                    Debug.Log(gameStats.powerups);
*/
                }
            }
        }
        // else there is problem with connection to server
        else
        {
            Debug.Log("Connection problem with the server!");
        }

        yield return null;
    }

    // coroutine which will send web request to our server and try updating progress record
    public IEnumerator updateOnlineProgressRecord()
    {
        //creating form that will be send within the request, will hold values needed for updating progress record
        WWWForm form = new WWWForm();
        form.AddField("reqUsername", gameStats.username);
        form.AddField("reqCurrentLv", gameStats.currentLv);
        form.AddField("reqLifeAmount", gameStats.lifeAmount);
        form.AddField("reqGoldAmount", gameStats.goldAmount);
        form.AddField("reqPowerups", gameStats.powerups);

        // make request with progress uri and form values
        UnityWebRequest request = UnityWebRequest.Post(updateProgressURI, form);
        // handler for sending request
        var handler = request.SendWebRequest();

        // start time for safety reasons to prevent infinitive loop
        float startTime = 0.0f;
        //while handler is not done
        while (!handler.isDone)
        {
            //increase time
            startTime += Time.deltaTime;

            //if time is more than 10sec, break the loop
            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        // when handler is done and request is success
        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(request.downloadHandler.text);
            progressUpdateResponse response = JsonUtility.FromJson<progressUpdateResponse>(request.downloadHandler.text);
            //Debug.Log("code from update: " + response.code);
        }
        // else there is problem with connection to server
        else
        {
            //Debug.Log("Connection problem with the server!");
        }

        yield return null;
    }
}
