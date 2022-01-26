using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Transform playerTransform;
    private Game.Side playerSide;
    private Main main;
    
    private float movementX;
    private float movementZ;
    
    void Start()
    {
        playerTransform = GetComponent<Transform>();
    }

    public void InitPlayer(Game.Side playerGameSide, Main m)
    {
        playerSide = playerGameSide;
        main = m;
    }
    
    public void Move(Vector2 movementVector)
    {
        Vector3 movement;
        
        switch (movementVector)
        {
            case Vector2 v when v == Vector2.up:    // == for floating point inaccuracy, Equals for precision
                movementX = 0f;
                movementZ = 1f;
                movement = new Vector3(movementX, 0f, movementZ);
                StartCoroutine(Move(movement));
                break;
            case Vector2 v when v == Vector2.down:
                movementX = 0f;
                movementZ = -1f;
                movement = new Vector3(movementX, 0f, movementZ);
                StartCoroutine(Move(movement));
                break;
            case Vector2 v when v == Vector2.left:
                movementX = -1f;
                movementZ = 0f;
                movement = new Vector3(movementX, 0f, movementZ);
                StartCoroutine(Move(movement));
                break;
            case Vector2 v when v == Vector2.right:
                movementX = 1f;
                movementZ = 0f;
                movement = new Vector3(movementX, 0f, movementZ);
                StartCoroutine(Move(movement));
                break;
            default:
                movementX = 0f;
                movementZ = 0f;
                break;
        }
    }

    // COROUTINES
    private IEnumerator Move(Vector3 movement)
    {
        // interaction with tiles
        Vector3 newPos = playerTransform.position + movement;
        Vector3Int pos = Vector3Int.FloorToInt(newPos);
        bool canMove = InteractWithSquare(pos);

        // move
        if (canMove) playerTransform.position += movement;
        yield break;
    }

    private bool InteractWithSquare(Vector3Int squarePosition)
    {
        Square square = main.GetSquare(squarePosition);
        
        foreach (var tile in square.GetTiles())
        {
            switch (tile)
            {
                case Game.Tile.RIVER_90:
                    return InteractWithWater();
                    break;
                case Game.Tile.RIVER_EDGE:
                    return InteractWithWater();
                    break;
                case Game.Tile.RIVER_STRAIGHT:
                    return InteractWithWater();
                    break;
                case Game.Tile.STONE:
                    return InteractWithStone(squarePosition);
                    break;
                default:
                    break;
            }
        }

        return true;
    }

    private bool InteractWithWater()
    {
        return false;
    }
    
    private bool InteractWithStone(Vector3Int squarePosition)
    {
        Vector3Int moveVector = squarePosition - Vector3Int.FloorToInt(playerTransform.position);
        // move stone
        main.MoveTileFromTo(Game.Tile.STONE, squarePosition, squarePosition + moveVector);
        return true;
    }
}
