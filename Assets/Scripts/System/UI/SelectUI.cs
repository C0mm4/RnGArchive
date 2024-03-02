using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SelectUI : Obj
{
    [SerializeField]
    GameObject ButtonObj;

    List<SelectButton> buttons;

    int listSize;
    public int selectIndex;

    public override void OnCreate()
    {
        base.OnCreate();
        transform.SetParent(GameManager.UIManager.canvas.transform, false);
        selectIndex = -1;
    }

    public void CreateHandler(int size, List<string> txts)
    {
        for(int i = 0; i < size; i++)
        {
            GameObject go = GameManager.InstantiateAsync("SelectButton");
            SelectButton sb = go.GetComponent<SelectButton>();
            sb.CreateHandler(i, txts[i]);
            sb.SetUI(this);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3();

        }
    }

    public async Task<int> Select()
    {
        while(selectIndex == -1)
        {
            await Task.Yield();
        }
        GameManager.Destroy(gameObject);
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        return selectIndex;
    }
}
