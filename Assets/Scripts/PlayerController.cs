//Creator: Kacey Walsh
    //Class: DIG3480 Computer As A Medium with Prof. Kenton Howard
    //University of Central Florida

    //Challenge 1: Roll a Ball Challenge
    //Started: January 30th, 2022
    //Due: February 6th, 2022

    //Criteria:
    //Design:
        //Change the pattern of the pickups
        //Change the color of the walls and floor
    //Coding:
        //Add an enemy red box pickup that reduces one point when collected
        //Add a second level
    //Gameplay:
    //Two obsticles move slowly back and forth on the playfield
    //Allow the player to loose

    //Status: Completed

    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI lifeText;
    public GameObject winTextObject;
    public GameObject looseTextObject;

    //Set instances of a rigidbody (collision), score count, lives, and level;
    //The last 3 are essentially "this is something we're tracking"
    private Rigidbody rb;
    private int count;
    private int lives;
    private int level;
    
    //essentially tracking movement
    private float movementX;
    private float movementY;
    private float movementZ;


    // Start is called before the first frame update
    void Start()
    {
        //Reference point for Debug log
        Debug.Log("Game Started");
        
        //Set main game components for the begining of the game: count (score), lives, and level
        rb = GetComponent<Rigidbody>();
        count = 0;
        lives = 3;
        level = 1;


        //Setting the two 'endings' to be present but not shown
        SetCountText();
        winTextObject.SetActive(false);

        SetLifeText();
        looseTextObject.SetActive(false);

    }

    void Update()
    {
        //Quitting the game
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

            if (Input.GetKeyDown("P"))
        {
            transform.position = new Vector3 (52f,.5f,-15f);
        }
    }

    //Movement
    void OnMove(InputValue movementValue) 
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }


    //If statement for loosing lives/end screen -- deletes the ball
    void SetLifeText()
    {
        lifeText.text ="Lives: " + lives.ToString();
        if(lives<=0)
        {
            looseTextObject.SetActive(true);
            Destroy(gameObject);
        }
    }


    //Teleport function comes before the command so it can be referenced later, otherwise
    //this may be skipped if the first criteria isnt met.
    void Teleport()
    {
        if((level == 1) && (count == 8))
        {
            transform.position = new Vector3 (0f,.5f,60f);
            level = 2;
            Debug.Log("Moving to Next Level");
        }
    }
    
        //make sure you include the double brackets around and statements, that's where
        //you got tripped up, it's similar to math in the way you break down complex
        //math problems into simpler ones, in this you're reading it as "do this and this"
        //but the computer is reading it as "Do this this" which is where the confusion
        //and bugs come in    

    //If statement for winning (((Needs to be changed to account for second level)))
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        //for moving to the next level
        if((count == 8) || (level == 1))
        {  
            Teleport();
        }

        //for winning the game (16 pickup items) + DEBUG to ensure
        if(count >= 16)
        {
            winTextObject.SetActive(true);
            Debug.Log("Congratulations");
        }
    }
    


void FixedUpdate()
{
    //Movement and momentum
    Vector3 movement = new Vector3(movementX, 0.0f, movementY);

    rb.AddForce(movement * speed);
}

     void OnTriggerEnter(Collider other)
    {
        //pickup command "if collide with pickup = +1 count and hide the pickup object"
        if(other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            Debug.Log("Pickup Collected");

            SetCountText();
        }
        //pickup command for enemies, takes a life and destroys the enemies
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives = lives -1;
            Debug.Log("Enemy Collected");

            SetLifeText();
        }
        
    }
    
}



