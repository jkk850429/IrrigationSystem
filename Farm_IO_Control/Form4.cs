using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Farm_IO_Control
{
    public partial class Form4 : Form
    {
        public string userID4;
        private Point startPoint;
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
        SqlConnection con;
        SqlDataAdapter adap;
        DataSet ds;
        //SqlCommandBuilder cmdbl;
        public Form4()
        {
            InitializeComponent();
        }
        private void Form4_Load(object sender, EventArgs e)     //initialization
        {
            dataGridView1.DataSource = GetOrderList();
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[8].ReadOnly = true;
            dataGridView1.Columns[9].ReadOnly = true;
            dataGridView1.DataSource = GetOrderList();
            foreach (DataGridViewColumn gCol in dataGridView1.Columns)
                gCol.Width = 100;

            foreach (DataGridViewRow grow in dataGridView1.Rows)
                grow.Height = 50;
            this.dataGridView1.DefaultCellStyle.Font = new Font("微軟正黑體", 15, FontStyle.Regular);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 12, FontStyle.Bold);
            foreach (DataGridViewRow dr in dataGridView1.Rows)      //initialize checkbox
            {
                dr.Cells[0].Value = false;
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)   //行不可SORTABLE(避免刪除時發生錯誤)
            {

                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private DataTable GetOrderList()            //載入資料
        {
            con = new SqlConnection();
            con.ConnectionString = (@"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;");
            con.Open();
            adap = new SqlDataAdapter("Select Time as'灌溉時間',mixerA as'高濃度桶攪拌時間',mixerM as'混和桶攪拌時間',motorA as'A捅抽水秒數',motorB as'B桶抽水秒數', motorC as'C桶抽水秒數',motorD as'D桶抽水秒數',motorE as'E桶抽水秒數',motorW as'水桶抽水秒數',Block1 as'區域1',Block2 as'區域2',Block3 as'區域3',Block4 as'區域4',Block5 as'區域5',Block6 as'區域6',Block7 as'區域7',Block8 as'區域8',Block9 as'區域9',Block10 as'區域10' from OrderList where  uID='"+userID4+"' and oid%10=0 and oid>10 order by Time asc ", con);
            ds = new System.Data.DataSet();
            adap.Fill(ds, "TEST");
            return ds.Tables[0];
        }

        public int transform(String s)
        {
            int a = int.Parse(s);
            return a;
        }
        public String findhidmax()          //找出history_table中最大hID，+1後並回傳
        {
            string strSQL = "select MAX(hid) FROM history_table  ";
            SqlCommand findhidmax = new SqlCommand(strSQL, con);
            object max = findhidmax.ExecuteScalar();
            String maxNumInString = string.Format("{0:0}", max);
            int maxNumInInt = transform(maxNumInString);
            int newMax = maxNumInInt + 1;
            String newMaxInString = string.Format("{0:0}", newMax);
            return newMaxInString;
        }
   

        private void button1_Click_1(object sender, EventArgs e)      //按下確認鈕
        {
            con = new SqlConnection();
            con.ConnectionString = (@"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;");
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            foreach (DataGridViewRow dr1 in dataGridView1.Rows)                                   //真正delete function，同時需記錄到history_table，並把Orderlist中使用者以勾選的資料刪除
            {
                cmd1.CommandText = "Insert into history_table values ('" + findhidmax() + "','" + dr1.Cells[1].Value.ToString() + "', '刪除' ,'" + dr1.Cells[2].Value.ToString() + "','" + dr1.Cells[3].Value.ToString() + "','" + dr1.Cells[4].Value.ToString() + "','" + dr1.Cells[5].Value.ToString() + "','" + dr1.Cells[6].Value.ToString() + "','" + dr1.Cells[7].Value.ToString() + "','" + dr1.Cells[8].Value.ToString() + "' ,'" + dr1.Cells[9].Value.ToString() + "','" + dr1.Cells[10].Value.ToString() + "','" + dr1.Cells[11].Value.ToString() + "','" + dr1.Cells[12].Value.ToString() + "','" + dr1.Cells[13].Value.ToString() + "','" + dr1.Cells[14].Value.ToString() + "','" + dr1.Cells[15].Value.ToString() + "','" + dr1.Cells[16].Value.ToString() + "','" + dr1.Cells[17].Value.ToString() + "' ,'none','"+userID4+"')";
                cmd.CommandText = "Delete from orderlist where Time = '" + dr1.Cells[1].Value.ToString() + "' and uid='"+userID4+"'";

                if (dr1.Cells[0].Value.ToString() == "True") //判斷是否勾選
                {
                    cmd.ExecuteNonQuery();
                    cmd1.ExecuteNonQuery();
                }
            }

            con.Close();
            dataGridView1.DataSource = GetOrderList();              //刪除完後reload界面
            foreach (DataGridViewRow dr in dataGridView1.Rows)      //initialize checkbox again
            {
                dr.Cells[0].Value = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
