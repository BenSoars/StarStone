﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Kurtis Watson
public class Player_Controller : MonoBehaviour
{
    [Header("Referenced Scripts")]
    [Space(2)]
    public Portal_Controller portalController;
    private Game_End m_gameEnd;
    private Prototype_Classes m_prototypeClasses;

    [Header("Player Properties")]
    [Space(2)]
    public Transform camera;
    public Rigidbody rb;
    [Tooltip("Set the players max health.")]
    public float playerHealth; //Player health.
    [Tooltip("Set the players walk speed.")]
    public float walkSpeed; //Walk speed.
    [Tooltip("Set the players sprint speed.")]
    public float sprintSpeed; //Run speed.
    [Tooltip("Set the max speed of the player.")]
    public float maxSpeed; //Max run speed.
    [Tooltip("Set how high the player can jump.")]
    public float jumpHeight; //How high the player can jump.
    [Tooltip("Set the level of gravity in the game after player jumps.")]
    public float extraGravity; //How much gravity is applied when the player is in the air.
    [Tooltip("Player rotation along X axis.")]
    public float playerRotX; //Player object rotation.
    [Tooltip("Check for if the player is sprinting.")]
    public bool isSprinting; //True/false is the player sprinting.
    [Tooltip("Check for if the player is crouching.")]
    public bool isCrouching; //True/false is the player crouching.
    [Tooltip("Check to see if the player is able to move.")]
    public bool isPlayerActive; //True/false is the player able to move.
    [Tooltip("Position where the bullets are shot from.")]
    public Transform m_shotPoint; //Bullet shotpoint.
    private Animator m_animator; //Reference animator in game object.
    private float m_speed; //Set the speed.
    [Tooltip("Set the default defence value before abilities.")]
    public float defenceValue = 1;
    public Image transition;
    
    [Header("Camera Rotation Properties")]
    [Space(2)]
    [Tooltip("How fast the camera looks around.")]
    public float camRotSpeed; //How fast the camera looks around. 
    [Tooltip("Set the maximum Y look rotation.")]
    public float camMinY; //How low the camera can look before it stops.
    [Tooltip("Set the maximum X look rotation.")]
    public float camMaxY; //How high the camera can look before it stops.
    [Tooltip("Set how smoother the camera is (the higher the faster the player looks).")]
    public float camSmoothSpeed; //How smooth the camera around.
    private float m_camRotY;
    private Vector3 m_directionIntentX; //Direction intended to look towards on X axis.
    private Vector3 m_directionIntentY; //Direction intended to look towards on Y axis.
    [Tooltip("Enemies killed during current round.")]
    public int enemiesKilled; //Enemies kill sum per round.

    [Header("Ladder Properties")]
    [Space(2)]
    [Tooltip("Check for if the player is on the ground.")]
    public bool grounded; //Check if the player is grounded.
    [Tooltip("Check if the player can move around.")]
    public bool canPlayerMove; //Can player move.
    [Tooltip("Check if the player is looking at a ladder.")]
    public bool isLadder; 
    [Tooltip("Check if the player is on the ladder.")]
    public bool isUsingLadder;
    [Tooltip("Check for if the player is at the top of the ladder.")]
    public bool topOfLadder; //Stopping point for ladder.
    [Tooltip("Set position to climb from on ladder.")]
    public Transform desiredPos; //Teleport location for ladder mechanic.

    [Header("Audio")]
    [Space(2)]
    public Audio_System audio; //Get the audio system component to allow for sounds.
    public AudioSource runSound;

    public List<AudioClip> moveSounds = new List<AudioClip>();
    private bool switched;

    [Header("Other Assets")]
    [Space(2)]
    [Tooltip("Check if the player is invisible or not (used for ability).")]
    public bool isPlayerInvisible;
    public GameObject gameHUDCanvas;
    public GameObject tutorialCanvas;
    //public Rigidbody grenade;
    //public int grenadeAmount = 3;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPlayerActive = true;
        canPlayerMove = true;

        audio = GameObject.FindObjectOfType<Audio_System>(); //Get the audio system.
        m_animator = GetComponent<Animator>();
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();

        runSound.volume = PlayerPrefs.GetFloat("volumeLevel"); //Set the volume of the run sound.
        runSound.enabled = false; //Stop the run sound.
    }

    //Kurtis Watson
    void Update()
    {
        //f_drone();
        f_transition();

        if (playerHealth > 100) { playerHealth = 100; } //Set player health to 100 if max health is exceeded.

        if (isPlayerActive == true) //Check if the player is active.
        {            
            f_climb();
            f_lookAround();
            f_moveAround();
            f_strongerGravity();
            f_groundCheck();

            if (grounded == true && Input.GetButtonDown("Jump"))
            {
                f_playerJump();
            }
        }
    }

    //Kurtis Watson
    void f_transition()
    {
        var tempColor = transition.color;

        if (portalController.transitionActive == false && tempColor.a >= 0)
        {
            tempColor.a -= 0.5f * Time.deltaTime;
            transition.color = tempColor;
        }

        if (portalController.transitionActive == true) //Check if the portal has been activated.
        {
            tempColor.a += 0.5f * Time.deltaTime; //Increase opacity of the transition for smooth scene swtiching.
            transition.color = tempColor;
            Destroy(tutorialCanvas);
            Destroy(gameHUDCanvas);
            if (tempColor.a >= 1 && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game_Scene")) //When at solid colour state, based on the scene, load a new scene.
            {
                SceneManager.LoadScene("Ending_Scene"); //Load ending scene.
                portalController.transitionActive = false; //Disable portal.
            }
            if (tempColor.a >= 1 && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Scene"))
            {
                SceneManager.LoadScene("Main_Menu"); //Redirect to the main menu.
                portalController.transitionActive = false;
            }
        }

        if (tempColor.a <= 0)
        {
            transition.enabled = false;
        }
        else transition.enabled = true;

    }

    //Kurtis Watson
    void f_lookAround()
    {
        playerRotX += Input.GetAxis("Mouse X") * camRotSpeed; //Rotates player FPS view along X axis based on mouse movement.
        m_camRotY += Input.GetAxis("Mouse Y") * camRotSpeed; //Rotates the camera in Y axis so that the player object doesn't rotate upwards.

        m_camRotY = Mathf.Clamp(m_camRotY, camMinY, camMaxY); //Limit how far on the Y axis the player can look.

        Quaternion m_camTargetRotation = Quaternion.Euler(-m_camRotY, 0, 0); 
        Quaternion m_targetRotation = Quaternion.Euler(0, playerRotX, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, m_targetRotation, Time.deltaTime * camSmoothSpeed);

        camera.localRotation = Quaternion.Lerp(camera.localRotation, m_camTargetRotation, Time.deltaTime * camSmoothSpeed);
    }

    //Kurtis Watson
    void f_moveAround()
    {
        m_directionIntentX = camera.right;
        m_directionIntentX.y = 0;
        
        m_directionIntentX.Normalize(); //Normalize makes the numbers more 'usable' for the engine.

        m_directionIntentY = camera.forward;
        m_directionIntentY.y = 0;
        m_directionIntentY.Normalize();

        if (canPlayerMove == true)
        {
            rb.velocity = m_directionIntentY * Input.GetAxis("Vertical") * m_speed + m_directionIntentX * Input.GetAxis("Horizontal") * m_speed + Vector3.up * rb.velocity.y;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey("w")) //Make sure the player is moving forward to sprint.

            {
                SwitchRunSounds();
                runSound.clip = moveSounds[1];
                isSprinting = true;              
                m_speed = sprintSpeed; //Set sprint speed.
            }
            else
            {
                SwitchRunSounds();
                runSound.clip = moveSounds[0];
                isSprinting = false;               
                m_speed = walkSpeed; //Reset speed.
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                isCrouching = true; //Set true so the animator can run correctly.
            }
            else
            {
                isCrouching = false;
            }

            m_animator.SetBool("Crouch", isCrouching);

            if (rb.velocity.magnitude > 0.1f && grounded == true)
            {
              
                runSound.enabled = true; // Start run sound
            } else
            {
                runSound.enabled = false; // End run sound
            }
        }
    }

    //Kurtis Watson
    void f_strongerGravity()
    {
        if (canPlayerMove == true)
        {
            rb.AddForce(Vector3.down * extraGravity);
        }
    }

    //Kurtis Watson
    void f_groundCheck()
    {
        RaycastHit m_groundHit;
        grounded = Physics.Raycast(transform.position, -transform.up, out m_groundHit, 1.25f); //Automatically set bool value to true if an object is hit; else, returns false.
    }

    //Kurtis Watson
    void f_playerJump()
    {
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse); //Adds a force to the player object in the upwards direction.
    }

    //Kurtis Watson
    #region Drone
    //void f_drone()
    //{
    //    if (Input.GetKeyDown("c"))
    //    {
    //        isPlayerActive = !isPlayerActive; //Switch to either the drone or player based on current bool value.
    //    }
    //}
    #endregion

    //Kurtis Watson
    void f_climb()
    {
        RaycastHit m_ladderHit;

        if (Input.GetKeyDown("f") && isUsingLadder == true)
        {
            rb.useGravity = true; //Fall off the ladder.
            isUsingLadder = false;
            canPlayerMove = true; //Allow the player to move again in all directions.
        }
        
        else if (Physics.Raycast(camera.transform.position, camera.transform.forward, out m_ladderHit, 4f, 1<<10)) //Shoots a raycast forward of the players position at a distance of '2f'.
        {
            if (m_ladderHit.collider != null && m_ladderHit.collider.gameObject.layer == 10) //Check for the Ladder Layer.
            {
                if (Input.GetKeyDown("f") && isUsingLadder == false)
                {
                    isUsingLadder = true;
                    rb.useGravity = false; //Remove gravity to stop the player being pulled down when using the ladder.
                    canPlayerMove = false; //Stop left and right movement.
                    rb.velocity = Vector3.zero;

                    if (topOfLadder == true)
                    {
                        desiredPos = m_ladderHit.collider.gameObject.transform.Find("Climb Point Top");
                        this.transform.position = desiredPos.transform.position; //Set player central to the ladder for smoother animations.
                    }
                    if(topOfLadder == false)
                    {
                        desiredPos = m_ladderHit.collider.gameObject.transform.Find("Climb Point Bottom");
                        this.transform.position = desiredPos.transform.position;
                    }

                }   
            }
        }

        float m_upwardsSpeed = Input.GetAxis("Vertical") / 20;
        if (isUsingLadder == true)
        {
            transform.Translate(0, m_upwardsSpeed, 0); //Move player in the Y axis (climbing ladder) at a desired speed.
        }
    }

    //Kurtis Watson
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Top Stop Point") //Take the player off the ladder.
        {
            rb.useGravity = true;
            isUsingLadder = false;
            canPlayerMove = true;
        }
        if (other.gameObject.name == "Top of Ladder")
        {
            topOfLadder = true;
        }      
    }

    //Kurtis Watson
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Top of Ladder")
        {
            topOfLadder = false;
        }
    }

    private void SwitchRunSounds()
    {
        if (switched == isSprinting)
        {
            switched = !isSprinting;
            runSound.Play();
        }
    }

    //private void f_grenade()
    //{
    //    // Ben Soars
    //    if (Input.GetKeyDown("g") && grenadeAmount > 0)
    //    {
    //        Rigidbody thrownObject = Instantiate(grenade, m_shotPoint.transform.position, m_shotPoint.rotation); // create grenade
    //        thrownObject.AddForce(m_shotPoint.forward * 100); // push forwards
    //        thrownObject.AddForce(m_shotPoint.up * 50); // throw slightly upwards#
    //        grenadeAmount += 1;
    //    }

    //    Kurtis Watson
    //    if (Input.GetKeyDown("e"))
    //    {
    //        r_abilityMelee.f_melee();
    //    }
    //}
}
