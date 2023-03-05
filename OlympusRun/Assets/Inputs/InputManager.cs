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
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    #endregion
    
    private RunnerController runnerController;

    private new Camera camera;
    
    void Awake()
    {
        runnerController = new RunnerController();
        camera = Camera.main;
    }
    
    void OnEnable()
    {
        runnerController.Enable();
    }
    
    void OnDisable()
    {
        runnerController.Disable();
    }
    
    void Start()
    {
        runnerController.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        runnerController.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }
    
    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        StartCoroutine(WaitStartTouch(context));
    }

    private IEnumerator WaitStartTouch(InputAction.CallbackContext context)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        yield return new WaitForEndOfFrame();

        var position = runnerController.Touch.PrimaryPosition.ReadValue<Vector2>();
        if (position.x == 0 && position.y == 0)
        {
            yield return null;
        }

        Vector2 temp = runnerController.Touch.PrimaryPosition.ReadValue<Vector2>();
        if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(camera, runnerController.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
    }
    
    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) OnEndTouch(Utils.ScreenToWorld(camera, runnerController.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(camera, runnerController.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}
