
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public Animation anim;
    public AnimationState state;
    private string name;
    public float frame;
    public float length;
    public float fps = 100f;

    [HideInInspector]
    private List<Anim> animations;
    public int currentFrame;
    private int nextFrame;
    public Anim currAnimation;
    private int lastanimation;
    public int currentAnimation;
    private int rollto_anim;
    private bool RollOver;
    private float lastTime;
    private float lerptime;
    public bool manual = false;

	void Start () {

        //anim.clip.frameRate = 0;
        fps = anim.clip.frameRate;
        
        length = anim.clip.length;
        name = anim.clip.name;
        state = anim[name];
        state.speed = 0;
        state.wrapMode = WrapMode.Clamp;

	
	animations = new List<Anim> ();
	
		lastanimation = -1;
		currentAnimation = 0;
		rollto_anim = 0;
		RollOver=false;
		currentFrame = 0;
		lastTime = tickcount ();
	
		animations.Add(new Anim("walk", 2, 14, 60));
        animations.Add(new Anim("run", 16, 26, 60));
        animations.Add(new Anim("jump", 42, 54, 60));
        animations.Add(new Anim("crouch", 56, 60, 60));
        animations.Add(new Anim("staycrouch", 60, 69, 60));
        animations.Add(new Anim("getup", 70, 74, 60));

        animations.Add(new Anim("idle1", 75, 88, 60));
        animations.Add(new Anim("idle2", 90, 110, 60));
        animations.Add(new Anim("idle3", 292, 325, 60));
        animations.Add(new Anim("idle4", 327, 360,60));

        animations.Add(new Anim("swipe", 112, 126, 60));
        animations.Add(new Anim("jumpattack", 128, 142, 60));
        animations.Add(new Anim("spin", 144, 160, 60));
        animations.Add(new Anim("swipes", 162, 180, 60));
        animations.Add(new Anim("stab", 182, 192, 60));
        animations.Add(new Anim("block", 194,210, 60));

        animations.Add(new Anim("die1", 212, 227, 60));
        animations.Add(new Anim("die2", 230,251, 60));

        animations.Add(new Anim("nod", 253, 272, 60));
        animations.Add(new Anim("shake", 274, 290,60));

		currentFrame=0;
		nextFrame = 1;

        setAnimation(0);
	
	}

	private float tickcount()
	{
		return Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
        
		float time = tickcount ();
		float elapsedTime =  time - lastTime;
		lerptime = tickcount() / (1.0f / currAnimation.fps);




        if (!manual)
        {
            nextFrame = (currentFrame + 1);
            if (nextFrame > currAnimation.frameEnd)
            {
                nextFrame = currAnimation.frameStart;
            }


            if (RollOver)
            {
                if (currentFrame >= currAnimation.frameEnd)
                {
                    setAnimation(rollto_anim);
                    RollOver = false;
                }
            }

            if (elapsedTime >= (1.0f / currAnimation.fps))
            {
                currentFrame = nextFrame;
                lastTime = tickcount();
            }

        }


        if (Input.GetKey(KeyCode.D))
        {
            NextAnimation();
        }
        else
            if (Input.GetKey(KeyCode.A))
            {
                BackAnimation();
            }

        frame=(currentFrame * 1.0f / fps);

        state.time = frame;
	}

    public int addAnimation(string name, int startFrame, int endFrame, int fps)
    {
        animations.Add(new Anim(name, startFrame, endFrame, fps));
        return (animations.Count - 1);

    }
    public int NumAnimations()
    {
        return animations.Count - 1;
    }

    public void BackAnimation()
    {
        currentAnimation = (currentAnimation - 1) % (NumAnimations());
        if (currentAnimation < 0) currentAnimation = NumAnimations();
        setAnimation(currentAnimation);
    }
    public void NextAnimation()
    {
        currentAnimation = (currentAnimation + 1) % (NumAnimations());
        if (currentAnimation > NumAnimations()) currentAnimation = 0;
        setAnimation(currentAnimation);
    }
    public void setAnimation(int num)
    {
        if (num == lastanimation) return;
        if (num > animations.Count) return;

        currentAnimation = num;
        currAnimation = animations[currentAnimation];
        currentFrame = animations[currentAnimation].frameStart;
        lastanimation = currentAnimation;
    }
    public void setAnimationByName(string name)
    {

        for (int i = 0; i < animations.Count; i++)
        {

            if (animations[i].name == name)
            {
                setAnimation(i);
                break;
            }

        }

    }
    public void SetAnimationRollOver(int num, int next)
    {
        if (num == lastanimation) return;
        if (num > animations.Count) return;

        currentAnimation = num;
        currAnimation = animations[currentAnimation];
        currentFrame = animations[currentAnimation].frameStart;
        lastanimation = currentAnimation;
        RollOver = true;
        rollto_anim = next;
    }

}
