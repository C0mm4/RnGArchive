using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

// Custom Mono Behaviour Class
// extend Object class for new Mono Behaviour Class
// Override the virtual methods of the Object class for custom functionality
public abstract class Obj : MonoBehaviour
{
    public delegate void Alarm();
    public Alarm[] alarms;
    public bool[] isAlarmRun;
    [SerializeField]
    private float[] alarmTime;

    [SerializeField]
    public string currentAnimation;

    // Start is called before the first frame update
    public void Awake()
    {
        alarms = new Alarm[]
        {
            Alarm0, Alarm1, Alarm2, Alarm3, Alarm4, Alarm5, Alarm6, Alarm7, Alarm8, Alarm9
        };

        isAlarmRun = new bool[alarms.Length];
        alarmTime = new float[alarms.Length];
        OnCreate();
    }

    void FixedUpdate()
    {
        if (!GameManager.isPaused) 
        {
            BeforeStep();

        }
    }

    // Update is called once per frame
    // Objects inheriting from the Object class do not declare the Update method.
    public virtual void Update()
    {
        if (!GameManager.isPaused)
        {
            KeyInput();
            Step();
        }
    }

    private void LateUpdate()
    {
        if(!GameManager.isPaused)
        {
            AfterStep();
        }
    }

    public virtual void OnCreate()
    {
        // Custom code to execute during object creation
    }

    // In each frame (On Update), the following steps are executed in order: BeforeStep > KeyInput > Step > AfterStep
    // Override the code to be used in this section.

    public virtual void BeforeStep()
    {
        // Add your code for Before Step here
    }

    public virtual void Step()
    {
        
        for(int i = 0; i < alarms.Length; i++)
        {
            if (isAlarmRun[i])
            {
                alarmTime[i] -= Time.deltaTime;

                if(alarmTime[i] <= 0) 
                {
                    isAlarmRun[i] = false;
                    alarms[i].Invoke();
                }
            }
        }
    }

    public virtual void AfterStep()
    {
        // Add your code for After Step here
    }

    public virtual void KeyInput()
    {
        // Describe the behavior of keyboard input here
    }

    // Execute the alarm at index 'index' after the specified 'time'.
    // No additional access is possible during the execution of the alarm.
    public void SetAlarm(int index, float time = 0)
    {
        if(time == 0)
        {
            alarms[1].Invoke();
        }
        else if (!isAlarmRun[index]) 
        {
            isAlarmRun[index] = true;
            alarmTime[index] = time;
        }
    }

    public virtual void Alarm0() { }
    public virtual void Alarm1() { }
    public virtual void Alarm2() { }
    public virtual void Alarm3() { }
    public virtual void Alarm4() { }
    public virtual void Alarm5() { }
    public virtual void Alarm6() { }
    public virtual void Alarm7() { }
    public virtual void Alarm8() { }
    public virtual void Alarm9() { }

    public void Destroy()
    {
        GameManager.Destroy(gameObject);
    }

    public void AnimationPlay(Animator animator, string clip, float spd = 1f)
    {
        if(animator != null)
        {
            if (!clip.Equals(currentAnimation))
            { 
                if(System.Array.Exists(animator.runtimeAnimatorController.animationClips.ToArray(), findClip => findClip.name.Equals(clip)))
                {
                    currentAnimation = clip;
                    animator.speed = spd;
                    animator.Play(clip);
                }
                else
                {
                    Debug.Log($"Can't Find Clip : {clip}");
                }
            }


        }
    }
}
