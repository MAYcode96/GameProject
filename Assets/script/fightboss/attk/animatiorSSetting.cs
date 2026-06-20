using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public string attackTrigger = "Attack";

    public void PlayAttack()
    {
        animator.SetTrigger(attackTrigger);
    }
}