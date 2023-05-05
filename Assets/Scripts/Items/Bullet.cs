using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float hitDamage;
    public Vector3 direction;
    [SerializeField] float speed;
    public Transform target;

    public ObjectPool<Bullet> myPool;

    private void Start()
    {
        speed = 3f;
    }
    private void FixedUpdate()
    {
        if (target == null || target.gameObject.activeSelf == false)
        {
            //Destroy(gameObject);
            myPool.ReturnObject(this);
            return;
        }

        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(hitDamage);
            //Destroy(gameObject);
            myPool.ReturnObject(this);
        }
    }
}
