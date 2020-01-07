using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneManager : MonoBehaviour
{
    public GameObject GarbageBag;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ThrowGarbage", 2.0f);
    }

    void ThrowGarbage()
    {
        Instantiate(GarbageBag, transform.position - new Vector3(0.0f, 2.0f, 0.0f), Quaternion.Euler(Random.Range(-90.0f, 90.0f), 0, 0));
        float nextRound = Random.Range(1.0f, 8.0f);
        Invoke("ThrowGarbage", nextRound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
