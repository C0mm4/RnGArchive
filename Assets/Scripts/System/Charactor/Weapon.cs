using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Weapon : Obj
{
    public PlayerController player;

    public Animator animator;

    public GameObject muzzle;
    public string attackPref;
    public string fireEffectPref;

    public GameObject fireEffect;

    public bool isReload = false;

    public bool isCharge;


    public async void Fire()
    {
            player.isAttack = true;
            AnimationPlay(animator, "Fire");

            var awaitGo = GameManager.InstantiateAsync(attackPref, muzzle.transform.position, player.transform.rotation);
            GameObject go = awaitGo;
            go.GetComponent<Attack>().CreateHandler(2, player.sawDir, player.charactor.atkType);

            fireEffect = GameManager.InstantiateAsync(fireEffectPref, muzzle.transform.position, player.transform.rotation);
            fireEffect.transform.SetParent(muzzle.transform, true);

            await Task.Delay(TimeSpan.FromSeconds(player.charactor.charaData.attackSpeed));

            player.isAttack = false;

            if (player.charactor.charaData.currentAmmo == 0)
            {
                Reload();
            }

    }

    public void SetIdleAnimation()
    {
        AnimationPlay(animator, "Idle");
    }

    public async void Reload() 
    {
        isReload = true;
        if (!isCharge)
        {
            player.charactor.charaData.currentAmmo = 0;
        }
        float t = 0;

        while (t < 0.1f)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 20), t * 10);
            await Task.Yield();
        }

        transform.localRotation = Quaternion.Euler(0, 0, 20);
        await Task.Yield();
        float loadT = 0f;
        while(player.charactor.charaData.currentAmmo < player.charactor.charaData.maxAmmo)
        {
            loadT += Time.deltaTime;
            if(loadT >= player.charactor.charaData.reloadT)
            {
                loadT -= player.charactor.charaData.reloadT;
            }

            await Task.Delay(TimeSpan.FromSeconds(player.charactor.charaData.reloadT));
            player.charactor.charaData.currentAmmo++;
        }

        float downT = 0f;
        await Task.Yield();
        while (downT < 0.1f)
        {
            downT += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 20), Quaternion.Euler(0, 0, 0), downT * 10);
            await Task.Yield();
        }

        transform.localRotation = Quaternion.Euler(0, 0, 0);

        player.charactor.charaData.currentAmmo = player.charactor.charaData.maxAmmo;
        SetIdleAnimation();
        isReload = false;
    }

}
