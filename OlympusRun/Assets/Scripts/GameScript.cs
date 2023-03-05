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
/// helps tests have access to the game features
/// </summary>
public class GameScript : MonoBehaviour
{
    /// The instance of this script in the scene.
    private static GameScript _instance;

    /// The runner player in the scene.
    [SerializeField]
    private GameObject _runner;

    /// The obstacle spawner in the scene.
    [SerializeField]
    private GameObject _obstacleSpawner;

    [SerializeField]
    private GameObject pickUpController;

    [SerializeField]
    private GameObject goalController;

    [SerializeField]
    private GameObject[] _gods;

    /// <summary>
    /// Sets up this script to have an instance of itself.
    /// </summary>
    private void Start()
    {
        _instance = this;
    }

    /// <summary>
    /// Gets the runner in the scene from the runner.
    /// </summary>
    /// <returns>The instance of the runner player script.</returns>
    public RunnerPlayer GetRunner()
    {
        RunnerPlayer p = _runner.GetComponent<RunnerPlayer>();

        p.OnStartAuthority();
        p.GetComponent<MoveForward>().OnStartLocalPlayer();
        p.GetComponent<Score>().OnStartAuthority();

        return p;
    }

    /// <summary>
    /// Gets the runner's AI in the scene from the runner.
    /// </summary>
    /// <returns>The instance of the runner's AI.</returns>
    public AIBrain GetRunnerAI()
    {
        AIBrain p = _runner.GetComponent<AIBrain>();

        p.GetComponent<RunnerPlayer>().OnStartAuthority();
        p.GetComponent<MoveForward>().OnStartLocalPlayer();

        return p;
    }

    /// <summary>
    /// Gets the obstacle spawner script from the object.
    /// </summary>
    /// <returns>The instance of the obstacle spawner script.</returns>
    public ObstacleSpawner GetObstacleSpawner()
    {
        return _obstacleSpawner.GetComponent<ObstacleSpawner>();
    }

    /// <summary>
    /// Gets the pickup controller script from the object.
    /// </summary>
    /// <returns>The instance of the obstacle spawner script.</returns>
    public PickupController GetPickupController()
    {
        return pickUpController.GetComponent<PickupController>();
    }

    /// <summary>
    /// Gets the pickup controller script from the object.
    /// </summary>
    /// <returns>The instance of the obstacle spawner script.</returns>
    public GoalController GetGoalController()
    {
        return goalController.GetComponent<GoalController>();
    }

    /// <summary>
    /// Gets the runner health controller script from the object.
    /// </summary>
    /// <returns>The instance of the runner health controller script.</returns>
    public RunnerHealthController GetHealthController()
    {

        RunnerPlayer p = _runner.GetComponent<RunnerPlayer>();

        p.OnStartAuthority();
        p.GetComponent<MoveForward>().OnStartLocalPlayer();

        RunnerHealthController health = p.GetComponent<RunnerHealthController>();

        health.OnStartLocalPlayer();

        return health;
    }

    /// <summary>
    /// Gets the move forward script from the object.
    /// </summary>
    /// <returns>The instance of the move forward script.</returns>
    public MoveForward GetMoveForward()
    {

        RunnerPlayer p = _runner.GetComponent<RunnerPlayer>();

        p.OnStartAuthority();
        p.GetComponent<MoveForward>().OnStartLocalPlayer();

        MoveForward moveF = p.GetComponent<MoveForward>();

        moveF.OnStartLocalPlayer();

        return moveF;
    }

    public GodPlayer GetGod()
    {
        return _gods[0].GetComponent<GodPlayer>();
    }

    public AIGod GetGodAI()
    {
        return _gods[0].GetComponent<AIGod>();
    }

    public Camera GetGodCamera()
    {
        return GameObject.Find("GodCamera").GetComponent<Camera>();
    }
}