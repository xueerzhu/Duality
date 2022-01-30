using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public enum Side
    {
        ALPHA,
        BETA
    };
    
    // TODO: EDITOR DEPENDENCY KEEP THIS UPDATED
    public enum Tile
    {
        GROUND,
        CLOUD,
        STONE,
        RIVER_ST_NX,
        RIVER_ST_PX,
        RIVER_ST_NZ,
        RIVER_ST_PZ,
        RIVER_EG_NX,
        RIVER_EG_PX,
        RIVER_EG_NZ,
        RIVER_EG_PZ

    };
    
    public enum WindDir
    {
        X,
        Z
    }
    
    
    
}
