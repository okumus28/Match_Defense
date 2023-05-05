using DG.Tweening;

public class CellSwapCommand : ICommand
{
    private readonly Cell[,] grid;

    private readonly Cell first;
    private readonly Cell second;

    public CellSwapCommand(Cell[,] grid, Cell first, Cell second)
    {
        this.grid = grid;
        this.first = first;
        this.second = second;
    }

    public void Execute()
    {
        ChangeGridTableCord(first, second);

        first.transform.DOLocalMove(second.transform.position, .5f)
            .OnComplete(() => Board.Instance.MatchControl(second));

        second.transform.DOLocalMove(first.transform.position, .5f)
            .OnComplete(() => Board.Instance.MatchControl(first));

        GameSignals.OnUpdateMoveCount(-1);

        CellCommandStack.Instance.AddCommand(this);
    }

    private void ChangeGridTableCord(Cell a, Cell b)
    {
        Board.Instance.grid[a.x, a.y] = b;
        Board.Instance.grid[b.x, b.y] = a;

        (b.x, b.y, a.x, a.y) = (a.x, a.y, b.x, b.y);
    }

    public void Undo()
    {
        ChangeGridTableCord(first, second);

        first.transform.DOLocalMove(second.transform.position, .5f);
        second.transform.DOLocalMove(first.transform.position, .5f);


        Board.Instance.MatchControl(first);
        Board.Instance.MatchControl(second);
    }
}
