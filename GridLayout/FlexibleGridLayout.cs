using UnityEngine;
using UnityEngine.UI;

namespace CustomUIComponents.TileLayout
{
	public class FlexibleGridLayout : LayoutGroup
	{
		public FitType fitType;

		public Vector2 spacing;
		public Vector2 cellSize;

		[Range(1, 8)] public int rows = 1;
		[Range(1, 8)] public int columns = 1;

		[HideInInspector] public bool fitX;
		[HideInInspector] public bool fitY;

		public enum FitType
		{
			Uniform,
			Width,
			Height,
			FixedRows,
			FixedColumns
		}


		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();

			if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
			{
				fitX = true;
				fitY = true;
				
				var sqrt = Mathf.Sqrt(transform.childCount);
				rows = Mathf.CeilToInt(sqrt);
				columns = Mathf.CeilToInt(sqrt);
			}

			if (fitType == FitType.Width || fitType == FitType.FixedColumns)
				rows = Mathf.CeilToInt(transform.childCount / (float) columns);
			else if (fitType == FitType.Height || fitType == FitType.FixedRows)
				columns = Mathf.CeilToInt(transform.childCount / (float) rows);

			var rect = rectTransform.rect;
			var parentWidth = rect.width;
			var parentHeight = rect.height;

			var cellWidth = parentWidth / columns - spacing.x / columns * 2 - padding.left / (float) columns - padding.right / (float) columns;
			var cellHeight = parentHeight / rows - spacing.y / rows * 2 - padding.top / (float) rows - padding.bottom / (float) rows;

			cellSize.x = fitX ? cellWidth : cellSize.x;
			cellSize.y = fitY ? cellHeight : cellSize.y ;

			var columnCount = 0;
			var rowCount = 0;

			for (var i = 0; i < rectChildren.Count; i++)
			{
				rowCount = i / columns;
				columnCount = i % columns;

				var item = rectChildren[i];
				var xPos = cellSize.x * columnCount + spacing.x * columnCount + padding.left;
				var yPos = cellSize.y * rowCount + spacing.y * rowCount + padding.top;

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
	}
}