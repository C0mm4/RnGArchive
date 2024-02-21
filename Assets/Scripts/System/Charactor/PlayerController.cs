using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEditor.Progress;

public class PlayerController : KinematicObject
{
    // Command Input System Variable
    // Command Input List in System
    private List<(KeyValues, float)> commandInputs;
    [SerializeField] List<KeyValues> cmds;
    // Check Last Input Time
    private float lastInputTime;
    // Input Correction start Time
    private float lastProcessBufferT;
    // Is Start Correction
    private bool isDetectKey;

    private List<(KeyValues, float)> inputbuffers;
    private List<(KeyValues, float)> previousinputbuffers;

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
    public bool isSit;

    public Vector2 presentSawDir;

    public bool isInit = false;

    public override void OnCreate()
    {
        base.OnCreate();
        
        body = GetComponent<Rigidbody2D>();

        inputbuffers = new();
        previousinputbuffers = new();
        commandInputs = new();
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
            SetCommandBuffer();
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
                var items = setInputBuffer();
                inputbuffers.AddRange(items.Where(item => !inputbuffers.Any(x => x.Item1 == item.Item1)));
                if (inputbuffers.Count > 0)
                {
                    lastInputTime = Time.time;
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SetSkin(1);
        }
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
            if(commandInputs.Count > 0)
            {
                if(commandInputs.Last().Item1 != KeyValues.O)
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
                        if(checkCombine(commandInputs.Last().Item1, inputbuffers[i].Item1))
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
                                KeyValues kv = (KeyValues)(int)commandInputs.Last().Item1 + (int)inputbuffers[i+1].Item1;
                                commandInputs.Add((kv, inputbuffers[i+1].Item2));
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
        Debug.Log(kv1 + " " +  kv2);
        if(kv1 == kv2)
        {
            return false;
        }
        if(Func.arrows.Contains(kv2)) 
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
        if(cmd != null)
        {
            Debug.Log(charactor.commands[cmd].GetType().Name);
        }
        else
        {
            if((commandInputs.Last().Item1 & KeyValues.Shot) == KeyValues.Shot)
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

        foreach(List<KeyValues> cmds in charactor.commands.Keys.ToList())
        {
            var (b1, b2) = CheckCommandsLastEnd(commandInputs, cmds);
            if (b1)
            {
                findCommands.Add((cmds, b2));
            }
        }

        if(findCommands.Count > 0)
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
            for(int i = predefinedCmds.Count - 1; i >= 0; i--)
            {
                for(int j = inputs.Count - 1; i >= 0; j--)
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
                Bounce(new Vector2((-rh.collider.transform.position.x + transform.position.x) * 4, Vector2.up.y * 3));
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
        isSit = false;
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


