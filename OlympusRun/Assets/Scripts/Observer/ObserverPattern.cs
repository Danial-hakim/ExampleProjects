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

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class used in Observer Pattern
/// Deals with reacting to notification types
/// </summary>
public abstract class Observer : MonoBehaviour
{
    public abstract void OnNotify(object value, int collValue, NotificationType notification);
}
/// <summary>
/// Abstract class for a Subject, used in Observer Pattern
/// Registers and Unregisters Observers and deals with sending notifications to Observers
/// </summary>
public abstract class Subject : MonoBehaviour
{
    private List<Observer> _observers = new List<Observer>();

    public void RegisterObserver(Observer observer)
    {
        _observers.Add(observer);
    }

    public void UnregisterObserver(Observer observer)
    {
        _observers.Remove(observer);
    }

    /// <summary>
    /// Loops through each observer in the list and notifies them of a new notification
    /// <summary>
    public void Notify(object value, int collValue, NotificationType notificationType)
    {
        foreach(var observer in _observers)
        {
            observer.OnNotify(value, collValue, notificationType);
        }
    }
}

public enum NotificationType
{
    Hit, Move, Stop, Jump, Slide, Die
}
