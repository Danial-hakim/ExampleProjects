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
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A class that defines player data
/// </summary>
public class Data
{
    public int playerID = 0;
    public string role = " ";
    public int distanceTravelled = 0;
    public int obstaclesAvoided = 0;
    public int obstaclesHit = 0;
    public int obstaclesPlaced = 0;
    public int score = 0;
    public double timePlayed = 0;
}

/// <summary>
/// Data specific to the leaderboard
/// </summary>
public class LeaderBoardData
{
    public string username = " ";
    public int score = 0;
}

/// <summary>
/// Receives, stores, packs and sends player data to an external database hosted on anvil
/// </summary>
public class AnalyticsManager : MonoBehaviour
{
    [SerializeField] private GameObject objectSpawner;
    
    private GameObject _runner;
    public GameObject Runner
    {
        set => _runner = value;
    }
    
    private GameObject _god;
    public GameObject God  {
        set => _god = value;
    }

    private static string _userID;

    private double _startTime;
    private double _endTime;
    private double _playTime;

    private readonly Data _runnerData = new Data(); //initialise all data
    private readonly Data _zeusData = new Data(); //initialise all data
    private readonly Data _poseidonData = new Data(); //initialise all data
    private readonly LeaderBoardData _leaderboardData = new LeaderBoardData();

    bool _dataSent;

    /// <summary>
    /// Sets default values for data
    /// </summary>
    void Start()
    {
        _endTime = 0;
        _playTime= 0;
        _startTime = System.DateTimeOffset.Now.ToUnixTimeSeconds();
        _dataSent = false;
        _runnerData.playerID = Random.Range(1, 100000000);
        _zeusData.playerID = Random.Range(1, 100000000);
        _poseidonData.playerID = Random.Range(1, 100000000);
        if (GameObject.Find("PlayerUserName") != null)
        {
            _userID = GameObject.Find("PlayerUserName").GetComponent<TextMeshProUGUI>().text;
        }
        else
        {
            _userID = "None";
        }
    }

    /// <summary>
    /// Sends data when the player dies
    /// </summary>
    void Update()
    {
  

        if (_runner == null)
            return;

        if (!_dataSent && _runner.transform.GetChild(0).gameObject.activeSelf == false)
        {
                _dataSent = true;
                sendData();
        }
    }

    /// <summary>
    /// Packs player data and calls a coroutine that will send the data to an external database on anvil
    /// </summary>
    public void sendData()
    {
        _endTime = System.DateTimeOffset.Now.ToUnixTimeSeconds();
        _playTime = _endTime - _startTime;
       // Debug.Log("Sending Data");

        if (_runner.CompareTag("Runner"))
        {
            _runnerData.role = "Runner";
            _runnerData.distanceTravelled = _runner.GetComponent<MoveForward>().GetDistanceTravelled();
            // extract data from finished scripts for the runner later on 
            _runnerData.obstaclesAvoided = _runner.GetComponent<MoveForward>().ObstaclesAvoided;
            _runnerData.obstaclesHit = 1;
            _runnerData.obstaclesPlaced = objectSpawner.GetComponent<ObstacleSpawner>().ObstaclesPlaced;
            _runnerData.score = _runner.GetComponent<Score>().GetScore();
            _runnerData.timePlayed = _playTime;



            string jsonData = JsonUtility.ToJson(_runnerData);
            StartCoroutine(PostMethod(jsonData));
            //Debug.Log(_userID);
            if (_userID != "None" || _userID == null)
            {
                _leaderboardData.username = _userID;
                _leaderboardData.score = _runnerData.score;
                Debug.Log("Score" + _leaderboardData.score);
                string jsonData2 = JsonUtility.ToJson(_leaderboardData);
                StartCoroutine(PostMethodLeaderBoard(jsonData2));
            }

        }
        if (_god.CompareTag("God"))
        {
            // extract data from the zeus player when done
            _zeusData.role = "God";
            _zeusData.obstaclesPlaced = 0;
            _zeusData.score = 0;
            _zeusData.timePlayed = 0;

            string jsonData = JsonUtility.ToJson(_zeusData);
            StartCoroutine(PostMethod(jsonData));
        }

    }

    /// <summary>
    /// Sends the data to an external database hosted on anvil
    /// </summary>
    public static IEnumerator PostMethod(string jsonData)
    {
        string url = "https://TQLOBBSN2N5PMVQY.anvil.app/IANHMSZIEXYQHRVG3CB6WIA4/_/api/playtestdata";

        using (UnityWebRequest request = UnityWebRequest.Put(url, jsonData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (!request.isNetworkError && request.responseCode == (int)HttpStatusCode.OK)
            {
                Debug.Log("Data successfully sent to the server");

            }
            else
            {
                Debug.Log("Error sending data to the server: Error " + request.responseCode);
            }
        }
    }

    /// <summary>
    /// Sends the data to an external database hosted on anvil
    /// </summary>
    public static IEnumerator PostMethodLeaderBoard(string jsonData)
    {
        string url = "https://TQLOBBSN2N5PMVQY.anvil.app/IANHMSZIEXYQHRVG3CB6WIA4/_/api/leaderboard";

        using (UnityWebRequest request = UnityWebRequest.Put(url, jsonData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            //Debug.Log(jsonData);
            yield return request.SendWebRequest();

            if (!request.isNetworkError && request.responseCode == (int)HttpStatusCode.OK)
            {
                Debug.Log("Data successfully sent to the leaderboard server");

            }
            else
            {
                Debug.Log("Error sending data to the leaderboard server: Error " + request.responseCode);
            }
        }
    }
}
