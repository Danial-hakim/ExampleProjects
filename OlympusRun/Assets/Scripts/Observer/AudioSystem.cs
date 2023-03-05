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
/// Deals with calling the correct audio based on the passed Notification Type from Subject
/// </summary>
public class AudioSystem : Observer
{
    void Start()
    {
        /// <summary>
        /// Finds all objects of type PointOfInterest and registers them to the observer
        /// </summary>
        foreach (var poi in FindObjectsOfType<PointOfInterest>())
        {
            poi.RegisterObserver(this);
        }
    }

    /// <summary>
    /// Is called from a PointOfInterest script, takes in the name of the object, the layer that it collided with and the type of notification sent through
    /// </summary>
    public override void OnNotify(object value, int collValue, NotificationType notificationType)
    {
        /// <summary>
        /// Plays the 'Hit' audio when the player interacts with an obstacle
        /// </summary>
        if (notificationType == NotificationType.Hit && (value.ToString().Contains("Player") || value.ToString().Contains("AI_God")) && collValue == LayerMask.NameToLayer("Obstacle"))
        {
            transform.Find("Hit").gameObject.GetComponent<AudioSource>().Play();
        }
        /// <summary>
        /// Plays the 'Collect' audio when the player interacts with a collectible 
        /// </summary>
        else if (notificationType == NotificationType.Hit && (value.ToString().Contains("Player") || value.ToString().Contains("AI_God")) && collValue == LayerMask.NameToLayer("Item"))
        {
            transform.Find("Collect").gameObject.GetComponent<AudioSource>().Play();
        }
        /// <summary>
        /// Plays the 'CollectCoin' audio when the player interacts with a coin 
        /// </summary>
        else if (notificationType == NotificationType.Hit && (value.ToString().Contains("Player") || value.ToString().Contains("AI_God")) && collValue == LayerMask.NameToLayer("Coin"))
        {
            transform.Find("CollectCoin").gameObject.GetComponent<AudioSource>().Play();
        }
        /// <summary>
        /// Plays the 'Drink' audio when the player interacts with a potion 
        /// </summary>
        else if (notificationType == NotificationType.Hit && (value.ToString().Contains("Player") || value.ToString().Contains("AI_God")) && collValue == LayerMask.NameToLayer("Potion"))
        {
            transform.Find("Drink").gameObject.GetComponent<AudioSource>().Play();
        }
        /// <summary>
        /// Plays the 'Jump' audio when the player jumps 
        /// </summary>
        if (notificationType == NotificationType.Jump)
        {
            transform.Find("Jump").gameObject.GetComponent<AudioSource>().Play();
        }
        /// <summary>
        /// Plays the 'Slide' audio when the player Slide 
        /// </summary>
        if (notificationType == NotificationType.Slide)
        {
            transform.Find("Slide").gameObject.GetComponent<AudioSource>().Play();
        }

        AudioSource walk = transform.Find("Walk").gameObject.GetComponent<AudioSource>();

        if (notificationType == NotificationType.Move)
        {  
            if (!walk.isPlaying)
            {
                walk.Play();
            }
        }
        /// <summary>
        /// If any other notification type is sent through, the player walking sound stops
        /// </summary>
        else if(notificationType == NotificationType.Stop)
        {
            if (walk.isPlaying)
            {
                walk.Stop();
            }
        }
    }
}
