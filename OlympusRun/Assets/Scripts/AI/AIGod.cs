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

public class AIGod : MonoBehaviour
{
    [SerializeField] private List<GodAttack> attackList = new List<GodAttack>();

    AIBrain runnerBrain;
    private Transform runner;
    Vector3 offset;

    private GameObject JumpObstacle;
    private GameObject SlideObstacle;
    private GameObject DodgeObstacle;

    [Header("JUST FOR DISPLAY DONT EDIT")]

    [SerializeField] int runnerLane;
    [SerializeField] int obstacleLane;
    [SerializeField] int preditedLane;

    private float attacksCooldown = 5.0f;
    private bool canAttack = true;

    int randomChoice;
    /// <summary>
    /// Simply returns a random attack from the list.
    /// </summary>
    /// <remarks>
    /// This method will be replaced by a proper AI logic later on (probably using fuzzy logic or something like that).
    /// </remarks>
    /// <returns>The selected attack</returns>
    public GodAttack GetRandomAttack()
    {
        int rand = UnityEngine.Random.Range(0, attackList.Count);
        return attackList[rand];
    }
    
    /// <summary>
    /// Returns the requested attack
    /// </summary>
    /// <param name="chosenAttack"></param>
    /// <returns></returns>
    public GodAttack GetAttack(int chosenAttack)
    {
        return attackList[chosenAttack];
    }

    private void Start()
    {
        //ruleDatabases = GameObject.FindGameObjectWithTag("RBS_Manager").GetComponent<RuleDatabases>();
        runnerBrain = GameObject.FindGameObjectWithTag("Runner").GetComponent<AIBrain>();

        runner = GameObject.FindGameObjectWithTag("Runner").gameObject.transform;
        offset = transform.position - runner.position;

        JumpObstacle = Resources.Load("JumpAttackObstacle") as GameObject;
        SlideObstacle = Resources.Load("SlideAttackObstacle") as GameObject;
        DodgeObstacle = Resources.Load("TridentAttack") as GameObject;
        //StartCoroutine(SpawnObstacle());
    }

    private void Update()
    {
        Vector3 newPos = runner.position + offset;
        transform.position = new Vector3(transform.position.x, transform.position.y, newPos.z);

        if(attacksCooldown <= 0)
        {
            canAttack = true;
        }
        else
        {
            attacksCooldown -= Time.deltaTime;
        }
    }

    public void predictLaneNow()
    {
        //preditedLane = ruleDatabases.PredictNextLane(runnerBrain.currentLane, "Inpenetrable", runnerBrain.getObstacleLane);
        preditedLane = RuleDatabases.PredictNextLane(runnerBrain.CurrentLane, "Inpenetrable", runnerBrain.GetObstacleLane);

        //Debug.Log("before obstalce :" + runnerBrain.currentLane + " || obstacle is :" + runnerBrain.getObstacleLane + " || after obstalce :" + preditedLane);
        obstacleLane = runnerBrain.GetObstacleLane;
        runnerLane = runnerBrain.CurrentLane;

        if(canAttack)
        {
            SpawnObstacle();
            attacksCooldown = 5.0f;
            canAttack = false;
        }

    }

    public void SpawnObstacle()
    {
        switch (preditedLane)
        {
            case 1:
                preditedLane = -2;
                break;
            case 2:
                preditedLane = 0;
                break;
            case 3:
                preditedLane = 2;
                break;
        }
        Vector3 newPos = runner.position + offset;

        randomChoice = Random.Range(0, 3);

        switch (randomChoice)
        {
            case 0:
                Instantiate(DodgeObstacle, new Vector3(preditedLane, 1, newPos.z + 15f), Quaternion.identity);
                break;
            case 1:
                Instantiate(JumpObstacle, new Vector3(-2, 0, newPos.z + 15f), Quaternion.identity);
                break;
            case 2:
                Instantiate(SlideObstacle, new Vector3(-2, 2, newPos.z + 15f), Quaternion.identity);
                break;
        }
    }
}