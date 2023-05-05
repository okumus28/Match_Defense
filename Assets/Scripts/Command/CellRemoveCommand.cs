using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellRemoveCommand : ICommand
{
    public CellRemoveCommand()
    {
        
    }
    public void Execute()
    {
        GameSignals.OnUpdateMoveCount?.Invoke(-1);
        UIManager.Instance.removePanelActive.gameObject.SetActive(false);
        CellCommandStack.Instance.AddCommand(this);
    }

    public void Undo()
    {
        Debug.Log("asdasdadadadas");
    }
}
