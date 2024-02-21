using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerController : KinematicObject
{
    // Command Input System Variable
    // Command Input List in System
    private List<KeyValuePair<KeyValues, float>> commandInputs;
    [SerializeField]
    private List<KeyValues> cmds;
    // Player Input List Last 3 seconds Freames
    private List<KeyValuePair<List<KeyCode>, float>> previousInputs;
    // Check Last Input Time
    private float lastInputTime;
    // For Correction Key Inputs 
    private List<KeyValues> inputBuffers;
    // Input Correction start Time
    private float lastProcessBufferT;
    // Is Start Correction
    private bool isDetectKey;



    /// <summary>
    /// Initial jump velocity at the start of a jump.
    /// </summary>
    public float jumpTakeOffSpeed = 7;
    public float jumpTime = 0f;
    private float maxLongJumpTime = 0.5f;

    /*internal new*/
    public bool controlEnabled = true;



    public Charactor charactor;
    public AsyncOperationHandle<RuntimeAnimatorController> bodySkinHandler, legSkinHandler;
    public Animator bodyAnimator, legAnimator;
    public bool isSetBody, isSetLeg;

    public Skill workingSkill;

    public string currentAnimationBody, currentAnimationLeg;

    public bool isHitState = false;
    public bool isImmune = false;

    public bool isAction = false;
    public bool isAttack = false;
    public float lastAttackT;
    public bool isAttackInput = false;

    public int currentSkin;

    [SerializeField]
    public string currentState;
    public bool isMove;

    public Vector2 presentSawDir;

    public bool isInit = false;

    public override void OnCreate()
    {
        base.OnCreate();
        
        body = GetComponent<Rigidbody2D>();

        commandInputs = new();
        previousInputs = new();
        inputBuffers = new();

        body.gravityScale = 1.0f;

        sawDir = new Vector2(1, 0);
        presentSawDir = new Vector2(1, 0);

        currentSkin = 0;
    }

    public void CreateHandler()
    {
        foreach(List<KeyValues> s in charactor.commands.Keys)
        {
            foreach(KeyValues a in s)
            {
                Debug.Log(a);
            }
            Debug.Log("");
        }


        charactor.playerController = this;

        gameObject.name = charactor.status.Name;

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
            if(velocity.x == 0)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            CommandInput();
            if(workingSkill != null)
            {
                workingSkill.Step();
            }

            if(charactor.passiveSkill.Count > 0)
            {
                foreach(Skill skill in charactor.passiveSkill)
                {
                    skill.PassiveStep();
                }
            }

            if (!controlEnabled && !isAction)
            {
                SetAlarm(2, 1f);
            }

            currentState = charactor.stateMachine.getStateStr();

            if (isAttackInput)
            {
                if (!isAction && !isAttack)
                {
                    if (IsGrounded)
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
                    SetAlarm(4, charactor.status.attackSpeed);
                }
            }
                        if (!isAction && IsGrounded && Mathf.Abs(velocity.x) == 0 && !isAttack)
            {
                charactor.SetIdle();
            }

            if (charactor != null)
            {
                charactor.Step();
            }
        }
    }

    public override void FlipX(bool isFlip)
    {
        bodyAnimator.GetComponent<SpriteRenderer>().flipX = isFlip;
        legAnimator.GetComponent<SpriteRenderer>().flipX = isFlip;
    }



    public override void KeyInput()
    {
        if (controlEnabled)
        {
            if ((GameManager.GetUIState() == UIManager.UIState.InPlay || true))
            {
                base.KeyInput();
                if (!isHitState)
                {
                    MoveKey();
                }

                List<KeyCode> InputKeys = GetPressedKey();

                if (InputKeys.Count != 0)
                {
                    lastInputTime = Time.time;
                }
                previousInputs.Add(new KeyValuePair<List<KeyCode>, float>(InputKeys, Time.time));

            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SetSkin(1);
        }
    }

    public float CalculateJumpForce()
    {
        // 스페이스바를 길게 누르는 동안의 시간에 따라 증가하는 힘 계산
        float normalizedJumpTime = Mathf.Clamp01(jumpTime / maxLongJumpTime);
        float jumpForce = jumpTakeOffSpeed * normalizedJumpTime;
        return jumpForce;
    }

    public override void CheckCollision(RaycastHit2D rh, bool yMovement)
    {
        base.CheckCollision(rh, yMovement);
        if(rh.collider.tag == "Enemy")
        {
            if (!isImmune)
            {
                Debug.Log("Hit");
                Bounce(new Vector2(-rh.collider.transform.position.x + transform.position.x * 4, Vector2.up.y * 3));
                isImmune = true;
                isHitState = true;

                SetAlarm(0, 2f);
                SetAlarm(1, 1f);
            }
        }
    }

    protected override void ComputeVelocity()
    {
        targetVelocity = moveAccel;
    }


    public List<KeyCode> GetPressedKey()
    {
        List<KeyCode> ret = new List<KeyCode>();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            ret.Add(KeyCode.UpArrow);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ret.Add(KeyCode.DownArrow);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ret.Add(KeyCode.LeftArrow);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ret.Add(KeyCode.RightArrow);
        }

        if (Input.GetKey(KeyCode.A))
        {
            ret.Add(KeyCode.A);
        }
        if (Input.GetKey(KeyCode.S))
        {
            ret.Add(KeyCode.S);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ret.Add(KeyCode.D);
        }

        return ret;
    }

    public List<KeyValues> GetKeyValues(List<KeyCode> kl)
    {
        List<KeyValues> ret = new List<KeyValues>();

        foreach (KeyCode key in kl)
        {
            if (key == KeyCode.UpArrow)
            {
                ret.Add(KeyValues.Up);
            }
            if (key == KeyCode.DownArrow)
            {
                ret.Add(KeyValues.Down);
            }

            if (key == KeyCode.LeftArrow)
            {
                ret.Add(KeyValues.Left);
            }
            if (key == KeyCode.RightArrow)
            {
                ret.Add(KeyValues.Right);
            }

            if (key == KeyCode.A)
            {
                ret.Add(KeyValues.Shot);
            }
            if (key == KeyCode.S)
            {
                ret.Add(KeyValues.Jump);
            }
            if (key == KeyCode.D)
            {
                ret.Add(KeyValues.Call);
            }
        }

        return ret;
    }

    public KeyValues GetKeyValue(List<KeyValues> kv)
    {
        KeyValues ret = new KeyValues();
        ret = KeyValues.None;
        foreach (KeyValues key in kv)
        {
            switch (key)
            {
                case KeyValues.Up:
                    ret += 1;
                    break;
                case KeyValues.Down:
                    ret += 2;
                    break;
                case KeyValues.Left:
                    ret += 4;
                    break;
                case KeyValues.Right:
                    ret += 8;
                    break;
                case KeyValues.Shot:
                    ret += 16;
                    break;
                case KeyValues.Jump:
                    ret += 32;
                    break;
                case KeyValues.Call:
                    ret += 64;
                    break;
            }
        }

        return ret;
    }

    public void CommandInput(KeyValues kv)
    {
        isDetectKey = false;

        if (kv != KeyValues.None && Enum.IsDefined(typeof(KeyValues), kv))
        {
            if (commandInputs.Count > 0)
            {
                if (commandInputs.Last().Key != kv)
                {
                    commandInputs.Add(new KeyValuePair<KeyValues, float>(kv, Time.time));
                    cmds.Add(kv);
                    CheckCommand();
                }
            }
            else
            {
                commandInputs.Add(new KeyValuePair<KeyValues, float>(kv, Time.time));
                cmds.Add(kv);
                CheckCommand();
            }

        }
    }


    public void CheckCommand()
    {
        List<KeyValues> cmd;
        bool isLeft;
        (cmd, isLeft) = CheckInputCommand(commandInputs, charactor.commands.Keys.ToList());
        if (cmd != null)
        {
            if (!isAction)
            {
                Debug.Log(charactor.commands[cmd].GetType().Name);
                if (isLeft)
                {
                    charactor.commands[cmd].Execute(new Vector2(-1, 0));

                }
                else
                {
                    charactor.commands[cmd].Execute(new Vector2(1, 0));

                }
            }
        }
        else
        {
            if((commandInputs.Last().Key & KeyValues.Shot) == KeyValues.Shot)
            {

                isAttackInput = true;
                SetAlarm(3, 0.2f);
            }
        }
    }

    public (List<KeyValues>, bool) CheckInputCommand(List<KeyValuePair<KeyValues, float>> inputCommand, List<List<KeyValues>> predefinedCommands)
    {
        List<(List<KeyValues>, bool)> findCommands = new List<(List<KeyValues>, bool)>();
        List<KeyValues> ret;
        bool isReverse;
        foreach (List<KeyValues> kv in predefinedCommands)
        {
            var (b1, b2) = CheckListsEnd(inputCommand, kv);
            if (b1)
            {
                findCommands.Add((kv, b2));
            }
        }
        if (findCommands.Count > 0)
        {
            (ret, isReverse) = findCommands.OrderByDescending(item => item.Item1.Count).ToList().First();
            return (ret, isReverse);
        }
        return (null, false);

    }

    public (bool, bool) CheckListsEnd(List<KeyValuePair<KeyValues, float>> a, List<KeyValues> b)
    {
        if (a.Count < b.Count) return (false, false);

        List<KeyValues> revKvs = Func.Reverse(b);
        bool isReverse;

        if ((a.Last().Key & b.Last()) == b.Last())
        {
            isReverse = false;
        }
        else if((a.Last().Key & revKvs.Last()) == revKvs.Last())
        {
            isReverse = true;
        }
        else
        {
            return (false, false);
        }
        if (isReverse)
        {
            for (int i = revKvs.Count; i > revKvs.Count; i--)
            {
                for (int j = a.Count - 1; ; j--)
                {
                    if (a[j].Key == KeyValues.O)
                    {
                        continue;
                    }
                    // out of index end check
                    if (j < 0)
                    {
                        return (false, false);
                    }

                    // if input delay is too late, end check
                    if (j >= 1 && a[j].Value - a[j - 1].Value >= 0.07f)
                    {
                        return (false, false);
                    }
                }

            }
        }
        else
        {
            for (int i = b.Count; i > b.Count; i--)
            {
                for (int j = a.Count - 1; ; j--)
                {
                    if (a[j].Key == KeyValues.O)
                    {
                        continue;
                    }
                    // out of index end check
                    if (j < 0)
                    {
                        return (false, false);
                    }

                    // if input delay is too late, end check
                    if (j >= 1 && a[j].Value - a[j - 1].Value >= 0.07f)
                    {
                        return (false, false);
                    }
                }

            }
        }



        if (isReverse)
            return (true, false);
        else
        {
            return (true, true);
        }
    }

    public override void Alarm0()
    {
        isImmune = false;
    }

    public override void Alarm1()
    {
        isHitState = false;
    }
    public override void Alarm2()
    {
        Addressables.ReleaseInstance(handle);
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

    public void MoveKey()
    {
        if (Input.GetKey(GameManager.Input._keySettings.upKey))
        {
            if (!isAction && !isAttack && canMove)
            {
                if(presentSawDir.Equals(sawDir))
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
                if(presentSawDir.Equals(sawDir))
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
            if (!isAction && !isAttack && canMove)
            {
                sawDir = new Vector2(-1, sawDir.y);
                presentSawDir.x = sawDir.x;
            }

        }
        else if (Input.GetKey(GameManager.Input._keySettings.rightKey))
        {
            if (!isAction && !isAttack && canMove)
            {
                sawDir = new Vector2(1, sawDir.y);
                presentSawDir.x = sawDir.x;
            }
        }
        if (Input.GetKeyDown(GameManager.Input._keySettings.Jump) && IsGrounded && canMove)
        {
            charactor.Jump();
            IsGrounded = false;
            jumpTime = 0f;
        }

        if (Input.GetKey(GameManager.Input._keySettings.Jump) && !IsGrounded && canMove)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime <= maxLongJumpTime)
            {
                float jumpF = CalculateJumpForce();

                velocity.y += jumpF * Time.deltaTime;
            }
        }
        if (Input.GetKey(GameManager.Input._keySettings.rightKey) && canMove)
        {
            if (moveAccel.x < 0)
            {
                moveAccel += new Vector2(1, 0) * Time.deltaTime * charactor.status.breakAccel;
            }
            else
            {
                if (Math.Abs(moveAccel.x) < charactor.status.activeMaxSpeed)
                {
                    moveAccel += new Vector2(1, 0) * Time.deltaTime * charactor.status.moveAccelSpeed;
                }
                else
                {
                    moveAccel.x = charactor.status.activeMaxSpeed;
                }
            }
        }
        else if (Input.GetKey(GameManager.Input._keySettings.leftKey) && canMove)
        {
            if (moveAccel.x > 0)
            {
                moveAccel -= new Vector2(1, 0) * Time.deltaTime * charactor.status.breakAccel;
            }
            else
            {
                if (Math.Abs(moveAccel.x) < charactor.status.activeMaxSpeed)
                {
                    moveAccel -= new Vector2(1, 0) * Time.deltaTime * charactor.status.moveAccelSpeed;
                }
                else
                {
                    moveAccel.x = -charactor.status.activeMaxSpeed;
                }
            }
        }
        else
        {
            if (Mathf.Abs(moveAccel.x) > 0.1f)
            {
                if (moveAccel.x < 0)
                    moveAccel += new Vector2(1, 0) * Time.deltaTime * charactor.status.breakAccel;
                else
                    moveAccel -= new Vector2(1, 0) * Time.deltaTime * charactor.status.breakAccel;
            }
            else
            {
                moveAccel.x = 0f;
            }

        }


    }

    public void CommandInput()
    {

        // Delete Input Buffers after 3 seconds
        if (previousInputs.Count > 0 && Time.time - previousInputs[0].Value >= 3f)
        {
            previousInputs.RemoveAt(0);
        }

        if (Time.time - lastInputTime >= 3f)
        {
            previousInputs.Clear();
            commandInputs.Clear();
        }

        if (previousInputs.Count >= 2)
        {
            if (previousInputs.Last().Key.Count == 0)
            {
                if (previousInputs[previousInputs.Count - 2].Key.Count != 0)
                {
                    CommandInput(GetKeyValue(inputBuffers));
                    CommandInput(KeyValues.O);
                }
            }
            else
            {
                if (!isDetectKey)
                {
                    lastProcessBufferT = Time.time;
                    inputBuffers = GetKeyValues(previousInputs.Last().Key);

                    CommandInput(GetKeyValue(inputBuffers));
                    isDetectKey = true;
                }
                else
                {
                    int previousInputValue = (int)GetKeyValue(inputBuffers);
                    if (previousInputValue > 0 && (previousInputValue & (previousInputValue - 1)) != 0)
                    {
                        CommandInput(GetKeyValue(inputBuffers));
                    }
                    else
                    {
                        List<KeyValues> tmp = new List<KeyValues>(inputBuffers);

                        List<KeyValues> keyValues = GetKeyValues(previousInputs.Last().Key);
                        foreach (KeyValues kv in keyValues)
                        {
                            if (!tmp.Contains(kv))
                            {
                                tmp.Add(kv);
                            }
                        }
                        if (Enum.IsDefined(typeof(KeyValues), (KeyValues)previousInputValue))
                        {
                            inputBuffers = tmp;
                        }
                        else
                        {
                            CommandInput(GetKeyValue(inputBuffers));
                        }

                    }
                }
            }
        }

        if (isDetectKey && Time.time - lastProcessBufferT >= 0.1f)
        {
            CommandInput(GetKeyValue(inputBuffers));
        }
    }

    public void SetSkin(int index)
    {
        if(bodySkinHandler.IsValid())
            Addressables.Release(bodySkinHandler);
        if (legSkinHandler.IsValid())
            Addressables.Release(legSkinHandler);
        currentSkin = index;

        Addressables.LoadAssetAsync<RuntimeAnimatorController>(charactor.status.skins[currentSkin]+"Body").Completed += handle =>
        {
            bodySkinHandler = handle;
            bodyAnimator.runtimeAnimatorController = handle.Result;
            isSetBody = true;
        };
        Addressables.LoadAssetAsync<RuntimeAnimatorController>(charactor.status.skins[currentSkin] + "Leg").Completed += handle =>
        {
            legSkinHandler = handle;
            legAnimator.runtimeAnimatorController = handle.Result;
            isSetLeg = true;
        };
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

    public void AnimationPlayLeg(string clip, float spd = 1)
    {
        if (isSetLeg)
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

        }
    }
}


