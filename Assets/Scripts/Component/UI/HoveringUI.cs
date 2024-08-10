using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringUI : MonoBehaviour
{
    public RectTransform rt;

    public RectTransform UpperLeft;
    public RectTransform UpperRight;
    public RectTransform DownLeft;
    public RectTransform DownRight;

    public Vector3 Position;
    public Vector2 Size;

    public void SetData(Vector3 pos, Vector2 size)
    {
        Position = pos; Size = size;
        rt.position = pos;
        rt.sizeDelta = size;
        Debug.Log(pos);
    }
}
