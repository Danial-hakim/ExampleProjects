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

using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class Deals with randomising goals for the player to reach during their run
/// </summary>
public class GoalController : MonoBehaviour
{

    private Score _runnerScoreScript;

    private GameObject _runner;
    public GameObject Runner
    {
        set
        {
            _runner = value;
            _runnerScoreScript = _runner.GetComponent<Score>();
        }
    }

    private int _goalMultiplier = 100;

    private List<int> _noToCollect = new List<int>();

    private int _chosenGoal = -1;
    private int _lastGoal = -1;
    private TextMeshProUGUI _goalText;
    private bool _goalComplete = false;

    private int _appleCount = 0;
    private int _pomCount = 0;
    private int _speedUpCount = 0;
    private int _speedDownCount = 0;
    private int _maxHealthCount = 0;
    private int _coinCount = 0;
    private int _leftToCollect;

    private int _currentCollectionGoal = 0;

    private bool _collectApples = false;
    private bool _collectPoms = false;
    private bool _collectSpeedUpPotions = false;
    private bool _collectSpeedDownPotions = false;
    private bool _collectMaxHealthPotions = false;
    private bool _collectCoins = false;

    void Start()
    {
        _goalText = GameObject.Find("Goal").GetComponent<TextMeshProUGUI>();
        AddCollectionGoals();

        _currentCollectionGoal = _noToCollect[Random.Range(0, 7)];
        _leftToCollect = _currentCollectionGoal;
        _chosenGoal = Random.Range(0, 6);

        _lastGoal = _chosenGoal;
        ChooseGoal();
    }

    void Update()
    {
        if (_runner == null)
        {
            _runner = GameObject.FindGameObjectWithTag("Runner");
            _runnerScoreScript = _runner.GetComponent<Score>();
        }

        if (_collectApples)
        {
            _goalText.text = "Collect " + _leftToCollect + " Apples";
            if (_appleCount >= _currentCollectionGoal && !_goalComplete)
            {
                _appleCount = 0;
                _goalComplete = true;
                _runnerScoreScript.AddGoalPoints(_currentCollectionGoal * _goalMultiplier);
            }
        }
        else if (_collectPoms)
        {
            _goalText.text = "Collect " + _leftToCollect + " Poms";
            if (_pomCount >= _currentCollectionGoal && !_goalComplete)
            {
                _pomCount = 0;
                _goalComplete = true;
                _runnerScoreScript.AddGoalPoints(_currentCollectionGoal * _goalMultiplier);
            }
        }
        else if (_collectSpeedUpPotions)
        {
            _goalText.text = "Use " + _leftToCollect + " Speed Up Potions";
            if (_speedUpCount >= _currentCollectionGoal && !_goalComplete)
            {
                _speedUpCount = 0;
                _goalComplete = true;
                _runnerScoreScript.AddGoalPoints(_currentCollectionGoal * _goalMultiplier);
            }
        }
        else if (_collectSpeedDownPotions)
        {
            _goalText.text = "Use " + _leftToCollect + " Speed Down Potions";

            if (_speedDownCount >= _currentCollectionGoal && !_goalComplete)
            {
                _speedDownCount = 0;
                _goalComplete = true;
                _runnerScoreScript.AddGoalPoints(_currentCollectionGoal * _goalMultiplier);
            }
        }
        else if (_collectMaxHealthPotions)
        {
            _goalText.text = "Use " + _leftToCollect + " Max Health Potions";

            if (_maxHealthCount >= _currentCollectionGoal && !_goalComplete)
            {
                _maxHealthCount = 0;
                _goalComplete = true;
                _runnerScoreScript.AddGoalPoints(_currentCollectionGoal * _goalMultiplier);
            }
        }
        else if (_collectCoins)
        {
            _goalText.text = "Collect " + _leftToCollect + " Coins";

            if (_coinCount >= _currentCollectionGoal && !_goalComplete)
            {
                _coinCount = 0;
                _goalComplete = true;
                _runnerScoreScript.AddGoalPoints(_currentCollectionGoal * _goalMultiplier);
            }
        }

        if (_goalComplete)
        {
            _chosenGoal = Random.Range(0, 6);
            while (_lastGoal == _chosenGoal)
            {
                _chosenGoal = Random.Range(0, 6);
            }
            _currentCollectionGoal = _noToCollect[Random.Range(0, 7)];
            _leftToCollect = _currentCollectionGoal;
            _goalComplete = false;
            ResetGoals();
            ChooseGoal();
            _lastGoal = _chosenGoal;
        }
    }

    /// <summary>
    /// Resets the goals so a new one can be picked
    /// </summary>
    private void ResetGoals()
    {
        _collectApples = false;
        _collectPoms = false;
        _collectSpeedUpPotions = false;
        _collectSpeedDownPotions = false;
        _collectMaxHealthPotions = false;
        _collectCoins = false;
    }

    /// <summary>
    /// Turns on appropriate bool based on chosen goals
    /// </summary>
    private void ChooseGoal()
    {
        switch (_chosenGoal)
        {
            case 0:
                _collectApples = true;
                break;
            case 1:
                _collectPoms = true;
                break;
            case 2:
                _collectSpeedUpPotions = true;
                break;
            case 3:
                _collectSpeedDownPotions = true;
                break;
            case 4:
                _collectMaxHealthPotions = true;
                break;
            case 5:
                _collectCoins = true;
                break;
        }
    }

    /// <summary>
    /// Adds the number of pickups that need to be collected for the 
    /// goal to be achieved to a list.
    /// </summary>
    private void AddCollectionGoals()
    {
        _noToCollect.Add(2);
        _noToCollect.Add(3);
        _noToCollect.Add(4);
        _noToCollect.Add(5);
        _noToCollect.Add(6);
        _noToCollect.Add(8);
        _noToCollect.Add(10);
    }

    /// <summary>
    /// Keeps track of the apples collected and left to collect to reach the goal
    /// </summary>
    public void AddApple()
    {
        _appleCount++;
        if (_collectApples)
        {
            _leftToCollect--;
        }
    }

    /// <summary>
    /// Keeps track of the Poms collected and left to collect to reach the goal
    /// </summary>
    public void AddPom()
    {
        _pomCount++;
        if (_collectPoms)
        {
            _leftToCollect--;
        }
    }

    /// <summary>
    /// Keeps track of the SpeedUp Potions collected and left to collect to reach the goal
    /// </summary>
    public void AddSpeedUp()
    {
         _speedUpCount++;
        if (_collectSpeedUpPotions)
        {
            _leftToCollect--;
        }
    }

    /// <summary>
    /// Keeps track of the Speed Down Potions collected and left to collect to reach the goal
    /// </summary>
    public void AddSpeedDown()
    {
        _speedDownCount++;
        if (_collectSpeedDownPotions)
        {
            _leftToCollect--;
        }
    }

    /// <summary>
    /// Keeps track of the Coins collected and left to collect to reach the goal
    /// </summary>
    public void AddCoins()
    {
        _coinCount++;
        if (_collectCoins)
        {
            _leftToCollect--;
        }
    }

    /// <summary>
    /// Keeps track of the Max Health Potions collected and left to collect to reach the goal
    /// </summary>
    public void AddMaxHealth()
    {
         _maxHealthCount++;
        if (_collectMaxHealthPotions)
        {
            _leftToCollect--;
        }
    }

    /// <summary>
    /// Gets the number of pickups that need to be collected for the goal to be complete
    /// </summary>
    public int GetCurrentCollectionGoal()
    {
        return _currentCollectionGoal;
    }

    /// <summary>
    /// Checks if the apple goal is active
    /// </summary>
    /// <returns></returns>
    public bool isAppleGoal()
    {
        if (_collectApples) { return _collectApples; }
        return false;
    }

    /// <summary>
    /// Checks if the Pom goal is active
    /// </summary>
    /// <returns></returns>
    public bool isPomGoal()
    {
        if (_collectPoms) { return _collectPoms; }
        return false;
    }

    /// <summary>
    /// Checks if the Coin goal is active
    /// </summary>
    /// <returns></returns>
    public bool isCoinGoal()
    {
        if (_collectCoins) { return _collectCoins; }
        return false;
    }

    /// <summary>
    /// Checks if the Max Health Potion goal is active
    /// </summary>
    /// <returns></returns>
    public bool isMaxHealthGoal()
    {
        if (_collectMaxHealthPotions) { return _collectMaxHealthPotions; }
        return false;
    }

    /// <summary>
    /// Checks if the Speed Down Potion goal is active
    /// </summary>
    /// <returns></returns>
    public bool isSpeedDownGoal()
    {
        if (_collectSpeedDownPotions) { return _collectSpeedDownPotions; }
        return false;
    }

    /// <summary>
    /// Checks if the Speed Up Potion goal is active
    /// </summary>
    /// <returns></returns>
    public bool isSpeedUpGoal()
    {
        if (_collectSpeedUpPotions) { return _collectSpeedUpPotions; }
        return false;
    }

    /// <summary>
    /// gets the goal multilier that will be used for points
    /// calculation when a goal is achieved.
    /// </summary>
    /// <returns></returns>
    public int GetGoalMultiplier()
    {
        return _goalMultiplier;
    }
}
