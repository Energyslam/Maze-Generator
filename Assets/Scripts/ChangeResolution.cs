using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeResolution : MonoBehaviour
{
    public string perspective = "3D";
    public Button btnRebuild;
    public Button btnChangePersp;
    public Button btnSpawn;
    public Button btnExitMaze;
    public InputField mazeWidth;
    public InputField mazeHeight;
    public Dropdown indexSelect;

    Maze maze;

    public void Start()
    {
        btnChangePersp.onClick.AddListener(delegate { ChangePerspective(); });
    }

    public void rebuildManager()
    {
        btnSpawn.gameObject.SetActive(false);
        GameObject.Find("Game Manager").GetComponent<GameManager>().Rebuild();
    }

    public void ChangePerspective()
    {
        Camera camera = Camera.main; //gets the main camera

        if (perspective == "2D") //if perspective is currently in 2D
        {
            camera.transform.position = new Vector3(0, 12, -7); //sets the positiion of the camera
            Vector3 temp = transform.rotation.eulerAngles;
            temp.x = 58;
            temp.y = 0;
            temp.z = 0;
            camera.transform.rotation = Quaternion.Euler(temp); //sets the rotation of the camera
            perspective = "3D"; //sets the camera to 3D
        }

        else //if perspective is not in 2D
        {
            camera.transform.position = new Vector3(0, 190, 0);
            Vector3 temp = transform.rotation.eulerAngles;
            temp.x = 90;
            temp.y = 0;
            temp.z = 0;
            camera.transform.rotation = Quaternion.Euler(temp);
            perspective = "2D";
        }
    }

    public void SpawnPlayer()
    {
        GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); //finds the gamemanager again with updated values
        if (gameManager.mazeFinished) //checks if the maze is done building
        {
            gameManager.SpawnPlayer(); //calls the spawnplayer function in the gamemanager

            //activates/deactivates buttons
            mazeWidth.gameObject.SetActive(false);
            mazeHeight.gameObject.SetActive(false);
            btnRebuild.gameObject.SetActive(false);
            btnChangePersp.gameObject.SetActive(false);
            btnSpawn.gameObject.SetActive(false);
            indexSelect.gameObject.SetActive(false);
            btnExitMaze.gameObject.SetActive(true);
        }
    }

    public void EnableSpawn() //activates the enter maze button
    {
        btnSpawn.gameObject.SetActive(true);
    }

    public void ExitMaze()
    {
        GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); //finds the gamemanager again with updated values

        //activates/deactivates buttons
        mazeWidth.gameObject.SetActive(true);
        mazeHeight.gameObject.SetActive(true);
        btnRebuild.gameObject.SetActive(true);
        btnChangePersp.gameObject.SetActive(true);
        btnSpawn.gameObject.SetActive(true);
        indexSelect.gameObject.SetActive(true);
        btnExitMaze.gameObject.SetActive(false);
        gameManager.DestroyPlayer();
    }
}











































/*
 *     void setButton(Button button, int yPosition)
    {
        //button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / heightDivision);
        //button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / widthDivision);

        button.GetComponent<RectTransform>().rect.Set(100, 0, 50, 0);
       // button.transform.position = new Vector3(0 + button.GetComponent<RectTransform>().rect.width / 2, screenHeight * yPosition - button.GetComponent<RectTransform>().rect.height /2, 0);
    }

    void setInputfield(InputField field, int yPosition)
    {
        field.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / heightDivision);
        field.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / widthDivision);
        field.transform.position = new Vector3(0 + field.GetComponent<RectTransform>().rect.width / 2, screenHeight * yPosition - field.GetComponent<RectTransform>().rect.height / 2, 0);
    }



    
public void ChangeTo600x480()
    {
        Screen.SetResolution(1920, 1080, false);
        Debug.Log("Changed to 600x480");

    }

public void ChangeToRandom()
    {
        Screen.SetResolution(Random.Range(300, 1920), Random.Range(300, 1080), false);
    }
*/
