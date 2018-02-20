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
    public partial class Form2 : Form
    {
        SqlDataAdapter adap;
        DataSet ds;
        SqlCommandBuilder cmdbl;
        private Point startPoint;
        public string userID2;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //當滑鼠擊點到Form1控制項的範圍內時，紀錄目前是窗的位置
            startPoint = new Point(-e.X + SystemInformation.FrameBorderSize.Width, -e.Y - SystemInformation.FrameBorderSize.Height);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //如果使用者使用的是左鍵按下，意旨使用右鍵拖曳無效
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                //新視窗的位置
                mousePos.Offset(startPoint.X, startPoint.Y);
                //改變視窗位置
                Location = mousePos;
            }

        }
        SqlConnection con = new SqlConnection(@"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;");
        public Form2()
        {
            InitializeComponent();
        }
        private void MorE()            //判斷使用者選擇的是上午or下午，若是下午則要+12小時(轉成24小時制)
        {
            if (comboBox1.Text == "上午" && comboBox2.Text == "12")
            {
                comboBox2.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox2.Text = "00"; 
            }
            else if (comboBox1.Text == "下午" && comboBox2.Text == "12")
            {
                comboBox2.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox2.Text = "12";
            }
            else
            {
                if (comboBox1.Text == "下午")
                {
                    int a = transform(comboBox2.Text);
                    a = (a + 12) % 24;
                    String hour = a.ToString("00");
                    comboBox2.DropDownStyle = ComboBoxStyle.DropDown;
                    comboBox2.Text = hour;
                }
            } 
        }

        public void check(SqlCommand cmd1, SqlCommand cmd)
        {
            if ((comboBox1.Text.Length == 0) || (comboBox2.Text.Length == 0) || (comboBox3.Text.Length == 0))     //若有相關資訊為輸入則提醒他們
            {
                MessageBox.Show("請輸入灌溉時間");
                con.Close();
            }else if (checkBox1.Checked==false && checkBox2.Checked == false && checkBox3.Checked == false && checkBox4.Checked == false && checkBox5.Checked == false && checkBox6.Checked == false && checkBox7.Checked == false && checkBox8.Checked == false && checkBox9.Checked == false && checkBox10.Checked == false)
            {
                MessageBox.Show("未選擇灌溉區域");
                con.Close();
            }
            else
            {
                //MorE();
                Boolean chk = checkclick();
                if (chk)
                {
                    con.Close();
                    MessageBox.Show("新增成功");
                    this.Close();
                }
                else if(chk ==false)
                {
                    con.Close();
                    MessageBox.Show("新增失敗");
                }
            }
        }
        public int transform(String s)              //傳入string並回傳int
        {
            try
            {
                int a = int.Parse(s);
                return a;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
        private DataTable GetOrderList()            //取得相關資訊from Database
        {
            con = new SqlConnection();
            con.ConnectionString = @"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;";
            con.Open();
            adap = new SqlDataAdapter("Select Time as'灌溉時間',mixerA as'高濃度桶桶攪拌時間',mixerM as'混和桶攪拌時間',motorA as'A捅抽水秒數',motorB as'B桶抽水秒數', motorC as'C桶抽水秒數',motorD as'D桶抽水秒數',motorE as'E桶抽水秒數',Block1 as'區域1',Block2 as'區域2',Block3 as'區域3',Block4 as'區域4',Block5 as'區域5',Block6 as'區域6',Block7 as'區域7',Block8 as'區域8',Block9 as'區域9',Block10 as'區域10' from OrderList where  uID='"+userID2+"' and oid%10=0 and oid > 10 order by Time asc", con);
            ds = new System.Data.DataSet();
            adap.Fill(ds, "TEST");
            return ds.Tables[0];
        }
        public String findmax()                     //從Order_list資料表中找出目前最大的oID，+1後回傳
        {
            String strSQL = "select MAX(oID) FROM OrderList where oID%10='0'";
            SqlCommand findmax = new SqlCommand(strSQL, con);
            object max = findmax.ExecuteScalar();
            String maxNumInString = string.Format("{0:0}", max);
            int maxNumInInt = transform(maxNumInString);
            int newMax = maxNumInInt + 10;
            String newMaxInString = string.Format("{0:0}", newMax);
            return newMaxInString;
        }
        public String findmax1()                    //從history_table資料表中找出目前最大的HID，+1後回傳
        {
            string strSQL = "select MAX(hid) FROM history_table   ";
            SqlCommand findmax1 = new SqlCommand(strSQL, con);
            object max = findmax1.ExecuteScalar();
            String maxNumInString = string.Format("{0:0}", max);
            int maxNumInInt = transform(maxNumInString);
            int newMax = maxNumInInt + 1;
            String newMaxInString = string.Format("{0:0}", newMax);
            return newMaxInString;
        }

        public Boolean checkclick()                 
        {
            Boolean flag = false;
            int hr = transform(comboBox2.Text);
            int min = transform(comboBox3.Text);
            for (int i = 0; i < dataGridView1.RowCount-1; i++)
            {
                int hr1 = transform(dataGridView1.Rows[i].Cells[0].Value.ToString().Substring(0, 2));   //取得每列時間的"時"
                int min1 = transform(dataGridView1.Rows[i].Cells[0].Value.ToString().Substring(3, 2));   //取得每列時間的"分"
                if (hr == hr1 && min == min1)
                {
                    MessageBox.Show("不能出現相同灌溉時間");
                    return flag;
                }
            }
            int[] a = new int[11];                       //存灌溉區域(1代表使用者有勾選，0代表沒有)
            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            SqlCommand cmd3 = con.CreateCommand();
            cmd3.CommandType = CommandType.Text;
            if (checkBox1.Checked == true)                //設定a[]內的值
            {
                a[1] = 1;
            }
            else
            {
                a[1] = 0;
            }
            if (checkBox2.Checked == true)
            {
                a[2] = 1;
            }
            else
            {
                a[2] = 0;
            }
            if (checkBox3.Checked == true)
            {
                a[3] = 1;
            }
            else
            {
                a[3] = 0;
            }
            if (checkBox4.Checked == true)
            {
                a[4] = 1;
            }
            else
            {
                a[4] = 0;
            }
            if (checkBox5.Checked == true)
            {
                a[5] = 1;
            }
            else
            {
                a[5] = 0;
            }
            if (checkBox6.Checked == true)
            {
                a[6] = 1;
            }
            else
            {
                a[6] = 0;
            }
            if (checkBox7.Checked == true)
            {
                a[7] = 1;
            }
            else
            {
                a[7] = 0;
            }
            if (checkBox8.Checked == true)
            {
                a[8] = 1;
            }
            else
            {
                a[8] = 0;
            }
            if (checkBox9.Checked == true)
            {
                a[9] = 1;
            }
            else
            {
                a[9] = 0;
            }
            if (checkBox10.Checked == true)
            {
                a[10] = 1;
            }
            else
            {
                a[10] = 0;
            }
            try
            {
                MorE();  //上下午轉換
                cmd2.CommandText = "insert into orderlist values(  '" + findmax() + "'  ,  '" + comboBox2.Text + ":" + comboBox3.Text + "'  ,  '" + textBox1.Text + "'  ,  '" + textBox2.Text + "'  ,  '" + textBox3.Text + "'  ,  '" + textBox4.Text + "'  ,  '" + textBox5.Text + "'  ,  '" + textBox6.Text + "'  ,  '" + a[1] + "',  '" + a[2] + "',  '" + a[3] + "',  '" + a[4] + "',  '" + a[5] + "',  '" + a[6] + "',  '" + a[7] + "',  '" + a[8] + "',  '" + a[9] + "',  '" + a[10] + "',  '" + textBox7.Text + "', '" + textBox12.Text + "','"+userID2+"')";        //將這筆單資訊insert到Orderlist資料表裡面

                cmd2.ExecuteNonQuery();

                cmd3.CommandText = "insert into history_table values(  '" + findmax1() + "'  ,  '" + comboBox2.Text + ":" + comboBox3.Text + "'  ,  ' 新增 '  , '" + textBox1.Text + "'  ,  '" + textBox2.Text + "'  ,  '" + textBox3.Text + "'  ,  '" + textBox4.Text + "'  ,  '" + textBox5.Text + "'  ,  '" + textBox6.Text + "'  ,  '" + a[1] + "',  '" + a[2] + "',  '" + a[3] + "',  '" + a[4] + "',  '" + a[5] + "',  '" + a[6] + "',  '" + a[7] + "',  '" + a[8] + "',  '" + a[9] + "',  '" + a[10] + "','null','"+userID2+"')";                                    //也將這筆單資訊insert到history_table資料表裡面

                cmd3.ExecuteNonQuery();
                flag = true;
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
                return flag;
                con.Close();
            }
        return flag;
        }

        private void button1_Click(object sender, EventArgs e)        //按下確認鈕
        {
            try
            {
                con.Open();
            }catch(Exception ex)
            {
            
            }
      
            SqlCommand cmd = con.CreateCommand();
            SqlCommand cmd1 = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd1.CommandType = CommandType.Text;
            check(cmd, cmd1);
            con.Close();
            this.Close();

        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)      //取消鈕
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)      
        {
            dataGridView1.DataSource = GetOrderList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(userID2);
        }
    }
}
