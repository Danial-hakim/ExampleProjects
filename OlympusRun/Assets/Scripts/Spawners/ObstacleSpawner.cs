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

using System.Collections;
using Mirror;
using UnityEngine;

/// <summary>
/// A class that spawns obstacles of different types at random
/// locations on the track.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _obstacleWidePrefab;
    [SerializeField] private GameObject _obstacleHighPrefab;
    [SerializeField] private GameObject _obstacleSmallPrefab;

    [HideInInspector] public int ObstaclesPlaced;
    
    private Vector3 _offset;
    
    private Transform _runner;
    private bool _isRunnerSet = false;

    private Quaternion _rotation = Quaternion.Euler(0f, 0f, 90f);

    public Transform Runner
    {
        [Server]    
        set
        {
            _runner = value;
            _offset = transform.position - _runner.position;
            StartCoroutine(SpawnObstacle());
            _isRunnerSet = true;
        }
    }


    /// <summary>
    /// Updates the position of the spawner to always be in front of the player
    /// </summary>
    [ServerCallback]
    private void LateUpdate()
    {
        if (!_isRunnerSet)
            return;
        
        var newPos = _runner.position + _offset;
        transform.position = new Vector3(transform.position.x, transform.position.y, newPos.z);
    }

    /// <summary>
    /// Coroutine that handles spawning obstacles of three different types
    /// </summary>
    [Server]
    private IEnumerator SpawnObstacle()
    {
        while (true)
        {
            var randomNumber = Random.Range(1, 4);
            yield return new WaitForSeconds(2.0f);
            Vector3 newPos = _runner.position + _offset;
            if (randomNumber == 1)
            {
                int randomXPos = Random.Range(1, 4);
                if (randomXPos == 1)
                    randomXPos = -2;
                else if (randomXPos == 3)
                    randomXPos = 0;
                
                GameObject obstacle = Instantiate(_obstacleSmallPrefab, new Vector3(randomXPos,0f,newPos.z), Quaternion.identity);
                NetworkServer.Spawn(obstacle);
                ObstaclesPlaced += 1;
            }
            else if(randomNumber == 2) // slide obstacle
            {
                GameObject obstacle = Instantiate(_obstacleHighPrefab, new Vector3(-3.5f, 0, newPos.z), Quaternion.identity);
                NetworkServer.Spawn(obstacle);
                ObstaclesPlaced += 1;
            }
            else if(randomNumber == 3)
            {
                GameObject obstacle = Instantiate(_obstacleWidePrefab, new Vector3(4, 1, newPos.z), _rotation);
                NetworkServer.Spawn(obstacle);
                ObstaclesPlaced += 1;
            }
        }
    }

    /// <summary>
    /// Spawns a new small obstacle into the scene.
    /// </summary>
    /// <returns>The new small obstacle.</returns>
    public GameObject SpawnSmallObstacle()
    {
        GameObject newObstacle = Instantiate(_obstacleSmallPrefab);
        newObstacle.SetActive(true);
        return newObstacle;
    }

    /// <summary>
    /// Spawns a new wide obstacle into the scene.
    /// </summary>
    /// <returns>The new wide obstacle.</returns>
    public GameObject SpawnWideObstacle()
    {
        GameObject newObstacle = Instantiate(_obstacleWidePrefab);
        newObstacle.SetActive(true);
        return newObstacle;
    }

    /// <summary>
    /// Spawns a new high obstacle into the scene.
    /// </summary>
    /// <returns>The new high obstacle.</returns>
    public GameObject SpawnHighObstacle()
    {
        GameObject newObstacle = Instantiate(_obstacleHighPrefab);
        newObstacle.SetActive(true);
        return newObstacle;
    }
}