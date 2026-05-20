using Minesweeper.Board;
using System;
using UnityEngine;

namespace Minesweeper.Configs
{
    [Serializable]
    public class CellStateVisualMapping
    {
        public CellState State;
        public Sprite Sprite;
    }
}
