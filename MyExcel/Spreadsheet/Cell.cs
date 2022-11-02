using MyExcel;
using Parser;

namespace MyExel.Spreadsheet
{
    public class Cell
    {
        public string value;
        public Formula formula;
        public DataGridViewCellEventArgs e;
        public delegate void Edited(bool ReCalculate);
        public event Edited? Calculate;

        public bool IsSubscribed(EventHandler Delegate)
        {
            if (Calculate == null)
            {
                return false;
            }

            var InvocationList = Calculate.GetInvocationList();

            foreach (var Entry in InvocationList)
            {
                if (Entry.Equals(Delegate))
                {
                    return true;
                }
            }

            return false;
        }

        public void Evaluate(bool ReCalculate)
        {
            var GridView = Program.mainForm.dataGridView1;
            var GVCell = GridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            try
            {
                if (ReCalculate)
                {
                    GVCell.Value = this.formula.GetResult();
                    Calculate?.Invoke(true);
                    GVCell.ErrorText = "";
                    return;
                }
                
                string value = GVCell.Value?.ToString();
                if (this.formula != null && this.formula.RawData == value)
                {
                    GVCell.Value = this.formula.GetResult();
                    GVCell.ErrorText = "";
                    return;
                }
                if (GVCell.Value != null)
                {
                    this.formula = new Formula(value);
                    Parser.Parser.Parse(this);
                    GVCell.Value = this.formula.GetResult();
                }
                else
                {
                    SpreadSheet.cells[e.ColumnIndex, e.RowIndex].value = null;
                    SpreadSheet.cells[e.ColumnIndex, e.RowIndex].formula = null;
                    GVCell.Value = null;
                }
                Calculate?.Invoke(true);
                GVCell.ErrorText = "";
            }
            catch(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                GVCell.Value = "Cant resolve operation for diferent types";
                GVCell.ErrorText = "Cant resolve operation for diferent types";
                MessageBox.Show("Cant do operation on diferent types(bool + bool, bool + 1, ets )");
            }
            catch (Exception ex)
            {
                GVCell.Value = ex.Message;
                GVCell.ErrorText = ex.Message;
            }

        }

    }
}