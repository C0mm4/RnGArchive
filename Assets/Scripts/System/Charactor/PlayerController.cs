using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerController : RigidBodyObject
{
    public bool controlEnabled;


    public bool isHitState = false;
    public bool isImmune = false;

    public bool _isAction = false;
    public bool isAction { get { return _isAction; } set { _isAction = value; } }
    public bool isAttack = false; 
    public bool isMove;
    public bool isSit;
    public bool isInit = false;
    public float lastAttackT;
    public bool isAttackInput = false;

    public Charactor charactor;

    public Vector2 presentSawDir;


    public List<InteractionTrigger> triggers;
    public int triggerIndex;

    public Animator bodyAnimator, legAnimator, backHairAnimator, haloAnimation;
    public string currentAnimationBody, currentAnimationLeg;
    public bool isSetBody, isSetLeg, isSetBackHair, isSetHalo;
    public int currentSkin;

    public Skill workingSkill;


    public string currentState;


    // Command Input System Variable
    // Command Input List in System
    private List<(KeyValues, float)> commandInputs;
    [SerializeField] List<KeyValues> cmds;

    private List<(KeyValues, float)> inputbuffers;
    private List<(KeyValues, float)> previousinputbuffers;


    public override void OnCreate()
    {
        base.OnCreate();


        inputbuffers = new();
        previousinputbuffers = new();
        commandInputs = new();
        triggers = new();
        gravityModifier = 1f;

        sawDir = new Vector2(1, 0);
        presentSawDir = new Vector2(1, 0);

        currentSkin = 0;

        int layer = LayerMask.NameToLayer("Player");
        gameObject.layer = layer;
    }

    public void CreateHandler()
    {
        charactor.playerController = this;

        gameObject.name = charactor.charaData.Name;

        charactor.stateMachine = new(this);

        charactor.ChangeState(new Idle());

        SetSkin(currentSkin);
        isInit = true;
    }


    public override void Step()
    {
        if (isInit)
        {
            base.Step();
            if (body.velocity.x == 0)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            if (GameManager.Progress.isActiveSkill)
            {
                if (workingSkill != null)
                {
                    workingSkill.Step();
                }

                if (charactor.passiveSkill.Count > 0)
                {
                    foreach (Skill skill in charactor.passiveSkill)
                    {
                        skill.PassiveStep();
                    }
                }
            }

            if (!controlEnabled && !isAction)
            {
                SetAlarm(2, 1f);
            }
            if (canMove)
            {
                SetCommandBuffer();
            }
            currentState = charactor.stateMachine.getStateStr();

            if (isAttackInput)
            {
                if (!isAction && !isAttack)
                {
                    if (isGrounded)
                    {
                        if (Input.GetKey(GameManager.Input._keySettings.leftKey))
                        {
                            sawDir = new Vector2(-1, sawDir.y);
                            presentSawDir.x = sawDir.x;
                        }
                        else if (Input.GetKey(GameManager.Input._keySettings.rightKey))
                        {
                            sawDir = new Vector2(1, sawDir.y);
                            presentSawDir.x = sawDir.x;
                        }
                    }
                    charactor.Attack();
                    SetAlarm(4, charactor.charaData.attackSpeed);
                }
            }
            if (!isAction && isGrounded && !isMove && !isAttack)
            {
                charactor.SetIdle();
            }

            if (charactor != null)
            {
                charactor.Step();
            }
        }
    }


    public override void KeyInput()
    {
        if((GameManager.GetUIState() == UIState.InPlay) || true)
        {
            if (!isForceMoving)
            {
                base.KeyInput();
                if (!isHitState)
                {
                    MoveKey();
                }
                var items = setInputBuffer();
                inputbuffers.AddRange(items.Where(item => !inputbuffers.Any(x => x.Item1 == item.Item1)));

                if (Input.GetKeyDown(GameManager.Input._keySettings.Interaction))
                {
                    triggers[triggerIndex].Interaction();
                }
            }
            else
            {
                Vector2 dir = new Vector2();
                if(targetMovePos.x > transform.position.x)
                {
                    dir.x = 1;
                }
                else
                {
                    dir.x = -1;
                }
                body.velocity = new Vector2(dir.x, body.velocity.y);
            }
        }
    }

    public void MoveKey()
    {
        isSit = false;
        if (Input.GetKey(GameManager.Input._keySettings.upKey))
        {
            if(!isAction && !isAttack && canMove)
            {
                if (presentSawDir.Equals(sawDir))
                {
                    presentSawDir.x = sawDir.x;
                }
                charactor.Up();
            }
        }
        else if (Input.GetKey(GameManager.Input._keySettings.downKey))
        {
            if (!isAction && !isAttack && canMove)
            {
                if (presentSawDir.Equals(sawDir))
                    presentSawDir.x = sawDir.x;
                charactor.Down();
            }
        }

        else
        {
            sawDir = presentSawDir;
        }
        if (Input.GetKey(GameManager.Input._keySettings.leftKey))
        {
            if (canMove)
            {
                body.velocity = new Vector2(-charactor.charaData.activeMaxSpeed, body.velocity.y);
                if (!isAction && !isAttack)
                {
                    sawDir = new Vector2(-1, sawDir.y);
                    presentSawDir.x = sawDir.x;
                }
            }

        }
        else if (Input.GetKey(GameManager.Input._keySettings.rightKey))
        {
            if (canMove)
            {
                body.velocity = new Vector2(charactor.charaData.activeMaxSpeed, body.velocity.y);
                if (!isAction && !isAttack)
                {
                    sawDir = new Vector2(1, sawDir.y);
                    presentSawDir.x = sawDir.x;
                }
            }

        }
        else
        {
            body.velocity = new Vector2(0f , body.velocity.y);
        }

        if (Input.GetKeyDown(GameManager.Input._keySettings.Jump) && isGrounded && canMove)
        {
            charactor.Jump();



            isGrounded = false;
        }

    }

    public override void FlipX()
    {
        bool isFlip;
        if(sawDir.x > 0f)
        {
            isFlip = false;
        }
        else
        {
            isFlip = true;
        }
        bodyAnimator.GetComponent<SpriteRenderer>().flipX = isFlip;
        legAnimator.GetComponent<SpriteRenderer>().flipX = isFlip;
        backHairAnimator.GetComponent<SpriteRenderer>().flipX = isFlip;
    }

    private List<(KeyValues, float)> setInputBuffer()
    {
        List<(KeyValues, float)> ret = new List<(KeyValues, float)>();
        float time = Time.time;
        if (Input.GetKey(GameManager.Input._keySettings.upKey))
        {
            ret.Add((KeyValues.Up, time));
        }
        else if (Input.GetKey(GameManager.Input._keySettings.downKey))
        {
            ret.Add((KeyValues.Down, time));
        }

        if (Input.GetKey(GameManager.Input._keySettings.leftKey))
        {
            ret.Add((KeyValues.Left, time));
        }
        else if (Input.GetKey(GameManager.Input._keySettings.rightKey))
        {
            ret.Add((KeyValues.Right, time));
        }

        if (Input.GetKey(GameManager.Input._keySettings.Shot))
        {
            ret.Add((KeyValues.Shot, time));
        }
        if (Input.GetKey(GameManager.Input._keySettings.Jump))
        {
            ret.Add((KeyValues.Jump, time));
        }
        if (Input.GetKey(GameManager.Input._keySettings.Call))
        {
            ret.Add((KeyValues.Call, time));
        }

        return ret;
    }

    private void SetCommandBuffer()
    {
        if (inputbuffers.Count == 0)
        {
            if (commandInputs.Count > 0)
            {
                if (commandInputs.Last().Item1 != KeyValues.O)
                {
                    commandInputs.Add((KeyValues.O, Time.time));
                    cmds.Add(KeyValues.O);
                    CheckCommands();
                }
            }
        }
        else
        {
            var tmp = inputbuffers.ToList();
            if (commandInputs.Count > 0)
            {
                if (commandInputs.Last().Item1 != KeyValues.O)
                {
                    inputbuffers.RemoveAll(x => previousinputbuffers.Any(y => y.Item1 == x.Item1));
                }
            }
            previousinputbuffers.Clear();
            previousinputbuffers.AddRange(tmp);
            for (int i = 0; i < inputbuffers.Count; i++)
            {
                // if First Input, Set Buffer Data
                if (commandInputs.Count == 0)
                {
                    commandInputs.Add((inputbuffers[i].Item1, inputbuffers[i].Item2));
                    cmds.Add(inputbuffers[i].Item1);
                    CheckCommands();
                }
                else
                {
                    // if Last Input is Nearby (6 frame), Combine Input Insert
                    if (inputbuffers[i].Item2 - commandInputs.Last().Item2 <= .1f)
                    {
                        if (checkCombine(commandInputs.Last().Item1, inputbuffers[i].Item1))
                        {
                            KeyValues kv = (KeyValues)(int)commandInputs.Last().Item1 + (int)inputbuffers[i].Item1;
                            commandInputs.Add((kv, inputbuffers[i].Item2));
                            cmds.Add(kv);
                            CheckCommands();
                        }
                    }
                    commandInputs.Add((inputbuffers[i].Item1, inputbuffers[i].Item2));
                    cmds.Add(inputbuffers[i].Item1);
                    CheckCommands();
                    // Last Input is not input buffer, input insert
                    if (commandInputs.Last().Item1 != inputbuffers[i].Item1)
                    {
                        commandInputs.Add((inputbuffers[i].Item1, inputbuffers[i].Item2));
                        cmds.Add(inputbuffers[i].Item1);
                        CheckCommands();
                    }
                    if (i < inputbuffers.Count - 1)
                    {
                        if (inputbuffers[i + 1].Item2 - inputbuffers[i].Item2 <= .1f)
                        {
                            if (checkCombine(inputbuffers[i].Item1, inputbuffers[i + 1].Item1))
                            {
                                KeyValues kv = (KeyValues)(int)commandInputs.Last().Item1 + (int)inputbuffers[i + 1].Item1;
                                commandInputs.Add((kv, inputbuffers[i + 1].Item2));
                                cmds.Add(kv);
                                CheckCommands();
                            }
                            else
                            {
                                commandInputs.Add((KeyValues.O, inputbuffers[i].Item2));
                                cmds.Add(KeyValues.O);
                                CheckCommands();
                            }
                        }
                    }
                }
            }
        }

        inputbuffers.Clear();

    }

    private bool checkCombine(KeyValues kv1, KeyValues kv2)
    {
        if (kv1 == kv2)
        {
            return false;
        }
        if (Func.arrows.Contains(kv2))
        {
            if (Func.arrows.Contains(kv1))
            {
                return true;
            }
            else return false;
        }
        if (Func.actions.Contains(kv2))
        {
            if (Func.directions.Contains(kv1))
            {
                return true;
            }
            else return false;
        }
        return false;
    }

    private void CheckCommands()
    {
        List<KeyValues> cmd = null;
        bool isLeft;
        (cmd, isLeft) = CheckCommandInputs();
        if (cmd != null)
        {
            Debug.Log(charactor.commands[cmd].name);
            if (isLeft)
            {
                charactor.commands[cmd].Execute(new Vector2(-1, 0));

            }
            else
            {
                charactor.commands[cmd].Execute(new Vector2(1, 0));
            }
        }
        else
        {
            if ((commandInputs.Last().Item1 & KeyValues.Shot) == KeyValues.Shot)
            {
                isAttackInput = true;
                SetAlarm(3, 0.2f);
            }
        }
    }

    private (List<KeyValues>, bool) CheckCommandInputs()
    {
        List<(List<KeyValues>, bool)> findCommands = new List<(List<KeyValues>, bool)>();

        List<KeyValues> ret;
        bool isLeft;

        foreach (List<KeyValues> cmds in charactor.commands.Keys.ToList())
        {
            var (b1, b2) = CheckCommandsLastEnd(commandInputs, cmds);
            if (b1)
            {
                findCommands.Add((cmds, b2));
            }
        }

        if (findCommands.Count > 0)
        {
            (ret, isLeft) = findCommands.OrderByDescending(item => item.Item1.Count).ToList().First();
            return (ret, isLeft);
        }

        return (null, false);
    }

    private (bool, bool) CheckCommandsLastEnd(List<(KeyValues, float)> inputs, List<KeyValues> predefinedCmds)
    {
        if (inputs.Count < predefinedCmds.Count) return (false, false);
        List<KeyValues> reverseCmds = Func.Reverse(predefinedCmds);
        bool isLeft;

        // Check command is Left command
        if ((inputs.Last().Item1 & predefinedCmds.Last()) == predefinedCmds.Last())
        {
            isLeft = false;
        }
        else if ((inputs.Last().Item1 & reverseCmds.Last()) == reverseCmds.Last())
        {
            isLeft = true;
        }
        else
        {
            return (false, false);
        }

        // If Forward
        if (!isLeft)
        {
            for (int i = predefinedCmds.Count - 1; i >= 0; i--)
            {
                for (int j = inputs.Count - 1; j >= 0; j--)
                {
                    if (inputs[j].Item1 == KeyValues.O)
                    {
                        continue;
                    }

                    if (predefinedCmds[i] == inputs[j].Item1)
                    {
                        break;
                    }


                }
            }
        }
        // If Reverse
        else
        {
            return (false, true);
        }

        if (isLeft)
        {
            return (true, true);
        }
        else
        {
            return (true, false);
        }
    }



    public override void CheckCollision(Collision2D obj)
    {
        base.CheckCollision(obj);
        if (obj.gameObject.tag == "Enemy")
        {
            if (!isImmune)
            {
                GetDmg(obj.gameObject);
            }
        }
    }

    public void GetDmg(GameObject obj)
    {
        HPDecrease(1);
        body.AddForce((new Vector2((-obj.transform.position.x + transform.position.x) * 4, Vector2.up.y * 4)), ForceMode2D.Impulse);
        isImmune = true;
        isHitState = true;
        Physics2D.IgnoreLayerCollision(7,8, true);
        SetAlarm(0, 2f);
        SetAlarm(1, 1f);
    }

    public override void Alarm0()
    {
        isImmune = false;
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    public override void Alarm1()
    {
        isHitState = false;
    }
    public override void Alarm2()
    {
        Addressables.ReleaseInstance(gameObject);
    }

    public override void Alarm3()
    {
        isAttackInput = false;
    }

    public override void Alarm4()
    {
        isAttack = false;
    }

    public override void Alarm5()
    {
        isAction = false;
    }



    public void SetSkin(int index)
    {
        currentSkin = index;


        bodyAnimator.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>(charactor.charaData.skins[currentSkin] + "Body");
        isSetBody = true;
        legAnimator.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>(charactor.charaData.skins[currentSkin] + "Leg");
        isSetLeg = true;
        backHairAnimator.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>(charactor.charaData.skins[currentSkin] + "Back");
        isSetBackHair = true;
        haloAnimation.runtimeAnimatorController = GameManager.LoadAssetDataAsync<RuntimeAnimatorController>(charactor.charaData.skins[currentSkin] + "Halo");
        isSetHalo = true;

    }

    public void AnimationPlayBody(string clip, float spd = 1)
    {
        if (isSetBody)
        {
            if (clip != currentAnimationBody)
            {
                if (System.Array.Exists(bodyAnimator.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
                {
                    currentAnimationBody = clip;
                    bodyAnimator.speed = spd;
                    bodyAnimator.Play(clip);
                }
                else
                {
                    Debug.Log($"Can't Find Clip : {clip}");
                }
            }

        }
    }

    public void AnimationPlayLeg(string clip, float spd = 1, bool isReverse = false)
    {
        if (isSetLeg)
        {
            if (isReverse)
            {
                if (clip != currentAnimationLeg)
                {
                    if (System.Array.Exists(legAnimator.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
                    {
                        var stateinfo = legAnimator.GetCurrentAnimatorStateInfo(0);
                        float normalizeT = stateinfo.normalizedTime;
                        float animationLength = stateinfo.length;

                        currentAnimationLeg = clip;
                        legAnimator.speed = spd;
                        legAnimator.Play(clip, 0, normalizeT / animationLength);
                    }
                    else
                    {
                        Debug.Log($"Can't Find Clip : {clip}");
                    }
                }
                else
                {
                    legAnimator.speed = spd;
                }
            }
            else
            {
                if (clip != currentAnimationLeg)
                {
                    if (System.Array.Exists(legAnimator.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
                    {
                        currentAnimationLeg = clip;
                        legAnimator.speed = spd;
                        legAnimator.Play(clip);
                    }
                    else
                    {
                        Debug.Log($"Can't Find Clip : {clip}");
                    }
                }
                else
                {
                    legAnimator.speed = spd;
                }

            }

        }
    }

    public void AnimationPlayHair(string clip, float spd = 1)
    {
        if (isSetBackHair)
        {
            if (clip != currentAnimationLeg)
            {
                if (System.Array.Exists(backHairAnimator.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
                {
                    currentAnimationLeg = clip;
                    backHairAnimator.speed = spd;
                    backHairAnimator.Play(clip);
                }
                else
                {
                    Debug.Log($"Can't Find Clip : {clip}");
                }
            }
            else
            {
                backHairAnimator.speed = spd;
            }
        }
    }

    public void AnimationPlayHalo(string clip, float spd = 1)
    {
        if (isSetHalo)
        {
            if (clip != currentAnimationLeg)
            {
                if (System.Array.Exists(haloAnimation.runtimeAnimatorController.animationClips.ToArray(), findclip => findclip.name == clip))
                {
                    currentAnimationLeg = clip;
                    haloAnimation.speed = spd;
                    haloAnimation.Play(clip);
                }
                else
                {
                    Debug.Log($"Can't Find Clip : {clip}");
                }
            }
            else
            {
                haloAnimation.speed = spd;
            }
        }
    }

    public void AddInterractionTrigger(InteractionTrigger trigger)
    {
        if (!triggers.Contains(trigger))
        {
            triggers.Add(trigger);
        }
    }

    public void RemoveInteractionTrigger(InteractionTrigger trigger)
    {
        if (triggers.Contains(trigger))
        {
            triggers.Remove(trigger);
            if (triggerIndex >= triggers.Count)
            {
                triggerIndex = triggers.Count - 1;
            }
            if (triggerIndex < 0)
                triggerIndex = 0;
        }
    }

    public void GenInteractionUI()
    {

    }

    public void DeleteInteractionUI()
    {

    }

    public void HPIncrease(int value)
    {
        charactor.charaData.currentHP += value;
        if (charactor.charaData.currentHP > charactor.charaData.maxHP)
        {
            charactor.charaData.currentHP = charactor.charaData.maxHP;
        }
    }

    public void HPDecrease(int value)
    {
        charactor.charaData.currentHP -= value;
        Debug.Log(GameManager.CharaCon.charactors[GameManager.Stage.currentCharactor.charaData.id].charaData.currentHP);
    }
}
