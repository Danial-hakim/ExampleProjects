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
/// Class that keeps track of the player behaviour
/// </summary>
public class Rule
{
    public int currentLane;
    public int nextLane;
    public string obstacleTag;
    public int obstacleLane;
    public int weight = 1;

    /// <summary>
    /// Checks how many times this behaviour is done by the player
    /// </summary>
    /// <param name="t_currentLane"></param>
    /// <param name="t_nextLane"></param>
    /// <param name="t_obstacleTag"></param>
    /// <param name="t_obstacleLane"></param>
    /// <returns></returns>
    public bool Matches(int t_currentLane, int t_nextLane, string t_obstacleTag, int t_obstacleLane)
    {
        // Check whether this rule matches the current state of the game
        return this.currentLane == t_currentLane
            && this.nextLane == t_nextLane
            && this.obstacleTag == t_obstacleTag
            && this.obstacleLane == t_obstacleLane;
    }

    /// <summary>
    /// Goes through database for information, picks one with highest weight
    /// </summary>
    /// <param name="t_currentLane"></param>
    /// <param name="t_obstacleTag"></param>
    /// <param name="t_obstacleLane"></param>
    /// <returns></returns>
    public int getNextLane(int t_currentLane, string t_obstacleTag, int t_obstacleLane)
    {
        if (currentLane == t_currentLane && obstacleLane == t_obstacleLane && obstacleTag == t_obstacleTag)
        { 
            return weight;
        }
        //Debug.Log("rule doesnt exist yet");
        return 0; // the rule doesnt exist yet 
    }

    /// <summary>
    /// Checking if rule already exists
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Rule other = (Rule)obj;
        return currentLane == other.currentLane &&
               nextLane == other.nextLane &&
               obstacleTag == other.obstacleTag &&
               obstacleLane == other.obstacleLane &&
               weight == other.weight;
    }

    public override int GetHashCode()
    {
        return currentLane.GetHashCode() ^
               nextLane.GetHashCode() ^
               obstacleTag.GetHashCode() ^
               obstacleLane.GetHashCode() ^
               weight.GetHashCode();
    }
}
