
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Mp3Tagger.Helpers
{
    public static class DTGHelper
    {
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static System.Windows.Controls.DataGridRow GetSelectedRow(this DataGrid grid)
        {
            return (System.Windows.Controls.DataGridRow) grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }
        public static System.Windows.Controls.DataGridRow GetRow(this DataGrid grid, int index)
        {
            System.Windows.Controls.DataGridRow row = (System.Windows.Controls.DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (System.Windows.Controls.DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public static DataGridCell GetCell(this DataGrid grid, System.Windows.Controls.DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        public static DataGridCell GetCell(this DataGrid grid, int row, int column)
        {
            System.Windows.Controls.DataGridRow rowContainer = grid.GetRow(row);
            return grid.GetCell(rowContainer, column);
        }
    }
}
