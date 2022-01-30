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

        // move
        if (CanMoveToSquare(pos)) playerTransform.position += movement;
        yield break;
    }
    
    private bool CanMoveToSquare(Vector3Int squarePosition)
    {
        Square square = main.GetSquare(squarePosition);

        if (square == null) return false;

        bool canMoveToSquare = true;
        foreach (var tile in square.GetTiles())
        {
            canMoveToSquare = canMoveToSquare && CanMoveToTile(tile);
        }

        return canMoveToSquare;
    }
    
    // return true if player can move there
    private bool CanMoveToTile(Game.Tile tile)
    {
        string tileType = tile.ToString().Split("_")[0];
        if (tileType == "RIVER" || tileType == "STONE")
        {
            return false;
        }
        return true;
    }
}
