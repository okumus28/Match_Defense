using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealt;
    public float currentHealt;

    public bool isPoisoned;

    public float maxSpeed;
    public float currentSpeed;

    public float hitDamage;

    private void Start()
    {
    }

    public void Init()
    {
        EnemySignals.OnUpdateLiveEnemyCount(+1);
        currentHealt = maxHealt;
        currentSpeed = maxSpeed;        
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.up * currentSpeed / 100 * Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        damage = isPoisoned ? damage + (damage * .33f): damage;

        currentHealt -= damage;
        if (currentHealt <= 0)
        {
            EnemySpawner.Instance.enemies.ReturnObject(this);
            EnemySignals.OnUpdateLiveEnemyCount(-1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Poison"))
        {
            isPoisoned = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Poison"))
        {
            isPoisoned = false;
        }
    }
}
