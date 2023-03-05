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
/// A class which accepts player input and makes the runner move, jump and slide accordingly
/// </summary>
public class RunnerPlayer : NetworkBehaviour
{
    /// The minimum distance the swipe is to be to be considered a swipe
    [SerializeField]
    private float _minimumDistance = 0.01f;

    /// The max amount of time the swipe can last
    [SerializeField]
    private float _maximumTime = 1f;

    /// the minimum amount of movement to move a direction.
    [SerializeField]
    private float _directionThreshold = .7f;

    /// The size of each lane.
    [SerializeField]
    private float _laneSize = 2;

    /// The amount of lanes there are.
    [SerializeField]
    private float _laneCount = 3;

    [Header("Sliding")]
    public float MaxSlideTime = 1.5f;
    
    /// The Y scale the Runner takes on while sliding.
    public float SlideYScale = 0.3f;

    /// The X scale the Runner takes on while sliding.
    public float SlideZScale = 1;

    /// The current lane the runner is in.
    public float CurrentLane { get; set; }

    /// Is used to detect swipes on the screen.
    private InputManager _inputManager;

    /// The start of the swipe.
    private Vector2 _startPosition;

    /// The time the swipe started.
    private float _startTime;

    /// The end of the swipe.
    private Vector2 _endPosition;

    /// The time the swipe ended.
    private float _endTime;

    /// The Rigidbody of the Runner - Used to apply the force for actions.
    private Rigidbody _rb;

    /// The timer that keeps track of how long the Runner has been sliding.
    private float _slideTimer;
    
    /// The Runner's original scale.
    private Vector3 _playerScale;

    /// The amount of force applied to the Runner in order to get them off the ground.
    private const float JumpForce = 7.0f;

    /// Bool that keeps track of whether the Runner is on the ground or not.
    private bool _grounded;

    /// How hard the User has to swipe up before the Runner jumps.
    private const float SwipeIntensity = 70.0f;

    /// Whether the runner is currently sliding.
    public bool Sliding { get; private set; }

    /// Whether the Runner is moving.
    private bool _moving;

    /// Whether the runner is currently jumping
    public bool Jumping { get; private set; }

    private void Awake()
    {
        // Disable this script because it must be enabled only for the Local Player
        enabled = false;
        
        // Also disable Point Of Interest
        transform.GetChild(0).GetComponent<PointOfInterest>().enabled = false;
    }

    public override void OnStartAuthority()
    {
        _inputManager = InputManager.Instance;
        CurrentLane = Mathf.Ceil(_laneCount / 2);

        _rb = GetComponent<Rigidbody>();
        _playerScale = transform.localScale;

        // Enable this script for the local player only
        // IMPORTANT: Keep this line at the end of OnStartAuthority otherwise OnEnable will be called before everything
        // is initialized
        enabled = true;
        transform.GetChild(0).GetComponent<PointOfInterest>().enabled = true;    
    }

    public override void OnStopAuthority()
    {
        enabled = false;
    }
    
    private void OnEnable()
    {
        if (_inputManager == null)
            return;
        
        _inputManager.OnStartTouch += SwipeStart;
        _inputManager.OnEndTouch += SwipeEnd;
    }
    
    private void OnDisable()
    {
        if (_inputManager == null)
            return;
        
        _inputManager.OnStartTouch -= SwipeStart;
        _inputManager.OnEndTouch -= SwipeEnd;
    }

    /// <summary>
    /// Is called to get the numbers of the swipe.
    /// </summary>
    /// <param name="position">The starting position of the swipe.</param>
    /// <param name="time">The start time of the swipe.</param>
    public void SwipeStart(Vector2 position, float time)
    {
        _startPosition = position;
        _startTime = time;
    }

    /// <summary>
    /// Is called to get the numbers of the swipe, and pass it on.
    /// </summary>
    /// <param name="position">The end position of the swipe.</param>
    /// <param name="time">The end time of the swipe.</param>
    public void SwipeEnd(Vector2 position, float time)
    {
        _endPosition = position;
        _endTime = time;
        DetectSwipe();
    }

    /// <summary>
    /// Figures out whether the input is a proper swipe.
    /// </summary>
    private void DetectSwipe()
    {
        if (Vector3.Distance(_startPosition, _endPosition) >= _minimumDistance &&
            (_endTime - _startTime) <= _maximumTime)
        {
            Vector3 direction = _endPosition - _startPosition;
            //Vector2 direction2D = new Vector2(direction.x, direction.x).normalized;
            direction = direction.normalized;
            SwipeDirection(direction);
        }
    }

    /// <summary>
    /// Figures out what the swipe will do to the runner.
    /// </summary>
    /// <param name="direction">The direction the swipe.</param>
    private void SwipeDirection(Vector2 direction)
    {
        if (!Sliding)
        {
            if (CurrentLane > 1)
            {
                if (Vector2.Dot(Vector2.left, direction) > _directionThreshold)
                {
                    _moving = true;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - _laneSize, gameObject.transform.position.y, gameObject.transform.position.z);
                    CurrentLane--;
                    Debug.Log(CurrentLane);
                    GetComponent<AIBrain>().CurrentLane = (int)CurrentLane;
                }

            }
            if (CurrentLane < _laneCount)
            {
                if (Vector2.Dot(Vector2.right, direction) > _directionThreshold)
                {
                    _moving = true;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + _laneSize, gameObject.transform.position.y, gameObject.transform.position.z);
                    CurrentLane++;
                    Debug.Log(CurrentLane);
                    GetComponent<AIBrain>().CurrentLane = (int)CurrentLane;

                }
            }
            if (Vector2.Dot(Vector2.down, direction) > _directionThreshold && !_moving)
            {
                Sliding = true;
                transform.GetChild(0).GetComponent<PointOfInterest>().StartSlide();
                StartSlide();
            }
        }
        _moving = false;
    }

    /// <summary>
    /// The update that is called every frame for the Runner.
    /// </summary>
    private void Update()
    {
        if (Sliding && _slideTimer > 0)
        {
            _slideTimer -= Time.deltaTime;
        }
        else
        {
            StopSlide();
        }
    }

    /// <summary>
    /// Changes the Runner's state to sliding.
    /// </summary>
    private void StartSlide()
    {
        transform.localScale = new Vector3(transform.localScale.x, SlideYScale, SlideZScale);
        _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        _slideTimer = MaxSlideTime;
    }

    /// <summary>
    /// Resets the Runner back to its running state.
    /// </summary>
    private void StopSlide()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.localScale = _playerScale;
        Sliding = false;

    }

    /// <summary>
    /// Performs the jump command.
    /// </summary>
    public void Jump()
    {
        if (_grounded && !Sliding)
        {
            transform.GetChild(0).GetComponent<PointOfInterest>().StartJump();
            Jumping = true;
            //_rb.AddForce(transform.up * JumpForce * 6);
            _rb.velocity = transform.up * JumpForce;
            _grounded = false;
        }
    }

    /// <summary>
    /// A check to see whether the Runner has landed back on the ground.
    /// </summary>
    /// <param name="collision">The object the Runner has collided with.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {  
            Land();
        }
    }

    /// <summary>
    /// What happens when the Runner is on the ground again.
    /// </summary>
    private void Land()
    {
        _grounded = true;
        Jumping = false;
    }

    /// <summary>
    /// Called every frame to update physics. is used for the swipe check.
    /// </summary>
    private void FixedUpdate()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            if (touch.deltaPosition.y > SwipeIntensity)
            {
                Jump();
            }
        }
    }

    public void TestChangePos(int lane,int xPos)
    {
        gameObject.transform.position = new Vector3(xPos, gameObject.transform.position.y, gameObject.transform.position.z);
        CurrentLane = lane;
        GetComponent<AIBrain>().CurrentLane = (int)CurrentLane;
    }
}
