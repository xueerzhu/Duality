using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour
{
    [SerializeField] private PlayerController playerA; 
    [SerializeField] private PlayerController playerB; 
    [SerializeField] private Board board; 
    [SerializeField] private Transform viewTransform; 
    [SerializeField] private float flipDuration;
    
    private Game.Side boardSide;
    
    void Start()
    {
        // init board
        SetBoardSide(Game.Side.ALPHA);
        
        // init players
        InitPlayerControllers();
    }

    private void InitPlayerControllers()
    {
        playerA.InitPlayer(Game.Side.ALPHA, this);
        playerB.InitPlayer(Game.Side.BETA, this);
    }

    public void SetBoardSide(Game.Side side)
    {
        boardSide = side;
    }
    
    // BOARD
    public Square GetSquare(Vector3Int squareLocation)
    {
        return board.GetSquareInternal(squareLocation);
    }

    public void MoveTileFromTo(Game.Tile tile, Vector3Int from, Vector3Int to)
    {
        board.MoveTileFromToInternal(tile, from, to);
    }

    // INPUTS
    private void OnMove(InputValue movementValue)
    {
        Vector2 vectorA = movementValue.Get<Vector2>();
        Vector2 vectorB = new Vector2(vectorA.x, -vectorA.y);
        
        if (boardSide == Game.Side.ALPHA)
        {
            playerA.Move(vectorA);
        }
        else
        {
            playerB.Move(vectorB);
        }
    }

    private void OnFlipBoard(InputValue flipValue)
    {
        float flip = flipValue.Get<float>();

        if (flip == 1f)
        {
            if (boardSide == Game.Side.ALPHA)
            {
                StartCoroutine(RotateBoard(Quaternion.Euler(180f, 0f, 0f), flipDuration));
                SetBoardSide(Game.Side.BETA);
            }
            else
            {
                StartCoroutine(RotateBoard(Quaternion.Euler(0f, 0f, 0f), flipDuration));
                SetBoardSide(Game.Side.ALPHA);
            }
        }
    }
    
    // COROUTINES
    private IEnumerator RotateBoard(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = viewTransform.rotation;
        
        while (time < duration)
        {
            viewTransform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        viewTransform.rotation = endValue;
    }
}
