Mohamed Hanou <mhanou442@gmail.com>
	
12:50 (il y a 7 minutes)
	
À tinaben.ca, aimedmendil9, medani176
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
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ado_connecte
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PopulateComboBox();//to fill the  combobox
        }


        //insertion + supprimer +charger
        private void inserer(int codel,string titre,string auteur,int nbexemplaires)
        {
            //connexion to the server super useful
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=PC404-02;Initial Catalog=Bibliotheque;Integrated Security=True"; //recuperer affichage>explorateur de serveurs
            conn.Open();
            //
            SqlCommand cmd = conn.CreateCommand(); // create sql command
            cmd.CommandText = "INSERT INTO Livre (CodeL, Titre, Auteur,NbExemplaires) " +
                              "VALUES (" + codel + ",'" + titre + "','" + auteur + "'," + nbexemplaires + ")"; //fill its text
            cmd.ExecuteNonQuery(); // execute the cmd we created
            MessageBox.Show("Ligne inseree");
            conn.Close();
        }
        private void charger()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=PC404-02;Initial Catalog=Bibliotheque;Integrated Security=True";
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Livre", conn);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            conn.Close();
        }
        private void supprimer(int codel)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=PC404-02;Initial Catalog=Bibliotheque;Integrated Security=True";

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Livre WHERE CodeL =" + codel;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Ligne supprimé");
            conn.Close();
        }
        private void PopulateComboBox()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=PC404-02;Initial Catalog=Bibliotheque;Integrated Security=True";
            conn.Open();
            Console.WriteLine("Reussi");
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT DISTINCT Auteur FROM Livre", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            comboBox1.Items.Clear();
            comboBox1.Items.Add("All"); // Option to show all rows
            foreach (DataRow row in dt.Rows)
                comboBox1.Items.Add(row["Auteur"].ToString());
            comboBox1.SelectedIndex = 0;
        }



         //buttons +event managers
        private void button1_Click(object sender, EventArgs e)
        {
            int codel=int.Parse(textBox1.Text);
            string titre=textBox2.Text;
            string auteur=textBox3.Text;
            int nbexemplaires = int.Parse(textBox4.Text);
            inserer(codel, titre, auteur, nbexemplaires);  

        }

        private void button2_Click(object sender, EventArgs e)
        {
            charger();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int codel = int.Parse(textBox1.Text);
            supprimer(codel);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate the textboxes with the row's values
                textBox1.Text = row.Cells["CodeL"].Value.ToString();
                textBox2.Text = row.Cells["Titre"].Value.ToString();
                textBox3.Text = row.Cells["Auteur"].Value.ToString();
                textBox4.Text = row.Cells["NbExemplaires"].Value.ToString();
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "All")
            {
                charger();
            }
            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Data Source=PC404-02;Initial Catalog=Bibliotheque;Integrated Security=True";
                conn.Open();
                string query = "SELECT * FROM Livre where Auteur = '" + comboBox1.SelectedItem.ToString() + "'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                conn.Close();
            }
        }
    }
}