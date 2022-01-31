using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    // TILES DATA
    public List<GameObject> tilePrefabs = new List<GameObject>();
    public int groundTileCount;

    // BOARD DATA
    [Header("LEVEL")]
    public int startBoardSizeX;
    public int startBoardSizeZ;
    public Vector3Int cloudPosition;
    public Vector3Int winPosition;
    public Vector3Int stonePosition;
    public List<Vector3Int> stonePositions;
    public List<Vector3Int> riverPositions;
    public GameObject levelPrefab;
    
    public Game.WindDir windDirection;
    
    private Dictionary<Vector3Int, Square> board = new Dictionary<Vector3Int, Square>();
    private Dictionary<Vector3Int, GameObject> groundTiles = new Dictionary<Vector3Int, GameObject>();
    private GameObject currentLevelPrefab;
    void Start()
    {
        // ProcessBoard();
        // RefreshBoard();

        ProcessBoardPrefab();
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
            int groundID = Random.Range(1, groundTileCount + 1);
            switch (groundID)
            {
                case 1:
                    board[loc].AppendTile(Game.Tile.GROUND_1);
                    break;
                case 2:
                    board[loc].AppendTile(Game.Tile.GROUND_2);
                    break;
                case 3:
                    board[loc].AppendTile(Game.Tile.GROUND_3);
                    break;
                case 4:
                    board[loc].AppendTile(Game.Tile.GROUND_4);
                    break;
            }
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
        groundTiles.Clear();
        
        foreach (Vector3Int loc in board.Keys)
        {
            Vector3 locc = loc;
            foreach (var tile in board[loc].GetTiles())
            {
                if (tile.name == Game.Tile.STONE)
                {
                    Instantiate(tilePrefabs[(int)tile.name], locc, Quaternion.identity);
                }
                else if (tile.name.ToString().Split('_')[0] == "GROUND")
                {
                    int r = Random.Range(0, 100);
                    if (r % 2 == 0)
                    {
                        groundTiles.Add(loc, Instantiate(tilePrefabs[(int) tile.name], locc,
                            Quaternion.Euler(0, r % 3 * 90, 180)));
                    }
                    else if (r % 2 == 1)
                    {
                        groundTiles.Add(loc, Instantiate(tilePrefabs[(int)tile.name], locc, Quaternion.Euler(0, r % 3 * 90, 0)));
                    }
                }
                else
                {
                    Instantiate(tilePrefabs[(int)tile.name], locc, Quaternion.identity);
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
                Debug.Log(loc + " tile: " + t.name);
            }
        }
    }
    
    // todo: just to populate the data structure for now
    public void ProcessBoardPrefab()
    {
        if (currentLevelPrefab != null) Destroy(currentLevelPrefab);
        groundTiles.Clear();
        
        // init prefab
        currentLevelPrefab = Instantiate(levelPrefab);
        
        // 0. dictionary
        int xHalf = (int) Math.Round(Decimal.ToDouble(startBoardSizeX) / 2);
        int zHalf = (int) Math.Round(Decimal.ToDouble(startBoardSizeZ) / 2);
        
        for (int x = -3; x <= 2; x++)
        {
            for (int z = -2; z <= 3; z++)
            {
                Vector3Int loc = new Vector3Int(x, 0, z);
                if (!board.ContainsKey(loc))
                    board[loc] = new Square();
            }
        }
        
        // fill with ground tiles
        foreach (var loc in board.Keys)
        {
            int groundID = Random.Range(1, groundTileCount + 1);
            switch (groundID)
            {
                case 1:
                    board[loc].AppendTile(Game.Tile.GROUND_1);
                    break;
                case 2:
                    board[loc].AppendTile(Game.Tile.GROUND_2);
                    break;
                case 3:
                    board[loc].AppendTile(Game.Tile.GROUND_3);
                    break;
                case 4:
                    board[loc].AppendTile(Game.Tile.GROUND_4);
                    break;
            }
        }
        // add cloud, river, stone
        board[cloudPosition].AppendTile(Game.Tile.CLOUD);
        foreach (var stone in stonePositions)
        {
            board[stone].AppendTile(Game.Tile.STONE);
        }

        foreach (var river in riverPositions)
        {
            board[river].AppendTile(Game.Tile.RIVER_EG_NX);
        }
        
        // add win tile walnut
        board[winPosition].AppendTile(Game.Tile.WIN);
        
        // construct ground tiles dict for flipping
        foreach (Transform child in GameObject.FindGameObjectWithTag("Ground").transform)
        {
            groundTiles.Add(Vector3Int.FloorToInt(child.position), child.gameObject);
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
    
    // returns true if we flipped a win tile
    public IEnumerator FlipGroundTileInternal(Vector3Int loc)
    {
        bool isWin = false;
        Tile gt = null;
        foreach (var t in board[loc].GetTiles())
        {
            if (t.name.ToString().Split("_")[0] == "GROUND")
            {
                gt = t;
                gt.Flip();
            }

            if (t.name == Game.Tile.WIN)
            {
                isWin = true;
                StartCoroutine(FlipSquareCoroutine(groundTiles[loc], new Vector3(0, 180, 0)));
                
            }
        }

        if (!isWin)
        {
            int r = Random.Range(10, 30);
            if (r % 2 == 0)
            {
                StartCoroutine(FlipSquareCoroutine(groundTiles[loc], new Vector3(0, 0, r)));
            }
            else if  (r % 2 == 1)
            {
                StartCoroutine(FlipSquareCoroutine(groundTiles[loc], new Vector3(r, 0, 0)));
            }
        }
        
        yield break;
    }

    private IEnumerator FlipSquareCoroutine(GameObject square, Vector3 endValue)
    {
        float time = 0f;
        while (time < 0.75f)
        {
            square.transform.eulerAngles = Game.EaseOut(time / 0.75f) * endValue; 
            time += Time.deltaTime;
            yield return null;
        }

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
    private List<Tile> tiles;
    public Square()
    {
        tiles = new List<Tile>();
    }

    public void AppendTile(Game.Tile tile)
    {
        tiles.Add(new Tile(tile));
    }

    public List<Tile> GetTiles()
    {
        return tiles;
    }
}

public class Tile
{
    public readonly Game.Tile name;
    public bool isFlipped { get; private set; }

    public Tile(Game.Tile tileName)
    {
        name = tileName;
    }
    
    // returns true if it is the win tile
    public bool Flip()
    {
        isFlipped = true;  // ground tiles can only be flipped once, not two way design
        return name == Game.Tile.WIN;
    }
}

