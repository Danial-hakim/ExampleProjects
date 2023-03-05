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

using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

/// <summary>
/// A class to manage the spawning of objects on the bounds of the track
/// </summary>
public class BushSpawner : MonoBehaviour
{
    private readonly List<GameObject> _sideObjectPrefabs = new();
    private Vector3 _nextSpawnPos;

    private readonly List<SideBush> _sideBushPool = new();

    /// <summary>
    /// Start spawns the first 15 pairs of columns that line the track
    /// </summary>
    [ServerCallback]
    private void Start()
    {
        _sideObjectPrefabs.Add(Resources.Load("FoilagePrefabs/Bush0") as GameObject);
        _sideObjectPrefabs.Add(Resources.Load("FoilagePrefabs/Bush1") as GameObject);
        _sideObjectPrefabs.Add(Resources.Load("FoilagePrefabs/Bush2") as GameObject);
        _sideObjectPrefabs.Add(Resources.Load("FoilagePrefabs/Bush3") as GameObject);
        _sideObjectPrefabs.Add(Resources.Load("FoilagePrefabs/Bush4") as GameObject);
        _sideObjectPrefabs.Add(Resources.Load("FoilagePrefabs/Bush5") as GameObject);
        _sideObjectPrefabs.Add(Resources.Load("FoilagePrefabs/Bush6") as GameObject);

        // Instantiate every bush twice to have a pool of 14 bushes
        for (var i = 0; i < 2; i++)
        {
            foreach (var sideObjectPrefab in _sideObjectPrefabs)
            {
                var bushGO = Instantiate(sideObjectPrefab);
                bushGO.SetActive(false);
                _sideBushPool.Add(bushGO.GetComponent<SideBush>());
            }
        }
        
        // Active randomly selected 14 bushes from the object pool
        for (var i = 0; i < 14; i++)
        {
            var inactiveBush = _sideBushPool.Where(bush => !bush.gameObject.activeSelf).ToList();
            
            var rand = Random.Range(0, inactiveBush.Count);
            inactiveBush[rand].transform.position = new Vector3(0, 1, _nextSpawnPos.z);
            inactiveBush[rand].gameObject.SetActive(true);
            NetworkServer.Spawn(inactiveBush[rand].gameObject);
            _nextSpawnPos = inactiveBush[rand].transform.GetChild(1).transform.position;
        }
    }

    /// <summary>
    /// Randomly select a bush from the pool and place it at the end of the track
    /// </summary>
    [Server]
    public void PlaceRandomBush()
    {
        var inactiveBush = _sideBushPool.Where(bush => !bush.gameObject.activeSelf).ToList();
        
        var rand = Random.Range(0, inactiveBush.Count);
        inactiveBush[rand].transform.position = new Vector3(0, 1, _nextSpawnPos.z);
        inactiveBush[rand].gameObject.SetActive(true);
        NetworkServer.Spawn(inactiveBush[rand].gameObject);
        _nextSpawnPos = inactiveBush[rand].transform.GetChild(1).transform.position;
    }
}
