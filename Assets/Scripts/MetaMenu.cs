using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaMenu : MonoBehaviour
{

    private int fps = 60; //FPS count over time
    public int fpsUpdateFrequency = 25; // frequency of detla time fps counter

    [Header("Meta Menu v1")]
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
    [Space(3)]
    [Header("Menu Meta Main")]
    public GameObject ParentMenu4Escape;
    [Space(10)]
    [Header("Escape menu buttons")]
    public Button Resume;
    public Button QuitMenu;
    public Button QuitDesktop;
    [Space(3)]
    [Header("Player object with controller script")]
    public GameObject player;

    private bool isEscapeMenuActive = false;
    private SC_FPSController controller; 
    
    // Start is called before the first frame update
    void Start()
    {
        controller = player.GetComponent<SC_FPSController>();
        _Mem.text = "Non-dev build :(";
        StartCoroutine(CountFrames());

        // attach listeners to buttons
        Resume.onClick.AddListener(() => buttonClickEvent(1));
        QuitMenu.onClick.AddListener(() => buttonClickEvent(2));
        QuitDesktop.onClick.AddListener(() => buttonClickEvent(3));
    }

    // handles all of the button inputs
    void buttonClickEvent(int buttonID)
    {
        switch (buttonID)
        {
            case 1:
                isEscapeMenuActive = false;
                break;
            case 2:
                // TO DO MAIN MENU LOADING SCREEN CODE
                break;
            case 3:
                Application.Quit();
#if UNITY_EDITOR
                Debug.Break();
#endif
                break;
        }
    }

    void Update()
    {
        if (Input.GetKey("f3")) // Activate 
        {
            MenuParent.SetActive(true);

            switch (Time.frameCount % fpsUpdateFrequency)
            {
                case 1:
                    _FPSFBF.text = "FPS (DT): ";
                    _FPSFBF.text += ((1 / Time.deltaTime) - ((1 / Time.deltaTime) % 1)).ToString(); // Estimate the fps by getting the delay between frames.
                    break;
            }

#if DEVELOPMENT_BUILD || UNITY_EDITOR // IDK why, but profiler can only be accessed in development build. This will do for now - jack
            _Mem.text = (UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong()/1000000) + "MB / " + (UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong() / 1000 / 1000) + "MB"; // Get amount of used vs free memory on the heap
#endif

        } else
        {
            MenuParent.SetActive(false);
        }

        if (isEscapeMenuActive)
        {
            ParentMenu4Escape.SetActive(true);
            Cursor.visible = true;
            controller.CursorLocked = false;
        } else
        {
            ParentMenu4Escape.SetActive(false);
            Cursor.visible = false;
            controller.CursorLocked = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEscapeMenuActive = !isEscapeMenuActive;
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
