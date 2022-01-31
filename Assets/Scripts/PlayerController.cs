using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float animationSpeed;

    private Transform playerTransform;
    private Game.Side playerSide;
    private Main main;
    
    private float movementX;
    private float movementZ;
    
    private Animator _animator;
    private bool isMoving;
    public bool anim_isLifted;
    
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _animator.speed = animationSpeed;
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
        Vector3Int newPos = Vector3Int.FloorToInt(playerTransform.position + movement);
        if (CanMoveToSquare(newPos) && !isMoving)
        {
            isMoving = true;
            // rotate player
            Vector3 newAngle = Vector3.forward;
            if (movement.x == 1)
            {
                newAngle = new Vector3(0, 90, 0);
            }
            
            if (movement.x == -1)
            {
                newAngle = new Vector3(0, 270, 0);
            }
            
            if (movement.z == 1)
            {
                newAngle = new Vector3(0, 360, 0);
            }
            
            if (movement.z == -1)
            {
                newAngle = new Vector3(0, 180, 0);
            }
            playerTransform.eulerAngles = newAngle;
            
            // set animator
            if (!anim_isLifted)
            {
                _animator.SetTrigger("TriggerLift");
                anim_isLifted = true;
            }
            float time = 0;
            Vector3 startPos = playerTransform.position;
            
            // flip current tile
            main.FlipGroundTile(Vector3Int.FloorToInt(startPos));
            
            // lerp position
            while (time < 0.75f)
            {
               
                playerTransform.position = Game.EaseOut(time / 0.75f) * movement + startPos;
                time += Time.deltaTime;
                // playerTransform.position += movement;
                yield return null;
            }

            if (anim_isLifted)
            {
                _animator.SetTrigger("TriggerLower");
                anim_isLifted = false;
            }
            isMoving = false;
        }
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
    
    private bool CanMoveToTile(Tile tile)
    {
        string tileType = tile.name.ToString().Split("_")[0];
        if (tileType == "RIVER" || tileType == "STONE")
        {
            return false;
        }
        
        if(tileType == "GROUND" && tile.isFlipped)
        {
            return false;
        }
        return true;
    }
}
