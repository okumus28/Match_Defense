using Enums;
using System;
using UnityEngine;

public class Cell : MonoBehaviour 
{
    public int value;
    public int x;
    public int y;

    public bool moving = false;

    [SerializeField] private float downSpeed;

    public Cell building;

    public static event Action<Cell> OnCellSelected;

    private void OnMouseDown()
    {
        if (GameManager.Instance.gameState == GameStates.matching)
        {
            OnCellSelected?.Invoke(this);
        }
    }

    private void Update()
    {
        if (moving)
        {
            Vector3 pos = new(x, y, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos, downSpeed * Time.deltaTime * 200);

            if (Vector3.Distance(transform.localPosition, pos) <= .005f)
            {
                Board.Instance.MatchControl(this);
                moving = false;
            }
        }
    }

    public void SetMoving()
    {
        moving = true;
        //GameSignals.OnDroppingCellCount?.Invoke(+1);
    }
}
