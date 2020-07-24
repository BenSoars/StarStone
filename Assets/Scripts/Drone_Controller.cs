using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Drone_Controller : MonoBehaviour
{
    [Header("Camera Components")]
    public Transform camera;
    public Rigidbody rigidbody;

    [Header("Camera Values")]
    [Space(2)]
    public float camRotSpeed;
    public float camMinY;
    public float camMaxY;
    public float camSmoothSpeed;
    public float flySpeed;
    public float maxSpeed;
    public float verticalSpeed;
    private float m_playerRotX;
    private float m_camRotY;
    private Vector3 m_directionIntentX;
    private Vector3 m_directionIntentY;

    [Header("Referenced Scripts")]
    [Space(2)]
    private Player_Controller m_playerController;


    private void Start()
    {
        m_playerController = GameObject.FindObjectOfType<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerController.isPlayerActive == false)
        {
            f_lookAround();
            f_moveAround();
        }

        if (!Input.anyKey)
        {
            rigidbody.velocity = Vector3.zero; //Keep the drone still.
        }
    }

    void f_lookAround()
    {
        Cursor.visible = false; //Remove cursor from the screen.
        Cursor.lockState = CursorLockMode.Locked; //Locks the cursor to the screen to prevent leaving the window.

        m_playerRotX += Input.GetAxis("Mouse X") * camRotSpeed; //Rotates player FPS view along X axis based on mouse movement.
        m_camRotY += Input.GetAxis("Mouse Y") * camRotSpeed; //Rotates the camera in Y axis so that the player object doesn't rotate upwards.

        m_camRotY = Mathf.Clamp(m_camRotY, camMinY, camMaxY); //Limit how far on the Y axis the player can look.

        Quaternion m_camTargetRotation = Quaternion.Euler(-m_camRotY, 0, 0); 
        Quaternion m_targetRotation = Quaternion.Euler(0, m_playerRotX, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, m_targetRotation, Time.deltaTime * camSmoothSpeed);

        camera.localRotation = Quaternion.Lerp(camera.localRotation, m_camTargetRotation, Time.deltaTime * camSmoothSpeed);
    }

    void f_moveAround()
    {
        m_directionIntentX = camera.right;
        m_directionIntentX.y = 0;
        
        m_directionIntentX.Normalize(); //Normalize makes the numbers more 'usable' for the engine.

        m_directionIntentY = camera.forward;
        m_directionIntentY.y = 0;
        m_directionIntentY.Normalize();

        rigidbody.velocity = m_directionIntentY * Input.GetAxis("Vertical") * flySpeed + m_directionIntentX * Input.GetAxis("Horizontal") * flySpeed + Vector3.up * rigidbody.velocity.y;
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.down * (verticalSpeed / 100)); //Decrease vertical movement.
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * (verticalSpeed / 100)); //Increase vertical movement.
        }
    }  
}
