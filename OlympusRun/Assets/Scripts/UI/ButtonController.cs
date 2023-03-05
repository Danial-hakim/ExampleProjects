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
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deals with settings and help button presses on main menu
/// </summary>
public class ButtonController : MonoBehaviour
{
    private int _lastChild;

    void Start()
    {
        _lastChild = GameObject.Find("MenuManager").transform.childCount;   
    }

    public  void OpenHelp()
    {
        GameObject.Find("MenuManager").transform.GetChild(_lastChild - 2).gameObject.SetActive(true);
    }

    public void CloseHelp()
    {
        GameObject.Find("MenuManager").transform.GetChild(_lastChild - 2).gameObject.SetActive(false);
    }

    public void OpenSettings()
    {
        GameObject.Find("MenuManager").transform.GetChild(_lastChild - 1).gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        GameObject.Find("MenuManager").transform.GetChild(_lastChild - 1).gameObject.SetActive(false);
    }
}
