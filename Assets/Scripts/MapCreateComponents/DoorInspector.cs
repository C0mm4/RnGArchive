using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorInspector : MapCreateInspector
{
    public TMP_InputField x;
    public TMP_InputField y;
    public TMP_InputField doorId;
    public TMP_InputField connectRoomId;
    public TMP_InputField connectDoorId;

    public Toggle startActivated;

    public void Start()
    {
        x.onValueChanged.AddListener((string text) => onChangeXValues(text));
        y.onValueChanged.AddListener((string text) => onChangeYValues(text));

        doorId.onValueChanged.AddListener((string text) => onChangeIDValues(text));
        connectRoomId.onValueChanged.AddListener((string text) => onChangeConnectRoomId(text));
        connectDoorId.onValueChanged.AddListener((string text) => onChangeConnectDoorId(text));
        startActivated.onValueChanged.AddListener((bool value) => onChangeToggleBox(value));
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
        controller.DataShowObj.GetComponent<Door>().id = value;
        if (controller.DataShowObj.GetComponent<SpawnP>() != null)
        {
            controller.DataShowObj.GetComponent<SpawnP>().id = "Door_" + value;
        }
    }

    public void onChangeConnectRoomId(string value)
    {
        controller.DataShowObj.GetComponent<Door>().connectRoomId = value;
    }

    public void onChangeConnectDoorId(string value)
    {
        controller.DataShowObj.GetComponent<Door>().connectDoorId = value;
    }

    public void onChangeToggleBox(bool value)
    {
        controller.DataShowObj.GetComponent<Door>().isActivate = value;
    }

    public override void SetData(GameObject go)
    {
        base.SetData(go); 
        x.text = go.transform.position.x.ToString();
        y.text = go.transform.position.y.ToString();
        doorId.text = go.GetComponent<Door>().id;

        connectRoomId.text = go.GetComponent<Door>().connectRoomId;
        connectDoorId.text = go.GetComponent<Door>().connectDoorId;

        startActivated.isOn = go.GetComponent<Door>().isActivate;
    }

    public override void ReSetData(string dataType, string value)
    {
        base.ReSetData(dataType, value);
        switch(dataType)
        {
            case "X":
                x.text = value;
                break;
            case "Y":
                y.text = value;
                break;

        }
    }
}
