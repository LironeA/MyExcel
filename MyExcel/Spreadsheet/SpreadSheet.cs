using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parser;
using MyExcel;

namespace MyExel.Spreadsheet
{
    static class SpreadSheet
    {
        public static DataGridView GridView { get; set; }
        public static Cell[,] cells;

        public static void CreateSpreadSheet()
        {
            var Form1 = Program.mainForm;
            if (Form1 == null) return;
            GridView = Form1.dataGridView1;
            Cell[,] temp = new Cell[1, 1];
            temp[0, 0] = new Cell();
            cells = temp;
        }

        public static void UpdateSpreadSheet(bool adding)
        {
            var Form1 = Program.mainForm;
            if (Form1 == null) return;
            var dgv = Form1.dataGridView1;
            Cell[,] temp = new Cell[dgv.ColumnCount, dgv.RowCount];
            var d0 = adding ? temp.GetLength(0) : cells.GetLength(0);
            var d1 = adding ? temp.GetLength(1) : cells.GetLength(1);
            for (int i = 0; i < d0; i++)
            {
                for(int j = 0; j < d1; j++)
                {
                    if (i > cells.GetLength(0)-1 || j > cells.GetLength(1)-1)
                    {
                        temp[i, j] = new Cell();
                        continue;
                    }
                    if (i > temp.GetLength(0) - 1 || j > temp.GetLength(1) - 1)
                    {
                        continue;
                    }
                    temp[i, j] = cells[i, j];
                    
                }
            }
            cells = temp;
        }

        public static void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var tempCell = cells[e.ColumnIndex, e.RowIndex];
            if (tempCell == null) return;
            if (tempCell.formula == null) return;
            string value = GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = tempCell.formula.RawData;
        }

        public static void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var tempCell = cells[e.ColumnIndex, e.RowIndex];
            tempCell.e = e;
            tempCell.Evaluate(false);
        }
        

        public static void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
