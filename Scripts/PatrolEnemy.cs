using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour, IResetable
{
    [SerializeField] private List<Transform> targets = new();
    
    private LineRenderer _lineRenderer;
    private SpriteRenderer _spriteRenderer;

    public Vector3 _currentTargetPos = Vector2.zero;
    private int _currentTargetIndex = 1;

    private Transform _playerTransform;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (targets.Count < 2)
        {
            print("Patrol enemy does not have enough targets!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _lineRenderer.positionCount = targets.Count;
        _lineRenderer.loop = true;

        transform.position = targets[0].position;
        for (int i = 0; i < targets.Count; i++) 
        {
            _lineRenderer.SetPosition(i, targets[i].position);
        }

        _currentTargetPos = targets[1].position;
        _currentTargetIndex = 1;

        _spriteRenderer.flipX = _playerTransform.position.x < _currentTargetPos.x;
    }

    private void OnEnable()
    {
        MovementDetector.instance.Subscribe(Move);
    }

    private void OnDisable()
    {
        MovementDetector.instance.Unsubscribe(Move);
    }


    private bool _isMoving = false;
    private void Move(Direction direction)
    {
        StartCoroutine(DelayMove());
        IEnumerator DelayMove()
        {
            yield return new WaitForFixedUpdate();


            _isMoving = true;
            Vector2 moveDir = (_currentTargetPos - transform.position).normalized;
            if (Mathf.Abs(moveDir.x) > 0.001f)
            {
                transform.localPosition += (Vector3)moveDir.normalized * 3f;
                _spriteRenderer.flipX = _currentTargetPos.x < transform.position.x;
            }
            else
            {
                transform.localPosition += (Vector3)moveDir.normalized * 2f;
                _spriteRenderer.flipX = _playerTransform.position.x < transform.position.x;
            }


            if (transform.position == _currentTargetPos)
            {
                if (_currentTargetIndex >= targets.Count - 1)
                {
                    _currentTargetIndex = 0;
                }
                else
                {
                    _currentTargetIndex++;
                }

                _currentTargetPos = targets[_currentTargetIndex].position;
            }

            _isMoving = false;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StageManager.instance.ResetStage();
        }
    }

    public void ResetObject()
    {
        StartCoroutine(WaitMoveFinish());
        IEnumerator WaitMoveFinish()
        {
            yield return new WaitUntil(() => !_isMoving);
            transform.position = targets[0].position;
            _currentTargetPos = targets[1].position;
            _currentTargetIndex = 1;
        }
        
    }
}
