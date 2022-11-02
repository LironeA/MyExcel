using MyExel.Spreadsheet;
using System.Diagnostics;
using Parser;

namespace MyExcel
{
    public partial class Form1 : Form
    {
        private string filePath = "";
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
            AboutForm about = new();
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
                if (MessageBox.Show("Зберегти файл?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (filePath == "") saveAsToolStripMenuItem_Click(null, null);
                    else saveToolStripMenuItem1_Click(null, null);
                }
                System.Windows.Forms.Application.Exit();
            }
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хочете зберегти файл?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (filePath == "") saveAsToolStripMenuItem_Click(null, null);
                else saveToolStripMenuItem1_Click(null, null);
            }
            Application.Restart();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveDialog();
                saveFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (filePath == "") SaveDialog();
                saveFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveFile()
        {
            TextWriter writer = new StreamWriter(filePath);
            writer.WriteLine(SpreadSheet.cells.GetLength(1).ToString());
            writer.WriteLine(SpreadSheet.cells.GetLength(0).ToString());
            for(int i = 0; i < SpreadSheet.cells.GetLength(0); i++)
            {
                for (int j = 0; j < SpreadSheet.cells.GetLength(1); j++)
                {
                    writer.WriteLine(SpreadSheet.cells[i,j].formula.RawData);
                }
            }
            writer.Close();
        }


        private void OpenDialog()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "TXT|*.txt";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                } else
                {
                    return;
                }
                if (openFileDialog.FileName == "") throw new Exception("Incorect Name");
            }
        }

        private void SaveDialog()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "TXT|*.txt";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
                if (saveFileDialog.FileName == "") throw new Exception("Incorect Name");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви впевнені?", "", MessageBoxButtons.OKCancel) != DialogResult.OK) return;
            if (MessageBox.Show("Хочете зберегти файл?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (filePath == "") saveAsToolStripMenuItem_Click(null, null);
                else saveToolStripMenuItem1_Click(null, null);
            }
            OpenFile();
        }

        private void OpenFile()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            OpenDialog();
            if (filePath == "") return;
            TextReader textReader = new StreamReader(filePath);
            var x = Int32.Parse(textReader.ReadLine());
            var y = Int32.Parse(textReader.ReadLine());
            for (int i = 0; i < x; i++)
            {
                AddColumn(null,null);
                for (int j = 0; j < y; j++)
                {
                    AddRow(null, null);
                    var t = textReader.ReadLine();
                    if(t == "") continue;
                    SpreadSheet.cells[i, j].formula = new Parser.Formula(t);
                    SpreadSheet.cells[i, j].e = new DataGridViewCellEventArgs(x-1, y-1);
                    dataGridView1.Rows[j].Cells[i].Value = t;
                    Parser.Parser.Parse(SpreadSheet.cells[i, j]);
                }
            }

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    SpreadSheet.cells[i, j].Evaluate(false, i, j);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Хочете зберегти файл?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (filePath == "") saveAsToolStripMenuItem_Click(null, null);
                else saveToolStripMenuItem1_Click(null, null);
            }
        }
    }
}
