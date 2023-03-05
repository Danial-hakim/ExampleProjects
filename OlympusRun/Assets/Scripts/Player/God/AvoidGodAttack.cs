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

using UnityEngine;
/// <summary>
/// Setting and spawning an avoidable obstacle at given position
/// </summary>
public class AvoidGodAttack : GodAttack
{
    private GameObject _avoidLightning;

    /// <summary>
    /// Sets attack type to Avoid and loads avoid gameobject prefab
    /// </summary>
    private void Start()
    {
        attackType = GodAttackType.Avoid;
        _avoidLightning = Resources.Load("AvoidLightning") as GameObject;
    }

    /// <summary>
    /// Spawns the avoid gameobject prefab at given target position
    /// </summary>
    /// <param name="targetPos"></param>
    public override void Attack(Vector3 targetPos)
    {
        Instantiate(_avoidLightning, targetPos, Quaternion.identity);
    }
}