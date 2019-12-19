using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormUI
{
    public partial class FormularMese : Form
    {
        

        List<Mese> mese = new List<Mese>();

        Button[] myButtons = new Button[20];

        public FormularMese()
        {
            InitializeComponent();

            UpdateBinding();
        }

        private void UpdateBinding()
        {
           

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            MaximizeBox = false;

            DataAccess db = new DataAccess();

            mese = db.getListaMese(); 

            var nr = mese.Count;
            nrMese.Text = "Nr total mese: " + nr.ToString();

            int nrCol = 0, nrRow = 0;
            int nrColumnTable = tableLayoutPanel1.ColumnCount;

            for (int i = 0; i < nr; i++)
            {
                int index = mese[i].NrMasa;
                this.myButtons[i] = new Button();
                if (i % nrColumnTable == 0 && i!=0) { nrRow++; nrCol = 0; }
                tableLayoutPanel1.Controls.Add(this.myButtons[i], nrCol /* Column Index */, nrRow /* Row index */);
                nrCol++;
                this.myButtons[i].Size = new System.Drawing.Size(250, 100);
                this.myButtons[i].TabIndex = mese[i].NrMasa;
                this.myButtons[i].Text = "Masa nr. " + (i + 1) + System.Environment.NewLine ;
                this.myButtons[i].UseVisualStyleBackColor = true;
                this.myButtons[i].Visible = true;
                if (mese[i].stare == 1){
                    this.myButtons[i].BackColor = Color.Red;
                    this.myButtons[i].Text += "Ocupata";
                }
                else{
                    this.myButtons[i].BackColor = Color.Green;
                    this.myButtons[i].Text += "Libera";
                }
                this.myButtons[i].FlatStyle = FlatStyle.Flat;
                myButtons[i].Click += (sender1, ex) => this.formularComanda(index);
            }
        }

        public void formularComanda(int idMasa)
        {
           
            var myForm = new FormularComanda(idMasa);
            myForm.StartPosition = FormStartPosition.Manual;
            myForm.Left = 200;
            myForm.Top = 100;
            myForm.Show();
            myForm.FormClosing += (obj, args) => { this.Close(); };
            this.Hide();
        }


    }
}
