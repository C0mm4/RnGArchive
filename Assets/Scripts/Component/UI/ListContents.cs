using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListContents : MonoBehaviour
{
    public List<ContentsSlot> slots;

    public string UIName;

    int index = 0;

    public void AddContents<T>(T contentsObj) where T : Contents    
    {
        GameObject obj = GameManager.InstantiateAsync(UIName);
        obj.GetComponent<ContentsSlot>().SetContent(contentsObj);
        obj.GetComponent<ContentsSlot>().index = index++;
        slots.Add(obj.GetComponent<ContentsSlot>());

        obj.transform.SetParent(gameObject.transform, false);


    }

    public void FreshUI()
    {
        foreach(var slot in slots)
        {
            slot.OnFresh();
        }
    }
}
