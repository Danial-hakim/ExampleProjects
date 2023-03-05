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

using System;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// A class which adds functionality to main menu buttons,game over buttons, and the 'Auto' game button
/// </summary>
public class UIManager : MonoBehaviour
{
    private struct RestartGameMessage : NetworkMessage {}
    
    [Header("Scenes (scene added here must be in build settings)")]
    [Scene] 
    [SerializeField] 
    private string mainMenuScene = "";

    public GameObject GameOverMenu;
    public Button ToggleAIButton;
    
    private bool _isRunnerAIOn = false;
    private bool _isGodAIOn = false;
    
    private GameObject _runner;
    public GameObject Runner
    {
        set => _runner = value;
    }

    private void Start()
    {
        NetworkServer.RegisterHandler<RestartGameMessage>(OnRestartGameRequest);
    }

    private void OnDestroy()
    {
        NetworkServer.UnregisterHandler<RestartGameMessage>();
    }

    /// <summary>
    /// Reset the state of the game and restart it from the beginning
    /// </summary>
    [Client]
    public void RestartGame()
    {
        NetworkClient.Send(new RestartGameMessage());
    }

    [Server]
    private void OnRestartGameRequest(NetworkConnectionToClient conn, RestartGameMessage msg)
    {
        if (ArgoNetworkManager.singleton.GameMode == NetworkGameMode.SinglePlayer)
        {

            var spawnedPlayers = ArgoNetworkManager.singleton.SpawnedPlayers.ToList();
            try
            {
                var runner = spawnedPlayers.Find(spawnedPlayer => spawnedPlayer.Type == PlayerType.Runner);
            }
            catch (ArgumentNullException)
            {
                Debug.LogError("Runner player not spawned. Closing server...");
                ArgoNetworkManager.singleton.StopServer();
            }

            var mainScene = NetworkManager.singleton.onlineScene;
            ArgoNetworkManager.singleton.ServerChangeScene(mainScene);
        }
    }

    /// <summary>
    /// Loads the MainMenu upon button press 
    /// </summary>
    public void GoToMainMenu()
    {
        if (ArgoNetworkManager.singleton.GameMode == NetworkGameMode.SinglePlayer)
        {
            ArgoNetworkManager.singleton.StopHost();
        }
        else
        {
            ArgoNetworkManager.singleton.StopClient();
        }
    }

    /// <summary>
    /// Toggles the gameover screen
    /// </summary>
    private void OnEnable()
    {
        ObstacleCollision.OnPlayerDeath += EnableGameOvermenu;
    }

    /// <summary>
    /// Toggle sthe game over screen
    /// </summary>
    private void OnDisable()
    {
        ObstacleCollision.OnPlayerDeath -= EnableGameOvermenu;
    }

    public void EnableGameOvermenu()
    {
        GameOverMenu.SetActive(true);
    }

    /// <summary>
    /// Toggles AI off and on upon button press
    /// </summary>
    public void ToggleRunnerAIButton()
    {
        //Debug.Log("here at toggle");
        // _toggleAIButton.text = "Hi";
        if (!_isRunnerAIOn)
        {
            _isRunnerAIOn = true;
            ToggleAIButton.image.color = Color.red;
            _runner.transform.GetChild(1).gameObject.SetActive(true);
            _runner.GetComponent<AIBrain>().enabled = true;
            _runner.GetComponent<RunnerPlayer>().enabled = false;
        }
        else
        {
            _isRunnerAIOn = false;
            ToggleAIButton.image.color = Color.white;
            _runner.transform.GetChild(1).gameObject.SetActive(false);
            _runner.GetComponent<AIBrain>().enabled = false;
            _runner.GetComponent<RunnerPlayer>().enabled = true;
        }
    }

    public void ToggleGodAIButton()
    {
        if (!_isGodAIOn)
        {
        }
        else
        {

        }
    }
}