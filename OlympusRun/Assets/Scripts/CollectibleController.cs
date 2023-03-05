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

using System;
using UnityEngine;

/// <summary>
/// Handles the destruction and score of pickups
/// </summary>
public class CollectibleController : MonoBehaviour
{
    private int _pointsForPickUp;
    public static event Action OnPlayerDeath;
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
    

    // Start is called before the first frame update
    void Start()
    {
        _pointsForPickUp = 5;
    }

    /// <summary>
    /// Checks Collision between pick ups, solid objects and a collider offscreen.
    /// Sets them to inactive if collision detected
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Inpenetrable" || other.tag == "JumpObstacle" || other.tag == "SlideObstacle")
        {
            gameObject.SetActive(false);
        }
        else if (other.tag == "BehindPlayer")
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Checks Collision between the runner and pick ups
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (_runner == null)
        {
            _runner = GameObject.FindGameObjectWithTag("Runner");
            _runnerScoreScript = _runner.GetComponent<Score>();
        }

        else if (other.tag == "Runner")
        {
            if (gameObject.transform.tag == "SpeedUp")
            {
                GameObject.Find("PickupController").GetComponent<PickupController>().SpeedUp();
                GameObject.Find("PickupController").GetComponent<GoalController>().AddSpeedUp();
                GetComponent<PointOfInterest>().StartHit(other);
            }
            else if (gameObject.transform.tag == "SpeedDown")
            {
                GameObject.Find("PickupController").GetComponent<PickupController>().SpeedDown();
                GameObject.Find("PickupController").GetComponent<GoalController>().AddSpeedDown();
                GetComponent<PointOfInterest>().StartHit(other);
            }
            else if (gameObject.transform.tag == "MaxHealth")
            {
                GameObject.Find("PickupController").GetComponent<GoalController>().AddMaxHealth();
                GameObject.FindGameObjectWithTag("Runner").GetComponent<RunnerHealthController>().InstantHeal();
                GetComponent<PointOfInterest>().StartHit(other);
            }
            else if (gameObject.transform.tag == "PartHealth")
            {
                GameObject.FindGameObjectWithTag("Runner").GetComponent<RunnerHealthController>().PartialHeal();
                GetComponent<PointOfInterest>().StartHit(other);
            }
            else if (gameObject.transform.tag == "Apple")
            {
                GameObject.Find("PickupController").GetComponent<GoalController>().AddApple();
                GetComponent<PointOfInterest>().StartHit(other);
                _runnerScoreScript.AddComboPoints(_pointsForPickUp);
            }
            else if (gameObject.transform.tag == "Pomegranate")
            {
                GameObject.Find("PickupController").GetComponent<GoalController>().AddPom();
                GetComponent<PointOfInterest>().StartHit(other);
                _runnerScoreScript.AddComboPoints(_pointsForPickUp);
            }
            else if (gameObject.transform.tag == "Coin")
            {
                Debug.Log("Got Coin");
                GameObject.Find("PickupController").GetComponent<GoalController>().AddCoins();
                GetComponent<PointOfInterest>().StartHit(other);
                _runnerScoreScript.AddComboPoints(_pointsForPickUp);
            }
            gameObject.SetActive(false);
        }
    }

    public int GetPointsForPickUps()
    {
        return _pointsForPickUp;
    }
}
