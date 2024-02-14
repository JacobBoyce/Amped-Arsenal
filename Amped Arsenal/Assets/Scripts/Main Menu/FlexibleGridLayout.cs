using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    #region old code
    
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }
    public int rows, columns;
    public Vector2 cellSize, spacing;
    public FitType fitType;
    public bool fitX, fitY;

    public override void CalculateLayoutInputHorizontal()
    {

        base.CalculateLayoutInputHorizontal();

        float parentWidth = rectTransform.rect.width - padding.horizontal; // Subtract the horizontal padding
    float parentHeight = rectTransform.rect.height - padding.vertical; // Subtract the vertical padding

    float availableWidth = parentWidth - spacing.x * (columns - 1); // Adjust for the spacing
    float availableHeight = parentHeight - spacing.y * (rows - 1); // Adjust for the spacing

    float cellWidth = availableWidth / columns; // Calculate the width of each cell
    float cellHeight = availableHeight / rows; // Calculate the height of each cell

    if (fitType == FitType.Uniform)
    {
        float cellSize = Mathf.Min(cellWidth, cellHeight); // Use the smaller dimension as the cell size
        this.cellSize.x = cellSize;
        this.cellSize.y = cellSize;
    }
    else if (fitType == FitType.Width)
    {
        this.cellSize.x = cellWidth; // Use the calculated width as the cell width
        this.cellSize.y = cellWidth; // Use the same width for the cell height
    }
    else if (fitType == FitType.Height)
    {
        this.cellSize.x = cellHeight; // Use the calculated height as the cell width
        this.cellSize.y = cellHeight; // Use the same height for the cell height
    }
    else if (fitType == FitType.FixedRows)
    {
        this.cellSize.x = cellWidth; // Use the calculated width as the cell width
        this.cellSize.y = cellHeight; // Use the calculated height as the cell height
    }
    else if (fitType == FitType.FixedColumns)
    {
        this.cellSize.x = cellWidth; // Use the calculated width as the cell width
        this.cellSize.y = cellHeight; // Use the calculated height as the cell height
    }

    // Calculate the total width and height of the grid layout
    float totalWidth = this.cellSize.x * columns + spacing.x * (columns - 1);
    float totalHeight = this.cellSize.y * rows + spacing.y * (rows - 1);

    // Calculate the offset to center the grid within the parent
    float xOffset = (parentWidth - totalWidth) / 2f;
    float yOffset = (parentHeight - totalHeight) / 2f;

    int columnCount = 0, rowCount = 0;

    for (int i = 0; i < rectChildren.Count; i++)
    {
        rowCount = i / columns;
        columnCount = i % columns;

        var item = rectChildren[i];

        var xPos = this.cellSize.x * columnCount + (spacing.x * columnCount) + padding.left + xOffset;
        var yPos = this.cellSize.y * rowCount + (spacing.y * rowCount) + padding.top + yOffset;

        SetChildAlongAxis(item, 0, xPos, this.cellSize.x);
        SetChildAlongAxis(item, 1, yPos, this.cellSize.y);
    }

    }
        /*if(fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
        
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);

            
        }

        if(fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float) columns);
        }
        if(fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float) rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / columns) - (spacing.x / ((float)columns / (columns - 1))) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / rows) - (spacing.y / ((float)rows / (rows - 1))) - (padding.top / (float)rows) - (padding.bottom / (float)rows);


        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0, rowCount = 0;

        for(int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;
            
            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }*/
        
    //}

    public override void CalculateLayoutInputVertical()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        //throw new System.NotImplementedException();
    }
    
    #endregion
    
}