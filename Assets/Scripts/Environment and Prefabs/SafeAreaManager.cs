using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaManager : MonoBehaviour
{
    // Start is called before the first frame update

    bool isShrinking = false;
    float toggleShrinkPeriod = 2.0f;

    public GameObject originHelperObject;
    public GameObject originEnd;
    Vector3 origin;

    float stepSize;

    ActivityManagement activityManager;
    void Start()
    {
        Invoke("toggleScaleShrink", toggleShrinkPeriod);
        activityManager = GameObject.FindWithTag("Player").GetComponent<ActivityManagement>();
        origin = originHelperObject.transform.position;

        stepSize = Time.deltaTime / 300;
    }

    // Update is called once per frame
    void Update()
    {
        if(isShrinking)
        {
            transform.localScale -= new Vector3(stepSize, 0.0f, stepSize);

            //Debug.Log(transform.localScale);
            if (transform.localScale.x <= 0.5f)
            {
                isShrinking = false;
                CancelInvoke();
            }

            originHelperObject.transform.position = Vector3.MoveTowards(originHelperObject.transform.position, originEnd.transform.position, 0.05f);

            transform.position = originHelperObject.transform.position;
        }
    }

    void toggleScaleShrink()
    {
        isShrinking = !isShrinking;
        Invoke("toggleScaleShrink", toggleShrinkPeriod);
    }
}
