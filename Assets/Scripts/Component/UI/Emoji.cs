using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Emoji : Obj
{
    public Animator animator;
    public Animator emojiAnimator;
    public GameObject origin;

    public async Task CreateHandler(string emojiName)
    {
        animator.Play("Generate");
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        emojiAnimator.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>(emojiName);
        emojiAnimator.Play("Play");

        var clip = emojiAnimator.GetCurrentAnimatorClipInfo(0);
        var length = clip.Length;

        await Task.Delay(TimeSpan.FromSeconds(length));

        GameManager.Destroy(origin);
    }
}
