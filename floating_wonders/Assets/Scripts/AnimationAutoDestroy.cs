using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimationAutoDestroy : MonoBehaviour
{
    public float delay = 0f;

    // Use this for initialization
    void Start()
    {
        var anim = GetComponent<Animator>();
        anim.speed = 2;
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length/anim.speed + delay);
    }
}