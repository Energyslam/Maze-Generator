using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IntVector2
{
    //creates a Vector2 using int instead of float

    public int x, z;

    public IntVector2(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b) //allows IntVector2's to be added to each other
    {
        a.x += b.x;
        a.z += b.z;
        return a;
    }
}
