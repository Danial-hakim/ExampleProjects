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
using Mirror;

/// <summary>
/// Handles the spawning of pickups
/// </summary>
public class PickupController : MonoBehaviour
{
    private Vector3 _offset = new Vector3();
    private Transform _runner;
    public Transform Runner
    {
        set
        {
            _runner = value;
            StartCoroutine(SpawnPickup());
        }
    }

    private GameObject _coin;
    private GameObject _pom;
    private GameObject _apple;

    private List<GameObject> _potions = new List<GameObject>();

    private GameObject _speedUp;
    private GameObject _speedDown;
    private GameObject _maxHealth;
    private GameObject _partHealth;
    private int iterations = 4;


    /// <summary>
    /// Start spawns the pickups that will randomly appear in a random lane
    [ServerCallback]
    private void Start()
    {
        _offset = new Vector3(0, 0, 40);
        _coin = Resources.Load("Pickups/Coin") as GameObject;
        _pom = Resources.Load("Pickups/Pomegranate") as GameObject;
        _apple = Resources.Load("Pickups/Apple") as GameObject;

        _potions.Add(Resources.Load("Pickups/SpeedUpPotion") as GameObject);
        _potions.Add(Resources.Load("Pickups/SpeedDownPotion") as GameObject);
        _potions.Add(Resources.Load("Pickups/MaxHealthPotion") as GameObject);
        _potions.Add(Resources.Load("Pickups/PartHealthPotion") as GameObject);

        for (int i = 0; i < iterations; i++)
        {
            GameObject obj = Instantiate(_coin, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
            obj.SetActive(false);
            NetworkServer.Spawn(obj);

            obj = Instantiate(_pom, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
            obj.SetActive(false);
            NetworkServer.Spawn(obj);

            obj = Instantiate(_apple, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
            obj.SetActive(false);
            NetworkServer.Spawn(obj);

            obj = Instantiate(_potions[i], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
            obj.SetActive(false);
            NetworkServer.Spawn(obj);
        }
    }

    /// <summary>
    /// Coroutine that handles spawning pickups by picking a random child object
    /// </summary>
    private IEnumerator SpawnPickup()
    {
        while (_runner.GetChild(0).gameObject.activeSelf)
        {
            yield return new WaitForSeconds(1.0f);
            if (transform.childCount > 0)
            {
                int chosenPickUp = Random.Range(0, transform.childCount);
                int randX = Random.Range(-2, 2);
                if (randX < 0)
                {
                    randX = -2;
                }
                else if (randX > 0)
                {
                    randX = 2;
                }
                Vector3 newPos = _runner.position + _offset;

                ActivatePickUp(chosenPickUp, randX, newPos.z);
            }
        }
    }

    /// <summary>
    /// Activates the pick ups in the scene
    /// </summary>
    /// <param name="ChosenPickUp"></param>
    /// <param name="XPos"></param>
    /// <param name="ZPos"></param>
    public void ActivatePickUp(int ChosenPickUp, int XPos, float ZPos)
    {
        //Debug.Log(chosenPickUp);
        if (!transform.GetChild(ChosenPickUp).gameObject.activeInHierarchy)
        {
            transform.GetChild(ChosenPickUp).transform.localPosition = new Vector3(XPos, 2, ZPos);
            transform.GetChild(ChosenPickUp).gameObject.SetActive(true);
        }
    }    

    /// <summary>
    /// Increases the players speed
    /// </summary>
    public void SpeedUp()
    {
        _runner.GetComponent<MoveForward>().SetSpeed(+2);
    }

    /// <summary>
    /// Decreases the players speed
    /// </summary>
    public void SpeedDown()
    {
        _runner.GetComponent<MoveForward>().SetSpeed(-2);
    }
}
