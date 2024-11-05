using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEditor.PlayerSettings;

public class Cursor : MonoBehaviour
{
    // Start is called before the first frame update

    public MapCreateController controller;
    [SerializeField]
    public List<Vector3Int> clickedPos = new();
    public bool isClicked;
    public bool isCenterClicked;

    private Vector3Int pastPos = new();
    private Vector3 pastMousePos;
    private GameObject tmpObj;
    [SerializeField]
    private GameObject clickObject;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = GameManager.CameraManager.maincamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, -1);

        var Pos = GetTilePos(mousePos); 
        
        if (tmpObj != null)
        {
            GameManager.Destroy(tmpObj);
        }
        if (controller.currentLayer != null)
        {
            controller.currentLayer.ClearAllEditorPreviewTiles();


            if (!isCenterClicked)
            {
                switch (controller.inputMode)
                {
                    case MapCreateController.InputMode.draw:
                        if (controller.selectedTile is Tile tile)
                        {
                            controller.currentLayer.SetEditorPreviewTile(Pos, tile);
                            if (Input.GetKey(KeyCode.Mouse0) && !controller.isShow && !clickedPos.Exists(item => item.x == Pos.x && item.y == Pos.y))
                            {
                                controller.currentLayer.SetTile(Pos, tile);
                            }
                        }
                        else if(controller.selectedTile is RuleTile ruleTile)
                        {
                            controller.currentLayer.SetEditorPreviewTile(Pos, ruleTile);
                            if (Input.GetKey(KeyCode.Mouse0) && !controller.isShow && !clickedPos.Exists(item => item.x == Pos.x && item.y == Pos.y))
                            {
                                controller.currentLayer.SetTile(Pos, ruleTile);
                            }
                        }
                        else if (controller.selectedTile is GameObject go)
                        {
                            tmpObj = Instantiate(go);
                            tmpObj.transform.position = controller.currentLayer.CellToWorld(Pos) + new Vector3(0.16f, 0.16f, 0);

                            if (Input.GetKey(KeyCode.Mouse0) && !controller.isShow && !clickedPos.Exists(item => item.x == Pos.x && item.y == Pos.y))
                            {
                                var instGo = Instantiate(go);
                                instGo.transform.position = tmpObj.transform.position;
                                instGo.transform.SetParent(controller.currentMap.NPCParent.transform, true);
                            }
                        }
                        break;
                    case MapCreateController.InputMode.selectObj:
                        if (!isClicked)
                        {
                            List<GameObject> goss = new();
                            foreach (Transform go in controller.currentMap.NPCParent.transform)
                            {
                                goss.Add(go.gameObject);
                            }
                            foreach(Transform go in controller.currentMap.DoorParents.transform)
                            {
                                goss.Add(go.gameObject);
                            }
                            foreach(Transform go in controller.currentMap.SpawnPointParents.transform)
                            {
                                goss.Add(go.gameObject);
                            }
                            Object tmp = new();
                            foreach (var go in goss)
                            {
                                if (Pos == GetTilePos(go.transform.position))
                                {
                                    tmp = go;
                                }
                            }

                            if (Input.GetKeyDown(KeyCode.Mouse0) && !controller.isShow)
                            {
                                if (tmp != null)
                                {
                                    clickObject = tmp as GameObject;
                                    controller.SetInspector(clickObject);
                                    
                                }
                                else
                                {
                                    controller.SetInspector(null);
                                }
                            }
                        }
                        else
                        {
                            if (Input.GetKey(KeyCode.Mouse0) && !controller.isShow && clickObject != null)
                            {
                                clickObject.transform.position = controller.currentLayer.CellToWorld(Pos) + new Vector3(0.16f, 0.16f, 0);
                            }
                            else
                            {
                                clickObject = null;
                            }
                        }
                        break;
                    case MapCreateController.InputMode.selectTrigger:

                        if (!isClicked && !controller.isShow)
                        {
                            List<GameObject> trigs = new();
                            foreach (Transform go in controller.currentMap.TriggerParent.transform)
                            {
                                trigs.Add(go.gameObject);
                            }
                            Object tmpTrigger = new();
                            foreach (var trig in trigs)
                            {
                                var size = trig.GetComponent<BoxCollider2D>().size;
                                float minX = trig.gameObject.transform.position.x - size.x/2;
                                float maxX = trig.gameObject.transform.position.x + size.x/2;
                                float minY = trig.gameObject.transform.position.y - size.y/2;
                                float maxY = trig.gameObject.transform.position.y + size.y/2;
                                if(mousePos.x >= minX && mousePos.x <= maxX)
                                {
                                    if(mousePos.y >= minY && mousePos.y <= maxY)
                                    {
                                        tmpTrigger = trig;
                                    }
                                }

                                if (Input.GetKeyDown(KeyCode.Mouse0) && !controller.isShow)
                                {
                                    if (tmpTrigger != null)
                                    {
                                        clickObject = tmpTrigger as GameObject;
                                        controller.SetInspector(clickObject);
                                        
                                    }
                                    else
                                    {
                                        controller.SetInspector(null);
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (Input.GetKey(KeyCode.Mouse0) && !controller.isShow && clickObject != null)
                            {
                                var startP = pastPos;
                                var tmpPos = Pos - startP;
                                clickObject.transform.position += new Vector3(0.32f * tmpPos.x, 0.32f * tmpPos.y, 0);
                                controller.DrawTriggerLine(Color.red, clickObject.GetComponent<Trigger>());
                            }
                            else
                            {
                                clickObject = null;
                            }
                        }
                        break;
                    case MapCreateController.InputMode.drawCutSceneTrigger:
                        if (!isClicked)
                        {
                            if (Input.GetKeyDown(KeyCode.Mouse0) && !controller.isShow)
                            {
                                AddTrigger(Pos, "CutScene");
                            }
                        }
                        else
                        {
                            if (Input.GetKey(KeyCode.Mouse0) && !controller.isShow)
                            {
                                TriggerSizeOnDraw(Pos);
                            }
                            else
                            {
                                clickObject = null;
                            }
                        }
                        break;

                    case MapCreateController.InputMode.drawSpawnTrigger:
                        if (!isClicked)
                        {
                            if (Input.GetKeyDown(KeyCode.Mouse0) && !controller.isShow)
                            {
                                AddTrigger(Pos, "Spawn");
                            }
                        }
                        else
                        {
                            if (Input.GetKey(KeyCode.Mouse0) && !controller.isShow)
                            {
                                TriggerSizeOnDraw (Pos);
                            }
                            else
                            {
                                clickObject = null;
                            }
                        }
                        break;

                    case MapCreateController.InputMode.erase:
                        List<GameObject> gos = new();
                        foreach (Transform go in controller.currentMap.NPCParent.transform)
                        {
                            gos.Add(go.gameObject);
                        }
                        foreach (Transform go in controller.currentMap.DoorParents.transform)
                        {
                            gos.Add(go.gameObject);
                        }
                        foreach (Transform go in controller.currentMap.SpawnPointParents.transform)
                        {
                            gos.Add(go.gameObject);
                        }
                        if (Input.GetKey(KeyCode.Mouse0) && !controller.isShow && !clickedPos.Exists(item => item.x == Pos.x && item.y == Pos.y))
                        {
                            if (controller.currentLayer.GetTile(Pos) != null && !controller.isShow)
                            {

                                controller.currentLayer.SetTile(Pos, null);
                            }
                            else if (!controller.isShow)
                            {
                                Object tmp = new();
                                foreach(GameObject go in gos)
                                {
                                    if (Pos == GetTilePos(go.transform.position))
                                    {
                                        tmp = go;
                                        break;
                                    }
                                }
                                if(tmp != null)
                                {
                                    GameManager.Destroy(tmp as GameObject);
                                }
                            }
                        }

                        break;
                }
            }
            else
            {
                Vector3 targetPos = controller.mapCenterObj.transform.position;
                var pastMousePosInField = GameManager.CameraManager.maincamera.ScreenToWorldPoint(pastPos);
                var MousePosInField = GameManager.CameraManager.maincamera.ScreenToWorldPoint(Pos);

                targetPos += (pastMousePosInField - MousePosInField) * 300;

                var backgroundBounds = GameManager.CameraManager.backgroundBounds;
                float minX = backgroundBounds.min.x + GameManager.CameraManager.cameraWidth / 2;
                float maxX = backgroundBounds.max.x - GameManager.CameraManager.cameraWidth / 2;
                float minY = backgroundBounds.min.y + GameManager.CameraManager.cameraHeight / 2;
                float maxY = backgroundBounds.max.y - GameManager.CameraManager.cameraHeight / 2;
                //Limit camera movement range

                targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
                targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
                controller.mapCenterObj.transform.position = new Vector3(targetPos.x, targetPos.y, 0);
                // Relatively smooth tracking of playr positions
            }

        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if(!clickedPos.Exists(item => item.x == Pos.x && item.y == Pos.y))
            clickedPos.Add(Pos);
            isClicked = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            clickedPos = new();
            isClicked = false;
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            isCenterClicked = true;
        }
        else
        {
            isCenterClicked = false;
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (controller.DataShowObj != null)
            {
                GameManager.Destroy(controller.DataShowObj);
                controller.DeleteInspector();
            }
        }
        pastPos = Pos;
        pastMousePos = mousePos;
    }


    private Vector3Int GetTilePos(Vector3 pos)
    {
        if (controller.currentLayer != null)
        {
            Vector3Int cellPos = controller.currentLayer.WorldToCell(pos);

            return cellPos;
        }
        else return new Vector3Int(-1, -1, 0);
    }

    private void AddTrigger(Vector3Int Pos, string type)
    {
        GameObject go = new GameObject();
        if (type.Equals("CutScene"))
        {
            go.AddComponent<CutSceneTrigger>().type = Trigger.TriggerType.CutScene;
        }
        else if (type.Equals("Spawn"))
        {
            go.AddComponent<SpawnTrigger>().type = Trigger.TriggerType.Spawn;

        }
        go.GetComponent<Trigger>().type = Trigger.TriggerType.CutScene;
        go.AddComponent<BoxCollider2D>();
        go.transform.SetParent(controller.currentMap.TriggerParent.transform);
        go.transform.position = controller.currentLayer.CellToWorld(Pos) + new Vector3(0.16f, 0.16f, 0);
        go.GetComponent<BoxCollider2D>().size = new Vector2(0.32f, 0.32f);
        go.GetComponent<BoxCollider2D>().isTrigger = true;
        clickObject = go;
        controller.SetInspector(clickObject);
    }

    private void TriggerSizeOnDraw(Vector3Int Pos)
    {
        var startPos = clickedPos[0];
        var endPos = Pos;
        var size = endPos - startPos;


        Debug.Log($"StartYPos {startPos.y} endYPos {endPos.y} size {size.y}");
        if (size.x < 0)
        {
            var tmp = startPos.x;
            startPos.x = endPos.x;
            endPos.x = tmp;

            size.x = -size.x;
        }
        if (size.y < 0)
        {
            var tmp = startPos.y;
            startPos.y = endPos.y;
            endPos.y = tmp;

            size.y = -size.y;
        }


        Debug.Log($"change StartYPos {startPos.y} endYPos {endPos.y} size {size.y}");

        clickObject.transform.position = (controller.currentLayer.CellToWorld(startPos) + controller.currentLayer.CellToWorld(endPos)) / 2 + new Vector3(0.16f, 0.16f, 0);

        clickObject.GetComponent<BoxCollider2D>().size = new Vector2(0.32f * (size.x + 1), 0.32f * (size.y + 1));

        controller.DrawTriggerLine(Color.red, clickObject.GetComponent<Trigger>());
    }
}
