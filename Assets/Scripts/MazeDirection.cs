using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeDirection //creates an enum to translate int to a direction
{

    North,
    East,
    South,
    West

}

public static class MazeDirections
{
    public const int Count = 4; // four possible directions to move in

    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count); //return random direction 
        }
    }

    private static MazeDirection[] opposites = //opposite of MazeDirection
    {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    public static MazeDirection GetOpposite(this MazeDirection direction) //gives opposite direction of the MazeDirection given
    {
        return opposites[(int)direction];
    }

    private static IntVector2[] vectors = //MazeDirections in IntVector2's
    {
        new IntVector2(0,1), //North
        new IntVector2(1,0), //East
        new IntVector2(0,-1), //South
        new IntVector2(-1,0) //West
    };

    public static IntVector2 toIntVector2(this MazeDirection direction) //returns the IntVector associated with given direction
    {
        return vectors[(int)direction];
    }

    private static Quaternion[] rotations = {
        Quaternion.identity, //North
        Quaternion.Euler(0f, 90f, 0f), //East
        Quaternion.Euler(0f, 180f, 0f), //South
        Quaternion.Euler(0f, 270f, 0f) //West
    };

    public static Quaternion ToRotation(this MazeDirection direction)
    {
        return rotations[(int)direction]; //returns the IntVector associated with given direction
    }

}
