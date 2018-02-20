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
    public partial class Form5 : Form
    {
        public string userID5;
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
        SqlCommandBuilder cmdbl;
        public Form5()
        {
            InitializeComponent();
        }
        private DataTable GetOrderList()  //載入所需資訊
        {
            con = new SqlConnection();
            con.ConnectionString = (@"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;");
            con.Open();
            adap = new SqlDataAdapter("Select hid as '歷史訂單編號',Action as'動作',Time as'灌溉時間',motorA as'A捅抽水秒數',motorB as'B桶抽水秒數', motorC as'C桶抽水秒數',motorD as'D桶抽水秒數',motorE as'E桶抽水秒數',motorW as'水桶抽水秒數',Block1 as'區域1',Block2 as'區域2',Block3 as'區域3',Block4 as'區域4',Block5 as'區域5',Block6 as'區域6',Block7 as'區域7',Block8 as'區域8',Block9 as'區域9',Block10 as'區域10' from history_table  where  uID='"+userID5+"' and hid!=0 ", con);
            ds = new System.Data.DataSet();
           adap.Fill(ds, "TEST");
            return ds.Tables[0];
        }

 

  
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form5_Load_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetOrderList();
            foreach (DataGridViewColumn gCol in dataGridView1.Columns)
                gCol.Width = 100;

            foreach (DataGridViewRow grow in dataGridView1.Rows)
                grow.Height = 50;
            this.dataGridView1.DefaultCellStyle.Font = new Font("微軟正黑體", 15, FontStyle.Regular);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 12, FontStyle.Bold);
        }
    }
}
