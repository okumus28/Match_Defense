using Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PoisonTower : IBuilding
{
    public float poisonDuration;
    public float hitDamage;
    //private bool isAttacking;

    protected override void FindTarget()
    {
        //Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, 1 , LayerMask.GetMask("Enemy"));

        //Debug.Log(targetColliders.Length);

        //if (targetColliders.Length > 0)
        //{
        //    currentState = BuildingState.attacking;
        //}

        //Collider2D collider = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask("Enemy"));

        //Debug.Log(collider);
        //if (collider != null)
        //{
        //    currentState = BuildingState.attacking;
        //}
    }

    protected override void AttackTarget()
    {
        //Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, 1, LayerMask.GetMask("Enemy"));

        //if (targetColliders.Length <= 0)
        //{
        //    currentState = BuildingState.finding;
        //    return;
        //}

        //foreach (Collider2D hitCollider in targetColliders)
        //{
        //    // Hasar vermek için diğer nesnenin Health component'ını alıyoruz.
        //    Enemy enemy = hitCollider.gameObject.GetComponent<Enemy>();

        //    if (enemy != null)
        //    {
        //        // Düşmana zehir veriyoruz.
        //        StartCoroutine(PoisonEnemy(enemy));
        //    }
        //}

        //Collider2D collider = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask("Enemy"));

        //if (collider == null)
        //{
        //    currentState = BuildingState.finding;
        //    return;
        //}

        //StartCoroutine(Attack());
    }

    IEnumerator PoisonEnemy(Enemy enemy)
    {
        float timer = 0;

        while (enemy.isPoisoned)
        {
            enemy.TakeDamage(hitDamage * Time.deltaTime);

            timer += Time.deltaTime;

            yield return null;
        }

        StopCoroutine(PoisonEnemy(enemy));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            enemy.isPoisoned = true;
            enemy.GetComponent<SpriteRenderer>().color = Color.green;

            StartCoroutine(PoisonEnemy(enemy));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.isPoisoned = false;
            enemy.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    protected override void BuildingMatch()
    {
        //isAttacking = false;
        StopAllCoroutines();
    }
}
