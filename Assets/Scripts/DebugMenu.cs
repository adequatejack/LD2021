using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{

    private int fps = 60; //FPS count over time

    [Header("Universal Debug Menu v1")]
    [Space(3)]
    [Header("Main panel")]
    public GameObject MenuParent;
    [Space(10)]
    [Header("FPS Text boxes")]
    public Text _FPSFBF;
    public Text _FPS;
    [Space(10)]
    [Header("Memory text boxes")]
    public Text _Mem;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountFrames());
    }

    void Update()
    {
        if (Input.GetKey("f3")) // Activate 
        {
            MenuParent.SetActive(true);
            _FPSFBF.text = "FPS: ";
            _FPSFBF.text += ((1 / Time.deltaTime) - ((1 / Time.deltaTime) % 1)).ToString(); // Estimate the fps by getting the delay between frames.

            _Mem.text = (UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong()/1000000) + "MB / " + (UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong() / 1000 / 1000) + "MB"; // Get amount of used vs free memory on the heap
        } else
        {
            MenuParent.SetActive(false);
        }

        fps++;
    }

    // PURPOSE: A self repeating coroutine which checks how many frames were made over a 1 second period
    public IEnumerator CountFrames()
    {
        yield return new WaitForSeconds(1f);
        _FPS.text = "FPS: " + fps.ToString();
        fps = 0;
        StartCoroutine(CountFrames()); // loop time
    }
}
