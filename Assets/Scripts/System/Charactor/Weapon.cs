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

    public async void Fire()
    {
        AnimationPlay(animator, "Fire");
        var awaitGo = GameManager.InstantiateAsync(attackPref, muzzle.transform.position, player.transform.rotation);
        GameObject go = awaitGo;
        go.GetComponent<Attack>().CreateHandler(2, player.sawDir, player.charactor.atkType);

        fireEffect = GameManager.InstantiateAsync(fireEffectPref, muzzle.transform.position, player.transform.rotation);
        fireEffect.transform.SetParent(muzzle.transform, true);

        await Task.Delay(TimeSpan.FromSeconds(player.charactor.charaData.attackSpeed));

        if (currentAnimation.Equals("Fire"))
        {
            SetIdleAnimation();
        }

    }

    public void SetIdleAnimation()
    {
        AnimationPlay(animator, "Idle");
    }
}
