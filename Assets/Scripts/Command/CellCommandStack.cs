using System.Collections.Generic;
using UnityEngine;

public class CellCommandStack : MonoSingleton<CellCommandStack>
{
    private readonly Stack<ICommand> commandStack = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Undo();
        }
    }

    public void AddCommand(ICommand command)
    {
        commandStack.Push(command);
    }

    public void Undo()
    {
        if (commandStack.Count <= 0)
            return;

        Debug.Log("undo");

        ICommand currentCommand = commandStack.Pop();
        currentCommand.Undo();
    }
}
