using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapCreateInspector : MonoBehaviour
{
    // Start is called before the first frame update
    public MapCreateController controller;
    public List<InspectorArea> components;

    public GameObject contents;

    public void SetComponentsRect()
    {
        float spacing = 10;
        float yPos = 0;
        foreach(InspectorArea rt in components)
        {
            rt.rt.anchorMin = new Vector2(0, 1);
            rt.rt.anchorMax = new Vector2(0, 1);
            rt.rt.pivot = new Vector2(0, 1);

            rt.rt.localPosition = new Vector2(0, yPos - spacing);
            yPos -= (rt.sizeY + spacing);
        }
    }

    public virtual void SetData(GameObject go)
    {
        foreach (Transform rt in contents.transform)
        {
            components.Add(rt.GetComponent<InspectorArea>());
        }
        SetComponentsRect();

    }


}
