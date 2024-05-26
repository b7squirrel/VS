using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [SerializeField] int currentHealth;
    [SerializeField] GameObject healEffect;

    [field: SerializeField] public int MaxHealth { get; set; } = 3000;
    [field: SerializeField] public int Armor { get; set; } = 0;
    [field: SerializeField] public float HpRegenerationRate { get; set; }
    [field: SerializeField] public float HpRegenerationTimer { get; set; }
    [field: SerializeField] public float MagnetSize { get; set; }
    [field: SerializeField] public float Cooldown { get; set; }
    [field: SerializeField] public float MoveSpeed { get; set; } = 6f;
    [field: SerializeField] public float ProjectileAmount { get; set; }
    [field: SerializeField] public float ProjectileSpeed { get; set; }
    [field: SerializeField] public float Area { get; set; }
    [field: SerializeField] public float knockBackChance { get; set; }
    [field: SerializeField] public int DamageBonus { get; set; }
    [field: SerializeField] public float CriticalDamageChance { get; set; }

    [SerializeField] StatusBar hpBar;
    [HideInInspector] public Level level;

    [SerializeField] DataContainer dataContainer;

    [SerializeField] AudioClip hurtSound;

    [SerializeField] ParticleSystem wallCollisionParticle;
    [SerializeField] float wallColParticleDuration; // 벽 충돌 파티클이 보여지는 시간

    // public event Action OnDie;
    public UnityEvent OnDie;
    Animator anim;

    void Awake()
    {
        level = GetComponent<Level>();
    }

    void Start()
    {
        ApplyPersistantUpgrade();
        currentHealth = MaxHealth;
        hpBar.SetStatus(currentHealth, MaxHealth);
        healEffect.SetActive(false);

        wallCollisionParticle = GetComponentInChildren<ParticleSystem>();
        wallCollisionParticle.Stop();
    }

    void Update()
    {
        HpRegenerationTimer += Time.deltaTime * HpRegenerationRate;

        if (HpRegenerationTimer > 1f)
        {
            Heal(1, false);
            HpRegenerationTimer -= 1f;
        }
    }

    // 처음 시작할 때 persistant 데이터들을 가져옴.
    void ApplyPersistantUpgrade()
    {
        //int hpUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.HP);
        //MaxHealth += MaxHealth / 10 * hpUpgradeLevel;
        //currentHealth = MaxHealth;

        //int damageUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.DAMAGE);
        //DamageBonus = 1f + 0.1f * damageUpgradeLevel;

        int ArmorUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.Armor);
        Armor += ArmorUpgradeLevel;

        int ProjSpeedUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.ProjectileSpeed);
        ProjectileSpeed = ProjSpeedUpgradeLevel;

        int ProJAmountUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.ProjectileAmount);
        ProjectileAmount += ProJAmountUpgradeLevel;

        int MagneticArea = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.MagnetRange);
        MagnetSize += 0.25f * MagneticArea * MagnetSize; // 레벨업 당 25% 증가

        int MoveSpeedUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.MoveSpeed);
        MoveSpeed += 0.05f * MoveSpeedUpgradeLevel * MoveSpeed; // 레벱업 당 5% 증가

        int CooldownUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.CoolDown);
        Cooldown -= 0.025f * CooldownUpgradeLevel * Cooldown; // 레벨업 당 2.5% 감소

        int AreaUpgradeLevel = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.Area);
        this.Area += 0.05f * AreaUpgradeLevel * this.Area; // 레벱업 당 5% 증가

        int KnockBackChanceLevel = dataContainer.GetUpgradeLevel(PlayerPersistentUpgrades.knockBackChance);
        this.knockBackChance += 0.1f * KnockBackChanceLevel * this.knockBackChance; // 레벱업 당 10% 증가

        if (GameManager.instance.startingDataContainer == null)
        {
            MaxHealth = 3000;
            DamageBonus = 0;
            return;
        }
        MaxHealth = GameManager.instance.startingDataContainer.GetLeadAttr().Hp;
        DamageBonus = GameManager.instance.startingDataContainer.GetLeadAttr().Atk;
        Debug.Log("In Character, Damage Bonus = " + DamageBonus);
    }

    public void TakeDamage(int damage)
    {
        if (GameManager.instance.IsPlayerDead)
            return;
        if (GameManager.instance.IsPlayerInvincible)
            return;

        // 슬로우 모션 상태에서 TakeDamage가 일어나지 않게 하기
        if (BossDieManager.instance.IsBossDead)
            return;
        ApplyArmor(ref damage);

        SoundManager.instance.PlaySingle(hurtSound);

        if (anim == null) anim = GetComponentInChildren<WeaponContainerAnim>().GetComponent<Animator>();

        anim.SetTrigger("Hurt");

        if (Time.frameCount % 3 != 0) return; // 3프레임 간격으로 데미지를 입도록

        currentHealth -= damage;
        if (currentHealth < 0)
        {
            Die();
            Debug.Log("Dead");
        }
        else
        {
            hpBar.SetStatus(currentHealth, MaxHealth);
        }
    }

    void ApplyArmor(ref int damage)
    {
        damage -= Armor;
        if (damage < 0)
        {
            damage = 0;
        }
    }

    public void Heal(int amount, bool needEffect)
    {
        if (currentHealth <= 0)
            return;
        currentHealth += amount;
        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }
        hpBar.SetStatus(currentHealth, MaxHealth);

        if (needEffect) healEffect.SetActive(true);
    }

    public int GetCurrentHP()
    {
        return currentHealth;
    }

    void Die()
    {
        hpBar.gameObject.SetActive(false);
        OnDie?.Invoke();
        GetComponent<CharacterGameOver>().GameOver();
    }
}
