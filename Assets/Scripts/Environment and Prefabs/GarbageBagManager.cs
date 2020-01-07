using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageBagManager : MonoBehaviour
{
    public GameObject collisionSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(Instantiate(collisionSound, transform.position, transform.rotation), 2.0f);
        enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
