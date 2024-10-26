using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOut : MonoBehaviour
{
    public GameObject pointOut1;
    public GameObject pointOut2;
    public GameObject pointOut3;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize start time
        startTime = Time.time;

        // Initially hide all points
        pointOut1.SetActive(false);
        pointOut2.SetActive(false);
        pointOut3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the time since start
        float elapsedTime = Time.time - startTime;

        // Show points in sequence
        if (elapsedTime > 0.25f && elapsedTime <= 0.5f)
        {
            pointOut1.SetActive(true);
            pointOut2.SetActive(false);
            pointOut3.SetActive(false);
        }
        else if (elapsedTime > 0.5f && elapsedTime <= 0.75f)
        {
            pointOut1.SetActive(false);
            pointOut2.SetActive(true);
            pointOut3.SetActive(false);
        }
        else if (elapsedTime > 0.75f && elapsedTime <= 1.0f)
        {
            pointOut1.SetActive(false);
            pointOut2.SetActive(false);
            pointOut3.SetActive(true);
        }
        else if (elapsedTime > 1.0f)
        {
            // Reset all points and the timer
            pointOut1.SetActive(false);
            pointOut2.SetActive(false);
            pointOut3.SetActive(false);

            // Reset the start time for the next cycle
            startTime = Time.time;
        }
    }
}
