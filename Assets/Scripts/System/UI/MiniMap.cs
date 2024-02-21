using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : Menu
{
    [SerializeField]
    private bool isClick = false;
    private Vector2 mosPos;
    Camera miniMapCam;

    public override void show()
    {
        base.show();
        miniMapCam = GameObject.Find("MiniMapCamera").GetComponent<Camera>();
        miniMapCam.transform.position = GameManager.Camera.gameObject.transform.position;

        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = miniMapCam.targetTexture;
    }

    public override void ConfirmAction()
    {
        exit();
    }

    public override void KeyInput()
    {
        if (isGetInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClick = true;
                mosPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isClick = false;
            }
        }
        base.KeyInput();
    }

    public override void BeforeStep()
    {
        if (isClick)
        {
            Vector2 deltaMousePos = Input.mousePosition;
            deltaMousePos -= mosPos;

            miniMapCam.transform.position += new Vector3(deltaMousePos.x, deltaMousePos.y);
            mosPos = Input.mousePosition;
        }
    }
}
