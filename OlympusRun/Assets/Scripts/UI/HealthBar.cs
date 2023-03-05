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
using UnityEngine.UI;
using Mirror;

public class HealthBar : NetworkBehaviour
{
    public int MaxHealth { get; private set; } = 100;

    public int HealthBonus { get; private set; } = 20;

    [SerializeField] private Slider _slider;

    [SyncVar(hook = nameof(OnHealthChanged))]
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = MaxHealth;
    }

    public void SetMaxHealth()
    {
        _slider.maxValue = MaxHealth;
        _slider.value = MaxHealth;
    }

    public void SetHealth(int health)
    {
        _slider.value = health;
        _currentHealth = health;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        SetHealth(_currentHealth);
    }

    //public void InstantHeal()
    //{
    //    _currentHealth = MaxHealth;
    //    SetHealth(_currentHealth);
    //}

    public void PartialHeal()
    {
        _currentHealth += HealthBonus;
        if (_currentHealth > MaxHealth)
        {
            _currentHealth = MaxHealth;
        }
        SetHealth(_currentHealth);
    }

    public bool IsDead()
    {
        return _currentHealth <= 0;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        if (!isLocalPlayer) return;

        _currentHealth = newHealth;
    }
}
