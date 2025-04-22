using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private CanvasManager canvasManager;

    public List<GameObject> stages = new();
    public GameObject CurrentStage;

    public static StageManager instance;

    private List<IResetable> resetableList = new();
    private int remainingGarbage = 0;
    private int totalGarbage = 0;
    private Vector2 playerStartPosition;
    private Transform playerTransform;

    private int _currentStageIndex = 0;
    private bool _isGameOver = false;

    public bool FirstMove { get; set; } = false;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public void StartGame()
    {
        _isGameOver = false;
        FirstMove = false;
        StartNextStage();
    }

    public void StartTimer()
    {
        timeManager.StartTimer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isGameOver)
        {
            ResetStage(playSound: false);
        }
    }
    private void StartNextStage()
    {
        if (CurrentStage != null)
            Destroy(CurrentStage);

        if (_currentStageIndex >= stages.Count)
        {
            _currentStageIndex = 0;
            AudioManager.instance.PlaySound("Celebrate");
            canvasManager.SwitchPanels(2);
            _isGameOver = true;
            return;
        }

        GameObject stage = Instantiate(stages[_currentStageIndex], Vector3.zero, Quaternion.identity);

        CurrentStage = stage;

        resetableList.Clear();
        totalGarbage = 0;
        GetResetables(CurrentStage.transform);

        remainingGarbage = totalGarbage;

        playerTransform.position = playerStartPosition;

        _currentStageIndex++;
    }

    public void OnGarbageCollected()
    {
        remainingGarbage--;

        if (remainingGarbage == 0)
            StartNextStage();
    }

    public void ResetStage(bool playSound=true)
    {
        if (playSound)
        {
            AudioManager.instance.PlaySound("Die");
        }

        playerTransform.position = playerStartPosition;
        remainingGarbage = totalGarbage;

        foreach (IResetable resetable in resetableList)
        {
            resetable.ResetObject();
        }
    }

    private void GetResetables(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.TryGetComponent(out GarbagePiece _))
            {
                totalGarbage++;
            }

            if (child.gameObject.TryGetComponent(out StartTile start))
            {
                playerStartPosition = start.GetStartPosition();
            }

            if (child.gameObject.TryGetComponent(out IResetable r))
            {
                resetableList.Add(r);
            }
            GetResetables(child);
        }
    }
}
