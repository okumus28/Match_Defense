using Enums;
using System.Collections;
using UnityEngine;

public class ILowRangeBuilding : IBuilding
{
    public float attackSpeed;
    public float range;
    public float hitDamage;

    public Enemy target;

    [SerializeField] Bullet bullet;
    private ObjectPool<Bullet> bulletPool;

    private void Awake()
    {
        bulletPool = new(bullet, 25, this.transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, range);
    }

    protected override void BuildingMatch()
    {
        target = null;
        StopAllCoroutines();
    }
    protected override void AttackTarget()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Enemy"));

        if (target == null) return;

        if (collider == null || target.gameObject.activeSelf == false) target = null;
    }
    protected override void FindTarget()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Enemy"));

        if (collider != null /*&& Attack().MoveNext()*/)
        {
            target = collider.GetComponent<Enemy>();
            StartCoroutine(Attack());
            currentState = BuildingState.attacking;
        }
    }

    IEnumerator Attack()
    {
        while (target != null)
        {
            //if (target != null)
            //{
                Bullet b = Instantiate(bullet, transform.position, Quaternion.identity);
                b.target = target.transform;
                b.transform.up = target.transform.position - b.transform.position;
                b.hitDamage = hitDamage;
                b.myPool = bulletPool;
                yield return new WaitForSeconds(100/attackSpeed);
            //}
            //else
            //{
            //    yield return new WaitForSeconds(100/attackSpeed);
            //    currentState = BuildingState.finding;
            //    StopAllCoroutines();
            //}
        }

        yield return new WaitForSeconds(100 / attackSpeed);
        currentState = BuildingState.finding;
        StopAllCoroutines();
    }
}

