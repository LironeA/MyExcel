using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyExcel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AddColumn(null,null);
            AddRow(null,null);

        }



        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,

                LineAlignment = StringAlignment.Center
            };
            //get the size of the string
            Size textSize = TextRenderer.MeasureText(rowIdx, this.Font);
            //if header width lower then string width then resize
            if (grid.RowHeadersWidth < textSize.Width + 40)
            {
                grid.RowHeadersWidth = textSize.Width + 40;
            }
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void AddRow(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        private void DeleteRow(object sender, EventArgs e)
        {
            if (MessageBox.Show("R U sure?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if(dataGridView1.Rows.Count <= 1)
                {
                    MessageBox.Show("Must be at least 1 row", "ERROR");
                    return;
                }
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }
            
        }

        private void AddColumn(object sender, EventArgs e)
        {
            int ColumnCount = dataGridView1.ColumnCount;
            string newColumnName = findColumName(ColumnCount + 1);
            dataGridView1.Columns.Add(newColumnName, newColumnName);
            dataGridView1.Columns[ColumnCount].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void DeleteColumn(object sender, EventArgs e)
        {
            if (MessageBox.Show("R U sure?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (dataGridView1.Columns.Count <= 1)
                {
                    MessageBox.Show("Must be at least 1 column", "ERROR");
                    return;
                }
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            }
        }

        private string findColumName(int index)
        {
            string result = "";
            int i = 0;
            if (index % 26 == 0) i++;
            for (; i < (int)index / 26; i++)
            {
                result += "A";
            }

            return result + (index % 26 == 0 ? "Z" : (char)(index % 26 + 64));

        }

        
    }
}
