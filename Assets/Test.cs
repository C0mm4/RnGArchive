using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    string test = $"{GameManager.Input._keySettings.Call}";
    string test1 = "{GameManager.Input._keySettings.Call}";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(test);
        Debug.Log(test1);
    }
}
