using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using System;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] CanvasManager canvasManager;

    private const string leaderboardKey = "";
    private const float scoreMultiplier = 1000f;

    private void Awake()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("Error starting LootLocker session");
                Debug.Log(response.errorData.ToString());
                return;
            }
        });
    }

    public void SubmitScore(string playerName, float rawScore)
    {
        float timebasedScore = rawScore * scoreMultiplier;

        LootLockerSDKManager.SubmitScore(playerName, (int) timebasedScore, leaderboardKey, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Could not submit score");
                Debug.Log(response.errorData.ToString());
                canvasManager.SubmissionError();
                return;
            }
        });
    }

    public void GetScoreboard(int entryAmount)
    {
        LootLockerSDKManager.GetScoreList(leaderboardKey, entryAmount, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Could not get scores");
                Debug.Log(response.errorData.ToString());
                return;
            }

            float timebasedScore;
            TimeSpan entryTime;
            List<Tuple<string, string>> scoreboard = new List<Tuple<string, string>>();

            if (response.items == null)
                return;

            foreach (var entries in response.items)
            {
                timebasedScore = entries.score / scoreMultiplier;
                entryTime = TimeSpan.FromSeconds(timebasedScore);

                int min = entryTime.Minutes >= 99 ? 99 : entryTime.Minutes;
                string score = string.Format("{0:D2}:{1:D2}:{2:D2}", min, entryTime.Seconds, entryTime.Milliseconds / 10);

                scoreboard.Add(new Tuple<string, string>(entries.member_id, score));
            }

            canvasManager.UpdateLeaderboardEntries(scoreboard);
        });
    }
}
