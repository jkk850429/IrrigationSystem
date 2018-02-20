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
    public partial class Form3 : Form
    {
        public string userID3;
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
        public Form3()
        {
            InitializeComponent();
        }

        private DataTable GetOrderList()            //從DB中取得資料並顯示出來
        {
            con = new SqlConnection();
            con.ConnectionString = (@"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;");
            con.Open();
            adap = new SqlDataAdapter("Select Time as'灌溉時間',mixerA as'高濃度桶攪拌時間',mixerM as'混和桶攪拌時間',motorA as'A捅抽水秒數',motorB as'B桶抽水秒數', motorC as'C桶抽水秒數',motorD as'D桶抽水秒數',motorE as'E桶抽水秒數',motorW as'水桶抽水秒數',Block1 as'區域1',Block2 as'區域2',Block3 as'區域3',Block4 as'區域4',Block5 as'區域5',Block6 as'區域6',Block7 as'區域7',Block8 as'區域8',Block9 as'區域9',Block10 as'區域10',oid as'*' from OrderList where  oid%10=0 and oid >10 and uID='" + userID3 + "' order by Time asc", con);
            ds = new System.Data.DataSet();
            adap.Fill(ds, "TEST");
            return ds.Tables[0];
        }

        public int transform(String s)              //丟入String傳回int
        {
            int a = int.Parse(s);
            return a;
        }
        public String findmax()                     //找出history_table最大的hID，+1後回傳
        {
            string strSQL = "select MAX(hId) FROM history_table";
            SqlCommand findmax = new SqlCommand(strSQL, con);
            object max = findmax.ExecuteScalar();
            String maxNumInString = string.Format("{0:0}", max);
            int maxNumInInt = transform(maxNumInString);
            int newMax = maxNumInInt + 1;
            String newMaxInString = string.Format("{0:0}", newMax);
            return newMaxInString;
        }
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)  //當有做修改的動作，就必須記錄到history_table裡面
        {
            con = new SqlConnection();
            con.ConnectionString = (@"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;");
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into history_table values(  '" + findmax() + "'  ,'" + dataGridView1.CurrentRow.Cells[0].Value + "'  ,  '" + "編輯" + "'  ,  '" + dataGridView1.CurrentRow.Cells[3].Value + "'  ,  '" + dataGridView1.CurrentRow.Cells[4].Value + "'  ,  '" + dataGridView1.CurrentRow.Cells[5].Value + "'  ,  '" + dataGridView1.CurrentRow.Cells[6].Value + "'  ,  '" + dataGridView1.CurrentRow.Cells[7].Value + "'  ,  '" + dataGridView1.CurrentRow.Cells[8].Value + "','" + dataGridView1.CurrentRow.Cells[9].Value + "','" + dataGridView1.CurrentRow.Cells[10].Value + "','" + dataGridView1.CurrentRow.Cells[11].Value + "','" + dataGridView1.CurrentRow.Cells[12].Value + "','" + dataGridView1.CurrentRow.Cells[13].Value + "','" + dataGridView1.CurrentRow.Cells[14].Value + "','" + dataGridView1.CurrentRow.Cells[15].Value + "','" + dataGridView1.CurrentRow.Cells[16].Value + "','" + dataGridView1.CurrentRow.Cells[17].Value + "','" + dataGridView1.CurrentRow.Cells[18].Value + "','non','"+userID3+"')";
            cmd.ExecuteNonQuery();
            con.Close();

        }

  

        private void button1_Click(object sender, System.EventArgs e)     //編輯玩按下確認鍵(此種寫法會自動更新Orderlist內容)
        {
            try
            {
                cmdbl = new SqlCommandBuilder(adap);
                try
                {
                    adap.Update(ds, "TEST");
                    MessageBox.Show("編輯成功", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //this.Close();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void Form3_Load_1(object sender, EventArgs e)      
        {
            dataGridView1.DataSource = GetOrderList();
            dataGridView1.ScrollBars = ScrollBars.Both;
            foreach (DataGridViewColumn gCol in dataGridView1.Columns)
                gCol.Width = 100;

            foreach (DataGridViewRow grow in dataGridView1.Rows)
                grow.Height = 50;
            this.dataGridView1.DefaultCellStyle.Font = new Font("微軟正黑體", 15, FontStyle.Regular);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 12, FontStyle.Bold);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("請輸入數字(勿超過30000)");
        }

    }
}
