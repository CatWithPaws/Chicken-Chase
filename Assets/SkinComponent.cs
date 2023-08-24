using UnityEngine;

public class SkinComponent : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void SetSkin(Skin skin)
    {
        animator.runtimeAnimatorController = skin.AnimatorController;
        animator.Play("Idle");
    }
}
