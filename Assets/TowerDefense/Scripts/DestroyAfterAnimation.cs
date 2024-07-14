using System.Collections;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckAnimationEnd());
    }

    private IEnumerator CheckAnimationEnd()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(stateInfo.length);

        Destroy(gameObject);
    }
}
