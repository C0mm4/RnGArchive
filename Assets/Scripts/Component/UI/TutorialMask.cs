using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMask : Obj
{
    public Vector2 topLeft;
    public Vector2 bottomRight;

    public Vector2 currentTopLeft;
    public Vector2 currentBottomRight;

    public Vector2 mouseRectPos;

    bool isSet = false;

    public Image up, left, down, right, upleft, upright, downleft, downright;

    public override void OnCreate()
    {
        currentTopLeft = new Vector2(0, 1080);
        currentBottomRight = new Vector2(1920f, 0);
        base.OnCreate();

        CreateHandler(new Vector2(460, 440), new Vector2(660, 240));
    }

    public void CreateHandler(Vector2 lu, Vector2 ud)
    {
        topLeft = lu;
        bottomRight = ud;
        StartCoroutine(MoveMask());
    }

    public IEnumerator MoveMask()
    {
        yield return new WaitForSeconds(0.5f);

        isSet = true;


    }

    public override void AfterStep()
    {
        if (isSet)
        {
            currentTopLeft = Vector2.Lerp(currentTopLeft, topLeft, Time.deltaTime * 2f);
            currentBottomRight = Vector2.Lerp(currentBottomRight, bottomRight, Time.deltaTime * 2f);


            up.GetComponent<RectTransform>().localPosition = new Vector2((currentTopLeft.x + currentBottomRight.x) / 2f - 960, up.GetComponent<RectTransform>().localPosition.y);
            down.GetComponent<RectTransform>().localPosition = new Vector2((currentTopLeft.x + currentBottomRight.x) / 2f - 960, down.GetComponent<RectTransform>().localPosition.y);
            left.GetComponent<RectTransform>().localPosition = new Vector2(left.GetComponent<RectTransform>().localPosition.x, (currentTopLeft.y + currentBottomRight.y) / 2f - 540);
            right.GetComponent<RectTransform>().localPosition = new Vector2(right.GetComponent<RectTransform>().localPosition.x, (currentTopLeft.y + currentBottomRight.y) / 2f - 540);

            left.GetComponent<RectTransform>().sizeDelta = new Vector2(currentTopLeft.x, currentTopLeft.y - currentBottomRight.y);
            right.GetComponent<RectTransform>().sizeDelta = new Vector2(1920 - currentBottomRight.x, currentTopLeft.y - currentBottomRight.y);
            up.GetComponent<RectTransform>().sizeDelta = new Vector2(currentBottomRight.x - currentTopLeft.x, 1080 - currentTopLeft.y);
            down.GetComponent<RectTransform>().sizeDelta = new Vector2(currentBottomRight.x - currentTopLeft.x, currentBottomRight.y);

            upleft.GetComponent<RectTransform>().sizeDelta = new Vector2(currentTopLeft.x,1080 - currentTopLeft.y);
            upright.GetComponent<RectTransform>().sizeDelta = new Vector2(1920 - currentBottomRight.x, 1080 - currentTopLeft.y);
            downleft.GetComponent<RectTransform>().sizeDelta = new Vector2(currentTopLeft.x, currentBottomRight.y);
            downright.GetComponent<RectTransform>().sizeDelta = new Vector2(1920 - currentBottomRight.x, currentBottomRight.y);
        }
    }
}
