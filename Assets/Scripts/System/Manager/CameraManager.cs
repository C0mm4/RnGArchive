using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class CameraManager : Obj
{
    public Transform player;
    public Camera maincamera;

    private float cameraWidth, cameraHeight;

    private Tilemap field;

    private BoundsInt fieldBound;

    private bool isSet;

    public override void OnCreate()
    {
        player = FindPlayerTransform();
        maincamera = GetComponent<Camera>();
        isSet = false;
    }

    public override void Step()
    {
        base.Step();
        if(player == null)
        {
            player = FindPlayerTransform();
        }
        cameraWidth = maincamera.pixelWidth; cameraHeight = maincamera.pixelHeight;
        Debug.Log(cameraWidth + " " + cameraHeight);
    }

    // Update is called once per frame
    public override void AfterStep()
    {
        SetBounds();
        if(player != null && fieldBound != null && isSet)
        {

            Vector3 targetPos = new Vector3(player.position.x, player.position.y, maincamera.transform.position.z);

            float distance = - maincamera.transform.position.z;
            float height = distance * Mathf.Tan(maincamera.fieldOfView * Mathf.Deg2Rad / 2);
            Debug.Log(maincamera.fieldOfView);
            float width = (cameraWidth / cameraHeight) * height;
            Debug.Log(distance);
            Debug.Log(height);
            Debug.Log(width);

            float minX = field.CellToWorld(fieldBound.min).x + width / 2f;
                        Debug.Log("minX : " + minX);
            float maxX = field.CellToWorld(fieldBound.max).x - width / 2f;
                        Debug.Log("maxX : " + maxX);
            float minY = field.CellToWorld(fieldBound.min).y + height / 2f;
                        Debug.Log("minY : " + minY);
            float maxY = field.CellToWorld(fieldBound.max).y - height / 2f;
                        Debug.Log("maxY : " + maxY);
            //Limit camera movement range

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
                        Debug.Log("targetPos.x : " + targetPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
                        Debug.Log("targetPos.y : " + targetPos.y);
            maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, targetPos, Time.deltaTime * 5f);
            // Relatively smooth tracking of playr positions
        }
    }

    public Transform FindPlayerTransform()
    {
        if (GameManager.player != null)
        {
            return GameManager.player.transform;
        }
        return null;
    }

    public void SetBounds()
    {
        field = GameObject.Find("InPlay").GetComponent<Tilemap>();
        BoundsInt bounds = new BoundsInt(field.cellBounds.min, field.cellBounds.size);

        // Iterate over all tiles in the tilemap
        foreach (Vector3Int pos in field.cellBounds.allPositionsWithin)
        {
            // Check if the tile is present
            if (field.HasTile(pos))
            {
                // Update bounds to include the position of the tile
                bounds.min = Vector3Int.Min(bounds.min, pos);
                bounds.max = Vector3Int.Max(bounds.max, pos);
            }
        }
        fieldBound = bounds;
        isSet = true;
    }
}
