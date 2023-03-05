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
/// Assign references to the runner to every script that needs it in the scene, because the runner is instantiated at runtime.
/// </summary>
public class RunnerReferencesSetter : NetworkBehaviour
{
    [SerializeField]
    private GameObject _uiPrefab;

    [SerializeField]
    private RunnerCamera _runnerCameraPrefab;

    public override void OnStartServer()
    {
        // We only need to set the references of the runner to environment objets in the server, because the environment
        // is generated on the server.
        // The environment is dispatched to the clients by the server.
        var obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            obstacleSpawner.Runner = transform;
        }
        else
        {
            Debug.LogWarning("No ObstacleSpawner found in the scene on server.");
        }
        
        var bgScroller = FindObjectOfType<BGScroller>();
        if (bgScroller != null)
        {
            bgScroller.Runner = transform;
        }
        else
        {
            Debug.LogWarning($"No BGScroller found in the '{bgScroller.name}' GameObject.");
        }
    }
    
    public override void OnStartLocalPlayer()
    {
        // Set the references to the runner in every script that needs it.

        // This camera is only the one of the Runner, so should only be used in the LocalClient
        RunnerCamera runnerCamera = Instantiate(_runnerCameraPrefab);
        runnerCamera.Runner = transform;

        GameObject ui = Instantiate(_uiPrefab);
        var uiManager = ui.GetComponentInChildren<UIManager>();
        if (uiManager != null)
        {
            uiManager.Runner = gameObject;
        }
        else
        {
            Debug.LogWarning($"No UIManager found in the '{ui.name}' GameObject.");
        }

        var analyticsManager = FindObjectOfType<AnalyticsManager>();
        if (analyticsManager != null)
        {
            analyticsManager.Runner = gameObject;
        }
        
        var pickupController = FindObjectOfType<PickupController>();
        if (pickupController != null)
        {
            pickupController.Runner = transform;
        }
        
        var collectibleController = FindObjectOfType<CollectibleController>();
        if (collectibleController != null)
        {
            collectibleController.Runner = gameObject;
        }

        var playerLight = FindObjectOfType<LightScroller>();
        if (playerLight != null)
        {
            playerLight.Runner = transform;
        }
    }
}