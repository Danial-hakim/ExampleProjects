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

using UnityEngine;

/// <summary>
/// A class that lets the camera follow the Runner
/// </summary>
public class RunnerCamera : MonoBehaviour
{
    private Transform _runner;
    public Transform Runner
    {
        set
        {
            _runner = value;
            _distanceFromRunner = transform.position - _runner.position;
        }
    }

    /// The distance from the camera to the target (ie. the runner)
    private Vector3 _distanceFromRunner;
    
    /// <summary>
    /// Makes the camera follow the player, runner and god
    /// </summary>
    private void LateUpdate()
    {
        if (_runner == null)
            return;
        
        var newPos = _runner.position + _distanceFromRunner;
        transform.position = new Vector3(transform.position.x, transform.position.y, newPos.z);
    }
}
