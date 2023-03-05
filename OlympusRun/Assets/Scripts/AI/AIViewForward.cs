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
/// Is a script that keeps track of what the runner AI "Sees".
/// </summary>
public class AIViewForward : MonoBehaviour
{
    private AIBrain _aiBrain;

    private void Start()
    {
        _aiBrain = transform.parent.GetComponent<AIBrain>();
    }

    /// <summary>
    /// Occurs when the collider that acts as the AI's eyes "sees" something. (aka collides with it)
    /// </summary>
    /// <param name="other">The object that it "sees" ahead.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled && other.CompareTag("Decoration"))
            return;
        
        _aiBrain.React(other);
    }
}