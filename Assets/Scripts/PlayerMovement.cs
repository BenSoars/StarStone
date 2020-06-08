using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform m_camera;
    public Rigidbody m_rb;

    public float m_camRotSpeed;
    public float m_camMinY;
    public float m_camMaxY;
    public float m_camSmoothSpeed;

    public float m_walkSpeed;
    public float m_sprintSpeed; 
    public float m_maxSpeed;
    public float m_jumpHeight;

    public float m_extraGravity;

    float m_playerRotX;
    float m_camRotY;
    Vector3 m_directionIntentX;
    Vector3 m_directionIntentY;
    float m_speed;
   

    public bool m_grounded;


    // Update is called once per frame
    void Update()
    {
        f_lookAround();
        f_moveAround();
        f_strongerGravity();
        f_groundCheck();
        if(m_grounded == true && Input.GetButtonDown("Jump"))
        {
            f_playerJump();
        }
    }

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
            m_speed = m_sprintSpeed;
        }
        if(!Input.GetKey(KeyCode.LeftShift))
        {
            m_speed = m_walkSpeed;
        }
    }

    void f_strongerGravity()
    {
        m_rb.AddForce(Vector3.down * m_extraGravity);
    }

    void f_groundCheck()
    {
        RaycastHit m_groundHit;
        m_grounded = Physics.Raycast(transform.position, -transform.up, out m_groundHit, 1.25f); //Automatically set bool value to true if an object is hit; else, returns false.
    }

    void f_playerJump()
    {
        m_rb.AddForce(Vector3.up * m_jumpHeight, ForceMode.Impulse);
    }
}
