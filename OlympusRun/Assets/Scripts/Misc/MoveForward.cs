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

using System;
using Mirror;
using UnityEngine;

/// <summary>
/// A class that moves the player forward and tracks the distance they have travelled.
/// </summary>
/// <remarks>The script should be used only by the local player.</remarks>
public class MoveForward : NetworkBehaviour
{
    [SerializeField] private Rigidbody _rb;
    
    [HideInInspector] public int ObstaclesAvoided;

    protected double _startTime;
    protected double _endTime;
    protected double _playTime;
    protected int _speed;
    private const int MaxSpeed = 30;
    private const int MinSpeed = 8;
    protected int _distanceTravelled;
    protected int _startingPos;
    private int _increaseWhen;

    BoxCollider fieldOfVIew;

    private void Awake()
    {
        enabled = false;
        fieldOfVIew = transform.Find("Field Of View").GetComponent<BoxCollider>();
    }

    public override void OnStartLocalPlayer()
    {
        _endTime = 0;
        _playTime = 0;
        _startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        _increaseWhen = 100;
        _speed = 8;
        _startingPos = (int)transform.position.z;
        ObstaclesAvoided = 0;

        enabled = true;
    }

    public override void OnStopLocalPlayer()
    {
        enabled = false;
    }


    /// <summary>
    /// Calculates the distance travelled by the runner and increases speed based on distance travelled
    /// </summary>
    private void FixedUpdate()
    {
        _distanceTravelled = (int)transform.position.z - (int)_startingPos;

        if (_distanceTravelled % _increaseWhen == 0 && _distanceTravelled != 0)
        {
            if (_speed < MaxSpeed)
            {
                _speed += 2;
                _increaseWhen += 100;

                fieldOfVIew.size = new Vector3(fieldOfVIew.size.x, fieldOfVIew.size.y, fieldOfVIew.size.z + 1);
                fieldOfVIew.center = new Vector3(fieldOfVIew.center.x, fieldOfVIew.center.y, fieldOfVIew.center.z + 0.5f);
            }
            else
            {
                _speed = MaxSpeed;
            }
        }

        Vector3 moveForward = transform.forward * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + moveForward);
    }

    /// <summary>
    /// Gets the distance the player has travelled
    /// </summary>
    /// <returns></returns>
    [Client]
    public int GetDistanceTravelled()
    {
        if (!isLocalPlayer)
        {
            Debug.LogError("GetDistanceTravelled called on non-local player. Default value returned.");
            return 0;
        }
        _distanceTravelled = (int)transform.position.z - (int)_startingPos;
        return _distanceTravelled;
    }

    /// <summary>
    /// Gets the speed the player is currently travelling at
    /// </summary>
    /// <returns></returns>
    [Client]
    public int GetSpeed()
    {
        if (!isLocalPlayer)
        {
            Debug.LogError("GetSpeed called on non-local player. Default value returned.");
            return 0;
        }

        return _speed;
    }

    /// <summary>
    /// Gets the playtime of the current run, survival time
    /// </summary>
    /// <returns></returns>
    [Client]
    public int GetPlayTime()
    {
        if (!isLocalPlayer)
        {
            Debug.LogError("GetPlayTime called on non-local player. Default value returned.");
            return 0;
        }

        _endTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        _playTime = _endTime - _startTime;
        return (int) _playTime;
    }

    /// <summary>
    /// Set the speed of the player.
    /// Should be called only on the local player.
    /// </summary>
    /// <param name="speedChange"></param>
    public void SetSpeed(int speedChange)
    {
        if (!isLocalPlayer)
        {
            Debug.LogError("SetSpeed called on non-local player");
            return;
        }
        
        if (speedChange < 0 && _speed > MinSpeed )
        {
            _speed += speedChange;
        }
        else if(speedChange > 0 && _speed < MaxSpeed)
        {
            _speed += speedChange;
        }
    }

    /// <summary>
    /// Used for tests to bypass the client functions
    /// Sets the players speed
    /// </summary>
    /// <param name="speedChange"></param>
    public void TestSetSpeed(int speedChange)
    {
        if (speedChange < 0 && _speed > MinSpeed)
        {
            _speed += speedChange;
        }
        else if (speedChange > 0 && _speed < MaxSpeed)
        {
            _speed += speedChange;
        }
    }

    /// <summary>
    /// Used for tests to bypass the client functions
    /// Gets the distance travelled by the runner
    /// </summary>
    /// <param name="speedChange"></param>
    public int TestGetDistanceTravelled()
    {
        if (!isLocalPlayer)
        {
            Debug.LogError("GetDistanceTravelled called on non-local player. Default value returned.");
            return 0;
        }
        _distanceTravelled = (int)transform.position.z - (int)_startingPos;
        return _distanceTravelled;
    }

    /// <summary>
    /// Used for tests to bypass the client functions
    /// Gets the players speed
    /// </summary>
    /// <param name="speedChange"></param>
    public int TestGetSpeed()
    {
        return _speed;
    }

    /// <summary>
    /// Used for tests to bypass the client functions
    /// Gets the time of the player's current run, how long they survived
    /// </summary>
    /// <param name="speedChange"></param>
    public int TestGetPlayTime()
    {
        if (!isLocalPlayer)
        {
            Debug.LogError("GetPlayTime called on non-local player. Default value returned.");
            return 0;
        }

        _endTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        _playTime = _endTime - _startTime;
        return (int)_playTime;
    }
}
