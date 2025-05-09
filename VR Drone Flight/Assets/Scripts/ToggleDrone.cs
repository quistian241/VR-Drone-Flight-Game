using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDrone : MonoBehaviour
{
    public GameObject drone;
    private string leftActiveGesture = "";
    public bool droneVisible = false;

    void Awake()
    {
        drone.SetActive(false);
        droneVisible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if (leftActiveGesture == "Change Visibilty" && droneVisible) {
        //     drone.SetActive(false);
        // }
    }

    public void ToggleDroneVisibility(string gestureName) {
        Debug.Log("ToggleDroneVisibility function activated " + gestureName);

        // remove this later? 
        leftActiveGesture = gestureName;

        if (leftActiveGesture == "Change Visibilty" && droneVisible) {
            drone.SetActive(false);
            droneVisible = !droneVisible;
        } else if (leftActiveGesture == "Change Visibilty" && !droneVisible){
            drone.SetActive(true);
            droneVisible = !droneVisible;
        }
    }
}
