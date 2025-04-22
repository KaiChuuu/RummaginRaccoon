using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class CountdownTile : Tile
{
    [SerializeField] private TextMeshProUGUI CountdownText;
    [SerializeField] private int Countdown;
    [SerializeField] private Animator FireAnimator;

    private int _currentCountdown;
    private bool _isDangerous = false;

    private void OnValidate()
    {
        if (_currentCountdown > 0)
            CountdownText.text = Countdown.ToString();
    }

    private void Start()
    {
        _currentCountdown = Countdown;
        CountdownText.text = Countdown.ToString();
    }

    private void OnEnable()
    {
        if (!Application.isPlaying) { return; }

        MovementDetector.instance.Subscribe(OnPlayerMoved);
    }

    private void OnDisable()
    {
        if (!Application.isPlaying) { return; }

        MovementDetector.instance.Unsubscribe(OnPlayerMoved);
    }

    private void OnPlayerMoved(Direction direction)
    {
        _currentCountdown--;

        if (_currentCountdown == 0)
        {
            _currentCountdown = 0;
            _isDangerous = true;
            CountdownText.text = "";
            AudioManager.instance.PlaySound("Fire");
            FireAnimator.SetBool("onFire", true);
        }

        if (_currentCountdown > 0)
            CountdownText.text = _currentCountdown.ToString();

        
    }

    protected override void OnPlayerEnter()
    {
        if (_isDangerous && IsTouchingPlayer())
        {
            StageManager.instance.ResetStage();
        }
    }

    public override void ResetObject()
    {
        CountdownText.text = Countdown.ToString();
        FireAnimator.SetBool("onFire", false);
        _currentCountdown = Countdown;
        _isDangerous = false;
    }
}
