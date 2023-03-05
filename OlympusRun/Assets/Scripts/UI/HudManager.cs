using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Classes that manages the hud between scenes
/// </summary>
public class HudManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static HudManager Instance;

    private bool _loggedIn = false;
    private string _username = "";

    /// <summary>
    /// Sets the username to be displayed when awake
    /// </summary>
    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _username = GameObject.Find("PlayerUserName").GetComponent<TextMeshProUGUI>().text;
        Debug.Log(_username);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// returns true if the user is logged in
    /// Returns false if the user has not logged in
    /// </summary>
    /// <returns></returns>
    public bool GetLoggedIn()
    {
        return _loggedIn;
    }

    /// <summary>
    /// Sets the login to true
    /// </summary>
    public void SetLoggedIn()
    {

        _loggedIn = true;
    }

    /// <summary>
    /// Gets and returns the players username
    /// </summary>
    /// <returns></returns>
    public string GetUsername()
    {
        return _username;
    }
}
