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
    
    // EDITOR DEPENDENCY KEEP THIS UPDATED
    // NAMING: SPLIT BY "_" BEFORE THAT IS TILE TYPE, AFTER THAT IS MODIFIERS
    public enum Tile
    {
        GROUND_1,
        GROUND_2,
        GROUND_3,
        GROUND_4,
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
    
    
    // ANIMATION
    public static float EaseOut(float t)
    {
        return Flip(Flip(t) * Flip(t) * Flip(t) * Flip(t));
    }
    
    public static float EaseIn(float t)
    {
        return Flip(t) * Flip(t) * Flip(t) * Flip(t);
    }
    
    public static float EaseInOut(float t)
    {
        return Mathf.Lerp(EaseIn(t), EaseOut(t), t);
    }

    static float Flip(float x)
    {
        return 1 - x;
    }

}
