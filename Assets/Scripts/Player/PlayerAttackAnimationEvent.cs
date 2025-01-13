using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimationEvent : MonoBehaviour
{
    private PlayerBase playerBase;

    private void Start()
    {
        playerBase = GetComponentInParent<PlayerBase>();
    }

    // Animation event
    public void Attack()
    {
        playerBase.Attack();
    }
}
