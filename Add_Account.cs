using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;

namespace XBCAD_Stock_Taking_application
{
    public partial class Add_Account : Form
    {
        public Add_Account()
        {
            InitializeComponent();
        }
      

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            bool strongPass = false;
            bool inputValid = false; ;
            string Password="";
            string hashedPassword="";
            if (txtPassword.Text.Length>0)
            {
                if (txtUsername.Text.Length>0)
                {
                    if (cbRole.Text.Length>0)
                    {
                        inputValid = true;
                        Password = txtPassword.Text;
                        hashedPassword = Crypto.HashPassword(Password);
                        strongPass = checkStrongPass(Password);
                    }
                }
            }

            string username = string.Empty;
            if (strongPass)
            {
                string checkQuery = "Select * from [Reg_Users] where Username = @Username";

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Connect"].ToString();
                SqlCommand cmd = new SqlCommand(checkQuery, con);
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    username = rd["Username"].ToString();
                    break;
                }
                if (username == txtUsername.Text)
                {
                    MessageBox.Show("This Username has already been used. \n Please pick another.");
                }
                else
                {
                    string insertQuery = "Insert into [Reg_Users](Username, Password, Role)" +
                                          "Values(@valUsername, @valPassword, @valRole)";

                    con.Close();
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand(insertQuery, con);

                    //Using parameters help defend against SQLinjection attacks.
                    cmd2.Parameters.AddWithValue("valUsername", txtUsername.Text);
                    cmd2.Parameters.AddWithValue("valPassword", hashedPassword);
                    cmd2.Parameters.AddWithValue("valRole", cbRole.SelectedItem.ToString());

                    int result = cmd2.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Your Account has been created successfully");
                        new Login().ShowDialog();
                        this.Close();
                    }
                }
            }
            else
            {
                if (inputValid)
                {
                    MessageBox.Show("Password Does Not Meet Requirments");
                }
                else
                {
                    MessageBox.Show("Please fill in all text fields");
                }
            }
            
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login Lgn = new Login();
            Lgn.Location = this.Location;
            Lgn.StartPosition = FormStartPosition.Manual;
            Lgn.ShowDialog();
            this.Close();
        }

        private bool checkStrongPass(string password)
        {
            bool strong = true;
            if (password.Length >8)//check password length
            {
                char[] uppercaseArr = new char[26];//check if contains uppercase

                for (int i = 0; i < 26; i++)
                {
                    uppercaseArr[i] = (char)(65 + i);
                }

                strong = false;
                for (int i = 0; i < uppercaseArr.Length; i++)
                {
                    if (password.Contains(uppercaseArr[i]))
                    {
                        strong = true;
                        i = uppercaseArr.Length + 1;
                    }
                }
                if (strong)//check if contains lowercase
                {
                    char[] lowercaseArr = new char[26];

                    for (int i = 0; i < 26; i++)
                    {
                        lowercaseArr[i] = (char)(97 + i);
                    }

                    strong = false;
                    for (int i = 0; i < lowercaseArr.Length; i++)
                    {
                        if (password.Contains(lowercaseArr[i]))
                        {
                            strong = true;
                            i = lowercaseArr.Length + 1;
                        }
                    }
                }

                if (strong)//check if contains symbol
                {
                    char[] symbolArr = {'@','!','#','$','%','^', '&','*','(',')','_','-','+','=','~','`','{','}','[',']',';',':','"','\'','\\','|','<','>',',','.','/'};

                    strong = false;
                    for (int i = 0; i < symbolArr.Length; i++)
                    {
                        if (password.Contains(symbolArr[i]))
                        {
                            strong = true;
                            i = symbolArr.Length + 1;
                        }
                    }
                }
            }
            else
            {
                strong = false;
            }
            return strong;
            //return false;
        }

        private void btnShowReq_Click(object sender, EventArgs e)
        {
            string req = "Passwords Must:\n" +
                "Be atleast 8 characters long,\n" +
                "Include atleast 1 lower case letter,\n" +
                "Include atleast 1 upper case letter,\n" +
                "Include atleast 1 symbol (e.g: @#$%!?;)\n";

            MessageBox.Show(req, "Password Requirements");
        }

        private void btnShowPass_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
        }
    }
}
