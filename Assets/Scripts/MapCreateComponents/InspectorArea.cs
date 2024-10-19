using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectorArea : MonoBehaviour
{
    // Start is called before the first frame update

    public float sizeY;
    public RectTransform rt;



    public void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public void AddSize(float value)
    {
        sizeY += value;
    }

    public void RemoveSize(float value)
    {
        sizeY -= value;
    }

}
