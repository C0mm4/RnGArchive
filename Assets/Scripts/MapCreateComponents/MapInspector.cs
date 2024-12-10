using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapInspector : MapCreateInspector
{
    public TMP_InputField xInput;
    public TMP_InputField yInput;
    public Button submitButton;

    public TMP_Dropdown skySelect;

    public override void SetData(GameObject go)
    {
        base.SetData(go);

        xInput.text = controller.currentLayer.size.x.ToString();
        yInput.text = controller.currentLayer.size.y.ToString();

        xInput.onSubmit.AddListener((value) => xValueChange(value));
        yInput.onSubmit.AddListener((value) => yValueChange(value));
        submitButton.onClick.AddListener(() => SubmitButton());

        skySelect.onValueChanged.AddListener((value) => ChangeSky(value));

        if (int.Parse(xInput.text) < 20)
        {
            xInput.text = "20";
        }
        if (int.Parse(yInput.text) < 11)
        {
            yInput.text = "11";
        }
        ChangeAllTilmepSize();
    }

    public void xValueChange(string value)
    {
        controller.currentLayer.size = new Vector3Int(int.Parse(value), controller.currentLayer.size.y, 0);
        if (int.Parse(xInput.text) < 20)
        {
            xInput.text = "20";
        }
        ChangeAllTilmepSize();
    }

    public void yValueChange(string value)
    {
        controller.currentLayer.size = new Vector3Int(controller.currentLayer.size.x, int.Parse(value), 0);
        if (int.Parse(yInput.text) < 11)
        {
            yInput.text = "11";
        }
        ChangeAllTilmepSize();
    }

    public void SubmitButton()
    {
        xInput.onSubmit.Invoke(xInput.text);
        yInput.onSubmit.Invoke(yInput.text);
    }

    public void ChangeSky(int sky)
    {

    }

    public void ChangeAllTilmepSize()
    {
        Debug.Log("onChangeSize");
        var tilemaps = controller.currentMap.GetComponentsInChildren<Tilemap>();


        Vector3Int inputBounds = new Vector3Int(int.Parse(xInput.text), int.Parse(yInput.text), 0);

        foreach (Tilemap tilemap in tilemaps)
        {
//            tilemap.CompressBounds();
            tilemap.size = inputBounds;
            tilemap.origin = Vector3Int.zero;
        }

        var bound = controller.currentMap.bound;


        bound.transform.position = (controller.currentLayer.CellToWorld(new Vector3Int(0,0,0)) + controller.currentLayer.CellToWorld(inputBounds)) / 2;

        bound.transform.localScale = new Vector2(0.32f * (inputBounds.x), 0.32f * (inputBounds.y));



    }
}
