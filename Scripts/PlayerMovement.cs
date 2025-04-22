using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask TileLayer;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private PlayerInput _playerInput;
    

    private MoveDirection _left;
    private MoveDirection _right;
    private MoveDirection _down;
    private MoveDirection _up;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _left  = new MoveDirection(Vector2.left*3 , - new Vector3(2.5f, 1.5f, 0) , new Vector3(0.5f, 0.5f, 0.5f));
        _right = new MoveDirection(Vector2.right*3, - new Vector3(-2, 1.5f, 0), new Vector3(0.5f, 0.5f, 0.5f));
        _down  = new MoveDirection(Vector2.down*2 , - new Vector3(0, 4f, 0) , new Vector3(0.5f, 0.5f, 0.5f));
        _up    = new MoveDirection(Vector2.up*2   , - new Vector3(0, -0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f));
    }

    private void CheckFirstMove()
    {
        if (!StageManager.instance.FirstMove)
        {
            StageManager.instance.FirstMove = true;
            StageManager.instance.StartTimer();
            return;
        }
    }

    private bool _canMove = true;

    private IEnumerator WaitMoveReset()
    {
        _canMove = false;

        yield return new WaitForFixedUpdate();
        
        _canMove = true;
    }

    private void Update()
    {
        if (!_canMove || StageManager.instance.IsGameOver()) { return; }

        if (_playerInput.GetMoveLeftInput())
        {
            Vector2 toMove = _left.CheckCanMove(transform.position, TileLayer);

            if (toMove != (Vector2)transform.position)
            {
                transform.position = toMove;
                MovementDetector.instance.TriggerPlayerMovedEvents(Direction.Left);
                _spriteRenderer.flipX = true;
                CheckFirstMove();
                StartCoroutine(WaitMoveReset());
            }

            
        }

        if (_playerInput.GetMoveRightInput())
        {
            Vector2 toMove = _right.CheckCanMove(transform.position, TileLayer);
            if (toMove != (Vector2)transform.position)
            {
                transform.position = toMove;
                MovementDetector.instance.TriggerPlayerMovedEvents(Direction.Right);
                _spriteRenderer.flipX = false;
                CheckFirstMove();
                StartCoroutine(WaitMoveReset());
            }

            
        }

        if (_playerInput.GetMoveDownInput())
        {
            Vector2 toMove = _down.CheckCanMove(transform.position, TileLayer);
            if (toMove != (Vector2)transform.position)
            {
                transform.position = toMove;
                MovementDetector.instance.TriggerPlayerMovedEvents(Direction.Down);
                CheckFirstMove();
                StartCoroutine(WaitMoveReset());
            }

            
        }

        if (_playerInput.GetMoveUpInput())
        {
            Vector2 toMove = _up.CheckCanMove(transform.position, TileLayer);
            if (toMove != (Vector2)transform.position)
            {
                transform.position = toMove;
                MovementDetector.instance.TriggerPlayerMovedEvents(Direction.Up);
                CheckFirstMove();
                StartCoroutine(WaitMoveReset());
            }

            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - new Vector3(2.5f,1.5f,0), new Vector3(0.5f,0.5f,0.5f)); // left
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position - new Vector3(-2,1.5f,0), new Vector3(0.5f,0.5f,0.5f)); // right
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position - new Vector3(0,4f,0), new Vector3(0.5f,0.5f,0.5f)); // down
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position - new Vector3(0,-0.5f,0), new Vector3(0.5f,0.5f,0.5f)); // up
    }
}

public class MoveDirection
{
    public Vector3 Movement;
    public Vector3 DetectPos;
    public Vector3 DetectSize;

    public MoveDirection(Vector3 movement, Vector3 detectPos, Vector3 detectSize)
    {
        Movement = movement;
        DetectPos = detectPos;
        DetectSize = detectSize;
    }

    public Vector3 CheckCanMove(Vector3 pos, LayerMask layer)
    {
        if (Physics2D.BoxCast(pos + DetectPos, DetectSize, 0, Vector2.zero, layer))
        {
            return pos + Movement;
        }

        return pos;
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}
