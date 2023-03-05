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
/// Upon colliding or movement change, sends notification to observer with Notification type
/// </summary>
public class PointOfInterest : Subject
{
    [SerializeField]
    private string _poiName;
    private void Start()
    {
        _poiName = gameObject.name;

        RegisterObserver(GameObject.Find("AudioManager").GetComponent<AudioSystem>());

    }

    /// <summary>
    /// Sends a Move notification when the gameobject when not Sliding or Jumping
    /// </summary>
    private void Update()
    {
        if (_poiName.Contains("Player"))
        {
            if (!GetComponentInParent<RunnerPlayer>().Sliding && !GetComponentInParent<RunnerPlayer>().Jumping)
            {
                Notify(_poiName, 0, NotificationType.Move);

            }
        }
        
    }

    /// <summary>
    ///Sends slide notification when the gameobject slides, sends stop notification
    /// </summary>
    public void StartSlide()
    {
        Notify(_poiName, 0, NotificationType.Stop);
        Notify(_poiName, 0, NotificationType.Slide);
    }
    /// <summary>
    ///Sends jump notification when the gameobject jumps, sends stop notification
    /// </summary>
    public void StartJump()
    {
        Notify(_poiName, 0, NotificationType.Stop);
        Notify(_poiName, 0, NotificationType.Jump);
    }

    public void StartHit(Collider collision)
    {
        Notify(collision.gameObject.name, gameObject.layer, NotificationType.Hit);

    }
}
