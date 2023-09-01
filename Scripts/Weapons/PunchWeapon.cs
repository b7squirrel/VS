using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchWeapon : WeaponBase
{
    [SerializeField] Transform punchSpin;
    [SerializeField] Transform punchPrefab;
    BoxCollider2D boxCol;
    Player player; // 무기 방향을 정하는 InputVec을 가져오기 위해
    Vector2 currentDir; // 정지해 있을 때의 방향을 정하기 위해
    bool isAttacking; // 공격 중일 떄는 무기가 회전하지 않도록 하기 위해
    SpriteRenderer sr;

    [Header("Sounds")]
    [SerializeField] AudioClip punch;

    protected override void Awake()
    {
        base.Awake();
        boxCol = GetComponentInChildren<BoxCollider2D>();
        anim = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
        isAttacking = false;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (player.InputVec == Vector2.zero) return;
        currentDir = player.InputVec;
    }

    protected override void SetAngle()
    {
        angle = Mathf.Atan2(currentDir.y, currentDir.x) * Mathf.Rad2Deg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Idamageable enemy = collision.transform.GetComponent<Idamageable>();

        if (enemy != null)
        {
            // Attck 할 때 damge와 knockback 값을 가져와서 저장했음
            
        }
    }

    public void CastDamage(Idamageable enemy, Vector3 enemyPos, Vector3 contactPos)
    {
        PostMessage(damage, enemyPos);

        GameObject hitEffect = GetComponent<HitEffects>().hitEffect;
        enemy.TakeDamage(damage, knockback, contactPos, hitEffect);
    }

    // 공격을 할 동안은 무기의 회전이나 Flip이 없어야 함
    protected override void Attack()
    {
        base.Attack();

        isAttacking = true;
        anim.SetTrigger("Attack");
        SoundManager.instance.Play(punch);
    }

    // IEnumerator AttackCo()
    // {

    // }

    protected override void RotateWeapon()
    {
        if (isAttacking) return;

        // Quaternion targetAngle = Quaternion.Euler(0, 0, angle);

        // punchSpin.rotation = Quaternion.Slerp(punchSpin.rotation, targetAngle, .6f);

        punchSpin.eulerAngles = new Vector3(0, 0, angle);
    }

    protected override void FlipWeaponTools()
    {
        if (isAttacking) return;

        if (currentDir.x >= 0)
        {
            sr.flipY = false;

        }
        else
        {
            sr.flipY = true;
        }
    }

    // animation events
    void BoxColOn() => boxCol.enabled = true;
    void BoxColOff() => boxCol.enabled = false;
    void EndAttack() => isAttacking = false;
}