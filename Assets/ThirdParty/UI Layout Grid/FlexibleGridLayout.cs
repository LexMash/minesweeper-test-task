using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterfaceGridLayout
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,    
            Width,      
            Height,     
            FixedRows,   
            FixedColumns,
            FixedBoth
        }

        public enum SortEnum
        {
            Rows,
            Columns
        }

        public enum SortVerticalyEnum
        {
            TopToBottom,
            BottomToTop
        }

        public enum SortHorizontalyEnum
        {
            LeftToRight,
            RightToLeft
        }

        [Header("GRID SETTING")]
        public FitType fitType = FitType.Uniform;
        public int rows = 2;   
        public int columns = 2;
        public Vector2 cellSize;
        public Vector2 spacing;

        [Header("CELL SETTINGS")]
        public bool fitX = true;
        public bool fitY = true;
        public bool keepCellsSquare;

        [Header("SORTING SETTINGS")]
        public SortEnum fillFirst = SortEnum.Rows;
        public SortVerticalyEnum sortVertically = SortVerticalyEnum.TopToBottom;
        public SortHorizontalyEnum sortHorizontally = SortHorizontalyEnum.LeftToRight;

        private int actualRows;
        private int actualColumns;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (transform.childCount == 0)
            {
                actualRows = 0;
                actualColumns = 0;
                return;
            }

            CalculateGridDimensions();

            float parentWidth = rectTransform.rect.width - padding.left - padding.right;
            float parentHeight = rectTransform.rect.height - padding.top - padding.bottom;

            float cellWidth = 0;
            float cellHeight = 0;

            if (actualColumns > 0)
                cellWidth = (parentWidth - (spacing.x * (actualColumns - 1))) / actualColumns;

            if (actualRows > 0)
                cellHeight = (parentHeight - (spacing.y * (actualRows - 1))) / actualRows;

            if (fitX && actualColumns > 0)
                cellSize.x = cellWidth;

            if (fitY && actualRows > 0)
                cellSize.y = cellHeight;

            if (keepCellsSquare)
            {
                float squareSize = Mathf.Min(cellSize.x, cellSize.y);
                cellSize.x = squareSize;
                cellSize.y = squareSize;
            }

            SortAndPositionChildren();
        }

        private void CalculateGridDimensions()
        {
            int childCount = transform.childCount;

            switch (fitType)
            {
                case FitType.Uniform:
                    float sqrt = Mathf.Sqrt(childCount);
                    actualRows = Mathf.CeilToInt(sqrt);
                    actualColumns = Mathf.CeilToInt(sqrt);
                    rows = actualRows;
                    columns = actualColumns;
                    break;

                case FitType.Width:
                case FitType.FixedColumns:
                    actualColumns = Mathf.Max(1, columns);
                    actualRows = Mathf.CeilToInt(childCount / (float)actualColumns);
                    rows = actualRows;
                    break;

                case FitType.Height:
                case FitType.FixedRows:
                    actualRows = Mathf.Max(1, rows);
                    actualColumns = Mathf.CeilToInt(childCount / (float)actualRows);
                    columns = actualColumns;
                    break;

                case FitType.FixedBoth:
                    actualRows = Mathf.Max(1, rows);
                    actualColumns = Mathf.Max(1, columns);
                    break;
            }
        }

        private void SortAndPositionChildren()
        {
            if (actualRows <= 0 || actualColumns <= 0) return;

            List<RectTransform> sortedChildren = new List<RectTransform>(rectChildren.Count);

            if (fillFirst == SortEnum.Rows)
            {
                for (int row = 0; row < actualRows; row++)
                {
                    for (int col = 0; col < actualColumns; col++)
                    {
                        int rowIndex = sortVertically == SortVerticalyEnum.TopToBottom ? row : actualRows - 1 - row;
                        int colIndex = sortHorizontally == SortHorizontalyEnum.LeftToRight ? col : actualColumns - 1 - col;
                        int index = rowIndex * actualColumns + colIndex;

                        if (index < rectChildren.Count)
                        {
                            sortedChildren.Add(rectChildren[index]);
                        }
                    }
                }
            }
            else
            {
                for (int col = 0; col < actualColumns; col++)
                {
                    for (int row = 0; row < actualRows; row++)
                    {
                        int colIndex = sortHorizontally == SortHorizontalyEnum.LeftToRight ? col : actualColumns - 1 - col;
                        int rowIndex = sortVertically == SortVerticalyEnum.TopToBottom ? row : actualRows - 1 - row;
                        int index = rowIndex + colIndex * actualRows;

                        if (index < rectChildren.Count)
                        {
                            sortedChildren.Add(rectChildren[index]);
                        }
                    }
                }
            }

            for (int i = 0; i < sortedChildren.Count; i++)
            {
                int rowCount, columnCount;

                if (fillFirst == SortEnum.Rows)
                {
                    rowCount = i / actualColumns;
                    columnCount = i % actualColumns;
                }
                else
                {
                    columnCount = i / actualRows;
                    rowCount = i % actualRows;
                }

                var item = sortedChildren[i];

                float xPos = padding.left + (cellSize.x + spacing.x) * columnCount;
                float yPos = padding.top + (cellSize.y + spacing.y) * rowCount;

                float totalGridWidth = actualColumns * cellSize.x + (actualColumns - 1) * spacing.x;
                float totalGridHeight = actualRows * cellSize.y + (actualRows - 1) * spacing.y;
                float availableWidth = rectTransform.rect.width - padding.left - padding.right;
                float availableHeight = rectTransform.rect.height - padding.top - padding.bottom;

                switch (childAlignment)
                {
                    case TextAnchor.UpperCenter:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.LowerCenter:
                        xPos = padding.left + (availableWidth - totalGridWidth) / 2 + (cellSize.x + spacing.x) * columnCount;
                        break;
                    case TextAnchor.UpperRight:
                    case TextAnchor.MiddleRight:
                    case TextAnchor.LowerRight:
                        xPos = padding.left + availableWidth - totalGridWidth + (cellSize.x + spacing.x) * columnCount;
                        break;
                }

                switch (childAlignment)
                {
                    case TextAnchor.MiddleLeft:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.MiddleRight:
                        yPos = padding.top + (availableHeight - totalGridHeight) / 2 + (cellSize.y + spacing.y) * rowCount;
                        break;
                    case TextAnchor.LowerLeft:
                    case TextAnchor.LowerCenter:
                    case TextAnchor.LowerRight:
                        yPos = padding.top + availableHeight - totalGridHeight + (cellSize.y + spacing.y) * rowCount;
                        break;
                }

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            rows = Mathf.Max(1, rows);
            columns = Mathf.Max(1, columns);
            spacing.x = Mathf.Max(0, spacing.x);
            spacing.y = Mathf.Max(0, spacing.y);
        }
#endif
    }
}