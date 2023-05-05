using System;
using UnityEngine;
using UnityEngine.Events;

public sealed class Field : MonoBehaviour
{
    [SerializeField] private int _wigth = 3;
    [SerializeField] private int _height = 3;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Cell _template;
    private Cell[,] _cells;

    public event UnityAction Victory;

    private void OnEnable()
    {
        Victory += OnVictory;
    }

    private void Start() => CreateCells();

    private void OnDisable()
    {
        Victory -= OnVictory;
    }

    private void OnElementPlaced()
    {
        foreach (var cell in _cells)
        {
            if (cell.Element == null)
                return;
            if (cell.CheackedIsCorrect== false)
                return;
        }

        Victory?.Invoke();
    }

    private void OnVictory()
    {
        Debug.Log("Victory");
        foreach (var cell in _cells)
            Destroy(cell.Element);


    }

    private void CreateCells()
    {
        _cells = new Cell[_wigth, _height];

        for (int i = 0; i < _wigth; i++)
            for (int j = 0; j < _height; j++)
                CreateCell(i, j);
    }

    private void CreateCell(int i, int j)
    {
        _cells[i, j] = Instantiate(_template, _content);
        _cells[i, j].Init(i * _wigth + j);
        _cells[i, j].ElementPlaced += OnElementPlaced;
    }
}
