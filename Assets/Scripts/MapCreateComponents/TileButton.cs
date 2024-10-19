using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileButton : CustomButton
{
    public MapCreateController controller;
    public Object tile;
    public Image img;

    public override void OnClick()
    {
        controller.selectedTile = tile;
    }
}
