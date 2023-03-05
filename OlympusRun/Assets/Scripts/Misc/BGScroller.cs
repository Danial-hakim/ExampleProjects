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
/// Ensures the background is constantly moving away from the player,
/// kept at a constant distance from the player
/// </summary>
public class BGScroller : NetworkBehaviour
{
    private Transform _runner;
    private bool _isRunnerSet = false;
    private Vector3 _offset;

    public Transform Runner
    {
        [Server]
        set
        {
            _runner = value;
            _offset = transform.position - _runner.position;
            _isRunnerSet = true;
        }
    }

    [ServerCallback]
    private void LateUpdate()
    {
        // Use of a boolean to check if the runner is set is more efficient than checking if the runner is null
        if (!_isRunnerSet)
            return;
        
        var newPos = _runner.position + _offset;
        transform.position = new Vector3(transform.position.x, transform.position.y, newPos.z);
    }
}
