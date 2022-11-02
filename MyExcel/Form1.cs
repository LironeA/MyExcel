using MyExel.Spreadsheet;
using System.Diagnostics;

namespace MyExcel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(SpreadSheet.CellValueChanged);
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(SpreadSheet.CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(SpreadSheet.CellEndEdit);
            AddColumn(null, null);
            AddRow(null, null);
        }



        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;
            var rowIdx = (e.RowIndex + 1).ToString();
            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            Size textSize = TextRenderer.MeasureText(rowIdx, this.Font);
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
            SpreadSheet.UpdateSpreadSheet(true);

        }

        private void DeleteRow(object sender, EventArgs e)
        {
            if (MessageBox.Show("R U sure?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (dataGridView1.Rows.Count <= 1)
                {
                    MessageBox.Show("Must be at least 1 row", "ERROR");
                    return;
                }
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }
            SpreadSheet.UpdateSpreadSheet(false);
        }

        private void AddColumn(object sender, EventArgs e)
        {
            int ColumnCount = dataGridView1.ColumnCount;
            string newColumnName = findColumName(ColumnCount + 1);
            dataGridView1.Columns.Add(newColumnName, newColumnName);
            dataGridView1.Columns[ColumnCount].SortMode = DataGridViewColumnSortMode.NotSortable;
            SpreadSheet.UpdateSpreadSheet(true);
        }

        private void DeleteColumn(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви впевнені?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (dataGridView1.Columns.Count <= 1)
                {
                    MessageBox.Show("Must be at least 1 column", "ERROR");
                    return;
                }
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            }
            SpreadSheet.UpdateSpreadSheet(false);
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new ();
            about.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm help = new HelpForm();
            help.Show();
            //Process.Start(new ProcessStartInfo { FileName = @"https://github.com/LironeA/MyExcel", UseShellExecute = true });
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви впевнені?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви впевнені?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Restart();
            }
        }
    }
}
