using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

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
            if (tile == Game.Tile.RIVER_ST_NX || tile == Game.Tile.RIVER_ST_PX
            || tile == Game.Tile.RIVER_ST_NZ || tile == Game.Tile.RIVER_ST_PZ
            || tile == Game.Tile.RIVER_EG_NX || tile == Game.Tile.RIVER_EG_PX
            || tile == Game.Tile.RIVER_EG_NZ || tile == Game.Tile.RIVER_EG_PZ)
            {
                return InteractWithWater();
            }
            
            if (tile == Game.Tile.STONE)
            {
                InteractWithStone(squarePosition);
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
