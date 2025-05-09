using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class wayfinder : MonoBehaviour
{
    public GameObject drone;
    public GameObject trackObject;
    public TextMeshProUGUI distanceDisplay;
    private track trackScript;
    private LineRenderer arrow;
    private float arrowOffset = 15f;
    // Start is called before the first frame update
    void Start()
    {
        trackScript = trackObject.GetComponent<track>();
        arrow = drone.GetComponent<LineRenderer>();
        arrow.material.color = Color.yellow;
        arrow.startWidth = 1.5f;
        arrow.endWidth = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackScript.reachedGoal()) {
            arrow.enabled = false;
            distanceDisplay.text = "Distance to Next Checkpoint: -- ft";
            return;
        }

        float newDistance = trackScript.getCheckpointDistance() / 0.3048f; 
        distanceDisplay.text = "Distance to Next Checkpoint: " + newDistance + " ft";
        arrow.SetPosition(0, drone.transform.position+drone.transform.forward*arrowOffset);
        arrow.SetPosition(1, trackScript.getNextCheckpoint().transform.position);
    }
}
