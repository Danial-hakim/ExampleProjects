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
/// Manage the health of the runner and update the health bar.
/// The health value is synchronised with the server.
/// </summary>
public class RunnerHealthController : NetworkBehaviour
{    
    [SerializeField] private HealthBar _healthBarPrefab;
    private HealthBar _healthBar;

    // Find the GameObject with the tag "MyTag"
    GameObject myTagObject;

    // Get the script component attached to the grandchild GameObject

    public override void OnStartLocalPlayer()
    {
        myTagObject = GameObject.FindGameObjectWithTag("HealthBar");
        _healthBar = myTagObject.GetComponent<HealthBar>();
        if(_healthBar == null)
        {
            Debug.Log("fail");
        }
        //_healthBar = Instantiate(_healthBarPrefab);
        _healthBar.SetMaxHealth();
    }

    public void TakeDamage(int damage)
    {
        if (!isLocalPlayer) return;
        
        CmdTakeDamage(damage);
    }

    /// <summary>
    /// Invoked on Server. Sets the health of the player.
    /// </summary>
    /// <param name="health"></param>
    [Command]
    private void CmdSetHealth(int health)
    {
        _healthBar.SetHealth(health);
    }
    
    /// <summary>
    /// Invoked on Server. Adds health to the player.
    /// </summary>
    /// <param name="health"></param>
    [Command]
    private void CmdTakeHealth(int health)
    {
        _healthBar.PartialHeal();
    }

    /// <summary>
    /// Invoked on Server. Removes health from the player.
    /// </summary>
    /// <param name="damage"></param>
    [Command]
    private void CmdTakeDamage(int damage)
    {
        _healthBar.TakeDamage(damage);
    }
    
    /// <summary>
    /// Do something on the client whenever health is changed on server
    /// </summary>
    /// <param name="oldHealth"></param>
    /// <param name="newHealth"></param>
    //private void OnHealthChanged(int oldHealth, int newHealth)
    //{
    //    if (!isLocalPlayer) return;
        
    //    _healthBar.SetHealth(newHealth);
    //}

    /// <summary>
    /// Give the player full health and sync it to the server
    /// </summary>
    public void InstantHeal()
    {
        CmdSetHealth(_healthBar.MaxHealth);
    }

    /// <summary>
    /// Give the player partial health and sync it to the server
    /// </summary>
    public void PartialHeal()
    {
        CmdTakeHealth(_healthBar.HealthBonus);
    }

    public bool IsRunnerDead()
    {
        return _healthBar.IsDead();
    }

    public int GetCurrentHealth()
    {
        return _healthBar.GetCurrentHealth();
    }

    public int GetMaxHealth()
    {
        return _healthBar.MaxHealth;
    }

    public void TestDamage(int damage)
    {
        _healthBar.TakeDamage(damage);
    }

    public void TestPartial()
    {
        _healthBar.PartialHeal();
    }

    public void TestMax()
    {
        _healthBar.SetHealth(_healthBar.MaxHealth);
    }
}