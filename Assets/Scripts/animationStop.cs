using UnityEngine;

public class animationStop : MonoBehaviour
{
    [SerializeField] Animator animator;


    public void StopAnimator()
    {
        animator.speed = 0f;
    }
}
