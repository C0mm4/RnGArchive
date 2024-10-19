using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CutSceneTrigger : Trigger
{
    public override async Task Action()
    {
        await ScriptPlay();
        ActiveSpawnObjs();
    }

    public override bool AdditionalCondition()
    {
        return true;
    }

    public void drawLine(Color color)
    {
        LineRenderer line = GetComponent<LineRenderer>();

        line.loop = true;
        Vector3[] positions = new Vector3[4];

        positions[0] = transform.position + new Vector3(-GetComponent<BoxCollider2D>().size.x/2, -GetComponent<BoxCollider2D>().size.y/2);
        positions[1] = transform.position + new Vector3(GetComponent<BoxCollider2D>().size.x / 2, -GetComponent<BoxCollider2D>().size.y / 2);
        positions[2] = transform.position + new Vector3(GetComponent<BoxCollider2D>().size.x / 2, GetComponent<BoxCollider2D>().size.y / 2);
        positions[3] = transform.position + new Vector3(-GetComponent<BoxCollider2D>().size.x / 2, GetComponent<BoxCollider2D>().size.y / 2);


        line.startColor = color;
        line.endColor = color;
        line.SetPositions(positions);
    }

}
