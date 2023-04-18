using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MoreMountains.Feedbacks;

public class PlayerVisual : MonoBehaviour
{
    Animator animatorNow;

    [Header("particle")]
    public ParticleSystem dashingPa;
    public ParticleSystem jumpUpPa;
    public ParticleSystem landingPa;
    public ParticleSystem movePa;

    // [Header("feedBack")]
    // public MMFeedbacks jumpFeedBack;
    // public MMFeedbacks landFeedBack;
    // public MMFeedbacks dashFeedBack;
   
    private void Start()
    {
        //animatorNow = GetComponent<Animator>();
    }
 
    public void PlayMovePa()
    {
        // if (!movePa.isPlaying)
        //     movePa.Play();
    }

    public void StopMovePa()
    {
        // if (movePa.isPlaying)
        //     movePa.Stop();
    }

    public void PlayGroundAnimation(float set)
    {
        // AnimatorStateInfo animatorInfo = animatorNow.GetCurrentAnimatorStateInfo(0);
        // if (set < 0.2f)
        // {
        //     //Debug.Log(" animatorNow.Play(idle");
        //     if (!animatorInfo.IsName("Idle"))
        //     {
        //         //Debug.Log(" animatorNow.canplay idle");
        //         animatorNow.Play("Idle");
        //     }
        //     return;
        // }
        // else if (set < 5f)
        // {
        //     if (!animatorNow.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        //         animatorNow.Play("walk");
        //     return;
        // }

        // if (!animatorNow.GetCurrentAnimatorStateInfo(0).IsName("run"))
        //     animatorNow.Play("run");
    }

   
    public void PlayIdleAnimation()
    {
       // animatorNow.Play("Idle");
    }

    public void PauseAnimation()
    {
       // animatorNow.speed = 0;
    }
    public void ContinueAnimation()
    {
        //animatorNow.speed = 1;
    }

    public void PlayJumpAnimation()
    {
        //jumpFeedBack.PlayFeedbacks();
        //animatorNow.Play("jump");
    }

    public void Land()
    {
        //landFeedBack.PlayFeedbacks();
    }

    public void PlayJumpPa()
    {
        //jumpUpPa.Play();
    }

    public void PlayDashAnimation()
    {
       
        // dashingPa.Play();
        // dashFeedBack.PlayFeedbacks();
       // animatorNow.Play("dash");
    }
  
    public void PlayFallAnimation()
    {
       //animatorNow.Play("fall");
    }
   

}
