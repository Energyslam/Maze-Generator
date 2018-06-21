using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    MazeCell currentCell;
    float distance = 0.75f; //movement speed of the player
    float speedHorizontal = 2.0f; //horizontal speed of camera rotation
    float speedVertical = 2.0f; //vertical speed of camera rotation
    float horizontalRotation = 0.0f;
    float verticalRotation = 0.0f;
    public GameObject endGO;
    bool winner;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //locks and hides cursor to the game
    }

    void Update()
    {
        if (!winner)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; //resets players velocity to zero at the start of each frame
        }
        horizontalRotation += speedHorizontal * Input.GetAxis("Mouse X"); //multiplies the rotation speed by horizontal axis of the mouse
        verticalRotation -= speedVertical * Input.GetAxis("Mouse Y"); //multiplies the rotation speed by vertical axis of the mouse
        transform.eulerAngles = new Vector3(0f, horizontalRotation, 0f); //only rotates player over the y axis
        GetComponentInChildren<Camera>().transform.eulerAngles = new Vector3(verticalRotation, transform.eulerAngles.y, 0f); //rotates the camera over the x and y axis


        if (Input.GetKey(KeyCode.LeftShift)) //sprinting
        {
            distance = 1.5f; //sets the movement speed twice as high
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * distance, Space.Self);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * distance, Space.Self);
        }

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * distance, Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * distance, Space.Self);
        }

        distance = 0.75f; //resets movementspeed after sprinting

    }

    public void SetStartingLocation(MazeCell cell)
    {
        currentCell = cell; //takes the random cell
        transform.localPosition = cell.transform.localPosition; //move player to the random cell position
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + transform.localScale.y, transform.localPosition.z); //adds localscale to y position so the playerobject isn't in the ground
    }

    void OnTriggerEnter(Collider other) //gets called once the player enters the endgame objects trigger collider
    {
        StartCoroutine(volumeControl(other)); //starts coroutine for fading current song and playing the end song
        GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f); //player now shoots through space and time with a velocity of 10
        winner = true;
    }

    IEnumerator volumeControl(Collider other)
    {
        yield return new WaitForSeconds(0.1f);
        float volumeF = GetComponent<AudioSource>().volume;
        if (volumeF > 0) //if current volume of song is still audible
        {
            GetComponent<AudioSource>().volume -= 0.03f; //reduces current song volume
            StartCoroutine(volumeControl(other)); //calls itself again if song is still audible
        }

        if (volumeF <= 0)
        {
            other.GetComponent<AudioSource>().Play(); //play song attached to the endgame object
        }

    }
}
