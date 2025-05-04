using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeObject : MonoBehaviour , IPoolable
{
    public SpriteRenderer sr;
    public BoxCollider Col;
    public Animator anim;

    public Action<Collider> HitAction;
    public Action DisableAction;
    AnimatorStateInfo stateInfo;
    public void OnDespawn()
    {
        this.gameObject.SetActive(false);
    }

    public void OnSpawn()
    {
        this.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        HitAction?.Invoke(other);
    }

    public void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime > 0.99f)
            DisableAction?.Invoke();
    }
}
