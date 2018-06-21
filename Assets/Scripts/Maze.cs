using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maze : MonoBehaviour
{

    public IntVector2 size;
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeWall wallPrefab;
    public Canvas canvas;

    private MazeCell[,] cells;
    public float generationStepDelay;

    IntVector2 coordinates;

    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z]; //return the MazeCell object at coordinates from cell array.
    }

    MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell; //creates new MazeCell at position in array
        newCell.coordinates = coordinates; //tells new cell what his coordinates are
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z; //assign name in inspector
        newCell.transform.parent = transform; //makes it a child of the maze
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f); //moves the cell to the corresponding position
        return newCell;
    }

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);


        InputField[] widthAndHeight = canvas.GetComponentsInChildren<InputField>(); //retrieves all inputfield from the canvas
        InputField width = widthAndHeight[0]; //assigns the first InputField found
        InputField height = widthAndHeight[1]; //assigns the second InputField found
        float widthSize = size.x; //assign widthSize with current size.x to avoid null error
        if (width.text != "") //if the InputField is not empty
        {
            widthSize = int.Parse(width.text); //parses to text from InputField to an int
        }
        float heightSize = size.z;
        if (height.text != "")
        {
            heightSize = int.Parse(height.text);
        }
        size.x = (int)widthSize; //generated maze is now assigned the width from the InputField
        size.z = (int)heightSize; //generated maze is now assigned the height from the InputField

        Camera camera = Camera.main; //gets Main Camera
        if (widthSize > heightSize) //sets camera orthographic size depending on which value is higher
        {
            camera.GetComponent<Camera>().orthographicSize = (widthSize + heightSize) / 4;
        }
        else
        {
            camera.GetComponent<Camera>().orthographicSize = heightSize / 2;
        }

        cells = new MazeCell[size.x, size.z]; //creates a new 2D array
        List<MazeCell> activeCells = new List<MazeCell>(); //creates empty list for active cells
        DoFirstGenerationStep(activeCells); //creates first cell in the list
        while (activeCells.Count > 0) //as long as cells are not fully initialized, this will loop
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
    }

    public IntVector2 RandomCoordinates //returns random coordinates within boundaries of cells
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 coordinate) //checks if the coordinate is within the boundaries of cells
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }

    void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));  //creates first cell at a random position
    }
    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = returnIndex(activeCells); //returns the indextype for the algorithm

        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.isFullyInitizalized)
        {
            activeCells.RemoveAt(currentIndex); //removes cell from the list once fully initialized
            return;
        }

        MazeDirection direction = currentCell.RandomUnitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.toIntVector2();

        if (ContainsCoordinates(coordinates) && GetCell(coordinates) == null) //if it's inside the limits of the field and target cell is empty
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
        }
        else
        {
            CreateWall(currentCell, null, direction);
        }
    }

    void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction) //creates empty passage between two cells and initializes it for both sides
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage; 
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null) //only creates a wall if there is another cell in that direction
        {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }
    
    int returnIndex(List<MazeCell> activeCells)
    {
        int i = canvas.GetComponentInChildren<Dropdown>().value; //gets current value in the dropdown menu
        if (i == (int)dropdownIndex.Last){
            return activeCells.Count - 1;
        }
        if (i == (int)dropdownIndex.Middle)
        {
            return 0;
        }
        if (i == (int)dropdownIndex.First)
        {
            return activeCells.Count / 2;
        }
        if (i == (int)dropdownIndex.Random)
        {
            return Random.Range(0, activeCells.Count);
        }
        return activeCells.Count - 1;
    }

    public enum dropdownIndex //creates an enum to keep track of which value goes with which index type
    {
        Last,
        Middle,
        First,
        Random
    }
}