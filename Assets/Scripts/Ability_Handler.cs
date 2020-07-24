using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Ability_Handler : MonoBehaviour
{
    [Header("Script Access")]
    private Player_Controller m_playerController;
    private Prototype_Classes m_prototypeClasses;

    [Header("Ability Mechanics")]
    [Space(2)]
    public GameObject pushBack; //Ability Game Objects.
    public GameObject invisibility;
    public GameObject wall; 
    public GameObject storm;
    public GameObject knife;
    public GameObject tornado;
    public GameObject infector;

    public int totalKnives;

    [Header("Player Mechanics")]
    [Space(2)] 
    public Transform shotPoint; //Where bullets are shot from.
    private RaycastHit m_hitscanCast; //The raycast that determines the direction of the bullet.
    public GameObject weapons;
    public GameObject hands;

    [Header("Animation Mechanics")]
    [Space(2)]
    private Animator m_handsAnim;
    public string[] animationType; //Store the animation type as a string so that it can be called easier, as well as being able to set all animations in the array to false when required; thus allowing the player to return to idle ability hands animation.

    private void Start()
    {
        m_playerController = GameObject.FindObjectOfType<Player_Controller>();
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();

        m_handsAnim = hands.GetComponent<Animator>();
    }

    public void Update()
    {
        abilityStateHandler();
    }

    void abilityStateHandler()
    {
        if (m_prototypeClasses.abilityState == true) //Check if the player has activated their abilities.
        {
            hands.active = true;
            weapons.active = false;
        }
        else if (GetComponent<Pickup_System>().itemHeld == false) //If the player isn't holding a clock item the show weapons.
        {
            hands.active = false;
            weapons.active = true;
            m_prototypeClasses.stateQ = false; //Set the abilities variables back to false so that the ability can be accessed again.
            m_prototypeClasses.stateV = false;
        }
    }

    public IEnumerator f_spawnInvisibility() //Become invisible. 
    {
        m_handsAnim.SetBool(animationType[0], true); //Set the animation true based on string value (0).
        yield return new WaitForSeconds(2); //Wait 2 seconds before running the next line of code.
        m_playerController.isPlayerInvisible = true; //Stops the enemies being able to see the player.
        invisibility.active = true; //Enable the invisible post process effect.
        yield return new WaitForSeconds(10);
        m_playerController.isPlayerInvisible = false; //After 10 seconds, allow the player to be visible again.
        invisibility.active = false; //Disable post process effect.
        StartCoroutine(f_resetAnimations()); //Start the reset animations phase.
    } 

    public IEnumerator f_spawnWall() //Spawn a wall. 
    {
        m_handsAnim.SetBool(animationType[1], true); //Begind wall rise animation.
        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast in direction player is looking.
        {
            GameObject o_wall = Instantiate(wall, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward)); //Instantiate a wall that summons at the position of the players crosshair location.
            o_wall.transform.eulerAngles = new Vector3(o_wall.transform.eulerAngles.x, m_playerController.playerRotX, o_wall.transform.eulerAngles.z); //Rotate the wall based on angle of player.
        }
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(f_resetAnimations());
    } 

    public IEnumerator f_spawnTornado() //Spawn a tornado. 
    {
        m_handsAnim.SetBool(animationType[2], true); //Start tornado animation.
        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast.
        {
            Instantiate(tornado, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward)); //Spawns a tornado at position of player crosshair.
        }
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(f_resetAnimations());
    } 

    public IEnumerator f_spawnStorm() //Spawn a storm. 
    {
        m_handsAnim.SetBool(animationType[3], true); //Start storm animation.
        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast.
        {
            Instantiate(storm, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward)); //Intantiate a tornado at crosshair location.
        }
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(f_resetAnimations());
    } 

    public IEnumerator f_spawnPushback() //Push enemies back. 
    {
        m_handsAnim.SetBool(animationType[4], true); //Begin pushback hand animation.
        yield return new WaitForSeconds(0.5f);
        Instantiate(pushBack, shotPoint.transform.position, shotPoint.rotation); //Spawn the pushback gameobject that adds force to any enemy that is hit.
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(f_resetAnimations());
    } 

    public IEnumerator f_spawnKnives() //Spawn knives. 
    {
        m_handsAnim.SetBool(animationType[5], true); //Begin knife throwing animation.
        yield return new WaitForSeconds(0.6f);
        float m_sideDirection = -40; //Angle of first knife.
        for (int i = 0; i < totalKnives; i++) //Instantiate a set amount of knives.
        {
            GameObject m_knife = Instantiate(knife, shotPoint.position, Quaternion.identity); 
            Rigidbody m_krb = m_knife.GetComponent<Rigidbody>(); //Access that specific knife RigidBody and >

            m_krb.AddForce(shotPoint.forward * 100); 
            m_krb.AddForce(shotPoint.right * m_sideDirection); 

            m_sideDirection += 10; //> change the RigidBody force from the right direction so it 'spreads' correctly.
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(f_resetAnimations());
    } 

    public void f_spawnInfector() //Spawn infector. 
    {
        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out m_hitscanCast, Mathf.Infinity)) //Creates a Raycast.
        {
            Instantiate(infector, new Vector3(m_hitscanCast.point.x, m_hitscanCast.point.y - 2, m_hitscanCast.point.z), Quaternion.LookRotation(Vector3.forward)); //Spawns a tornade of position of player crosshair.
        }
    } 

    IEnumerator f_resetAnimations() //Reset animations states and allow the player to switch weapons again. 
    {
        for (int i = 0; i < animationType.Length; i++)
        {
            m_handsAnim.SetBool(animationType[i], false); //Reset all animations to false so that the ability hands idle state can be accessed again.
        }
        yield return new WaitForSeconds(0.5f);
        m_prototypeClasses.canSwitch = true; //Allow the player to switch weapons again.
        m_prototypeClasses.abilityState = !m_prototypeClasses.abilityState; //Set ability state to false.
    }
}
