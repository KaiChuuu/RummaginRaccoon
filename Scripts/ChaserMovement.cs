using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChaserMovement : MonoBehaviour
{
    [SerializeField] private LayerMask TileLayer;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private MoveDirection _left;
    private MoveDirection _right;
    private MoveDirection _down;
    private MoveDirection _up;

    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        _left = new MoveDirection(Vector2.left * 3, -new Vector3(2.5f, 1.5f, 0), new Vector3(0.5f, 0.5f, 0.5f));
        _right = new MoveDirection(Vector2.right * 3, -new Vector3(-2, 1.5f, 0), new Vector3(0.5f, 0.5f, 0.5f));
        _down = new MoveDirection(Vector2.down * 2, -new Vector3(0, 4f, 0), new Vector3(0.5f, 0.5f, 0.5f));
        _up = new MoveDirection(Vector2.up * 2, -new Vector3(0, -0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f));
    }

    public void MakeMove(Direction direction)
    {
        Vector2 toMove = transform.position;
        switch (direction)
        {
            case Direction.Left:
                toMove = transform.position + _left.Movement;        
                break;

            case Direction.Right:
                toMove = transform.position + _right.Movement;
                break;

            case Direction.Up:
                toMove = transform.position + _up.Movement;
                break;
            
            case Direction.Down:
                toMove = transform.position + _down.Movement;
                break;
        }
        if (toMove != (Vector2)transform.position)
        {
            _spriteRenderer.flipX = _playerTransform.position.x < transform.position.x;
        }
        transform.position = toMove;
    }
}
