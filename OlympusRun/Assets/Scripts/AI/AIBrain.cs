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
/// The Brain of the AI Runner.
/// </summary>
public class AIBrain : MonoBehaviour
{
    /// The Rigidbody of the Runner - Used to apply the force for the jump.
    Rigidbody rb;

    /// The amount of force applied to the Runner in order to get them off the ground.
    private float _jumpForce = 7.0f;

    /// Bool that keeps track of whether the Runner is on the ground or not.
    private bool _grounded = false;

    [Header("AI_Sliding")]
    /// Maximum slide time 
    public float MaxSlideTime = 1.0f;

    /// Timer to check how long the gameobject is sliding for before going back to running 
    private float SlideTimer;

    /// The scale will be half of what the gameobject is to show that it is currently sliding 
    public float SlideYScale;
    /// The scale will be double of what the gameobject is to show that it is currently sliding
    public float SlideZScale;

    ///The current scale of the AI Runner.
    private Vector3 _AIScale;

    ///
    public Transform AIObj;

    /// How much the runner moves when changing lanes
    private int _laneSize = 2;

    /// current lane occupied by runner
    public int CurrentLane { get;  set; } = 2;

    public int PreviousLane { get; set; } = 2;
    /// Keeps track of whether the AI runner is currently sliding.
    bool _sliding;
    /// Public reference to _sliding.
    public bool sliding { get { return _sliding; } }

    public int GetObstacleLane { get; set; }

    AIGod _ai_God;
    /// <summary>
    /// Controller of how the AI Runner reacts, once it sees an obstacle.
    /// </summary>
    /// <param name="t_seenObstacle">The obstacle it sees ahead.</param>
    public void React(Collider t_seenObstacle)
    {
        float i = 0.0f;

        if (t_seenObstacle.CompareTag("Inpenetrable") || t_seenObstacle.CompareTag("AI_Inpenetrable"))
            i = 0.3f;

        if (t_seenObstacle.CompareTag("JumpObstacle"))
            i = 0.666f;

        if (t_seenObstacle.CompareTag("SlideObstacle"))
            i = 0.8f;

        float result = GetComponent<FuzzyLogic>().SetMemberShipFunctions(CurrentLane / 3.1f, i);

        Debug.Log("RESULT: " + result);
        bool solved = false;

        switch(result)
        {
            case 2:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + _laneSize, gameObject.transform.position.y, gameObject.transform.position.z);
                CurrentLane++;
                GetComponent<RunnerPlayer>().CurrentLane = CurrentLane;
                solved = true;
                break;

            case 3:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - _laneSize, gameObject.transform.position.y, gameObject.transform.position.z);
                CurrentLane--;
                GetComponent<RunnerPlayer>().CurrentLane = CurrentLane;
                solved = true;
                break;

            case 4:
                Jump();
                solved = true;
                break;

            case 5:
                StartSlide();
                solved = true;
                break;
        }
    }

    /// <summary>
    /// A function that is called at the start of the game, to get the components in the Runner.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        _AIScale = AIObj.localScale;
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
    void Land()
    {
        _grounded = true;
    }

    /// <summary>
    /// The physical motion for the runner to jump.
    /// </summary>
    void Jump()
    {
        if (_grounded)
        {
            rb.velocity = transform.up * _jumpForce;
            _grounded = false;
        }
    }

    /// <summary>
    /// Start the AI sliding.
    /// </summary>
    private void StartSlide()
    {
        _sliding = true;

        AIObj.localScale = new Vector3(AIObj.localScale.x, SlideYScale, SlideZScale);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        SlideTimer = MaxSlideTime;
    }

    /// <summary>
    /// End the AI sliding.
    /// </summary>
    private void StopSlide()
    {
        _sliding = false;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        AIObj.localScale = _AIScale;
    }


    /// <summary>
    /// Calls every frame to update the brain.
    /// </summary>
    private void Update()
    {
        if (_sliding && SlideTimer > 0)
        {
            SlideTimer -= Time.deltaTime;
        }
        else
        {
            StopSlide();
        }
    }

    public void reactToAI_God(Collider t_seenObstacle)
    {
        _ai_God = GameObject.FindGameObjectWithTag("God").GetComponent<AIGod>();
        
        if (t_seenObstacle.CompareTag("Inpenetrable") || t_seenObstacle.CompareTag("JumpObstacle") || t_seenObstacle.CompareTag("SlideObstacle"))
        {
            GetObstacleLane = Mathf.CeilToInt(t_seenObstacle.GetComponent<Transform>().position.x);

            switch (GetObstacleLane)
            {
                case -2:
                    GetObstacleLane = 1;
                    break;
                case 0:
                    GetObstacleLane = 2;
                    break;
                case 2:
                    GetObstacleLane = 3;
                    break;
            }
            _ai_God.predictLaneNow(); // ai god predict the lane before ai runner make a decision 
        }
    }
}
