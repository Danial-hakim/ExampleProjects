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
/// Camera following the runner from the POV of the god.
/// The camera is positioned at the front and look at the top of the runner.
/// </summary>
public class GodCamera : MonoBehaviour
{
    private Transform _target;
    
    /// Angle of the camera from the target
    /// <remarks>Should probably be renamed to something else as it's not really an angle but I am not sure what it is</remarks>
    [SerializeField] 
    private float _angleFromRunner = 10f;
    
    [SerializeField] private float _heightFromRunner = 40f;

    [Tooltip("The offset of the camera so the camera see the target at the top of the viewport")]
    [SerializeField] private float _offset = 18f;
    
    // Cached transform
    private Transform _transform;

    
    private void Start()
    {
        _target = GameObject.FindWithTag("Runner").transform;
        
        // Cache the transform for efficiency
        _transform = transform;
        
        // Caching properties for efficiency
        var targetPosition = _target.transform.position;
        var targetForward = _target.forward;
        
        // Calculate the initial position of the camera
        var initialPosition = targetPosition + (targetForward * _angleFromRunner);
        initialPosition.y = targetPosition.y + _heightFromRunner; // Add the height to the camera to keep it above the target
        _transform.position = initialPosition;
    }

    private void Update()
    {
        CalculateCameraTransform();
    }

    /// <summary>
    /// Calculate the position of the camera to see the target from the front.
    /// Work best if eulerAngle X stays at 0. The method need to be modified to use a different rotation if needed.
    /// </summary>
    private void CalculateCameraTransform()
    {
        // Caching properties for efficiency
       var targetPosition = _target.transform.position;
        var targetForward = _target.forward;

        // Calculate the initial position of the camera
        var initialPosition = targetPosition + (targetForward * _angleFromRunner);
        initialPosition.y = transform.position.y;
        _transform.position = initialPosition;
        
        // Look at the target from the initial calculated position
        _transform.LookAt(targetPosition);
        
        // Then, move the camera by an offset from the forward of the target
        var finalPosition = _transform.position + (targetForward * _offset);
        finalPosition.y = _transform.position.y; // Don't change camera height
        _transform.position = finalPosition;
    }
}
