using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [field : SerializeField] public string Name {get; private set;}
    [HideInInspector] public bool IsKnockBack{get; set;}
    [HideInInspector] public Rigidbody2D Target{get; set;}
    public EnemyStats Stats {get; set;}

    #region Component Variables
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;
    #endregion

    #region FeedBack Variables
    [Header("Effect")]
    [SerializeField] protected Material whiteMaterial;
    protected float whiteFlashDuration = 0.08f;
    protected Material initialMat;
    [HideInInspector] public Vector2 targetDir;
    protected float knockBackSpeed = 8f;

    [Header("Sounds")]
    [SerializeField] protected AudioClip hit;
    [SerializeField] protected AudioClip die;
    #endregion

    protected virtual void OnEnable()
    {
        Target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        initialMat = sr.material;
        IsKnockBack = false;
    }

    #region Movement Functions
    public void Flip()
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

    public virtual void ApplyMovement()
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
    #endregion

    #region 닿으면 player HP 감소
    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (GameManager.instance.player == null)
            return;
        if (collision.gameObject == Target.gameObject)
        {
            Attack();
        }
    }

    protected void Attack()
    {
        if (Target.gameObject == null)
            return;

        Target.gameObject.GetComponent<Character>().TakeDamage(Stats.damage);
    }
    #endregion

    #region Take Damage
    public virtual void TakeDamage(int damage, float knockBackChance)
    {
        Stats.hp -= damage;
        EffectManager.instance.GenerateEffect(0, this.transform);
        SoundManager.instance.Play(hit);
        float chance = UnityEngine.Random.Range(0, 100);

        WhiteFlash(whiteFlashDuration);
        if (chance > knockBackChance || knockBackChance == 0)
        {
            IsKnockBack = false;
            return;
        }
        KnockBack();
    }

    protected virtual void KnockBack()
    {
        Vector2 playerPos = Target.transform.position;
        IsKnockBack = true;
        targetDir = (rb.position - Target.position).normalized;
        
    }
    public void WhiteFlash(float delayTime)
    {
        StartCoroutine(WhiteFlashCo(delayTime));
    }

    protected IEnumerator WhiteFlashCo(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        sr.material = whiteMaterial;
        yield return new WaitForSeconds(.05f);
        sr.material = initialMat;

        IsKnockBack = false;
        if (Stats.hp < 1)
        {
            //target.GetComponent<Level>().AddExperience(experienceReward);
            GetComponent<DropOnDestroy>().CheckDrop();
            gameObject.SetActive(false);
        }
    }
    #endregion
}