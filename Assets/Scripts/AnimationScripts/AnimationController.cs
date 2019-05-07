using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    private Animation animationController;
    private CustomTypes.AnimationStateController previousState;
    private CustomTypes.AnimationStateController currentState;
    private float animationTime;
    private bool startTimer;

    public string walkAnimationName;
    public string idleAnimationName;
    public string divingAnimationName;
    public string jumpAnimationName;
    public string swimmingAnimationName;
    public string floatingAnimationName;
    public string drivingAnimationName;
    public string oneAttackAnimationName;    
    public string rangeAttackAnimationName;   
    public string deadAnimationName;
    public string talkAnimationName;

    // Use this for initialization
    void Start()
    {
        previousState = CustomTypes.AnimationStateController.IDLE;
        currentState = CustomTypes.AnimationStateController.NOTHING;
        animationController = GetComponent<Animation>();
        startTimer = false;          
    }

    // Update is called once per frame
   void Update()
    {      
        //Check if current state is different from previous state to change animation
        if (currentState != previousState)
        {
            previousState = currentState;
            //Switch animation based on state controller
            switch (currentState)
            {
                case CustomTypes.AnimationStateController.WALK:
                    animationController.CrossFade(walkAnimationName);
                    startTimer = false;
                    break;
                case CustomTypes.AnimationStateController.IDLE:
                    animationController.CrossFade(idleAnimationName);
                    startTimer = false;
                    break;
                case CustomTypes.AnimationStateController.DIVING:
                    animationController.CrossFade(divingAnimationName);
                    startTimer = false;
                    break;
                case CustomTypes.AnimationStateController.JUMP:
                    animationController.CrossFade(jumpAnimationName);
                    startTimer = false;
                    break;
                case CustomTypes.AnimationStateController.SWIM:
                    animationController.CrossFade(swimmingAnimationName);
                    startTimer = false;
                    break;
                case CustomTypes.AnimationStateController.FLOATING:
                    animationController.CrossFade(floatingAnimationName);
                    startTimer = false;
                    break;
                case CustomTypes.AnimationStateController.DRIVING:
                    animationController.CrossFade(drivingAnimationName);
                    startTimer = false;
                    break;
                case CustomTypes.AnimationStateController.MEELEATTACK:                   
                    animationController.CrossFade(oneAttackAnimationName);
                    animationTime = animationController.GetClip(oneAttackAnimationName).length;
                    startTimer = true;
                    break;               
                case CustomTypes.AnimationStateController.RANGEATTACK:                   
                    animationController.CrossFade(rangeAttackAnimationName);
                    animationTime = animationController.GetClip(rangeAttackAnimationName).length;                   
                    startTimer = true;
                    break;
                case CustomTypes.AnimationStateController.DEATH:
                    animationController.CrossFade(deadAnimationName);
                    animationTime = animationController.GetClip(deadAnimationName).length;
                    startTimer = true;
                    break;
                case CustomTypes.AnimationStateController.TALK:
                    animationController.CrossFade(talkAnimationName);                    
                    break;
            }
            //Check if we are in a state that needs a change of decision
            if (startTimer)
            {
                Invoke("StartNewTimer", animationTime);
            }

        }
    }

    public void AnimationStateChange(CustomTypes.AnimationStateController NewState)
    {
        currentState = NewState;
    }
}
