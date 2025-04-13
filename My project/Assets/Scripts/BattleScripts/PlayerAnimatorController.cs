
using UnityEngine;
public class PlayerAnimatorController : MonoBehaviour
{

    private Animator animator;

    public enum PlayerAction
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

    public void PerformAction(PlayerAction action)
    {
        switch (action)
        {
            case PlayerAction.basicAttack:
            animator.SetTrigger("basicAttack");
            break;
            
            case PlayerAction.specialAttack:
            animator.SetTrigger("specialAttack");
            break;

            case PlayerAction.death:
            animator.SetTrigger("death");
            break;

            default:
            //idle, doing nothing
            break;
        }
    }
}
