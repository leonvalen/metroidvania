using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationId
{
    Idle = 0,
    Jump = 1,
    Run = 2,
    Attack = 3,
    StandShoot = 4,
    RunShoot = 5,
    Duck = 6,
    UpShoot = 7
}

public class AnimatorController : MonoBehaviour
{
    Animator animator;

    public void Play(AnimationId animationId)
    {
        // si el animator es nulo se captura el componente del anmimator
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.Play(animationId.ToString());
    }

}
