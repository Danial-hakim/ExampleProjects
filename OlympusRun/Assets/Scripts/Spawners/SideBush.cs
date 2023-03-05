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
using Mirror;
using UnityEngine;

/// <summary>
/// A class to manage the spawning of objects on the bounds of the track
/// </summary>
public class SideBush : MonoBehaviour
{
    private BushSpawner _bushSpawner;

    /// <summary>
    /// Finds the spawner of this object in the scene
    /// </summary>
    [ServerCallback]
    private void Start()
    {
        _bushSpawner = FindObjectOfType<BushSpawner>();
    }

    /// <summary>
    /// Checks for collision with the collider behind the player to
    /// check if off screen. Spawns a new side object and destroys the old one
    /// </summary>
    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Runner"))
        {
            StartCoroutine(ReplaceAndMove());
        }
    }

    [Server]
    private IEnumerator ReplaceAndMove()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        NetworkServer.UnSpawn(gameObject);
        _bushSpawner.PlaceRandomBush();
    }
}
