using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreateSceneCamera : CameraManager
{

    public override void OnCreate()
    {
        maincamera = GetComponent<Camera>();

        cameraHeight = maincamera.orthographicSize * 2f;
        cameraWidth = cameraHeight * maincamera.aspect;
    }

    public override void Step()
    {
    }

    public override void AfterStepAlways()
    {
        if (player != null && background != null)
        {
            backgroundBounds = background.bounds;

            Vector3 targetPos = new Vector3(player.position.x, player.position.y, maincamera.transform.position.z);


            float minX = backgroundBounds.min.x + cameraWidth / 2;
            float maxX = backgroundBounds.max.x - cameraWidth / 2;
            float minY = backgroundBounds.min.y + cameraHeight / 2;
            float maxY = backgroundBounds.max.y - cameraHeight / 2;
            //Limit camera movement range

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
            maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, targetPos, Time.deltaTime * 5f);
            // Relatively smooth tracking of playr positions
        }
    }


}
