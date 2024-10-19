using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPointInspector : MapCreateInspector
{
    public TMP_InputField x;
    public TMP_InputField y;
    public TMP_InputField spawnPId;

    public void Start()
    {
        x.onValueChanged.AddListener((string text) => onChangeXValues(text));
        y.onValueChanged.AddListener((string text) => onChangeYValues(text));
        spawnPId.onValueChanged.AddListener((string text) => onChangeIDValues(text));
    }

    public void onChangeXValues(string value)
    {
        controller.DataShowObj.transform.position = 
            new Vector3(float.Parse(value), controller.DataShowObj.transform.position.y, 0);
    }

    public void onChangeYValues(string value)
    {

        controller.DataShowObj.transform.position =
            new Vector3(controller.DataShowObj.transform.position.x, float.Parse(value), 0);
    }

    public void onChangeIDValues(string value)
    {
        controller.DataShowObj.GetComponent<SpawnP>().id = value;
    }

    public override void SetData(GameObject go)
    {
        base.SetData(go);
        x.text = go.transform.position.x.ToString();
        y.text = go.transform.position.y.ToString();
        spawnPId.text = go.GetComponent<SpawnP>().id;
    }
}
