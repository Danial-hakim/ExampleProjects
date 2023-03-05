using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardRowUI : MonoBehaviour
{
    private TextMeshProUGUI _noText;
    private TextMeshProUGUI _usernameScoreText;
    private TextMeshProUGUI _bestRunnerScoreText;
    private TextMeshProUGUI _bestGodScoreText;

    private void Awake()
    {
        _noText = transform.Find("NoText").GetComponent<TextMeshProUGUI>();
        _usernameScoreText = transform.Find("UsernameText").GetComponent<TextMeshProUGUI>();
        _bestRunnerScoreText = transform.Find("BestRunnerScoreText").GetComponent<TextMeshProUGUI>();
        _bestGodScoreText = transform.Find("BestGodScoreText").GetComponent<TextMeshProUGUI>();
    }
    
    public void SetData(int no, string username, string bestRunnerScore, int bestGodScore)
    {
        _noText.text = no.ToString();
        _usernameScoreText.text = username;
        _bestRunnerScoreText.text = bestRunnerScore;
        _bestGodScoreText.text = bestGodScore.ToString();
    }
}
