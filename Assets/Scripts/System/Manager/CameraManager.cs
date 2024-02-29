using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class CameraManager : Obj
{
    public Transform player;
    public Camera maincamera;
    public Camera minimapCam;

    private float cameraWidth, cameraHeight;

    private Tilemap field;

    private BoundsInt fieldBound;

    private bool isSet;

    public override void OnCreate()
    {
        player = FindPlayerTransform();
        maincamera = GetComponent<Camera>();
        isSet = false;
        if(minimapCam == null)
        {
            minimapCam = GameManager.InstantiateAsync("MiniMapCam").GetComponent<Camera>();
        }
    }

    public override void Step()
    {
        base.Step();
        if(player == null)
        {
            player = FindPlayerTransform();
        }
        cameraWidth = maincamera.pixelWidth; cameraHeight = maincamera.pixelHeight;
        if (minimapCam == null)
        {
            minimapCam = GameManager.InstantiateAsync("MiniMapCam").GetComponent<Camera>();
        }
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
            float width = (cameraWidth / cameraHeight) * height;

            float minX = field.CellToWorld(fieldBound.min).x + width;
            float maxX = field.CellToWorld(fieldBound.max).x - width ;
            float minY = field.CellToWorld(fieldBound.min).y + height;
            float maxY = field.CellToWorld(fieldBound.max).y - height ;
            //Limit camera movement range

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
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

    public void CameraMove(Transform trans)
    {
        player = trans;

    }

    public async Task CameraMoveV2V(Vector3 startPos, Vector3 endPos, float spd = 1f)
    {
        Debug.Log("Move Start");
        GameObject go = new GameObject();
        go.transform.position = startPos;
        player = go.transform;
        float startTime = Time.time; // 시작 시간 기록

        while ((go.transform.position - endPos).magnitude >= 0.05f)
        {
            float distanceCovered = (Time.time - startTime) * spd;
            float fractionOfJourney = distanceCovered / Vector3.Distance(startPos, endPos);
            go.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney); 
            await Task.Delay(TimeSpan.FromSeconds(0.016f)); 
        }

        player = GameManager.player.transform;
        // 이동 완료 후 게임 오브젝트 제거
        Destroy(go);
    }
}
