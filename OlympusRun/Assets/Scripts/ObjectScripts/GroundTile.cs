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
/// A class that defines the ground tiles that create the path,
/// checks for collision to detect when they are off screen
/// </summary>
public class GroundTile : MonoBehaviour
{
    private GroundSpawner _groundSpawner;

    [ServerCallback]
    private void Start()
    {
        _groundSpawner = FindObjectOfType<GroundSpawner>();
    }

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Runner"))
        {
            // Checks for collision with the runners offscreen collider
            // to detect when the ground tiles are off screen.
            // Move the the ground tile to continue the endless path
            StartCoroutine(ReplaceTile());
        }
    }

    /// <summary>
    /// Hide the tile after 2 seconds and move it behind the player.
    /// </summary>
    [Server]
    private IEnumerator ReplaceTile()
    {
        yield return new WaitForSeconds(2);
        _groundSpawner.MoveTile(this);
    }
}