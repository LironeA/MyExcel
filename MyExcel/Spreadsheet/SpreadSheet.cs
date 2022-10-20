using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parser;

namespace MyExel.Spreadsheet
{
    static class SpreadSheet
    {
        public static DataGridView GridView { get; set; }
        private static Cell[,] cells = new Cell[100,100];
        public static void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            cells[e.ColumnIndex, e.RowIndex] = new Cell();
            cells[e.ColumnIndex, e.RowIndex].formula = new Formula((string)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }
    }
}
