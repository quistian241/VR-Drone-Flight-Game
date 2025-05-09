using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class timer : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI countdownDisplay;
    public GameObject gameAudio;
    private float time = 0.0f;
    private int minutes = 0;
    private int seconds = 0;
    private int milliseconds = 0;
    private bool timerOn = false;
 
    // Start is called before the first frame update
    void Start()
    {
        FixedMovement fixedMovement = FindAnyObjectByType<FixedMovement>();
        StartCoroutine(Countdown(fixedMovement));
    }

    public void StartPause(FixedMovement fixedMovement) {
        StartCoroutine(Countdown(fixedMovement));
    }

    IEnumerator Countdown(FixedMovement fixedMovement) {
        gameAudio.GetComponents<AudioSource>()[1].Play();
        fixedMovement.canMove = false;
        countdownDisplay.text = "3";
        yield return new WaitForSeconds(1);
        countdownDisplay.text = "2";
        yield return new WaitForSeconds(1);
        countdownDisplay.text = "1";
        yield return new WaitForSeconds(1);
        countdownDisplay.text = "GO!";
        timerOn = true;
        // Enable Movement here
        fixedMovement.canMove = true;
        yield return new WaitForSeconds(1);
        countdownDisplay.text = "";
    }

    public void disableTimer() {
        timerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn) {
            time += Time.deltaTime;
            seconds = (int)(time % 60);
            minutes = (int)(time / 60);
            milliseconds = (int)(time * 1000 % 1000);

            string milliString = milliseconds.ToString();
            string secString = seconds.ToString();
            string minString = minutes.ToString();
            if (seconds < 10) {
                secString = "0" + secString;
            }
            if (minutes < 10) {
                minString = "0" + minString;
            }

            string newTime = "Time - " + minString + ":" + secString + ":" + milliString;
            timeDisplay.text = newTime;
        }
    }
}
