using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{

    public IntVector2 coordinates;

    int initializedEdgeCount;

    public bool isFullyInitizalized
    {
        get
        {
            return initializedEdgeCount == MazeDirections.Count; //returns true if all sides are initialized
        }
    }

    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count]; //array edges the size of count in mazedirections

    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return edges[(int)direction]; //gets the edge of given direction
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        edges[(int)direction] = edge; //checks the direction and takes that edge
        initializedEdgeCount += 1; //adds 1 count to the initialzed edges
    }

    public MazeDirection RandomUnitializedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount); //chooses a random int between 0 and the amount of uninitialized edges to skip
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                if (edges[i] == null) //if the edge is empty
                {
                    if (skips == 0) 
                    {
                        return (MazeDirection)i;
                    }
                    skips -= 1;
                }
            }
            throw new System.InvalidOperationException("MazeCell has no unitialized directions left"); //throws an exception instead of return
        }
    }
}
