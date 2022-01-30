using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public List<GameObject> tilePrefabs = new List<GameObject>();
    
    // BOARD DATA
    public int startBoardSizeX;
    public int startBoardSizeZ;
    public Vector3Int cloudPosition;
    public Vector3Int stonePosition;
    public Game.WindDir windDirection;
    
    private Dictionary<Vector3Int, Square> board = new Dictionary<Vector3Int, Square>();
    private Dictionary<Vector3Int, GameObject> rocks = new Dictionary<Vector3Int, GameObject>();
    private bool isDirty;

    void Start()
    {
        ProcessBoard();
        RefreshBoard();
    }

    private void Update()
    {
        if (isDirty)
        {
            RefreshBoard();
            isDirty = false;
        }
    }

    // Process board data using Rules:
    //  - Cloud generates River
    void ProcessBoard()
    {
        // 0. dictionary
        int xHalf = (int) Math.Round(Decimal.ToDouble(startBoardSizeX) / 2);
        int zHalf = (int) Math.Round(Decimal.ToDouble(startBoardSizeZ) / 2);
        
        for (int x = 0 - xHalf; x < 0 - xHalf + startBoardSizeX; x++)
        {
            for (int z = 0 - zHalf; z < 0 - zHalf + startBoardSizeZ; z++)
            {
                Vector3Int loc = new Vector3Int(x, 0, z);
                if (!board.ContainsKey(loc))
                    board[loc] = new Square();
            }
        }
        
        // 1. fill with ground tiles
        foreach (var loc in board.Keys)
        {
            board[loc].AppendTile(Game.Tile.GROUND);
        }
        
        // 2. squares with cloud tile, stone tile
        if (!board.ContainsKey(cloudPosition))
        {
            // Debug.LogError("Invalid cloud location: outside of board");
        }
        board[cloudPosition].AppendTile(Game.Tile.CLOUD);
        board[stonePosition].AppendTile(Game.Tile.STONE);
        
        // 3. squares with water
        Game.Tile riverTile;
        if (windDirection == Game.WindDir.X)
        {
            for (int x = 0 - xHalf; x <  0 - xHalf + startBoardSizeX; x++)
            {
                Vector3Int r = new Vector3Int(x, cloudPosition.y, cloudPosition.z);
                getRiverTile(r, windDirection, out riverTile);
                board[r].AppendTile(riverTile);

            }
        }
        else
        {
            for (int z = 0 - zHalf; z < 0 - zHalf + startBoardSizeZ; z++)
            {
                Vector3Int r = new Vector3Int(cloudPosition.x, cloudPosition.y, z);
                getRiverTile(r, windDirection, out riverTile);
                board[r].AppendTile(riverTile);
            }
        }
    }

    void RefreshBoard()
    {
        rocks.Clear();
        foreach (Vector3Int loc in board.Keys)
        {
            Vector3 locc = loc;
            foreach (var tile in board[loc].GetTiles())
            {
                if (tile == Game.Tile.STONE)
                {
                    rocks[loc] = Instantiate(tilePrefabs[(int)tile], locc, Quaternion.identity);
                }
                else
                {
                    Instantiate(tilePrefabs[(int)tile], locc, Quaternion.identity);
                }
                
            }
        }
    }

    void PrintBoard()
    {
        foreach (var loc in board.Keys)
        {
            foreach (var t in board[loc].GetTiles())
            {
                Debug.Log(loc + " tile: " + t);
            }
        }
    }

    public Square GetSquareInternal(Vector3Int squareLocation)
    {
        if (board.ContainsKey(squareLocation))
        {
            return board[squareLocation];

        }
        else
        {
            return null;
        }
    }

    public void MoveTileFromToInternal(Game.Tile tile, Vector3Int from, Vector3Int to)
    {
        board[from].GetTiles().Remove(tile);
        if (tile == Game.Tile.STONE) Destroy(rocks[from]);
        board[to].GetTiles().Add(tile);
        isDirty = true;
    }

    private void getRiverTile(Vector3Int square, Game.WindDir wind, out Game.Tile river)
    {
        int xHalf = (int) Math.Round(Decimal.ToDouble(startBoardSizeX) / 2);
        int zHalf = (int) Math.Round(Decimal.ToDouble(startBoardSizeZ) / 2);

        if (wind == Game.WindDir.X)
        {
            if (square.x == 0 - xHalf)
            {
                river = Game.Tile.RIVER_EG_NX;
            }
            else if (square.x == 0 - xHalf + startBoardSizeX - 1)
            {
                river = Game.Tile.RIVER_EG_PX;
            }
            else
            {
                river = Game.Tile.RIVER_ST_NX;
            }
        }
        else
        {
            if (square.z == 0 - zHalf)
            {
                river = Game.Tile.RIVER_EG_NZ;
            }
            else if (square.z == 0 - zHalf + startBoardSizeZ - 1)
            {
                river = Game.Tile.RIVER_EG_PZ;
            }
            else
            {
                river = Game.Tile.RIVER_ST_NZ;
            }
        }
    }
}

public class Square
{
    private List<Game.Tile> tiles;
    public Square()
    {
        tiles = new List<Game.Tile>();
    }

    public void AppendTile(Game.Tile tile)
    {
        tiles.Add(tile);
    }

    public List<Game.Tile> GetTiles()
    {
        return tiles;
    }
}

