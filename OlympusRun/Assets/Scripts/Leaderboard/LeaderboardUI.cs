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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private RectTransform rowTemplatePrefab;
    [SerializeField] private RectTransform content;

    private Leaderboard _leaderboard;
    private bool _loaded = false;
    private List<LeaderboardData> _data;

    private void Awake()
    {
        _leaderboard = new Leaderboard();
        StartCoroutine(_leaderboard.GetData());
        _data = _leaderboard.FetchLeaderboardData();
        foreach (var value in _data)
        {
            AddRow(value);
        }
        _loaded = false;
    }

    private void Update()
    {

        if (_data == null)
        {
            _data = _leaderboard.FetchLeaderboardData();
        }
        else
        {
            if (_loaded == false)
            {
                for(int i =0; i < _data.Count; i++)
                {
                    AddRow(_data[i]);
                    if(i == _data.Count -1)
                    {
                        _loaded = true;
                    }
                }
            }
        }
    }

    public void AddRow(LeaderboardData data)
    {
        var newRow = Instantiate(rowTemplatePrefab, content.transform);
        LeaderboardRowUI rowUI = newRow.GetComponent<LeaderboardRowUI>();
        if (rowUI == null)
        {
            UnityEngine.Debug.LogError("The row template prefab does not have a LeaderboardRowUI component.");
            return;
        }
        rowUI.SetData(data.No, data.Username, data.BestRunnerScore, data.BestGodScore);
    }
    
    public void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
