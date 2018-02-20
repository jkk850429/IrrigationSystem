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
    public partial class Form6 : Form
    {
        public string userID6;
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
        public Form6()
        {
            InitializeComponent();
        }


        private DataTable GetOrderList()    //取得所需資訊
        {
            con = new SqlConnection();
            con.ConnectionString = @"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;";
            con.Open();
            adap = new SqlDataAdapter("Select mixerA as'高濃度桶攪拌時間',mixerM as'混和桶攪拌時間',motorA as'A捅抽水秒數',motorB as'B桶抽水秒數', motorC as'C桶抽水秒數',motorD as'D桶抽水秒數',motorE as'E桶抽水秒數',motorW as'水桶抽水秒數',Block1 as'區域1',Block2 as'區域2',Block3 as'區域3',Block4 as'區域4',Block5 as'區域5',Block6 as'區域6',Block7 as'區域7',Block8 as'區域8',Block9 as'區域9',Block10 as'區域10'  ,eid as'***'from emergency_list where  uID='" + userID6+"'  order by Time asc", con);

            ds = new System.Data.DataSet();
            adap.Fill(ds, "TEST");
            return ds.Tables[0];
        }
        public void check(SqlCommand cmd, SqlCommand cmd1)             //check使否為空
        {
            //cmd.CommandText = "insert into OrderList values(  '" + findIndex() + "'  ,  '" + getSystemTime() + "'  ,  '" + textBox1.Text + "'  ,  '" + textBox2.Text + "'  ,  '" + textBox3.Text + "'  ,  '" + textBox4.Text + "'  ,  '" + textBox5.Text + "'  ,  '" + textBox6.Text + "'  ,  '" + comboBox4.Text + "')";
            //cmd1.CommandText = "Insert into history_table values ('" + findMax() + "','" + getSystemTime() + "', 'emergency' ,'" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + comboBox4.Text + "' , 'none')";
            //cmd.ExecuteNonQuery();
            //cmd1.ExecuteNonQuery();

            con.Close();
            MessageBox.Show("insert success");
            this.Close();
        }


        public int transform(String s)
        {
            int a = int.Parse(s);
            return a;
        }
        /*
        public String findIndex()                                           //找到要加入的index
        {
            string strSQL = "select MAX(eID) FROM emergency_list where  uID='"+userID6+"' and Time='" + findInsertInd() + "'";
            SqlCommand findind = new SqlCommand(strSQL, con);
            object max = findind.ExecuteScalar();
            String maxNumInString = string.Format("{0:0}", max);
            int maxNumInInt = transform(maxNumInString);
            int newMax = maxNumInInt + 1;
            String newMaxInString = string.Format("{0:0}", newMax);
            return newMaxInString;
        }
        
        public String findMax1()                                           //找到要加入的index in History
        {
            string strSQL = "select MAX(hID) FROM history_table ";
            SqlCommand findind = new SqlCommand(strSQL, con);
            object max = findind.ExecuteScalar();
            String maxNumInString = string.Format("{0:0}", max);
            //MessageBox.Show(maxNumInString);
            int maxNumInInt = transform(maxNumInString);
            int newMax = maxNumInInt + 1;
            String newMaxInString = string.Format("{0:0}", newMax);
            return newMaxInString;
        }
        */
        public String getSystemTime()                         //取得系統時間(10秒後的)
        {
            return DateTime.Now.AddSeconds(10).ToString("HH:mm");
        }
        /*
        public String findInsertInd()
        {
            String a = "123";
            int hr = transform(DateTime.Now.AddSeconds(10).ToString("HH:mm").Substring(0, 2));
            int min = transform(DateTime.Now.AddSeconds(10).ToString("HH:mm").Substring(3, 2));
            for (int i = dataGridView1.RowCount - 1; i >= 0; i--)
            {
                int hr1 = transform(dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0, 2));   //取得每列時間的"時"
                int min1 = transform(dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(3, 2));   //取得每列時間的"分"
                if (hr1 <= hr && min1 <= min)
                {
                    a = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    break;
                }
            }
            return a;
        }
        */
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)   // 自動編號
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }



        private void Form6_Load_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetOrderList();
            dataGridView1.Columns["***"].ReadOnly = true;
            foreach (DataGridViewColumn gCol in dataGridView1.Columns)
                gCol.Width = 100;

            foreach (DataGridViewRow grow in dataGridView1.Rows)
                grow.Height = 50;
            this.dataGridView1.DefaultCellStyle.Font = new Font("微軟正黑體", 15, FontStyle.Regular);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 12, FontStyle.Bold);
            foreach (DataGridViewColumn column in dataGridView1.Columns)   //行不可SORTABLE(避免刪除時發生錯誤)
            {

                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
      
        private void button1_Click_1(object sender, EventArgs e)        //相當於更新emergency_list資料表
        {
            try
            {
                try
                {
                    cmdbl = new SqlCommandBuilder(adap);
                    adap.Update(ds, "TEST");
                    MessageBox.Show("緊急單更新成功", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("含有空白的欄位");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("請輸入數字(勿超過30000)");
        }
    }
}