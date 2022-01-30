using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Main : MonoBehaviour
{
    public Vector3 testAngle;
    [SerializeField] private PlayerController playerA; 
    [SerializeField] private PlayerController playerB; 
    [SerializeField] private Board board; 
    [SerializeField] private Transform viewTransform;
    [SerializeField] private bool isFlip;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float flipSpeed;
    
    private Game.Side boardSide;
    private bool isRotating = false;
    
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

        if (isRotating)
        {
            isRotating = false;
            return;
        }
        
        if (flip == 1f)
        {
            if (boardSide == Game.Side.ALPHA)
            {
                StartCoroutine(RotateBoard(new Vector3(90f, 0f, 0f), isFlip));
                SetBoardSide(Game.Side.BETA);
            }
            else
            {
                StartCoroutine(RotateBoard(new Vector3(270f, 0f, 0f), isFlip));
                SetBoardSide(Game.Side.ALPHA);
            }
        }
    }
    
    // COROUTINES
    private IEnumerator RotateBoard(Vector3 endValue, bool isFlip)
    {
        isRotating = true;
        
        float time = 0;
        Vector3 startEulerAngles = viewTransform.eulerAngles;
        Vector3 deltaEulerAngles = Vector3.zero;
        
        // rotate continuously
        if (!isFlip)  
        {
            while (isRotating)
            {
                deltaEulerAngles += testAngle * Time.deltaTime * rotationSpeed;
                time += Time.deltaTime;
                viewTransform.eulerAngles = startEulerAngles + deltaEulerAngles;
                yield return null;
            }
        }
        else
        {
            float p = 0f;
            float s = flipSpeed;
            
            // rotate for a certain duration
            while (time < 0.5f)  
            {
                p = time / 0.5f * Mathf.PI / 2;
                s = flipSpeed * Mathf.Sin((Mathf.PI / 2 + p));  // flip using sin wave
                Debug.Log("s " + s + " p " + p);
                /*speed = Mathf.Lerp(speed, 1 ,time / 0.5f);
                speed *= 0.99f;*/
                deltaEulerAngles += new Vector3(1,0,0) * Time.deltaTime * s ;  //  flip across x, vertical in view
                viewTransform.eulerAngles = startEulerAngles + deltaEulerAngles;
                time += Time.deltaTime;
                yield return null;
            }
            
            // complete rotation
            while (deltaEulerAngles.x < endValue.x)
            {
                deltaEulerAngles += new Vector3(1, 0, 0) * Time.deltaTime * s;
                viewTransform.eulerAngles = startEulerAngles + deltaEulerAngles;
                time += Time.deltaTime;
                yield return null;
            }
            viewTransform.eulerAngles = endValue;
            isRotating = false;
        }

        
    }
    
}
