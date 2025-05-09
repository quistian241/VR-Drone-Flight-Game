using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using TMPro;

public class track : MonoBehaviour
{
    public TextAsset file;
    public GameObject checkpoint;
    public GameObject drone;
    public List<GameObject> checkpoints;
    public TextMeshProUGUI display;
    public GameObject timerObject;
    public GameObject gameAudio;
    private int nextCheckpoint;
    private bool finished = false;
    List<Vector3> ParseFile() {
	    float ScaleFactor = 1.0f / 39.37f;
	    List<Vector3> positions = new List<Vector3>();
	    string content = file.ToString();
	    string[] lines = content.Split('\n');
	    for (int i = 0; i < lines.Length; i++) {
		    string[] coords = lines[i].Split(' ');
		    Vector3 pos = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
		    positions.Add(pos * ScaleFactor);
	    }
	    return positions;
    }

    public bool reachedCheckpoint() {
        return getCheckpointDistance() <= 9.144f;
    }

    public bool reachedGoal() {
        return nextCheckpoint >= checkpoints.Count;
    }

    public GameObject getNextCheckpoint() {
        return checkpoints[nextCheckpoint];
    }

    public GameObject getPreviousCheckpoint() {
        return checkpoints[nextCheckpoint-1];
    }

    public float getCheckpointDistance() {
        return Vector3.Distance(drone.transform.position, checkpoints[nextCheckpoint].transform.position);
    }

    void displayCheckpointMessage() {
        if (reachedGoal()) {
            finished = true;
            gameAudio.GetComponents<AudioSource>()[4].Play();
            display.text = "Final Checkpoint Reached!";
        } else {
            gameAudio.GetComponents<AudioSource>()[2].Play();
            display.text = "Checkpoint " + (nextCheckpoint + 1) + " Reached!";
        }
    }
    // Start is called before the first frame update
    void Start() {
    }
    void Awake()
    {
        List<Vector3> positions = ParseFile();
        for (int i = 0; i < positions.Count; i++) {
            checkpoints.Add(Instantiate(checkpoint, positions.ElementAt(i), Quaternion.identity));
        }
        drone.transform.position = positions.ElementAt(0);
        drone.transform.LookAt(checkpoints[1].transform.position);
        drone.transform.rotation = Quaternion.Euler(0f, drone.transform.eulerAngles.y, 0f);
        // Quaternion startRotation = Quaternion.identity;
        // startRotation.eulerAngles = new Vector3(0,0,0);
        // drone.transform.rotation = startRotation;

        checkpoints[0].SetActive(false);
        nextCheckpoint = 1;
        checkpoints[1].GetComponent<Renderer>().material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished) {
            if (reachedGoal()) {
                displayCheckpointMessage();
                timerObject.GetComponent<timer>().disableTimer();
                return;
            }
            else if (reachedCheckpoint()) {
                getNextCheckpoint().SetActive(false);
                displayCheckpointMessage();
                nextCheckpoint++;
                getNextCheckpoint().GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }

}
