using Parser;
using MyExcel;

namespace MyExel.Spreadsheet
{
    public class Cell
    {
        public string value;
        public Formula formula;
        public DataGridViewCellEventArgs e;
        public delegate void Edited(bool ReCalculate);
        public event Edited? Calculate;

        public void Evaluate(bool ReCalculate)
        {
            try
            {
                var GridView = Program.mainForm.dataGridView1;
                if (ReCalculate)
                {
                    GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.formula.GetResult();
                    return;
                }
                if (GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null) return;
                string value = GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (this.formula != null && this.formula.RawData == value)
                {
                    GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.formula.GetResult();
                    return;
                }
                this.formula = new Formula(value);

                Parser.Parser.Parse(this);
                GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.formula.GetResult();
                Calculate?.Invoke(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

    }
}