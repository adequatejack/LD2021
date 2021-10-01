using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCountFBF : MonoBehaviour
{
    private Text UI_Text;

    void Start()
    {
        UI_Text = GetComponent<Text>();
    }

    void Update()
    {
        UI_Text.text = "FPS: ";
        UI_Text.text += ((1 / Time.deltaTime) - ((1/Time.deltaTime) % 1)).ToString(); // Estimate the fps by getting the delay between frames.
    }
}
