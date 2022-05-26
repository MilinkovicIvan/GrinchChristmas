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

    //link to alertMessage and submit button
    [SerializeField] private TextMeshProUGUI alertMessage;
    [SerializeField] private Button createButton;
    [SerializeField] private Button loginButton;

    //links to user inputs
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;

    //authentication uri's
    [SerializeField] private string createURI = "http://localhost:22222/authentication/create";
    [SerializeField] private string loginURI = "http://localhost:22222/authentication/login";

    //progress uri's
    [SerializeField] private string createProgressURI = "http://localhost:22222/progress/create";
    [SerializeField] private string fetchProgressURI = "http://localhost:22222/progress/fetch";
    [SerializeField] private string updateProgressURI = "http://localhost:22222/progress/update";

    // function for triggering scenes
    public void LoadScene(string name) 
    {
        SceneManager.LoadScene(name);
    }

    // function to open login window when cloud backup btn is clicked
    public void openLoginWindow() 
    {
        loginUI.SetActive(true);
    }

    // function to close login window when user click x button 
    public void closeLoginWindow()
    {             
        //if this is first timer, and is logged in
        if (gameStats.userLoggedin == true && gameStats.firstTimer == true)
        {
            //check to see if user maybe had account before with more advanced progress
            StartCoroutine(fetchOnlineProgressRecord());

            //make a online progress record
            StartCoroutine(createOnlineProgressRecord());

        }

        if (gameStats.userLoggedin == true) 
        {
            //update record with dummy data for now
            StartCoroutine(updateOnlineProgressRecord());
        }
        
        //turn off login ui
        loginUI.SetActive(false);

    }

    // function which is triggered when user click create button from sign up/sign in window
    public void onCreateClicked()
    {
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

                //Debug.Log(gameStats.userLoggedin);
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
    private IEnumerator updateOnlineProgressRecord()
    {
        //creating form that will be send within the request, will hold values needed for updating progress record
        WWWForm form = new WWWForm();
        form.AddField("reqUsername", gameStats.username);
        form.AddField("reqCurrentLv", 999);     //change later 999 to gameStats.currentLv
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
