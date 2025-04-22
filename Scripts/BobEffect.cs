using UnityEngine;

public enum BobDirectionStart
{
    Random,
    Up,
    Down
}

public class BobEffect : MonoBehaviour
{
    [SerializeField] public float BobTime;
    [SerializeField] public float Amplitude;
    public BobDirectionStart StartDirection = BobDirectionStart.Random;

    private float _bobUpPos;
    private float _bobDownPos;

    private void Awake()
    {
        _bobUpPos = transform.localPosition.y + (Amplitude / 2f);
        _bobDownPos = transform.localPosition.y - (Amplitude / 2f);
    }

    private void Start()
    {
        if (StartDirection == BobDirectionStart.Random)
        {
            if (Random.Range(0,1) > 0.5f)
            {
                LeanTween.moveLocalY(gameObject, _bobUpPos, BobTime / 2f).setOnComplete(() => { StartBobbingDown(); });
            }
            else
            {
                LeanTween.moveLocalY(gameObject, _bobDownPos, BobTime / 2f).setOnComplete(() => { StartBobbingUp(); });
            }
        }
        else if (StartDirection == BobDirectionStart.Up)
        {
            LeanTween.moveLocalY(gameObject, _bobUpPos, BobTime / 2f).setOnComplete(() => { StartBobbingDown(); });
        }
        else
        {
            LeanTween.moveLocalY(gameObject, _bobDownPos, BobTime / 2f).setOnComplete(() => { StartBobbingUp(); });
        }
    }

    private void StartBobbingDown()
    {
        LeanTween.moveLocalY(gameObject, _bobDownPos, BobTime).setLoopPingPong();
    }

    private void StartBobbingUp()
    {
        LeanTween.moveLocalY(gameObject, _bobUpPos, BobTime).setLoopPingPong();
    }
}
