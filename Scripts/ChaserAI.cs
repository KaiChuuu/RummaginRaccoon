using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChaserAI : MonoBehaviour, IResetable
{
    [Tooltip("This is from 0 to width-1 and 0 to height-1")]
    [SerializeField] Vector2 chaserStart;
    [SerializeField] Vector2 playerStart;

    [SerializeField] List<Vector2> validPositions;

    Vector2 chaserGlobalPos;
    Vector2 currentPos;
    Vector2 playerPos;
    HashSet<Vector2> hashValidPositions;
    
    ChaserMovement movement;

    private void Start()
    {
        chaserGlobalPos = transform.position;
        currentPos = chaserStart;
        playerPos = playerStart;
        hashValidPositions = new HashSet<Vector2>(validPositions);

        movement = GetComponent<ChaserMovement>();
    }

    private void OnEnable()
    {
        if (!Application.isPlaying) { return; }

        MovementDetector.instance.Subscribe(ChaserMove);
    }

    private void OnDisable()
    {
        if (!Application.isPlaying) { return; }

        MovementDetector.instance.Unsubscribe(ChaserMove);
    }

    private void ChaserMove(Direction direction)
    {
        UpdateLocalPlayerPos(direction);

        Direction nextMove = GenerateMove();
        
        if (nextMove != Direction.None)
        {
            UpdateLocalRaccoonPos(nextMove);
            movement.MakeMove(nextMove);
        }
    }

    void UpdateLocalRaccoonPos(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                currentPos.x -= 1;
                break;
            case Direction.Right:
                currentPos.x += 1;
                break;
            case Direction.Down:
                currentPos.y -= 1;
                break;
            case Direction.Up:
                currentPos.y += 1;
                break;
        }
    }

    void UpdateLocalPlayerPos(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                playerPos.x -= 1;
                break;
            case Direction.Right:
                playerPos.x += 1;
                break;
            case Direction.Down:
                playerPos.y -= 1;
                break;
            case Direction.Up:
                playerPos.y += 1;
                break;
        }
    }

    Direction GenerateMove()
    {
        Queue<Vector2> queue = new Queue<Vector2>();
        HashSet<Vector2> visited = new HashSet<Vector2>();

        Dictionary<Vector2, Vector2> path = new Dictionary<Vector2, Vector2>();

        queue.Enqueue(currentPos);
        visited.Add(currentPos);

        while(queue.Count > 0)
        {
            Vector2 position = queue.Dequeue();
            if (position == playerPos)
            {
                return BacktrackPath(ref path, currentPos, playerPos);
            }

            //Up
            CheckNeighbour(ref queue, ref visited, ref path, new Vector2(position.x, position.y + 1), position);

            //Down
            CheckNeighbour(ref queue, ref visited, ref path, new Vector2(position.x, position.y - 1), position);

            //Left
            CheckNeighbour(ref queue, ref visited, ref path, new Vector2(position.x - 1, position.y), position);

            //Right
            CheckNeighbour(ref queue, ref visited, ref path, new Vector2(position.x + 1, position.y), position);

        }

        return Direction.None;
    }

    Direction BacktrackPath(ref Dictionary<Vector2, Vector2> path, Vector2 start, Vector2 destination)
    {
        Vector2 prev = destination;
        Vector2 cur = destination;

        while(cur != start)
        {
            prev = cur;
            cur = path[cur];
        }

        if(start.x > prev.x)
        {
            return Direction.Left;
        }
        if (start.y < prev.y)
        {
            return Direction.Up;
        }
        if (start.y > prev.y)
        {
            return Direction.Down;
        }
        if(start.x < prev.x)
        { 
            return Direction.Right; 
        }

        return Direction.None;
    }

    void CheckNeighbour(ref Queue<Vector2> queue, ref HashSet<Vector2> visited, ref Dictionary<Vector2, Vector2> path, Vector2 pos, Vector2 original)
    {
        if (!visited.Contains(pos) && hashValidPositions.Contains(pos))
        {
            {
                visited.Add(pos);
                queue.Enqueue(pos);
                path[pos] = original;
            }
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
        currentPos = chaserStart;
        playerPos = playerStart;
        transform.position = chaserGlobalPos;
    }
}