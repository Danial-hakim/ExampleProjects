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

using Mirror;
using UnityEngine;

/// <summary>
/// A class that spawns ground tiles to create an endless path
/// </summary>
public class GroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _groundTilePrefab;
    private Vector3 _nextSpawnPos;

    /// <summary>
    /// Start spawns the first 15 ground tiles that create the track
    /// </summary>
 //  [ServerCallback]
    private void Start()
    {
        for (var i = 0; i < 15; i++)
        {
            var groundTile = Instantiate(_groundTilePrefab, _nextSpawnPos, Quaternion.identity);
            //NetworkServer.Spawn(groundTile);
            _nextSpawnPos = groundTile.transform.GetChild(1).transform.position;
        }
    }
    
    /// <summary>
    /// Move the tile that has gone off screen to the end of the track
    /// </summary>
    /// <param name="tile">Tile to be moved</param>
    //[Server]
    public void MoveTile(GroundTile tile)
    {
        tile.transform.position = _nextSpawnPos;
        _nextSpawnPos = tile.transform.GetChild(1).transform.position;
    }
}
