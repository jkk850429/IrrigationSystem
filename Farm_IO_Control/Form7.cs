using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Farm_IO_Control
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)                       //由使用者輸入帳號密碼，若正確則跳到Form1(程式主畫面)
        {
            SqlConnection con = new SqlConnection(@"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;");
            string query = "Select * from user_table where uID ='" + textBox1.Text + "' and password = '" + textBox2.Text + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                this.Hide();
                Form1 f1 = new Form1();
                f1.Show();
                f1.userID1 = textBox1.Text;                                          //根據帳號顯示所屬資訊
            }
            else
            {
                MessageBox.Show("請檢查帳號或密碼");
            }
        }
        private void button2_Click(object sender, EventArgs e)                       //取消按鈕
        {
            this.Close();
        }
    }
}
