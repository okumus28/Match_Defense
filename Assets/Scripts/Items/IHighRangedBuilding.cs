using Enums;
using System.Collections;
using UnityEngine;

public abstract class IHighRangedBuilding : IBuilding
{
    public float attackSpeed;
    public float range;
    public float hitDamage;

    public Vector3 direction;

    public Enemy target;

    [SerializeField] Bullet bullet;

    private ObjectPool<Bullet> bulletPool;

    [SerializeField] bool coroutineStart = false;
    [SerializeField] bool isAttacking = true;

    private void Awake()
    {
        bulletPool = new (bullet, 25 , this.transform); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position + range * -direction, 2 * range * direction);
    }

    protected override void AttackTarget()
    {
        RaycastHit2D hit = DrawHit();

        if (target == null) return;

        if (hit.collider == null || target.gameObject.activeSelf == false) target = null;
    }

    protected override void FindTarget()
    {
        RaycastHit2D hit = DrawHit();

        if(hit.collider != null /*&& Attack().MoveNext()*/)
        {
            target = hit.collider.GetComponent<Enemy>();
            
            StartCoroutine(Attack());
            currentState = BuildingState.attacking;
        }
    }

    protected override void BuildingMatch()
    {
        target = null;
        StopAllCoroutines();
    }

    private RaycastHit2D DrawHit()
    {
        Vector3 b = range * -direction;

        RaycastHit2D hit = Physics2D.Raycast(transform.position + b, direction, range * 2, LayerMask.GetMask("Enemy"));

        return hit;
    }

    protected IEnumerator Attack()
    {
        while (target != null)
        {
            //if (target != null)
            //{
                Bullet b = bulletPool.GetObject();
                b.transform.up = -(target.transform.position - b.transform.position);

                b.hitDamage = this.hitDamage;
                b.direction = direction;
                b.target = target.transform;
                b.myPool = bulletPool;

                yield return new WaitForSeconds(100 / attackSpeed);
            //}

            //else
            //{
            //    yield return new WaitForSeconds(100 / attackSpeed);
            //    currentState = BuildingState.finding;
            //    StopAllCoroutines();
            //}
        }

        yield return new WaitForSeconds(100 / attackSpeed);
        currentState = BuildingState.finding;
        StopAllCoroutines();
    }
}
