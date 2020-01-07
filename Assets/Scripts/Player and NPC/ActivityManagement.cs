using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActivityManagement : MonoBehaviour // if you want to extend the project, break ActivityManagement into two scripts if you need to add more activities
    // so as to maintain KISS principle -> HealthManagement, ActionManagement
{
    // health
    float health = 100.0f;
    public Slider healthSlider;

    //toxicity
    float intoxicatedTime = 0.0f;
    float maxIntoxicatedTime = 16.0f;
    public GameObject toxicityDisplay;
    public Text toxicityMeter;

    // shoot ball
    float lastShotTime = 0.0f;
    public GameObject snowBall;
    public GameObject snowBallImpactAudio;

    // eat
    float canEatUptoDistance = 30.0f;
    public GameObject EatFoodAudio;

    public Text gameResultDisplay;

    void Start()
    {
        healthSlider.value = 1;
        toxicityDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       // Input.GetMouseButtonDown(0) || 
       if (Input.GetMouseButtonDown(0) || OVRInput.Get(OVRInput.Button.Two))
       {
           EatFood();
       }

        // Input.GetMouseButtonDown(1) || 
        if ((Input.GetMouseButtonDown(1) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) ) && Time.time > lastShotTime + 0.5f)
       {
            lastShotTime = Time.time;
            GameObject ball = Instantiate(snowBall, Camera.main.transform.position, Camera.main.transform.rotation);
            ball.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 500.0f);
            Destroy(Instantiate(snowBallImpactAudio, transform.position, transform.rotation),2);
        }
    }

    // Health and Survival
    public void LoseHealth(float damage)
    {
        health = Mathf.Max(0.0f, (health - damage));
        healthSlider.value = health / 100;
        if(health == 0.0f)
        {
            EndGame(false);
        }
    }

    public void GainHealth(float gain) // kept separate functions for lose and gain to avoid complications in the future if the project is extended
    {
        health = Mathf.Min(100.0f, (health + gain));
        healthSlider.value = health / 100;
    }

    public void EndGame(bool gameResult) // two ways by which game ends-> player dies due to 0 health, the player reaches sfu igloo
    {
        Debug.Log("in endgame");
        gameResultDisplay.GetComponent<Text>().enabled = true;
        if (gameResult)
        {
            gameResultDisplay.GetComponent<Text>().text = "Congratulations, You survived.";
        }
        else
        {
            gameResultDisplay.GetComponent<Text>().text = "You died. Try again!";
        }
        Invoke("SceneRestart", 2.0f);
    }

    void SceneRestart()
    {
        SceneManager.LoadScene(0);
    }

    // Toxicity
    public void GetInToxicated()
    {
        Debug.Log("in intoxicattt");
        CancelInvoke(); // cancel if already running, start afresh
        intoxicatedTime = maxIntoxicatedTime;
        updateToxicityDisplay(true);
        ToxicHurt();
    }

    public void EnteredUnsafeArea()
    {
        intoxicatedTime = 0.0f;
        updateToxicityDisplay(false);
    }

    void ToxicHurt()
    {
        LoseHealth(1.0f);
        intoxicatedTime = Mathf.Max(0.0f, (intoxicatedTime - 1.0f));
        toxicityMeter.text = Mathf.RoundToInt(intoxicatedTime).ToString();

        if (intoxicatedTime > 0.0f)
        {
            Invoke("ToxicHurt", 1.0f);
        }
        else
        {
            updateToxicityDisplay(false);
        }
    }

    void updateToxicityDisplay(bool showToxicity)
    {
        toxicityDisplay.SetActive(showToxicity);
    }

    // Damage by polar bear
    public void polarBearAttackDamage(float damage)
    {
        LoseHealth(damage);
    }

    // Eat Food (fishes)
    private void EatFood()
    {
        int layerMask = 1 << 9; 
        RaycastHit rayHit;
        if (Physics.Raycast(Camera.main.transform.position, (Camera.main.transform.forward), out rayHit, canEatUptoDistance, layerMask))
        {
            Destroy(rayHit.transform.gameObject);
            if (rayHit.transform.gameObject.tag == "SafeFood")
            {
                GainHealth(20.0f);
                Destroy(Instantiate(EatFoodAudio, transform.position, transform.rotation), 2);
            }
            else if (rayHit.transform.gameObject.tag == "ToxicFood")
            {
                GainHealth(20.0f);
                GetInToxicated(); // player suffers if a toxic fish is eaten
                Destroy(Instantiate(EatFoodAudio, transform.position, transform.rotation), 2);
            }
        }
    }

    
   
}
