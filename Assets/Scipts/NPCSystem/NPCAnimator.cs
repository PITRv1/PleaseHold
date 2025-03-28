using UnityEngine;

public class NPCAnimator : MonoBehaviour
{
    [SerializeField] private NPCMovement npc;
    private Animator animator;
    private const string IS_WALKING = "IsWalking";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        animator.SetBool(IS_WALKING, npc.IsWalking());
    }
}
