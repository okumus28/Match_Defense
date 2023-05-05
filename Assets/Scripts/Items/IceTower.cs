using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : IBuilding
{
    public float slowRate;

    public List<Enemy> targetEnemies;

    protected override void AttackTarget()
    {
        return;
    }

    protected override void BuildingMatch()
    {
        return;
    }

    protected override void FindTarget()
    {
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log("Slow start" + collision.name);
            if (collision.GetComponent<Enemy>().currentSpeed >= collision.GetComponent<Enemy>().maxSpeed)
            {
                collision.GetComponent<Enemy>().currentSpeed -= collision.GetComponent<Enemy>().GetComponent<Enemy>().maxSpeed * slowRate / 100;
            }
            //targetEnemies.Add(collision.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log("Slow End" + collision.name);
            targetEnemies.Remove(collision.GetComponent<Enemy>());
            collision.GetComponent<Enemy>().currentSpeed = collision.GetComponent<Enemy>().maxSpeed;
        }
    }
}
