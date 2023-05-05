using System;
using UnityEngine;
using UnityEngine.Events;

public class Castle : MonoSingleton<Castle>
{
    [SerializeField] private int healt;
    public int Healt 
    {
        get 
        { return healt; }
        set
        {
            healt = value;
            OnCastleHealt.Invoke(value);
        }
    }

    public static event Action<int> OnCastleHealt;
    public static event Action OnCastleMouseDown;

    private void Start()
    {
        OnCastleHealt?.Invoke(healt);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Healt--;
            OnCastleHealt?.Invoke(healt);
            collision.GetComponent<Enemy>().TakeDamage(collision.GetComponent<Enemy>().currentHealt);

            if (healt <= 0)
            {
                GameSignals.OnGameOver?.Invoke();
                Debug.Log("GameOver");
            }
        }
    }

    private void OnMouseDown()
    {
        OnCastleMouseDown?.Invoke();
    }

    public void AddHealt()
    {
        Healt++;
        //OnCastleHealt?.Invoke(healt);
    }
}
