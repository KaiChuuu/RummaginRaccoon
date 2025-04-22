using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] LeaderboardManager leaderboardManager;
    [SerializeField] TimeManager timeManager;

    [Space]

    [SerializeField] GameObject startPanel;        // 0
    [SerializeField] GameObject inGamePanel;       // 1
    [SerializeField] GameObject endPanel;          // 2
    [SerializeField] GameObject leaderboardPanel;  // 3
    private GameObject currentPanel;

    [Space]

    [Header("Ingame UI")]
    [SerializeField] TMP_Text gameTime;

    [Space]

    [Header("Endgame UI")]
    [SerializeField] GameObject invalidText;
    [SerializeField] Button submitButton;
    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] TMP_Text playerTime;

    [Space]

    [Header("Leaderboard UI")]
    [SerializeField] Transform leaderboardContent;
    [SerializeField] GameObject entryPrefab;
    [SerializeField] int entryDisplayAmount = 1;

    void Start()
    {
        currentPanel = startPanel;

        if (leaderboardManager == null)
        {
            Debug.Log("Missing Leaderboard Script");
        }
        if(timeManager == null)
        {
            Debug.Log("Missing Timemanager Script");
        }
    }

    private void Update()
    {
        if(inGamePanel.activeSelf)
        {
            gameTime.text = timeManager.GetTime();
        }
    }

    public void HideInvalidText()
    {
        if (invalidText.activeSelf)
        {
            invalidText.SetActive(false);
        }
    }

    public void SubmitScore()
    {
        if (playerNameInput.text == "")
        {
            AudioManager.instance.PlaySound("Error");
            invalidText.GetComponent<TMP_Text>().text = "Invalid Name";
            invalidText.SetActive(true);
            return;
        }

        AudioManager.instance.PlaySound("Click");
        submitButton.interactable = false;
        leaderboardManager.SubmitScore(playerNameInput.text, timeManager.GetRawScore());
    }

    public void SubmissionError()
    {
        AudioManager.instance.PlaySound("Error");
        invalidText.GetComponent<TMP_Text>().text = "Submission Error";
        invalidText.SetActive(true);
        submitButton.interactable = true;
    }

    public void UpdateLeaderboardEntries(List<Tuple<string, string>> scores)
    {
        foreach(Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        for(int i =0; i < scores.Count; i++)
        {
            GameObject entry = Instantiate(entryPrefab, leaderboardContent);

            TMP_Text[] entryTexts = entry.GetComponentsInChildren<TMP_Text>();
            entryTexts[0].text = (i + 1).ToString();
            entryTexts[1].text = scores[i].Item1;
            entryTexts[2].text = scores[i].Item2;
        }
    }

    public void SwitchPanels(int panel)
    {
        AudioManager.instance.PlaySound("Click");
        switch (panel)
        {
            case 0:
                EnablePanel(ref startPanel);
                break;
            case 1:
                StageManager.instance.StartGame();
                EnablePanel(ref inGamePanel);
                break;
            case 2:
                timeManager.StopTimer();
                playerTime.text = timeManager.GetTime();
                submitButton.interactable = true;
                EnablePanel(ref endPanel);
                break;
            case 3:
                leaderboardManager.GetScoreboard(entryDisplayAmount);
                EnablePanel(ref leaderboardPanel);
                break;
        }
    }

    void EnablePanel(ref GameObject panel)
    {
        currentPanel.SetActive(false);
        currentPanel = panel;
        panel.SetActive(true);
    }
}