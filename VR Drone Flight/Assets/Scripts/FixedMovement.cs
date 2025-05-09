using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMovement : MonoBehaviour
{
    public Transform xrOrigin;
    public Transform camOrigin;
    public CharacterController player;
    public GameObject gameAudio;
    public LineRenderer line;
    private float playerSpeed = 50.0f;
    static private float turnSpeed = 45f; // degrees per second
    static private float lineLength = 10f;

    private string rightActiveGesture = "";
    private string leftActiveGesture = "";

    Vector3 position = new Vector3(0f, 3f, 0f);
    public bool canMove = false;
    
    public GameObject timerObj; 
    private timer timerRef;

    public GameObject trackObj;
    private track trackRef;


    // Start is called before the first frame update
    void Start()
    {
        // finalMove = Vector3.zero;
        // timerRef.StartPause(this);
    }

    void Awake()
    {
        gameAudio.GetComponents<AudioSource>()[0].Play();
        player = GetComponentInParent<CharacterController>();
        timerRef = timerObj.GetComponent<timer>();
        trackRef = trackObj.GetComponent<track>();
        // get component from the 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = xrOrigin.forward;
        Vector3 right = xrOrigin.right;
        Vector3 up = xrOrigin.up;
        forward.Normalize();
        right.Normalize();
        up.Normalize(); 
        Vector3 angles = xrOrigin.eulerAngles;

        Vector3 finalMove = Vector3.zero;

        // hand input movements
        if((rightActiveGesture == "Right Point One") && canMove){
            gameAudio.GetComponents<AudioSource>()[0].pitch = 1.5f;
            playerSpeed = 50f;
            finalMove = forward;
        } else if((rightActiveGesture == "Right Point Two") && canMove){
            gameAudio.GetComponents<AudioSource>()[0].pitch = 2f;
            playerSpeed = 100f;
            finalMove = forward;
        } else if((rightActiveGesture == "Right Hand Thumbs Right") && canMove){
            gameAudio.GetComponents<AudioSource>()[0].pitch = 1f;
            playerSpeed = 5f;
            finalMove = right;
        } else if((rightActiveGesture == "Right Hand Thumbs Left") && canMove){
            gameAudio.GetComponents<AudioSource>()[0].pitch = 1f;
            playerSpeed = 5f;
            finalMove = -right;
        } else if((rightActiveGesture == "") && canMove){
            gameAudio.GetComponents<AudioSource>()[0].pitch = 1f;
            playerSpeed = 5f;
            finalMove = forward;
        }

        // hand input rotations 
        if (leftActiveGesture == "Left Hand Thumbs Left") // Turn left
            xrOrigin.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        else if (leftActiveGesture == "Left Hand Thumbs Right") // Turn right
            xrOrigin.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        else if (leftActiveGesture == "Left Hand Thumbs Up") // Turn up
            xrOrigin.Rotate(Vector3.right, -turnSpeed * Time.deltaTime);
        else if (leftActiveGesture == "Left Hand Thumbs Down") // Turn down
            xrOrigin.Rotate(Vector3.right, turnSpeed * Time.deltaTime);
        else if (leftActiveGesture == "Left Reset XZ") // reset xy
            xrOrigin.rotation = Quaternion.Euler(0f, angles.y, 0f); // xrOrigin.rotation = Quaternion.identity;
        else if (leftActiveGesture == "Left Face Next Point") // reset xy
            xrOrigin.LookAt(trackRef.getNextCheckpoint().transform.position);
        else if (leftActiveGesture == "") {
            xrOrigin.Rotate(Vector3.up, 0 * Time.deltaTime); // stop the rotation
            xrOrigin.Rotate(Vector3.right, 0 * Time.deltaTime); // stop the rotation
            xrOrigin.Rotate(Vector3.forward, 0 * Time.deltaTime); // stop the rotation
        }
             

        CollisionFlags flags = player.Move(finalMove * playerSpeed * Time.deltaTime);

        if (((flags & CollisionFlags.Sides) != 0) || ((flags & CollisionFlags.Below) != 0) || ((flags & CollisionFlags.Above) != 0)) {
            Debug.Log("Hit somthing, will now teleport and wait");

            gameAudio.GetComponents<AudioSource>()[3].Play();
            position = trackRef.getPreviousCheckpoint().transform.position;
            TeleportTo(position);
        }
        
        // drawing a line kinda sucks
        // Vector3 sPoint = player.transform.position;
        // Vector3 ePoint = sPoint + player.transform.forward * lineLength;

        // line.SetPosition(0, sPoint);
        // line.SetPosition(1, ePoint);
    }

    public void MovePlayer(string gestureName) {
        Debug.Log("function activated " + gestureName);

        rightActiveGesture = gestureName;
    }

    public void RotatePlayer(string gestureName) {
        Debug.Log("function activated " + gestureName);

        leftActiveGesture = gestureName;
    }

    void TeleportTo(Vector3 position) {
        player.enabled = false;
        player.transform.position = position;
        player.enabled = true;

        timerRef.StartPause(this);
    }
}
