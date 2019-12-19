using Microsoft.VisualBasic;
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
    
    public partial class FormularComanda : Form
    {
        List<Gestiuni> gestiuni = new List<Gestiuni>();

        Button[] myButtons = new Button[100];
        Button[] myButtons2 = new Button[100];

        DataAccess db = new DataAccess();

        int idMasa;
        public FormularComanda(int idMasa)
        {
            InitializeComponent();
            this.idMasa = idMasa;
        }

        private void FormularComanda_Load(object sender, EventArgs e)
        {

            bool test = db.stareMasa(this.idMasa);

            // daca masa este libera adauga o comanda noua .. 

            if(!test)
            {
                db.adaugaComandaNoua(this.idMasa);
                db.schimbaStareMasa(this.idMasa,1);
                
            }

            MaximizeBox = false;

            setColumnsGrid();

            timer1.Start();
            label1.Text = "Masa cu nr: " + this.idMasa.ToString();
            gestiuni = db.getListaGestiuni();
            var nr = gestiuni.Count;
            produseGestiune(1);
            int nrCol = 0, nrRow = 0;
            int nrColumnTable = tableLayoutPanel1.ColumnCount;

            for (int i = 0; i < nr; i++)
            {
                int index = gestiuni[i].id;
                this.myButtons[i] = new Button();

                if (i % nrColumnTable == 0 && i != 0) { nrRow++; nrCol = 0; }

                tableLayoutPanel1.Controls.Add(this.myButtons[i], i /* Column Index */, 0 /* Row index */);
                nrCol++;
                this.myButtons[i].Size = new System.Drawing.Size(250, 100);
                this.myButtons[i].TabIndex = gestiuni[i].id;
                this.myButtons[i].Text = gestiuni[i].nume_gestiune;
                this.myButtons[i].UseVisualStyleBackColor = true;
                this.myButtons[i].Visible = true;
                this.myButtons[i].Font = new Font(myButtons[i].Font.FontFamily, 14);

                this.myButtons[i].FlatStyle = FlatStyle.Flat;

                myButtons[i].Click += (sender1, ex) => this.listaProduseGestiune(index);
            }

            adaugaProduseDataGrid();

        }

        public void adaugaProduseDataGrid()
        {

            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            var produseComanda = db.listaProduseComanda(this.idMasa);

            decimal totalSum = 0;

            for (int i = 0; i < produseComanda.Count; i++)
            {
                this.dataGridView2.Rows.Add();
                this.dataGridView2.Rows[i].Cells[0].Value = i + 1;
                this.dataGridView2.Rows[i].Cells[1].Value = produseComanda[i].denumire;
                this.dataGridView2.Rows[i].Cells[2].Value = produseComanda[i].cantitate;
                this.dataGridView2.Rows[i].Cells[3].Value = produseComanda[i].pret;
                this.dataGridView2.Rows[i].Cells[4].Value = produseComanda[i].valoare;

                totalSum = totalSum + produseComanda[i].valoare;
            }


            total.Text = "Total: " + totalSum + " LEI";

        }

        public void listaProduseGestiune(int idGestiune)
        {
            produseGestiune(idGestiune);
        }


        public void produseGestiune(int id)
        {

            flowLayoutPanel1.Controls.Clear();

            var produse = db.listaProduseGestiune(id);



            for (int j = 0; j < produse.Count; j++)
            {

                int index = produse[j].id;
                this.myButtons2[j] = new Button();
                this.myButtons2[j].TabIndex = produse[j].id;
                this.myButtons2[j].Size = new System.Drawing.Size(225, 100);
                this.myButtons2[j].Text = produse[j].denumire + "Pret: " + produse[j].pret.ToString() + "LEI";
                this.myButtons2[j].Font = new Font(this.myButtons2[j].Font.FontFamily, 12);
                this.myButtons2[j].FlatStyle = FlatStyle.Flat;
                this.myButtons2[j].Padding = new Padding(0);
                flowLayoutPanel1.Controls.Add(this.myButtons2[j]);
                this.myButtons2[j].Click += (sender1, ex) => this.addNewProdus(index);
            }

 
        }

        public void addNewProdus(int idProdus)
        {

            String valoareInput = Interaction.InputBox("Introduceti cantitatea doritia", "Cantitate", "0", -1, -1);

            if(valoareInput != "")
            {
                int myInt;
                bool isNumerical = int.TryParse(valoareInput, out myInt);

                if (isNumerical && Int32.Parse(valoareInput) > 0)
                {
                    var produs = db.detaliiProdus(idProdus);
                    int cantitate = Int32.Parse(valoareInput);
                    int idComanda = db.cautaIdComanda(this.idMasa);
                    db.adaugaProdusComanda(produs[0].id, idComanda, cantitate, produs[0].pret);
                    adaugaProduseDataGrid();
                } 
                if(Int32.Parse(valoareInput) == 0)
                {
                    MessageBox.Show("Cantitatea nu poate fi 0");
                }

            }

        }


        public void setColumnsGrid()
        {
            string[] coloane = { "NrCRT", "Denumire produs", "Cantitate", "Pret", "Valoare" };
            for (int i = 0; i < coloane.Length; i++)
            {
                dataGridView2.Columns.Add("newColumnName", coloane[i]);
            }
            dataGridView2.RowTemplate.Resizable = DataGridViewTriState.True;
            dataGridView2.RowTemplate.Height = 50;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.RowHeadersVisible = false;



            this.dataGridView2.DefaultCellStyle.Font = new Font("Tahoma", 10);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            int change = flowLayoutPanel1.VerticalScroll.Value - flowLayoutPanel1.VerticalScroll.SmallChange * 10;
            flowLayoutPanel1.AutoScrollPosition = new Point(0, change);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int change = flowLayoutPanel1.VerticalScroll.Value + flowLayoutPanel1.VerticalScroll.SmallChange * 10;
            flowLayoutPanel1.AutoScrollPosition = new Point(0, change);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            DialogResult res = MessageBox.Show("Sunteti sigur ca doriti sa stergeti acest produs?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                int idComanda = db.cautaIdComanda(this.idMasa);
                String numeProdus = dataGridView2.SelectedCells[1].Value.ToString();
                int idProdus = db.cautaIdProdus(numeProdus);

                db.stergeProdusComanda(idComanda, idProdus);
                adaugaProduseDataGrid();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Start();
            lblOra.Text = "Este ora: " +  DateTime.Now.ToLongTimeString();
        }

        public static int selecteazaCantitate(string text, string caption)
        {
            Form prompt = new Form();
            prompt.MaximizeBox = false;
            prompt.Width = 500;
            prompt.Height = 170;
            prompt.Text = caption;
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 50, Width = 400 , Height = 70 };
            Button confirmation = new Button() { Text = "Ok", Left = 240, Width = 100, Top = 90 };
            Button cancel = new Button() { Text = "Cancel", Left = 350, Width = 100, Top = 90 };
            cancel.Click += (sender, e) => {  prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(cancel);
            prompt.ShowDialog();
            return 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var myForm = new FormularMese();
            myForm.StartPosition = FormStartPosition.Manual;
            myForm.Left = 300;
            myForm.Top = 100;
            myForm.Show();
            myForm.FormClosing += (obj, args) => { this.Close(); };
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Comanda a fost achitata ?", "Warning", 
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                int idComanda = db.cautaIdComanda(this.idMasa);
                db.schimbaStareComanda(this.idMasa, 1);
                db.stergeProduseComanda(idComanda);
                db.schimbaStareMasa(this.idMasa, 0);

                deschideFormularMese();
            }
            else if(result == DialogResult.No)
            {
                int idComanda = db.cautaIdComanda(this.idMasa);
                db.schimbaStareComanda(this.idMasa, 2);
                db.stergeProduseComanda(idComanda);
                db.schimbaStareMasa(this.idMasa, 0);

                deschideFormularMese();
            }
            else if (result == DialogResult.Cancel)
            {
  
            }
        }

        public void deschideFormularMese()
        {
            var myForm = new FormularMese();
            myForm.StartPosition = FormStartPosition.Manual;
            myForm.Left = 300;
            myForm.Top = 100;
            myForm.Show();
            myForm.FormClosing += (obj, args) => { this.Close(); };
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String valoareInput = Interaction.InputBox("Introduceti noua cantitate", "Cantitate", "0", -1, -1);

            if (valoareInput != "")
            {
                int myInt;
                bool isNumerical = int.TryParse(valoareInput, out myInt);

                if (isNumerical && Int32.Parse(valoareInput) > 0)
                {

                    int idComanda = db.cautaIdComanda(this.idMasa);
                    String numeProdus = dataGridView2.SelectedCells[1].Value.ToString();
                    int idProdus = db.cautaIdProdus(numeProdus);

                    int cantitate = Int32.Parse(valoareInput);
                    decimal pretProdus = db.pretProdus(idProdus);

                    db.modificaCantitate(idComanda, idProdus, cantitate, pretProdus);

                    adaugaProduseDataGrid();

                }
                if (Int32.Parse(valoareInput) == 0)
                {
                    MessageBox.Show("Cantitatea nu poate fi 0");
                }

            }

            


        }
    }
}
