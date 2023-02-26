using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public int hp = 999;
    public float speed = 5;
    public int damage = 1;
    public int experience_reward = 0;

    public EnemyStats(EnemyStats stats)
    {
        this.hp = stats.hp;
        this.speed = stats.speed;
        this.damage = stats.damage;
        this.speed = stats.speed;
        this.experience_reward= stats.experience_reward;
    }
}

public class Enemy : EnemyBase, Idamageable
{
    public int ExperienceReward {get; private set;}
    bool isLive;

    protected override void OnEnable()
    {
        base.OnEnable();
        isLive = true;
    }
    void FixedUpdate()
    {
        if (!isLive)
            return;
        if (GameManager.instance.player == null)
            return;
        ApplyMovement();
    }

    private void LateUpdate()
    {
        if (!isLive)
            return;
        if (GameManager.instance.player == null)
            return;
        Flip();
    }

    void Flip()
    {
        if (Target.position.x < rb.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    void ApplyMovement()
    {
        if (IsKnockBack)
        {
            rb.velocity = knockBackSpeed * targetDir;
            return;
        }
        Vector2 dirVec = Target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * Stats.speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero;
    }

    public void Init(EnemyData data)
    {
        anim.runtimeAnimatorController = data.animController;
        this.Stats = new EnemyStats(data.stats);
        ExperienceReward = this.Stats.experience_reward;
    }
    public override void TakeDamage(int damage, float knockBackChance)
    {
        anim.SetTrigger("Hit");
        base.TakeDamage(damage, knockBackChance);
    }
}
