using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCount : MonoBehaviour
{
    public float count;
    private Text txt;
    // Start is called before the first frame update
    void Awake()
    {
        txt = GetComponent<Text>();
        count = 0f;
        float exp = 60f;
        txt.text = exp.ToString();
        StartCoroutine(countT());
    }
    void Update()
    {
        count++;
    }
    public IEnumerator countT()
    {
        yield return new WaitForSeconds(1f);
        txt.text = count.ToString();
        count = 0f;
        StartCoroutine(countT());
    }
}
