using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //Kurtis Watson
    private Animator m_animator;
    public Transform m_camera;
    public Rigidbody m_rb;

    public Transform m_shotPoint;

    private Ability_Melee r_abilityMelee;
    private Ability_Wall r_abilityWall;

    public float m_camRotSpeed;
    public float m_camMinY;
    public float m_camMaxY;
    public float m_camSmoothSpeed;

    public float m_walkSpeed;
    public float m_sprintSpeed; 
    public float m_maxSpeed;
    public float m_jumpHeight;

    public float m_playerHealth;

    public float m_extraGravity;

    public float m_playerRotX;
    float m_camRotY;
    Vector3 m_directionIntentX;
    Vector3 m_directionIntentY;
    float m_speed; 

    public bool m_grounded;
    public bool m_isSprinting;
    public bool m_isCrouching;

    public Object m_pushBack;
    public Rigidbody m_grenade;

    public bool m_isPlayerActive;

    private void Start()
    {
        m_isPlayerActive = true;

        m_animator = GetComponent<Animator>();
        r_abilityMelee = GameObject.FindObjectOfType<Ability_Melee>();
        r_abilityWall = GameObject.FindObjectOfType<Ability_Wall>();
    }

    // Update is called once per frame
    //Kurtis Watson
    void Update()
    {
        f_drone();

        if (m_isPlayerActive == true)
        {
            f_lookAround();
            f_moveAround();
            f_strongerGravity();
            f_groundCheck();
            if (m_grounded == true && Input.GetButtonDown("Jump"))
            {
                f_playerJump();
            }
            f_ability();
            gameObject.GetComponentInChildren<Camera>().enabled = true;
        }
        else gameObject.GetComponentInChildren<Camera>().enabled = false;
    }

    //Kurtis Watson
    void f_lookAround()
    {
        Cursor.visible = false; //Remove cursor from the screen.
        Cursor.lockState = CursorLockMode.Locked; //Locks the cursor to the screen to prevent leaving the window.

        m_playerRotX += Input.GetAxis("Mouse X") * m_camRotSpeed; //Rotates player FPS view along X axis based on mouse movement.
        m_camRotY += Input.GetAxis("Mouse Y") * m_camRotSpeed; //Rotates the camera in Y axis so that the player object doesn't rotate upwards.

        m_camRotY = Mathf.Clamp(m_camRotY, m_camMinY, m_camMaxY); //Limit how far on the Y axis the player can look.

        Quaternion m_camTargetRotation = Quaternion.Euler(-m_camRotY, 0, 0); 
        Quaternion m_targetRotation = Quaternion.Euler(0, m_playerRotX, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, m_targetRotation, Time.deltaTime * m_camSmoothSpeed);

        m_camera.localRotation = Quaternion.Lerp(m_camera.localRotation, m_camTargetRotation, Time.deltaTime * m_camSmoothSpeed);
    }

    //Kurtis Watson
    void f_moveAround()
    {
        m_directionIntentX = m_camera.right;
        m_directionIntentX.y = 0;
        //Normalize makes the numbers more 'usable' for the engine.
        m_directionIntentX.Normalize();

        m_directionIntentY = m_camera.forward;
        m_directionIntentY.y = 0;
        m_directionIntentY.Normalize();

        m_rb.velocity = m_directionIntentY * Input.GetAxis("Vertical") * m_speed + m_directionIntentX * Input.GetAxis("Horizontal") * m_speed + Vector3.up * m_rb.velocity.y;
        m_rb.velocity = Vector3.ClampMagnitude(m_rb.velocity, m_maxSpeed);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_isSprinting = true;
            m_speed = m_sprintSpeed;
        }
        if(!Input.GetKey(KeyCode.LeftShift))
        {
            m_isSprinting = false;
            m_speed = m_walkSpeed;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            m_isCrouching = true;
        }
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            m_isCrouching = false;
        } 
        m_animator.SetBool("Crouch", m_isCrouching);
    }

    //Kurtis Watson
    void f_strongerGravity()
    {
        m_rb.AddForce(Vector3.down * m_extraGravity);
    }

    //Kurtis Watson
    void f_groundCheck()
    {
        RaycastHit m_groundHit;
        m_grounded = Physics.Raycast(transform.position, -transform.up, out m_groundHit, 1.25f); //Automatically set bool value to true if an object is hit; else, returns false.
    }

    //Kurtis Watson
    void f_playerJump()
    {
        m_rb.AddForce(Vector3.up * m_jumpHeight, ForceMode.Impulse);
    }

    //Kurtis Watson
    void f_ability()
    {
        if (Input.GetKeyDown("q"))
        {
            Instantiate(m_pushBack, m_shotPoint.transform.position, m_shotPoint.rotation); // 'm_shotPoint.rotation' makes the position of firing relative to where the player is looking based on camera rotation.
        }

        // Ben Soars
        if (Input.GetKeyDown("g"))
        {
            Rigidbody thrownObject = Instantiate(m_grenade, m_shotPoint.transform.position, m_shotPoint.rotation); // create grenade
            thrownObject.AddForce(m_shotPoint.forward * 100); // push forwards
            thrownObject.AddForce(m_shotPoint.up * 50); // throw slightly upwards
        }

        if (Input.GetKeyDown("e"))
        {
            r_abilityMelee.f_melee();
        }

        if (Input.GetKeyDown("v"))
        {
            r_abilityWall.f_spawnWall();
        }
    }
    
    void f_drone()
    {
        if (Input.GetKeyDown("c"))
        {
            m_isPlayerActive = !m_isPlayerActive;
        }
    }
}
