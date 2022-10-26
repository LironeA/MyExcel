using MyExel.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyExcel
{
    internal static class Program
    {
        public static Form1 mainForm;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Form1();
            SpreadSheet.CreateSpreadSheet();
            mainForm.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(SpreadSheet.CellValueChanged);
            mainForm.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(SpreadSheet.CellBeginEdit);
            mainForm.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(SpreadSheet.CellEvaluate);
            Application.Run(mainForm);
            
        }
    }
}
