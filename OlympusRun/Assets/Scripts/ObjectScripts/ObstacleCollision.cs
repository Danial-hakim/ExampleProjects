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
using System.Collections;
using Mirror;
using UnityEngine;

/// <summary>
/// A class that checks for collision with the player.
/// Determines wheter an obstacle has hit the player 
/// or has gone offscreen
/// </summary>
public class ObstacleCollision : MonoBehaviour
{
    private GameObject _runner;
    public static event Action OnPlayerDeath;
    private int _pointsForAvoidingObstacle = 5;
    private const int Damage = 20;

    [Header("RBS")]
    public Rule rule;
    AIBrain ai_Brain;
    int izzysWay;

    [ServerCallback]
    private void Start()
    {
        rule = new Rule();
        _runner = GameObject.FindGameObjectWithTag("Runner");
        ai_Brain = _runner.GetComponent<AIBrain>();
    }

    /// <summary>
    /// Checks for collision with the player directly
    /// and an offscreen collider to determine if the player 
    /// avoided or hit an obstacle
    /// </summary>
    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Runner"))
        {
            GetComponent<PointOfInterest>().StartHit(other);
            if (!_runner.GetComponent<RunnerHealthController>().IsRunnerDead())
            {
                _runner.GetComponent<RunnerHealthController>().TakeDamage(Damage);
                _runner.GetComponent<Score>().ResetCombo();
            }
            if (_runner.GetComponent<RunnerHealthController>().IsRunnerDead())
            {
                OnPlayerDeath?.Invoke();
                other.gameObject.SetActive(false);
            }
            NetworkServer.Destroy(gameObject);
        }
        
        if (other.CompareTag("BehindPlayer"))
        {
            if (tag == "Inpenetrable")
            {
                setupRule();
                RuleDatabases.AddRule(rule);
            }
            if (_runner != null)
            {
                _runner.GetComponent<MoveForward>().ObstaclesAvoided += 1;
                _runner.GetComponent<Score>().AddComboPoints(_pointsForAvoidingObstacle);
            }
            else
            {
                _runner = GameObject.FindGameObjectWithTag("Runner");
            }

            StartCoroutine(ServerDestroyAfterTime(2f));
        }
    }

    [Server]
    private IEnumerator ServerDestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        NetworkServer.Destroy(gameObject);
    }

    /// <summary>
    /// Setting the current lane , next lane and the type of obstacle the runner avoid to be add to database
    /// </summary>
    public void setupRule()
    {
        rule.currentLane = ai_Brain.PreviousLane;
        rule.nextLane = ai_Brain.CurrentLane;
        rule.obstacleTag = "Inpenetrable";
        switch (transform.position.x)
        {
            case -2:
                izzysWay = 1;
                break;
            case 0:
                izzysWay = 2;
                break;
            case 2:
                izzysWay = 3;
                break;
        }

        rule.obstacleLane = izzysWay;
        ai_Brain.PreviousLane = ai_Brain.CurrentLane;
    }
}
