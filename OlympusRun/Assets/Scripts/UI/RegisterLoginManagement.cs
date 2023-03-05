/*
Olympus Run - A game made as part of the ARGO Project at SETU Carlow
Copyright (C) 2023 Caroline Percy <lineypercy@me.com>, Patrick Donnelly <patrickdonnelly3759@gmail.com>, Izabela Zelek <izabelawzelek@gmail.com>, Danial-hakim <danialhakim01@gmail.com>, Adrien Dudon <dudonadrien@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using TMPro;

/// <summary>
/// Data needed to validate a login
/// </summary>
public class LoginData
{
    public string username= " ";
    public string password = " ";
}

/// <summary>
/// Class that allows the user to register a username and login to the game
/// </summary>
public class RegisterLoginManagement : MonoBehaviour
{
    private readonly LoginData _loginData = new LoginData();
    private GameObject _logInButton;
    private GameObject _popUpUnsuccessfulLoginButton;
    private GameObject _popUpUnsuccessfulRegistrationButton;
    private GameObject _popUpSuccessfulRegistrationButton;
    private GameObject _popUpUserNameTaken;
    private GameObject _hud;



    public List<string> ExistingUsernames;
    public string[] ExistingPasswords;
    public Image TogglePasswordCensorIcon;
    public Sprite Locked;
    public Sprite unlocked;
    public TMPro.TextMeshProUGUI UsernameHud;

    public TMPro.TMP_InputField usernameInput;
    public TMPro.TMP_InputField passwordInput;
#pragma warning disable CS0414
    private bool _validPassword;
#pragma warning restore CS0414
#pragma warning disable CS0414
    private bool _validUsername;
#pragma warning restore CS0414
#pragma warning disable CS0414
    private bool _loggedIn;
#pragma warning restore CS0414
    private string _urlGameServer;
    private string _pathUpdateCredentials;
#pragma warning disable CS0414
    private string _pathUpdateFeedback;
#pragma warning restore CS0414
    private string _pathCheckUserName;
    private string _pathCheckUsernameAndPassword;
    private string _jsonData;
    [FormerlySerializedAs("LoggedInUser")] public string loggedInUser;

    // Start is called before the first frame update
    void Awake()
    {
        _loggedIn = false;
        _urlGameServer = "https://TQLOBBSN2N5PMVQY.anvil.app/IANHMSZIEXYQHRVG3CB6WIA4/_/api/";
        _pathUpdateCredentials = "credentials";
        _pathUpdateFeedback = "playtestdata";
        _pathCheckUserName = "checkusername";
        _pathCheckUsernameAndPassword = "checkusernameandpassword";
        _logInButton = GameObject.Find("LogInButton");

        _popUpUnsuccessfulLoginButton = GameObject.Find("PopUpUnsuccessfulLogin");

        _popUpUnsuccessfulRegistrationButton = GameObject.Find("PopUpUnsuccessfulRegistration");

        _popUpSuccessfulRegistrationButton = GameObject.Find("PopUpSuccessfulRegistration");

        _popUpUserNameTaken = GameObject.Find("PopUpUserNameTaken");

        _hud = GameObject.Find("HUD");




        passwordInput.contentType = TMPro.TMP_InputField.ContentType.Password;
        TogglePasswordCensorIcon.sprite = Locked;

        _validPassword = false;
        _validUsername = false;
        ClosePopUp();
        _hud.SetActive(false);
        if (_hud.GetComponent<HudManager>().GetLoggedIn())
        {
            _logInButton.SetActive(false);
            _hud.SetActive(true);
        }
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Clears the input fields on the registration menu
    /// </summary>
    public void clearFields()
    {
        usernameInput.text = "";
        passwordInput.text = "";
        _validPassword = false;
        _validUsername = false;
    }

    /// <summary>
    /// Goes to the main menu
    /// </summary>
    public void GoToMainMenu()
    {
        //Debug.Log("Turn me off");

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Toggles the password text between censored and uncensored
    /// </summary>
    public void CensorText()
    {
       // Debug.Log("Function Called");
        if (passwordInput.contentType == TMPro.TMP_InputField.ContentType.Password)
        {
          //  Debug.Log("Standard");
            TogglePasswordCensorIcon.sprite = unlocked;
            passwordInput.contentType = TMPro.TMP_InputField.ContentType.Standard;
        }
        else
        {
           // Debug.Log("Password");
            TogglePasswordCensorIcon.sprite = Locked;
            passwordInput.contentType = TMPro.TMP_InputField.ContentType.Password;
        }
    }

    /// <summary>
    /// Removes spaces in a username and password
    /// </summary>
    public void RemoveSpaces()
    {
        //Debug.Log("With Spaces : " + usernameInput.text);
        //Debug.Log("With Spaces : " + passwordInput.text);
        usernameInput.text = usernameInput.text.Replace(" ", "");
        passwordInput.text = passwordInput.text.Replace(" ", "");
        //Debug.Log("Without Spaces : " + usernameInput.text);
        //Debug.Log("Without Spaces : " + passwordInput.text);
    }

    /// <summary>
    /// Checks if the username and password was of a valid length to start logging in
    /// </summary>
    public void Login()
    {
        Debug.Log("getting Data");
        RemoveSpaces();
        if (passwordInput.text.Length >= 6 && usernameInput.text.Length <= 10)
        {
            Debug.Log("Valid username and password Length");
            _loginData.username = usernameInput.text;
            _loginData.password = passwordInput.text;
            clearFields();
            _jsonData = JsonUtility.ToJson(_loginData);
            StartCoroutine(LogInAccount(_jsonData));
        }
        else
        {
            ClosePopUp();
            _popUpUnsuccessfulLoginButton.SetActive(true);
        }
    }

    /// <summary>
    /// Sends data to the anvil server to verify the username and password entered.
    /// Logs the user in if the credentials were successful
    /// Prompts them to try again if not
    /// </summary>
    /// <param name="jsonData"></param>
    /// <returns></returns>
    public IEnumerator LogInAccount(string jsonData)
    {
        //Debug.Log("Made It this far");

        string url = _urlGameServer + _pathCheckUsernameAndPassword;

        using (UnityWebRequest request = UnityWebRequest.Put(url, jsonData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.downloadHandler.text == "CanLogIn")
            {
                Debug.Log("Can log in valid details entered");
                UsernameHud.text = _loginData.username;
                _logInButton.SetActive(false);
                if(_hud == null)
                {
                    
                }
                _hud.SetActive(true);
                _hud.GetComponent<HudManager>().SetLoggedIn();
                _loggedIn = true;
                GoToMainMenu();
            }
            else
            {
                ClosePopUp();
                _popUpUnsuccessfulLoginButton.SetActive(true);
            }

            if (!request.isNetworkError && request.responseCode == (int)HttpStatusCode.OK)
            {
                //Debug.Log("Downloaded : " + request.downloadHandler.text);
                //Debug.Log("Data successfully sent to the server");

            }
            else
            {
                Debug.Log("Error sending data to the server: Error " + request.responseCode);
            }
        }
    }

    /// <summary>
    /// Checks if the username and password was of a valid length, 
    /// between 6 and 10 letters
    /// before sending it to the anvil server to register the details
    /// </summary>
    public void RegisterData()
    {
        //Debug.Log("getting Data");
        RemoveSpaces();
        if (passwordInput.text.Length >= 6 && usernameInput.text.Length <=10)
        {
            _loginData.username = usernameInput.text;
            _loginData.password = passwordInput.text;
            clearFields();
            _jsonData = JsonUtility.ToJson(_loginData);
            StartCoroutine(RegisterUsername(_jsonData));
        }
        else
        {
            ClosePopUp();
            _popUpUnsuccessfulRegistrationButton.SetActive(true);
        }
        // StartCoroutine(GetUsername());
    }

    /// <summary>
    /// Sends the username and password data to the anvil server.
    /// Checks if the username has been taken.
    /// If not the users details are registered successfully.
    /// If taken a pop up prompts the user to try again
    /// </summary>
    /// <param name="jsonData"></param>
    /// <returns></returns>
    public IEnumerator RegisterUsername(string jsonData)
    {
        string url = _urlGameServer + _pathCheckUserName;

        using (UnityWebRequest request = UnityWebRequest.Put(url, jsonData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.downloadHandler.text == "NotTaken")
            {
                ClosePopUp();
                _popUpSuccessfulRegistrationButton.SetActive(true);
                StartCoroutine(PostMethod(jsonData, _pathUpdateCredentials));
            }
            else
            {
                _validUsername = false;
                ClosePopUp();
                _popUpUserNameTaken.SetActive(true);
            }

            if (!request.isNetworkError && request.responseCode == (int)HttpStatusCode.OK)
            {
                //Debug.Log("Downloaded : " + request.downloadHandler.text);
               // Debug.Log("Data successfully sent to the server");

            }
            else
            {
                //Debug.Log("Error sending data to the server: Error " + request.responseCode);
            }
        }
    }

    /// <summary>
    /// Sends the data to an external database hosted on anvil
    /// </summary>
    public IEnumerator PostMethod(string jsonData, string t_path)
    {
        // player data and credentials
        string url = "https://TQLOBBSN2N5PMVQY.anvil.app/IANHMSZIEXYQHRVG3CB6WIA4/_/api/credentials";

        using (UnityWebRequest request = UnityWebRequest.Put(url, jsonData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (!request.isNetworkError && request.responseCode == (int)HttpStatusCode.OK)
            {
                //Debug.Log("Data Registered");

            }
            else
            {
                //Debug.Log("Error sending data to the server: Error " + request.responseCode);
            }
        }
    }

    /// <summary>
    /// Sets the login screen to active, making it visible to the player
    /// </summary>
    public void ActivateLogInScreen()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Closes the pop ups that display errors and prompts to the user.
    /// </summary>
    public void ClosePopUp()
    {
        _popUpSuccessfulRegistrationButton.SetActive(false);
        _popUpUnsuccessfulLoginButton.SetActive(false);
        _popUpUnsuccessfulRegistrationButton.SetActive(false);
        _popUpUserNameTaken.SetActive(false);
    }

}