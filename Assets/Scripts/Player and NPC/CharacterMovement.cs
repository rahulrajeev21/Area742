using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
    CharacterController c;
    float walkSpeed;
    float rotateSpeed;

    public bool isInSafeArea = true;

    ActivityManagement activityManager;

    public GameObject toxicityEffectDisplay;

    float lastToxicHit = 0.0f;
    void Start()
    {
        c = GetComponent<CharacterController>();
        walkSpeed = 5.0f;
        rotateSpeed = 5.0f;

        activityManager = GetComponent<ActivityManagement>();
    }


    void Update()
    {
        // Input.GetKey(KeyCode.Space) || 
        if (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.Button.One))
        {
            c.SimpleMove(Camera.main.transform.forward * walkSpeed);
        }
        //remove the line below when building
        transform.Rotate(Input.GetAxis("Vertical") * rotateSpeed, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        if(!isInSafeArea)
        {
            activityManager.LoseHealth(Time.deltaTime); //lose health if not in safe area
        }
        
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Heya");
        GameObject hit = collision.gameObject;
        if (hit.tag == "ToxicObject")
        {
            Debug.Log("fnhfh");
            activityManager.GetInToxicated();
        }
    }
         */


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "ToxicObject" && Time.time > lastToxicHit + 4.0f) //cool down for 4 seconds
        {
            Debug.Log("fnhfh");
            activityManager.GetInToxicated();
            lastToxicHit = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "WinningLine")
        {
            activityManager.EndGame(true);
        }
        else
        {
            if (isInSafeArea == false) // base check
            {
                activityManager.GetInToxicated(); // if entering safe area from unsafe area, suffer because it was wrong to go to unsafe area
            }
            if (other.gameObject.tag == "SafeAreaOutline")
            {
                isInSafeArea = true;
                toxicityEffectDisplay.SetActive(false);
            }
            Debug.Log("enter, in safe area: " + isInSafeArea.ToString());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SafeAreaOutline")
        {
            isInSafeArea = false;
            toxicityEffectDisplay.SetActive(true);
            activityManager.EnteredUnsafeArea(); //turn off intoxication until player is in unsafe area and keep on decreasing health (check in update)

        }
        Debug.Log("exit, outside safe area: " + isInSafeArea.ToString());
    }
}
