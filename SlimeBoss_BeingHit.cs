using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_BeingHit : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    EnemyStats stats;
    bool isKnockBack;
    EnemyBase enemyBase;
    EnemyBoss enemyBoss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameManager.instance.player.transform;
        rb = animator.GetComponent<Rigidbody2D>();
        enemyBase = animator.GetComponent<EnemyBase>();
        enemyBoss = animator.GetComponent<EnemyBoss>();
        stats = enemyBase.Stats;
        isKnockBack = enemyBase.IsKnockBack;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyBoss.RePosition();
        enemyBase.Flip();
        enemyBase.ApplyMovement();
        enemyBoss.ShootTimer();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
