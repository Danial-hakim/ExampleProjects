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
/// Just an example script to show how to create a new attack.
/// </summary>
public class DummyGodAttack : GodAttack
{
    /// <summary>
    /// Don't forget to set the attack type in the Start method.
    /// </summary>
    private void Start()
    {
        attackType = GodAttackType.Dummy;
    }

    /// <summary>
    /// Just an example method to show how to create a new attack with target position. You should put the attack logic here in real code.
    /// </summary>
    public override void Attack(Vector3 targetPos)
    {
        Debug.Log("God attack with 'target pos'!");
    }

    /// <summary>
    /// Just an example method to show how to create a new attack. You should put the attack logic here in real code.
    /// </summary>
    public override void Attack()
    {
        Debug.Log("God attack with 'Dummy'!");
    }
}