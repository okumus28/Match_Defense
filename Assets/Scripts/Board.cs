using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoSingleton<Board>
{
    public int numRows;
    public int numColumns;

    public Cell[,] grid;

    private Cell firstSelectionCell;
    private Cell secondSelectionCell;

    private int matchCount;

    private bool removeCell;

    private void OnEnable()
    {
        Cell.OnCellSelected += SelectedCell;
        GameSignals.OnRemoveCell += (arg0) => removeCell = arg0;
        Castle.OnCastleMouseDown += OnSelectedCastle;
        //GameSignals.OnGameStarting += () => FillTable();
    }

    private void OnDisable()
    {
        Cell.OnCellSelected -= SelectedCell;
        GameSignals.OnRemoveCell -= (arg0) => removeCell = arg0;
        Castle.OnCastleMouseDown += OnSelectedCastle;
        //GameSignals.OnGameStarting -= () => FillTable();
    }

    private void OnSelectedCastle()
    {
        if (firstSelectionCell != null)
        {
            if (firstSelectionCell.value == 1 && firstSelectionCell.y == numColumns - 1)
            {
                Castle.Instance.AddHealt();
                GameSignals.OnUpdateMoveCount?.Invoke(-1);
                DropCells(firstSelectionCell);
            }
        }
    }

    private void Start()
    {
        FillTable();
    }

    private void FillTable()
    {
        grid = new Cell[numRows, numColumns];

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numColumns; j++)
            {
                CreateNewCell(i, j);
            }
        }
    }

    #region CELL OPERATİON
    void SelectedCell(Cell cell)
    {
        if (firstSelectionCell == null)
        {
            RemoveCell(cell);

            firstSelectionCell = cell;
            firstSelectionCell.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (firstSelectionCell == cell || firstSelectionCell.value == cell.value)
        {
            if (cell.GetComponent<Chest>() != null)
            {
                DropCells(cell);

                cell.GetComponent<Chest>().OpenChest();
            }

            firstSelectionCell.GetComponent<SpriteRenderer>().color = Color.white;
            firstSelectionCell = null;
        }

        else
        {
            firstSelectionCell.GetComponent<SpriteRenderer>().color = Color.white;
            secondSelectionCell = cell;
            SwapCells(cell);
        }
    }

    void RemoveCell(Cell cell)
    {
        if (removeCell)
        {
            if (cell.GetComponent<IBuilding>() == null)
            {
                DropCells(cell);

                var cmd = new CellRemoveCommand();
                cmd.Execute();

                removeCell = false;
            }
            return;
        }
    }
    void SwapCells(Cell cell)
    {
        int distance = Mathf.Abs(firstSelectionCell.x - secondSelectionCell.x) + Mathf.Abs(firstSelectionCell.y - secondSelectionCell.y);

        if (distance <= 1 && GameManager.Instance.moveCount > 0)
        {
            var cmd = new CellSwapCommand(grid, firstSelectionCell, secondSelectionCell);
            cmd.Execute();

            matchCount = 0;
            CellCommandStack.Instance.AddCommand(cmd);

            firstSelectionCell = null;
            secondSelectionCell = null;
        }
        else
        {
            firstSelectionCell = cell;
            secondSelectionCell = null;
        }
    }
    public void CreateNewCell(int x , int y)
    {
        Cell cell = Instantiate(GameManager.Instance.basicCells[Random.Range(0, GameManager.Instance.basicCells.Count)], this.transform);
        cell.transform.localPosition = new Vector3(x, 10, 0);
        cell.SetMoving();

        grid[x, y] = cell;
        cell.x = x;
        cell.y = y;

    }

    public void DropCells(Cell cell)
    {
        cell.transform.DOScale(Vector3.zero, .75f).OnComplete(() =>
        {
            Destroy(cell.gameObject);
            //GameSignals.OnDroppingCellCount?.Invoke(-1);

            for (int i = cell.y; i < numColumns - 1; i++)
            {
                grid[cell.x, i] = grid[cell.x, i + 1];
                grid[cell.x, i].y = i;
                //grid[cell.x, i].moving = true;
                grid[cell.x, i].SetMoving();
                //GameSignals.OnDroppingCellCount?.Invoke(+1);
            }

            CreateNewCell(cell.x, numColumns - 1);
        });
    }
    #endregion

    #region MATCHİNG
    public void MatchControl(Cell cell)
    {
        List<Cell> verticalMatchList = new() { cell };
        List<Cell> horizontMatchList = new() { cell };

        HorizontalNeighborsControl(cell, horizontMatchList);
        VerticalNeighborsControl(cell, verticalMatchList);

        if (horizontMatchList.Count < 3 && verticalMatchList.Count < 3)
        {
            CellSignals.OnMatchCount?.Invoke(matchCount);
            return;
        }

        matchCount++;
        CellSignals.OnMatchCount?.Invoke(matchCount);

        Cell building = Instantiate(cell.building, this.transform);

        building.transform.localScale = Vector3.zero;
        building.transform.DOScale(.95f, .75f);

        building.transform.localPosition = cell.transform.localPosition;
        building.x = cell.x;
        building.y = cell.y;

        grid[cell.x, cell.y] = building;

        MatchControl(building);

        if (horizontMatchList.Count >= 5 || verticalMatchList.Count >= 5)
        {
            UIManager.Instance.EarnedMoveText(cell.transform, +3);   
            GameSignals.OnUpdateMoveCount(+3);
        }
        else if (horizontMatchList.Count >= 4 || verticalMatchList.Count >= 4 || matchCount >= 3)
        {
            UIManager.Instance.EarnedMoveText(cell.transform, +1);   
            GameSignals.OnUpdateMoveCount(+1);
        }
        cell.transform.DOScale(Vector3.zero, .5f).OnComplete(() => Destroy(cell.gameObject));

        if (horizontMatchList.Count >= 3)
            horizontMatchList.ForEach(h => DropCells(h));
        if (verticalMatchList.Count >= 3)
            verticalMatchList.ForEach(v => DropCells(v));
    }
    void HorizontalNeighborsControl(Cell cell, List<Cell> matchList)
    {

        for (int i = cell.x + 1; i < numRows; i++)
        {
            Cell neighborCell = grid[i, cell.y];

            if (neighborCell == null || cell.value != neighborCell.value)
            {
                break;
            }

            matchList.Add(neighborCell);
        }


        for (int i = cell.x - 1; i >= 0; i--)
        {
            Cell neighborCell = grid[i, cell.y];

            if (neighborCell == null || cell.value != neighborCell.value)
            {
                break;
            }

            matchList.Add(neighborCell);
        }
    }
    void VerticalNeighborsControl(Cell cell, List<Cell> matchList)
    {
        for (int i = cell.y + 1; i < numColumns; i++)
        {
            Cell neighborCell = grid[cell.x, i];

            if (neighborCell == null || cell.value != neighborCell.value)
            {
                break;
            }

            matchList.Add(neighborCell);
        }

        for (int i = cell.y - 1; i >= 0; i--)
        {
            Cell neighborCell = grid[cell.x, i];

            if (neighborCell == null || cell.value != neighborCell.value)
            {
                break;
            }

            matchList.Add(neighborCell);
        }
    }
    #endregion
}