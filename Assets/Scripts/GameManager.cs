using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Maze mazePrefab;
    private Maze mazeInstance;
    public Player playerPrefab;
    public Player playerInstance;

    public GameObject startGO;
    public GameObject startGOInstance;
    public GameObject startGOInstanceOpposite;
    public GameObject endGO;
    public GameObject EndGOInstance;

    private Canvas canvasInstance;
    public Canvas canvasPrefab;

    public bool mazeFinished;
    public Camera camera;

    void Start()
    {
        canvasInstance = Instantiate(canvasPrefab) as Canvas;
        StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        mazeFinished = false;
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.transform.parent = this.transform; //assigns the GameManager as parent of the Maze
        mazeInstance.canvas = canvasInstance; //assigns instantiated canvas to the canvas in Maze
        yield return StartCoroutine(mazeInstance.Generate()); //generates the Maze and waits for it to be finished
        mazeFinished = true;
        canvasInstance.GetComponent<ChangeResolution>().EnableSpawn();
    }

    public void Rebuild()
    {
        StopAllCoroutines(); //stops the mazeinstance generate coroutine
        Destroy(mazeInstance.gameObject); //destroys existing maze
        StartCoroutine(Begin()); //create new maze
    }

    public void SpawnPlayer()
    {
        playerInstance = Instantiate(playerPrefab) as Player; //instantiates the player
        MazeCell startingLocation = mazeInstance.GetCell(mazeInstance.RandomCoordinates); //takes a random location from the completed maze
        playerInstance.SetStartingLocation(startingLocation); //uses the random cell to allocate its position to the player
        startGOInstance = Instantiate(startGO) as GameObject; //instantiates the block indicating starting position
        startGOInstance.transform.parent = transform; //uses the gamemanager as parent in the inspector
        startGOInstance.transform.position = startingLocation.transform.position; //uses the same position as the playerprefab

        startGOInstanceOpposite = Instantiate(startGO) as GameObject;
        startGOInstanceOpposite.transform.parent = transform;
        startGOInstanceOpposite.transform.position = startingLocation.transform.position;
        startGOInstanceOpposite.transform.rotation = Quaternion.Euler(0f, 180f, 0f); //rotates the second starting indicator so its easier for the player to see where he started

        EndGOInstance = Instantiate(endGO) as GameObject;
        MazeCell endLocation = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
        EndGOInstance.transform.parent = transform;
        EndGOInstance.transform.position = endLocation.transform.position;
        camera.gameObject.SetActive(false); //turns off the main camera
    }

    public void DestroyPlayer() //destroys objects needed to play in the maze
    {
        Destroy(playerInstance.gameObject);
        Destroy(startGOInstance);
        Destroy(startGOInstanceOpposite);
        Destroy(EndGOInstance);
        camera.gameObject.SetActive(true); //sets the main camera active again after the players camera is destroyed
    }
}
