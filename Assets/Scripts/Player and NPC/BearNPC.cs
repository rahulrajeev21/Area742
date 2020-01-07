using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearNPC : MonoBehaviour
{
    GameObject player;

    Animator animator;
    bool isDead = false;
    bool isPlayerDetected = false;
    int hitsTaken = 0;
    int maxHits = 3;
    float lastActionTime = 0.0f;

    ActivityManagement playerActivityManagement;

    public GameObject bearAttackAudio;
    public GameObject BearCryAudio;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerActivityManagement = player.GetComponent<ActivityManagement>();
        Physics.IgnoreCollision(GetComponent<CharacterController>(), player.GetComponent<CharacterController>());
    }

    // Update is called once per frame
    void Update()
    {
        
        float distFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        float angleDifference = Vector3.Angle(player.transform.position - transform.position, transform.forward);

        // let polar bear's range of vision be around 50 units (metres) and it can detect player at an angle of 45 degree
        if (!isPlayerDetected && distFromPlayer <= 50 && angleDifference <= 210)
        {
            isPlayerDetected = true;

            animator.SetBool("isPlayerDetected", true);
            animator.SetBool("isRunning", true);
        }


        // if player detected, chase him until you are 3 units close; attack him
        if (isPlayerDetected)
        {
            if(distFromPlayer > 2)
            {
                Vector3 playerPosition = player.transform.position;
                transform.LookAt(playerPosition);
                Quaternion desiredRotation2 = Quaternion.LookRotation(playerPosition - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation2, Time.deltaTime);

                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);
            }
            else // turn towards player if only he is around 3 units close
            {
                if (Time.time >= lastActionTime)
                {
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isAttacking", true);
                    animator.SetTrigger("closeAttack"); // will trigger for the needed case

                    lastActionTime = Time.time + 1.2f;
                    playerActivityManagement.polarBearAttackDamage(12.0f);
                    Destroy(Instantiate(bearAttackAudio, transform.position, transform.rotation), 2);
                }
                else
                {
                    // do nothing as of now
                }
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject snowBallObject = hit.gameObject;
        if (snowBallObject.tag == "SnowBall")
        {
            Destroy(snowBallObject);
            getHit();
        }
    }

    public void getHit()
    {
        Debug.Log("will Hit polar bear");
        hitsTaken += 1;
        if (hitsTaken <= maxHits)
        {
            animator.SetTrigger("getHit");
            Debug.Log("will Hit polar bear True");
            //animator.SetTrigger("getHit");
        }
        else
        {
            animator.SetTrigger("getHit");
            animator.SetBool("isDead", true);
            Debug.Log("Polar bear shall die now! RIP");
            Invoke("retireBearNPC", 1.5f);
        }
        Destroy(Instantiate(BearCryAudio, transform.position, transform.rotation), 2);
    }

    void retireBearNPC()
    {
        enabled = false;
    }

    // the below functions are just declared to avoid bug-fixing in bear animation (they are being called through animation events)
    public void FootR()
    {

    }
    public void FootL()
    {

    }
    public void Hit()
    {

    }
}
