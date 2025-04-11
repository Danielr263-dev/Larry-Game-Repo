
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{

    private Animator animator;

    public enum EnemyAction
    {
        None,
        basicAttack,
        specialAttack,
        death
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PerformAction(EnemyAction action)
    {
        switch (action)
        {
            case EnemyAction.basicAttack:
            animator.SetTrigger("basicAttack");
            break;
            
            case EnemyAction.specialAttack:
            animator.SetTrigger("specialAttack");
            break;

            case EnemyAction.death:
            animator.SetTrigger("death");
            break;

            default:
            //idle, doing nothing
            break;
        }
    }
}
