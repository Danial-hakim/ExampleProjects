/*
Olympus Run - A game made as part of the ARGO Project at SETU Carlow
Copyright (C) 2023 Caroline Percy <lineypercy@me.com>, Patrick Donnelly <patrickdonnelly3759@gmail.com>, 
                   Izabela Zelek <izabelawzelek@gmail.com>, Danial Hakim <danialhakim01@gmail.com>, 
                   Adrien Dudon <dudonadrien@gmail.com>

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

using System;
using Mirror;
using TMPro;
using UnityEngine;

/// <summary>
/// Class that updates and manages the player's scores
/// </summary>
public class Score : NetworkBehaviour
{
    private TextMeshProUGUI _comboText;
    private GameObject _comboCanvas;
    private bool _timerIsRunning;
    private float _comboTimeRemaining;
    private int _totalComboPointsSoFar;
    private int _currentCombo;
    private int MaxComboCount =20;

    private TextMeshProUGUI _bonusPointsText;
    private GameObject _bonusPointsCanvas;
    private int _baseIncrease;
    private int _currentPlayTime;
    private int _currentDistanceTravelled;
    private int _currentSpeed;
    private int _totalBonusPointsSoFar;
    private float _popUpTimeRemaining;
    private bool _popUpTimerIsRunning;
    private bool _distanceMissionComplete;
    private bool _speedMissionComplete;
    private bool _timeAliveMissionComplete;
    private bool _goalComplete;

    private TextMeshProUGUI _scoreText;
    private GameObject _scoreCanvas;
    private int _totalScore;

    private MoveForward _moveForwardScript;

    private void Awake()
    {
        // Disable the script by default. It will be enabled only by the local player
        enabled = false;

        _comboCanvas = GameObject.Find("ComboCanvas");
        _bonusPointsCanvas = GameObject.Find("BonusPointsCanvas");
        _scoreCanvas = GameObject.Find("ScoreCanvas");
        
        // Deactivate every UI by default if you're not the local player
        if (_comboCanvas != null)
            _comboCanvas.SetActive(false);
        
        if (_bonusPointsCanvas != null)
            _bonusPointsCanvas.SetActive(false);
        
        if (_scoreCanvas != null)
            _scoreCanvas.SetActive(false);
    }

    public override void OnStartAuthority()
    {
        _comboCanvas.SetActive(true);
        _bonusPointsCanvas.SetActive(true);
        _scoreCanvas.SetActive(true);
        
        _distanceMissionComplete = false;
        _speedMissionComplete = false;
        _timeAliveMissionComplete = false;
        _totalScore = 0;
        _baseIncrease = 50;
        _currentCombo = 0;
        _totalComboPointsSoFar = 0;
        _comboTimeRemaining = 3.0f;
        _comboText = _comboCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _comboCanvas.SetActive(false);

        _popUpTimeRemaining = 3.0f;
        _bonusPointsText = _bonusPointsCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _bonusPointsCanvas.SetActive(false);

        _scoreText = _scoreCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        _moveForwardScript = GetComponent<MoveForward>();
        _currentPlayTime = 0;
        _currentSpeed = 8;
        
        enabled = true;
    }

    public override void OnStopAuthority()
    {
        enabled = false;
    }

    void Update()
    {
        if (_comboText == null || _comboCanvas == null)
        {
            _comboCanvas = GameObject.Find("ComboCanvas");
            _comboText = _comboCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _comboCanvas.SetActive(false);
        }

        if (_bonusPointsText == null || _bonusPointsCanvas == null)
        {
            Debug.Log("Cannot Find BonusPointsCanvas - null refernce");
            _bonusPointsCanvas = GameObject.Find("BonusPointsCanvas");
            _bonusPointsText = _bonusPointsCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _bonusPointsCanvas.SetActive(false);
        }

        if (_scoreText == null || _scoreCanvas == null)
        {
            Debug.Log("Cannot Find ScoreCanvas - null refernce");
            _scoreCanvas = GameObject.Find("ScoreCanvas");
            _scoreText = _scoreCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        if (_timerIsRunning)
        {
            if (_comboTimeRemaining > 0)
            {
                _comboTimeRemaining -= Time.deltaTime;
            }
            else
            {
                ResetCombo();
            }
        }

        if (_popUpTimerIsRunning)
        {
            if (_popUpTimeRemaining > 0)
            {
                _popUpTimeRemaining -= Time.deltaTime;
            }
            else
            {
                ResetPopUp();
            }
        }

        //Debug.Log(_moveForwardScript.GetPlayTime());

        CheckDistanceBonusPoints(_moveForwardScript.GetDistanceTravelled());
        CheckSpeedBonusPoints(_moveForwardScript.GetSpeed());
        CheckTimeAliveBonusPoints(_moveForwardScript.GetPlayTime());
    }

    /// <summary>
    /// Checks if the player has earned any bonus points based on the speed levels they have reached
    /// </summary>
    /// <param name="Speed"></param>
    public void CheckSpeedBonusPoints(int Speed)
    {
        if (CheckIfSpeedChanged(Speed))
        {
            if (Speed < 24)
            {
                if (Speed % 4 == 0 && Speed > 8)
                {
                    _speedMissionComplete = true;
                    _baseIncrease = 50;
                    AddMissionPoints(Speed / 4);
                }
            }
            else if (Speed == 24)
            {
                _speedMissionComplete = true;
                _baseIncrease = 100;
                AddMissionPoints(Speed / 6);
            }
            else if (Speed == 30)
            {
                _speedMissionComplete = true;
                _baseIncrease = 100;
                AddMissionPoints(Speed / 5);
            }
        }
    }

    /// <summary>
    /// Ensures Bonus points for Distance Travelled Missions are only applied
    /// once and not multiple times
    /// </summary>
    /// <returns></returns>
    private bool CheckIfDistanceTravelledChanged(int DistanceTravelled)
    {
        if (_currentDistanceTravelled < DistanceTravelled)
        {
            _currentDistanceTravelled = DistanceTravelled;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Ensures Bonus points for Speed Missions are only applied
    /// once and not multiple times
    /// </summary>
    /// <returns></returns>
    private bool CheckIfSpeedChanged(int Speed)
    {
        if (_currentSpeed < Speed)
        {
            _currentSpeed = Speed;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Ensures Bonus points for Play Time Missions are only applied
    /// once and not multiple times
    /// </summary>
    /// <returns></returns>
    private bool CheckIfPlayTimeChanged(int PlayTime)
    {
        if (_currentPlayTime < PlayTime)
        {
            _currentPlayTime = PlayTime;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the player has earned bonus points based on how long they have survived their run
    /// </summary>
    /// <param name="PlayTime"></param>
    public void CheckTimeAliveBonusPoints(int PlayTime)
    {
        if (CheckIfPlayTimeChanged(PlayTime))
        {
            if (PlayTime >= 30)
            {
                if (PlayTime == 30)
                {
                    _timeAliveMissionComplete = true;
                    _baseIncrease = 100;
                    AddMissionPoints(PlayTime / 15);
                }

                else if (PlayTime == 60)
                {
                    _timeAliveMissionComplete = true;
                    _baseIncrease = 100;
                    AddMissionPoints(PlayTime / 12);
                }
                else if (PlayTime % 60 == 0)
                {
                    _timeAliveMissionComplete = true;
                    _baseIncrease = 100;
                    AddMissionPoints(PlayTime / 12);
                }
            }
        }
    }

    /// <summary>
    /// Resets the popup for bonus points
    /// </summary>
    private void ResetPopUp()
    {
        _popUpTimeRemaining = 0;
        _bonusPointsText.text = "";
        _totalBonusPointsSoFar = 0;
        _popUpTimerIsRunning = false;
        _bonusPointsCanvas.SetActive(false);
        _distanceMissionComplete = false;
        _speedMissionComplete = false;
        _timeAliveMissionComplete = false;
        _goalComplete = false;
    }

    /// <summary>
    /// Gets and returns the players current combo
    /// </summary>
    /// <returns></returns>
    public int GetCurrentCombo()
    {
        return _currentCombo;
    }

    /// <summary>
    /// Resets the players combo to 0
    /// </summary>
    public void ResetCombo()
    {
        _currentCombo = 0;
        _totalComboPointsSoFar = 0;
        _comboText.text = "";
        _comboTimeRemaining = 0;
        _timerIsRunning = false;
        _comboCanvas.SetActive(false);
    }

    /// <summary>
    /// Adds points for any bonus missions the player has achieved to their total score
    /// </summary>
    /// <param name="multiplier"></param>
    private void AddMissionPoints(int multiplier)
    {
        _totalScore += _baseIncrease * multiplier;
        SetBonusPointsText(_baseIncrease * multiplier);
    }

    /// <summary>
    /// Adds any combo points the player has earned on an active combo to the players score
    /// </summary>
    /// <param name="collectiblePoints"></param>
    public void AddComboPoints(int collectiblePoints)
    {
        if (!isLocalPlayer)
        {
            Debug.LogError("AddComboPoints() called on a non-local player.");
            return;
        }

        SetComboTimer();

        _currentCombo += 1;
        if (_currentCombo <= MaxComboCount)
        {
            _totalComboPointsSoFar += collectiblePoints * _currentCombo;
            _totalScore += collectiblePoints * _currentCombo;
        }
        else
        {
            _totalComboPointsSoFar += collectiblePoints * MaxComboCount;
            _totalScore += collectiblePoints * MaxComboCount;
        }
        SetComboText(_currentCombo);
        _scoreText.text = "" + _totalScore + "";
    }

    /// <summary>
    /// Adds points for any pick up goal achievements to the players total score
    /// </summary>
    /// <param name="GoalPoints"></param>
    public void AddGoalPoints(int GoalPoints)
    {
        _goalComplete = true;
        if (!isLocalPlayer)
        {
            Debug.LogError("AddComboPoints() called on a non-local player.");
            return;
        }
        _totalScore += GoalPoints;
        _scoreText.text = "" + _totalScore + "";
        SetBonusPointsText(GoalPoints);
    }

    /// <summary>
    /// Sets the combo timer to determine how long a combo streak will be active
    /// </summary>
    private void SetComboTimer()
    {
        _comboTimeRemaining = 3.0f;
        _timerIsRunning = true;
    }

    /// <summary>
    /// Gets and returns wheter the combo timer is running or not
    /// </summary>
    /// <returns></returns>
    public bool GetComboTimer()
    {
        return _timerIsRunning;
    }

    /// <summary>
    /// Sets the text of the combo canvas to display the players combo
    /// </summary>
    /// <param name="currentCombo"></param>
    private void SetComboText(int currentCombo)
    {
        _comboCanvas.SetActive(true);
        _comboText.text = "COMBO X " + currentCombo + " : " + _totalComboPointsSoFar + " Points";
    }

    /// <summary>
    /// Sets how long the bonus points pop up will be displayed
    /// </summary>
    private void SetPopUpTimer()
    {
        _popUpTimeRemaining = 3.0f;
        _popUpTimerIsRunning = true;
    }

    /// <summary>
    /// Sets the pop up text for any bonus points earned
    /// </summary>
    /// <param name="bonusPoints"></param>
    private void SetBonusPointsText(int bonusPoints)
    {
        _totalBonusPointsSoFar += bonusPoints;
        _bonusPointsText.text = "";
        SetPopUpTimer();
        _bonusPointsCanvas.SetActive(true);
        if (_distanceMissionComplete)
        {
            _bonusPointsText.text += ("Distance Travelled " + _moveForwardScript.GetDistanceTravelled() + "\r\n");
        }

        if (_speedMissionComplete)
        {
            _bonusPointsText.text += ("Speed Reached " + _moveForwardScript.GetSpeed() + "\r\n");
        }

        if (_timeAliveMissionComplete)
        {
            _bonusPointsText.text += ("Seconds Survived " + _moveForwardScript.GetPlayTime() + "\r\n");
        }

        if(_goalComplete)
        {
            _bonusPointsText.text += ("Goal Complete ");
        }

        _bonusPointsText.text += "+" + _totalBonusPointsSoFar + " Points";
        _scoreText.text = "" + _totalScore + "";
    }

    /// <summary>
    /// Checks if player has earned bonus points based on the distance travelled
    /// </summary>
    /// <param name="DistanceTravelled"></param>
    public void CheckDistanceBonusPoints(int DistanceTravelled)
    {
        if (CheckIfDistanceTravelledChanged(DistanceTravelled))
        {
            if (DistanceTravelled > 0)
            {
                if (DistanceTravelled == 100)
                {
                    _distanceMissionComplete = true;
                    _baseIncrease = 50;
                    AddMissionPoints(DistanceTravelled / 100);
                }
                else if (DistanceTravelled % 250 == 0 &&
                         DistanceTravelled <= 1000)
                {
                    _distanceMissionComplete = true;
                    _baseIncrease = 50;
                    AddMissionPoints(DistanceTravelled / 125);
                }
                else if (DistanceTravelled % 500 == 0)
                {
                    _distanceMissionComplete = true;
                    _baseIncrease = 100;
                    AddMissionPoints(DistanceTravelled / 250);
                }
            }
        }
    }

    /// <summary>
    /// Resets the Players score to zero
    /// </summary>
    public void ResetScore()
    {
        _totalScore = 0;
    }

    /// <summary>
    /// Gets and returns the players score
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return _totalScore;
    }
}