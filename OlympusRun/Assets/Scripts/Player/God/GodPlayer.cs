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

using System.Collections;
using Mirror;
using UnityEngine;

/// <summary>
/// A class managing the God Player attacks
/// </summary>
public class GodPlayer : NetworkBehaviour
{
    [Header("The UI linked to the god")]
    [SerializeField]
    private GameObject uiPrefab;

    [Header("The camera linked to the god")]
    [SerializeField]
    private GodCamera _cameraPrefab;
    
    /// Whether the God is controlled by an AI or a user.
    [SerializeField] private bool isAI = true;
    [Header("Delay between attacks (in seconds")]
    [SerializeField] private float attackCooldownTime = 5f;

    /// Whether the God can currently attack.
    private bool _canAttack = true;
    private AIGod _ai;
    private GodAttack _activeAttack;
    private int _chosenAttack = -1;

    [SerializeField] GameObject[] AttackButtons;

    private Vector3 _attackPos = new Vector3();

    public float distance = 10f;

    public LayerMask mask;

    private void Awake()
    {
        var analyticsManager = FindObjectOfType<AnalyticsManager>();
        if (analyticsManager != null)
        {
            analyticsManager.God = gameObject;
        }
    }

    /// <summary>
    /// the start of the god player script. assigns to the private variables.
    /// </summary>
    private void Start()
    {
        _ai = GetComponent<AIGod>();
        
        foreach (GameObject g in AttackButtons)
        {
            g.transform.Find("Background").gameObject.SetActive(false);
            g.transform.Find("Selected").gameObject.SetActive(false);
        }
    }

    public override void OnStartLocalPlayer()
    {
        GodCamera runnerCamera = Instantiate(_cameraPrefab);
        GameObject ui = Instantiate(uiPrefab);
    }

    /// <summary>
    /// if is an AI, calls Attack
    /// If not an AI and detects touch, grabs and converts the position to world space
    /// </summary>
    private void Update()
    {
        if (isAI && _canAttack)
        {
            Attack();
        }

        if (!isAI && _canAttack)
        {
            if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && _chosenAttack != -1)
            {
                if (PlacementAllow())
                {
                    _activeAttack = _ai.GetAttack(_chosenAttack);
                    Vector3 pos = Input.mousePosition;
                    pos.z = Camera.main.nearClipPlane + 25;
                    pos = Camera.main.ScreenToWorldPoint(pos);
                    if (_chosenAttack == 1)
                    {
                        pos.y = 2;
                    }
                    else
                    {
                        pos.y = 2;
                    }
                    _attackPos = pos;
                    PlayerAttack();
                }
                else
                {
                    //Debug.Log("pick another place");
                }
            }
        }
    }

    /// <summary>
    /// Chooses the next attack for the God.
    /// </summary>
    /// <param name="t_attack">The number of the attack clicked on.</param>
    public void ChooseAttack(int t_attack)
    {
        //foreach (GameObject g in AttackButtons)
        //{
        //    g.transform.Find("Selected").gameObject.SetActive(false);
        //}

        //AttackButtons[t_attack - 1].transform.Find("Selected").gameObject.SetActive(true);

        _activeAttack = _ai.GetAttack(t_attack);
        _chosenAttack = t_attack;
    }

    /// <summary>
    /// Performs the attack and start the cooldown.
    /// </summary>
    private void Attack()
    {
        if (!_canAttack)
            return;

        if (isAI)
        {
            _activeAttack = _ai.GetRandomAttack();
        }

        _activeAttack.Attack();
        StartCoroutine(StartAttackCooldown());
    }
    /// <summary>
    /// Starts the cooldown that deactivate any attack. The player can't attack during this time.
    /// </summary>
    private IEnumerator StartAttackCooldown()
    {
        foreach (GameObject g in AttackButtons)
        {
            g.transform.Find("Background").gameObject.SetActive(true);
        }

        _canAttack = false;
        _activeAttack = null;
        yield return new WaitForSeconds(attackCooldownTime);
        _canAttack = true;

        foreach (GameObject g in AttackButtons)
        {
            g.transform.Find("Background").gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Performs the player attack and start the cooldown.
    /// </summary>
    public void PlayerAttack()
    {
        if (!_canAttack)
            return;

        if (!isAI)
        {
            _activeAttack.Attack(_attackPos);
            StartCoroutine(StartAttackCooldown());
        }
    }

    private bool PlacementAllow()
    {
        if (Input.mousePosition.y > 400)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // Check if the raycast hits a specific GameObject
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
                Debug.Log(hitInfo.collider.gameObject.tag);
                GameObject hitObject = hitInfo.collider.gameObject;
                if (hitObject.tag == "Ground")
                {
                    //Debug.Log(hitInfo.collider.gameObject.tag);
                    hitObject = hitInfo.collider.gameObject;
                    if (hitObject.tag == "Ground")
                    {
                        return true;
                    }
                }
                return false;
            }

        }

        return false;
    }
}
