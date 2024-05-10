using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BossAlter : Obj
{
    public Image topScroll;
    public Image bottomScroll;
    public Image warning, alter;

    public async override void OnCreate()
    {
        base.OnCreate();
        await ShowWarning();
    }


    public override void Step()
    {
        base.Step();

    }

    public async Task ShowWarning()
    {
        Color alpha = Color.white;
        float t = 0;
        while (t <= 3f)
        {
            t += Time.deltaTime;

            if(t <= 3)
            {
                // 0.8 seconds alpha down
                if(t - Mathf.Floor(t) < 0.9f)
                {
                    alpha.a = (1 - ((t - Mathf.Floor(t)) / 0.8f) * 0.7f);

                }
                else
                {
                    Debug.Log(alpha.a);
                    Mathf.Lerp(alpha.a, 1, 1 - t - Mathf.Floor(t));
                }
            }
            topScroll.color = alpha;
            bottomScroll.color = alpha;
            warning.color = alpha;
            alter.color = alpha;
            await Task.Yield();
        }

        Destroy();
    }
}
