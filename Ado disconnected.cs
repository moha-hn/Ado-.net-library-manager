using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AdoDeconnected
{
    public partial class Form1 : Form
    {
        DataSet dbdeconnecte { get; set; }
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        public Form1()
        {
            InitializeComponent();
            dbdeconnecte = new DataSet();
            load_dataset();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }
        private void load_dataset() //bring the db and put it in a dataset
        {
            try
            {
                //connection to the db
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Data Source=PC404-02;Initial Catalog=Bibliotheque;Integrated Security=True"; //recuperer affichage>explorateur de serveurs
                conn.Open();

                //reteiving the data from the db
                SqlDataAdapter adapter1 = new SqlDataAdapter("Select * From Livre ", conn);

                //filling the dataset with the table
                adapter1.Fill(dbdeconnecte, "Livre");
                conn.Close();
               


            }
            catch
            {
                Console.WriteLine("Erreur de connexion");
            }
        }
        private void updatedb()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=PC404-02;Initial Catalog=Bibliotheque;Integrated Security=True"; //recuperer affichage>explorateur de serveurs
            conn.Open();
            SqlDataAdapter adapter1 = new SqlDataAdapter("Select * From Livre ", conn);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter1);
            adapter1.Update(dbdeconnecte, "Livre");
            MessageBox.Show("Base de donnees mise a jour");

        }
        private void affichergrid()
        {

            //clears the gridview
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            //afficher le gridview
            dataGridView1.DataSource = dbdeconnecte.Tables["Livre"];

        }






        //buttons
        private void button1_Click(object sender, EventArgs e) //add new to the table
        {
            DataRow newrow = dbdeconnecte.Tables["Livre"].NewRow();
            newrow["CodeL"] = int.Parse(textBox1.Text);
            newrow["Titre"] = textBox2.Text;
            newrow["Auteur"] = textBox3.Text;
            newrow["NbExemplaires"] = int.Parse(textBox4.Text);
            dbdeconnecte.Tables["Livre"].Rows.Add(newrow);
        }

        private void button2_Click(object sender, EventArgs e) //load the database
        {
            affichergrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updatedb();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Charger les valeurs de la ligne sélectionnée dans les TextBox
                textBox1.Text = selectedRow.Cells["CodeL"].Value?.ToString();
                textBox2.Text = selectedRow.Cells["titre"].Value?.ToString();
                textBox3.Text = selectedRow.Cells["Auteur"].Value?.ToString();
                textBox4.Text = selectedRow.Cells["NbExemplaires"].Value?.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e) //delete
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int primarykey = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CodeL"].Value);
               
                //LINQ pour recuperer la ligne qui contient cette cle primaire et modifier
                var row = dbdeconnecte.Tables["Livre"].AsEnumerable().Where(r => r.Field<int>("CodeL") == primarykey).FirstOrDefault();
               
                if (row != null)
                {
                    row.Delete();
                    MessageBox.Show("Livre supprimé");
                    affichergrid();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) //update row
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                int primarykey = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CodeL"].Value);

                //LINQ pour recuperer la ligne qui contient cette cle primaire et modifier
                var row = dbdeconnecte.Tables["Livre"].AsEnumerable().Where(r => r.Field<int>("CodeL") == primarykey).FirstOrDefault();

                if (row != null)
                {
                    row["CodeL"] = int.Parse(textBox1.Text);
                    row["titre"] = textBox2.Text;
                    row["Auteur"] = textBox3.Text;
                    row["NbExemplaires"] =int.Parse(textBox4.Text);
                }
            }
        }
    }
}