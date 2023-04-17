using UnityEngine;

public class HoopProjectile : ProjectileBase
{
    protected override void Update()
    {
        if (Time.frameCount % 6 != 0) // 6프레임 간격으로 공격을 함
            return;
        CastDamage();
    }

    protected override void HitObject()
    {
        GetComponentInParent<HoopWeapon>().TakeDamageProjectile();
    }
}
