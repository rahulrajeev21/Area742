using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManagement : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] waterbodies;
    bool raiseWaterBodies = false;

    float limitMaxTranslation = 7.0f;
    void Start()
    {
        waterbodies = GameObject.FindGameObjectsWithTag("ToxicWaterBody");
        Invoke("TurnOnRaisingWaterLevel", 10.0f);
    }

    void TurnOnRaisingWaterLevel()
    {
        raiseWaterBodies = true;
        Invoke("TurnOffRaisingWaterLevel", 3.0f);
    }

    void TurnOffRaisingWaterLevel()
    {
        raiseWaterBodies = false;
        Invoke("TurnOnRaisingWaterLevel", 10.0f);
    }
    // Update is called once per frame
    void Update()
    {
        if(raiseWaterBodies && limitMaxTranslation > 0.0f)
        {
            foreach(GameObject waterbody in waterbodies)
            {
                float translateBy = Time.deltaTime/5;
                limitMaxTranslation -= translateBy;
                waterbody.transform.position += new Vector3(0, translateBy, 0);
            }
        }
    }
}
