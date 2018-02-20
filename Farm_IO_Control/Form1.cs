using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Timers;
using System.Threading;
using CS_ProtocolNS;
using CS_Chen_Farm;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.VisualBasic;



namespace Farm_IO_Control
{
    public partial class Form1 : Form
    {
        public string userID1;        //form7傳過來的(user name)
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
        SqlDataAdapter adap;
        DataSet ds;
        SqlCommandBuilder cmdbl;
        #region delegate
        private delegate void UpdateFormInvokeHandler();
        private delegate string SetProtocalMessage(short[] data, byte dataLength);
        #endregion

        #region declare
        System.Timers.Timer m_tmrUpdateForm;
        DateTime m_nowTime;
        SerialPort m_serialPort;
        CS_Protocol_Hnet_Server m_hnetServer;
        Image[] m_Indicator;
        /*******************************************/
        bool m_isLinkDevice = false;
        string m_connectPortName = "";
        /*******************************************/
        Chen_Farm_Config m_farmConfig;
        DateTimeGroup[] m_dateTimeGroup;
        FarmIoGroup[] m_emergencyGroup;
        byte m_counterDI, m_counterDO;
        bool[] m_doState;
        bool[] m_diState;
        string m_showReadConfig = "";
        int m_dateTimeLength, m_emergencyLength;
        #endregion

        #region construct
        public Form1()
        {
            InitializeComponent();
            this.InitialForm();
        }

        private void InitialForm()
        {
            this.m_Indicator = new Image[2] { Image.FromFile(@"Resource\Circle Red_Dark.png"), Image.FromFile(@"Resource\Circle Green_Light.png") };
            this.m_farmConfig = new Chen_Farm_Config();
            this.m_farmConfig.EventGetConfigLength += m_farmConfig_EventGetConfigLength;
            this.m_farmConfig.EventGetConfigWaitLevelStableDuration += m_farmConfig_EventGetConfigWaitLevelStableDuration;
            this.m_farmConfig.EventGetConfigIoGroup += m_farmConfig_EventGetConfigIoGroup;
            this.m_farmConfig.EventGetConfigGroupStartTime += m_farmConfig_EventGetConfigGroupStartTime;
            this.m_farmConfig.EventGetConfigDescription += m_farmConfig_EventGetConfigDescription;
            this.m_farmConfig.EventGetRunningIndex += m_farmConfig_EventGetRunningIndex;
            this.m_farmConfig.EventGetSystemTime += m_farmConfig_EventGetSystemTime;

            this.m_farmConfig.EventWriteConfigLength += m_farmConfig_EventWriteConfigLength;
            this.m_farmConfig.EventWriteConfigWaitLevelStableDuration += m_farmConfig_EventWriteConfigWaitLevelStableDuration;
            this.m_farmConfig.EventWriteConfigIoGroup += m_farmConfig_EventWriteConfigIoGroup;
            this.m_farmConfig.EventWriteConfigGroupStartTime += m_farmConfig_EventWriteConfigGroupStartTime;
            this.m_farmConfig.EventWriteConfigDescription += m_farmConfig_EventWriteConfigDescription;
            this.m_farmConfig.EventWriteRunningIndex += m_farmConfig_EventWriteRunningIndex;
            this.m_farmConfig.EventWriteSystemTime += m_farmConfig_EventWriteSystemTime;
            this.m_farmConfig.EventWriteDeviceSaveSetting += m_farmConfig_EventWriteDeviceSaveSetting;

            this.m_hnetServer = new CS_Protocol_Hnet_Server();

            this.m_nowTime = DateTime.Now;
            this.m_tmrUpdateForm = new System.Timers.Timer(1000);
            this.m_tmrUpdateForm.Elapsed += m_tmrUpdateForm_Elapsed;

            this.m_serialPort = new SerialPort();
            this.m_serialPort.BaudRate = 115200;
            this.m_serialPort.DataBits = 8;
            this.m_serialPort.Parity = Parity.None;
            this.m_serialPort.StopBits = StopBits.One;
            //this.m_serialPort.ReadTimeout = CS_Protocol_Hnet.HNET_READ_TIMEOUT;
            this.m_serialPort.ReadTimeout = 500;
            this.m_serialPort.NewLine = this.m_hnetServer.ProtocolNewLine;

            this.m_hnetServer.CheckDeviceID = 1;
            this.m_hnetServer.EventResponseGetCardType += m_hnetServer_EventResponseGetCardType;
            this.m_hnetServer.EventResponseGetIO += m_hnetServer_EventResponseGetIO;
            this.m_hnetServer.EventResponseWriteIO += m_hnetServer_EventResponseWriteIO;
            this.m_hnetServer.EventResponseWriteConfig += m_hnetServer_EventResponseWriteConfig;
            this.m_hnetServer.EventResponseGetConfig += m_hnetServer_EventResponseGetConfig;
            this.m_hnetServer.EventResponseError += m_hnetServer_EventResponseError;


            //11 16 新增
            this.m_farmConfig.EventWriteSpecialIO += m_farmConfig_EventWriteSpecialIO;
            this.m_farmConfig.EventGetSpecialIO += m_farmConfig_EventGetSpecialIO;

            this.UpdateForm();
            this.m_tmrUpdateForm.Start();
            this.QueryDeviceExist();
        }
        #endregion

        private void DemoShowGroup()
        {
            FarmIoUnit[] tempIoUnit;
            byte tempUnitLength;
            string tempShow;
            tempShow = "Emergency Length : " + this.m_emergencyLength + " , DateTime length : " + this.m_dateTimeLength + Environment.NewLine + Environment.NewLine;
            if (this.m_emergencyLength != 0)
            {
                for (int i = 0; i < this.m_emergencyLength; i++)
                {
                    tempShow += "Emergency " + Convert.ToString(i) + " wait duration : " + Convert.ToString(this.m_emergencyGroup[i].WaitLevelStableDuration) + Environment.NewLine;
                    for (byte j = 0; j < 4; j++)
                    {
                        tempUnitLength = this.m_emergencyGroup[i].GetIoUnitLengthByProcessIndex(j);
                        if (tempUnitLength != 0)
                        {
                            tempIoUnit = this.m_emergencyGroup[i].GetIoUnitByProcessIndex(j);
                            tempShow += "Emergency " + Convert.ToString(i) + ", process : " + Convert.ToString(j) + " >> ";
                            tempShow += this.GetDemoIoUnit(tempUnitLength, tempIoUnit);
                        }
                    }
                }
            }
            tempShow += Environment.NewLine;
            if (this.m_dateTimeLength != 0)
            {
                for (int i = 0; i < this.m_dateTimeLength; i++)
                {
                    tempShow += "DateTime " + Convert.ToString(i) + " wait duration : " + Convert.ToString(this.m_dateTimeGroup[i].WaitLevelStableDuration) + Environment.NewLine;
                    tempShow += "DateTime " + Convert.ToString(i) + " start time : " + Convert.ToString(this.m_dateTimeGroup[i].GroupStartHour) + ":" + Convert.ToString(this.m_dateTimeGroup[i].GroupStartMinute) + ":" + Convert.ToString(this.m_dateTimeGroup[i].GroupStartSecond) + Environment.NewLine;
                    for (byte j = 0; j < 4; j++)
                    {
                        tempUnitLength = this.m_dateTimeGroup[i].GetIoUnitLengthByProcessIndex(j);
                        if (tempUnitLength != 0)
                        {
                            tempShow += "DateTime " + Convert.ToString(i) + ", process : " + Convert.ToString(j) + " >> ";
                            tempShow += this.GetDemoIoUnit(tempUnitLength, this.m_dateTimeGroup[i].GetIoUnitByProcessIndex(j));
                        }
                    }
                    tempShow += Environment.NewLine;
                }
            }
            MessageBox.Show(tempShow);
        }

        string GetDemoIoUnit(byte unitLength, FarmIoUnit[] demoUnit)
        {
            string unitStr = "";
            for (int i = 0; i < unitLength; i++)
                unitStr += "(" + Convert.ToString(demoUnit[i].ID) + ", " + Convert.ToString(demoUnit[i].Duration) + ")  ";
            unitStr += Environment.NewLine;
            return unitStr;
        }
        #region event



        //11/16
        void m_farmConfig_EventWriteSpecialIO(object sender, ConfigSpecialIOEventArgs e)
        { 
            string ioString = "";
            int[] ioID = e.IO;
            for (int i = 0; i < e.IOLength; i++)
            {
                ioString += Convert.ToString(ioID[i]);
                if (i != e.IOLength - 1)
                    ioString += ", ";
            }
            MessageBox.Show("設定特殊 IO " + Convert.ToString(e.IOLength) + " || " + ioString);
        }
        void m_farmConfig_EventGetSpecialIO(object sender, ConfigSpecialIOEventArgs e)
        {
            string ioString = "";
            int[] ioID = e.IO;
            for (int i = 0; i < e.IOLength; i++)
            {
                ioString += Convert.ToString(ioID[i]);
                if (i != e.IOLength - 1)
                    ioString += ", ";
            }
            MessageBox.Show("設定特殊 IO " + Convert.ToString(e.IOLength) + " || " + ioString);
        }






        void m_farmConfig_EventGetConfigLength(object sender, ConfigLengthEventArgs e)
        {
            this.m_dateTimeLength = e.DateTimeGroupLength;
            this.m_emergencyLength = e.EmergencyGroupLength;
            this.m_dateTimeGroup = new DateTimeGroup[this.m_dateTimeLength];
            this.m_emergencyGroup = new FarmIoGroup[this.m_emergencyLength];
            for (int i = 0; i < this.m_dateTimeLength; i++)
                this.m_dateTimeGroup[i] = new DateTimeGroup();
            for (int i = 0; i < this.m_emergencyLength; i++)
                this.m_emergencyGroup[i] = new FarmIoGroup();
        }

        void m_farmConfig_EventGetConfigWaitLevelStableDuration(object sender, ConfigWaitLevelStableDurationEventArgs e)
        {
            try
            {
                if (e.ConfigIndex <= 3)
                    this.m_emergencyGroup[e.ConfigIndex].WaitLevelStableDuration = e.Duration;
                else
                {
                    int idx = e.ConfigIndex - 4;
                    this.m_dateTimeGroup[idx].WaitLevelStableDuration = e.Duration;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void m_farmConfig_EventGetConfigIoGroup(object sender, ConfigIoGroupEventArgs e)
        {
            try
            {
                if (e.ConfigIndex <= 3)
                    this.m_emergencyGroup[e.ConfigIndex].SetIoUnitByIndex(e.ProcessIndex, e.IoUnitLength, e.FarmIoUnit);
                else
                {
                    int idx = e.ConfigIndex - 4;
                    this.m_dateTimeGroup[idx].SetIoUnitByIndex(e.ProcessIndex, e.IoUnitLength, e.FarmIoUnit);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void m_farmConfig_EventGetConfigGroupStartTime(object sender, ConfigGroupStartTimeEnentArgs e)
        {
            try
            {
                int idx = e.ConfigIndex - 4;
                this.m_dateTimeGroup[idx].GroupStartHour = e.Hour;
                this.m_dateTimeGroup[idx].GroupStartMinute = e.Minute;
                this.m_dateTimeGroup[idx].GroupStartSecond = e.Second;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void m_farmConfig_EventGetConfigDescription(object sender, ConfigDescriptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        void m_farmConfig_EventGetRunningIndex(object sender, ConfigRunningIndexEventArgs e)
        {
            MessageBox.Show("Device running index : " + Convert.ToString(e.RunningIndex) + Environment.NewLine);
        }

        void m_farmConfig_EventGetSystemTime(object sender, ConfigSystemTimeEventArgs e)
        {
            DateTime recvDateTime = e.DeviceDateTime;
            MessageBox.Show("讀取設備時間" + recvDateTime.ToString(" hh : mm : ss"));
        }

        void m_farmConfig_EventWriteConfigLength(object sender, ConfigLengthEventArgs e)
        {
            //MessageBox.Show("Receive write length");
            //receive from device by write config length command
        }

        void m_farmConfig_EventWriteConfigWaitLevelStableDuration(object sender, ConfigWaitLevelStableDurationEventArgs e)
        {
            //MessageBox.Show("Receive write level stable");

            this.m_showReadConfig = "Wait Stable : " + Convert.ToString(e.ConfigIndex) + " . " + Convert.ToString(e.Duration) + Environment.NewLine;
        }

        void m_farmConfig_EventWriteConfigIoGroup(object sender, ConfigIoGroupEventArgs e)
        {
            //MessageBox.Show("Receive io group");
            this.m_showReadConfig += "Group : " + Convert.ToString(e.ConfigIndex) + " . " + Convert.ToString(e.ProcessIndex) + " . " + Convert.ToString(e.FarmIoUnit) + Environment.NewLine;
            if (e.ConfigIndex <= 3 && e.ProcessIndex == 3)
                MessageBox.Show(this.m_showReadConfig);
        }

        void m_farmConfig_EventWriteConfigGroupStartTime(object sender, ConfigGroupStartTimeEnentArgs e)
        {
            //MessageBox.Show("Receive start time");
            this.m_showReadConfig += "Start Time : " + Convert.ToString(e.ConfigIndex) + " . " + Convert.ToString(e.Hour) + " : " + Convert.ToString(e.Minute) + Environment.NewLine;
           // MessageBox.Show(this.m_showReadConfig);
        }

        void m_farmConfig_EventWriteConfigDescription(object sender, ConfigDescriptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        void m_farmConfig_EventWriteRunningIndex(object sender, ConfigRunningIndexEventArgs e)
        {
            MessageBox.Show("設定執行訂單: " + Convert.ToString(e.RunningIndex));
        }

        void m_farmConfig_EventWriteSystemTime(object sender, ConfigSystemTimeEventArgs e)
        {
            MessageBox.Show("設定設備時間: " + e.DeviceDateTime.ToString(" hh : mm : ss"));
        }

        void m_farmConfig_EventWriteDeviceSaveSetting(object sender, EventArgs e)
        {

            if (this.m_serialPort.BytesToRead > 0)
            {
                string tempData = this.m_serialPort.ReadExisting();
            }
            MessageBox.Show("設定完成");
        }
        /*********************************************************************/
        void m_tmrUpdateForm_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new UpdateFormInvokeHandler(this.UpdateForm));
            else
                this.UpdateForm();
        }
        /*********************************************************************/
        void m_hnetServer_EventResponseGetCardType(object sender, HnetProtocolEventArgs e)
        {
            if ((e.Data[0]) == CS_Protocol_Hnet.CARD_TYPE_CUSTOM)
            {
                this.m_counterDI = (byte)(e.Data[1] >> 8);
                this.m_counterDO = (byte)(e.Data[1] & 0x00ff);
                this.m_doState = new bool[24];
                this.m_diState = new bool[8];
                this.m_connectPortName = this.m_serialPort.PortName;
                this.m_isLinkDevice = true;
            }
        }

        void m_hnetServer_EventResponseGetConfig(object sender, HnetProtocolEventArgs e)
        {
            this.m_farmConfig.ParseChenFarmConfig(e.DataLength, e.Data, true);
        }

        void m_hnetServer_EventResponseGetIO(object sender, HnetProtocolEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if (e.Data[i] > 0)
                    this.m_diState[i] = true;
                else
                    this.m_diState[i] = false;
            }
            for (int i = 0; i < 24; i++)
            {
                if (e.Data[i + 8] > 0)
                    this.m_doState[i] = true;
                else
                    this.m_doState[i] = false;
            }
        }

        void m_hnetServer_EventResponseWriteConfig(object sender, HnetProtocolEventArgs e)
        {
            this.m_farmConfig.ParseChenFarmConfig(e.DataLength, e.Data, false);
        }

        void m_hnetServer_EventResponseWriteIO(object sender, HnetProtocolEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if (e.Data[i] > 0)
                    this.m_diState[i] = true;
                else
                    this.m_diState[i] = false;
            }
            for (int i = 0; i < 24; i++)
            {
                if (e.Data[i + 8] > 0)
                    this.m_doState[i] = true;
                else
                    this.m_doState[i] = false;
            }
        }

        void m_hnetServer_EventResponseError(object sender, HnetProtocolEventArgs e)
        {
            string[] errorList = CS_Protocol_Hnet.GetErrorCodeDescription(e.Data, e.DataLength);
            string errorMessage = "";
            foreach (string error in errorList)
            {
                errorMessage += error + "\r\n";
            }
            MessageBox.Show(errorMessage);
        }
        /*********************************************************************/
        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.QueryDeviceExist();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (this.m_serialPort.IsOpen)
            {
                this.CloseSerial();
                this.m_isLinkDevice = false;
                this.ShowControlByLinkState();
            }
        }
        /*********************************************************************/
        private void txt_KeyPressNumberOnly(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < '0' || (int)e.KeyChar > '9') && (int)e.KeyChar != '\b')
                e.Handled = true;
        }
        /*********************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.CloseSerial();
        }
        #endregion

        #region private
        /*********************************************************************/
        private void QueryDeviceExist()
        {
            this.lblLinkStatus.Text = "搜尋設備";
            string[] portName = SerialPort.GetPortNames();
            foreach (string port in portName)
            {
                if (this.m_isLinkDevice || this.m_serialPort.IsOpen)
                    break;
                this.m_serialPort.PortName = port;
                try
                {
                    this.m_serialPort.Open();
                }
                catch
                {
                }
                this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdGetCardType, new short[1] { 0 }, 1);
                this.CloseSerial();
            }

            if (this.m_isLinkDevice)
            {
                this.m_serialPort.PortName = this.m_connectPortName;
                try
                {
                    this.m_serialPort.Open();
                }
                catch
                {
                    this.m_isLinkDevice = false;
                }
            }
            else
                MessageBox.Show("未搜尋到設備，請確認設備是否正確連接\r\n", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.ShowControlByLinkState();
        }

        private void SendAndReceivceProtocol(SetProtocalMessage setMsg, short[] data, byte dataLength)
        {
            if (this.m_serialPort.IsOpen)
            {
                string readLine;
                try
                {
                    this.m_serialPort.Write(setMsg(data, dataLength));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.m_isLinkDevice = false;

                }
                readLine = this.ReadLineFromSerial();
                if (readLine != "")
                {
                    try
                    {
                        this.m_hnetServer.ParseRecvBuffer(readLine);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            //else
            //    MessageBox.Show("Port is disconnect");
        }

        private string ReadLineFromSerial()
        {
            string readLine = "";
            try
            {
                readLine = this.m_serialPort.ReadLine();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return readLine;
        }
        /*********************************************************************/
        private void UpdateForm()
        {
            this.m_nowTime = DateTime.Now;
            this.lblShowSystemTime.Text = this.m_nowTime.ToString("t");
            this.CheckLinkState();
        }

        private void CheckLinkState()
        {
            if (this.m_isLinkDevice)
                this.m_isLinkDevice = this.m_serialPort.IsOpen;
            this.ShowControlByLinkState();
        }

        private void ShowControlByLinkState()
        {
            this.picLinkState.Image = this.ShowLightByLinkState(this.m_isLinkDevice);
            if (this.m_isLinkDevice)
            {
             
               
                this.btnReadConfig.Enabled = true;
                this.btnSettingConfig.Enabled = true;
                this.btnSetDeviceSystemTime.Enabled = true;
                this.btnConnect.Enabled = false;
                this.btnDisconnect.Enabled = true;
                this.lblLinkStatus.Text = " 已連結 : " + this.m_connectPortName;
            }
            else
            {
          
              
                this.btnReadConfig.Enabled = false;
                this.btnSettingConfig.Enabled = false;
                this.btnSetDeviceSystemTime.Enabled = false;
                this.btnConnect.Enabled = true;
                this.btnDisconnect.Enabled = false;
                this.lblLinkStatus.Text = " 未連結";
            }
        }

        private Image ShowLightByLinkState(bool linkState)
        {
            if (linkState)
                return this.m_Indicator[1];
            else
                return this.m_Indicator[0];
        }
        /*********************************************************************/
        private void CloseSerial()
        {
            try
            {
                this.m_serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        

        //在此之前皆為一些系統的configuration!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        SqlConnection con = new SqlConnection(@"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;");  //sqlconnection資訊
        String[,] trans = new string[100, 24];  //轉換成api的格式所需陣列
        String[,] trans1 = new string[100, 24]; //同上
        private DataTable GetOrderList()        //取得所有資訊並顯示於主畫面
        {
            con = new SqlConnection();
            con.ConnectionString = @"Data Source=163.22.17.232;database=farm;uID=test;pwd=1234;";
            con.Open();
            adap = new SqlDataAdapter("Select Time as'灌溉時間',mixerA as'高濃度桶桶攪拌時間',mixerM as'混和桶攪拌時間',motorA as'A捅抽水秒數',motorB as'B桶抽水秒數', motorC as'C桶抽水秒數',motorD as'D桶抽水秒數',motorE as'E桶抽水秒數',motorW as'水桶抽水秒數',Block1 as'區域1',Block2 as'區域2',Block3 as'區域3',Block4 as'區域4',Block5 as'區域5',Block6 as'區域6',Block7 as'區域7',Block8 as'區域8',Block9 as'區域9',Block10 as'區域10' from OrderList where Uid='"+userID1+"' and oid%10=0 and oid > 10 order by Time asc", con);
            ds = new System.Data.DataSet();
            adap.Fill(ds, "TEST");
            return ds.Tables[0];
        }
        public int transform(String s)          //傳入string 並回傳其int型態
        {
            int a = int.Parse(s);
            return a;
        }
        void toFormal(String[,] save, int cnt)          //將抓下來的資料(save[]，在後面)轉成trans[] 1-6抽 7-16區 17-21攪 22混攪 23灌溉
        {
            for (int l = 0; l < cnt; l++)
            {
                for (int i = 1; i <= 5; i++)
                {
                    if (!save[l, i].Equals("0"))
                    {
                        trans[l, i + 16] = save[l,17];   //元素桶攪拌時間為save[17~21]的值
                    }
                    else
                    {
                        trans[l, i + 16] = "0";
                    }
                    //trans[l, i + 16] = save[l,i + 16];   //元素桶攪拌時間為save[17~21]的值
                }
                for (int j = 1; j <= 6; j++)
                {
                    trans[l, j] = save[l, j];
                }
                for (int k = 7; k <= 16; k++)                           //電磁閥啟動600sec
                {
                    if (save[l, k].Equals("1"))
                    {
                        trans[l, k] = "600";
                    }
                    else
                    {
                        trans[l, k] = "0";
                    }
                }
                trans[l, 22] = save[l,18];
                trans[l, 23] = "600";     //灌溉馬達啟動600sec
            }
        }
        void toFormal1(String[,] save, int cnt)          //將抓下來的資料(save1[]，在後面)轉成trans1[] 1-6抽 7-16區 17-21攪 22混攪 23灌溉  (緊急單)
        {
            for (int l = 0; l < cnt; l++)
            {
                for (int i = 1; i <= 5; i++)
                {
                    if (!save[l, i].Equals("0"))
                    {
                        trans1[l, i + 16] = save[l,17];   //元素桶攪拌時間為save[17~21]的值
                    }
                    else
                    {
                        trans1[l, i + 16] = "0";
                    }
                }
                for (int j = 1; j <= 6; j++)
                {
                    trans1[l, j] = save[l, j];
                }
                for (int k = 7; k <= 16; k++)                           //電磁閥啟動600sec
                {
                    if (save[l, k].Equals("1"))
                    {
                        trans1[l, k] = "600";
                    }
                    else
                    {
                        trans1[l, k] = "0";
                    }
                }
                trans1[l, 22] = save[l,18];      //混合桶攪拌save[22]的值
                trans1[l, 23] = "600";     //灌溉馬達啟動600sec
            }
        }
        int countrelay(int i, String[,] a, int z)   //算有幾個relay需要開啟 (丟trans進來)
        {
            int count = 0;
            if (i == 0)
            {
                for (int j = 17; j <= 21; j++)
                {
                    if (!a[z, j].Equals("0"))
                    {
                        count++;
                    }
                }
            }
            else if (i == 1)
            {
                for (int j = 1; j <= 6; j++)
                {
                    if (!a[z, j].Equals("0"))
                    {
                        count++;
                    }
                }
            }
            else if (i == 2)
            {
                if (!a[z, 22].Equals("0"))
                {
                    count++;
                }
            }
            else
            {
                for (int j = 7; j <= 16; j++)
                {
                    if (!a[z, j].Equals("0"))
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    return 0;
                }
                return count + 1;   //最後一步驟要+1(灌溉馬達)
            }
            return count;
        }
        String openRelay(int i, String[,] a, int z)  //哪幾個relay要開啟(參數:步驟幾，一個二維array，及一個z整數(相關資訊在upload_Click))
        {
            String str = "";
            if (i == 0)                           //攪拌馬達a~e
            {
                for (int j = 17; j <= 21; j++)
                {
                    if (!a[z, j].Equals("0"))
                    {
                        if (str == "")
                        {
                            str += j;
                        }
                        else
                        {
                            str += "," + j.ToString();
                        }
                    }
                }
            }
            else if (i == 1)                        //抽水馬達a~e.w
            {
                for (int j = 1; j <= 6; j++)
                {
                    if (!a[z, j].Equals("0"))
                    {
                        if (str == "")
                        {
                            str += j;
                        }
                        else
                        {
                            str += "," + j.ToString();
                        }
                    }
                }
            }
            else if (i == 2)                 //混合桶攪拌
            {
                if (!a[z, 22].Equals("0"))
                {
                    if (str == "")
                    {
                        str += "22";
                    }
                    else
                    {
                        str += ",22";
                    }
                }
            }
            else                               //灌溉哪幾區
            {
                for (int j = 7; j <= 16; j++)
                {
                    if (!a[z, j].Equals("0"))
                    {
                        if (str == "")
                        {
                            str += j;
                        }
                        else
                        {
                            str += "," + j.ToString();
                        }
                    }
                }
                if (!str.Equals(""))
                {
                    str += ",23";
                }
            }
            return str;
        }
        String openPeriod(int i, String[,] a, int z)  //要開啟多久(參數:步驟幾，一個二維array，及一個z整數(相關資訊在upload_Click))
        {
            String str1 = "";
            if (i == 0)                           //攪拌馬達a~e
            {
                for (int j = 17; j <= 21; j++)
                {
                    if (!a[z, j].Equals("0"))
                    {
                        if (str1 == "")
                        {
                            str1 += a[z, j];
                        }
                        else
                        {
                            str1 += "," + a[z, j].ToString();
                        }
                    }
                }
            }
            else if (i == 1)                          //抽水
            {
                for (int j = 1; j <= 6; j++)
                {
                    if (!a[z, j].Equals("0"))
                    {
                        if (str1 == "")
                        {
                            str1 += a[z, j];
                        }
                        else
                        {
                            str1 += "," + a[z, j].ToString();
                        }
                    }
                }
            }
            else if (i == 2)
            {
                if (!a[z, 22].Equals("0"))
                {
                    if (str1 == "")
                    {
                        str1 += a[z, 22];
                    }
                    else
                    {
                        str1 += "," + a[z, 22].ToString();
                    }
                }
            }
            else
            {
                for (int j = 7; j <= 16; j++)
                {
                    if (!a[z, j].Equals("0"))
                    {
                        if (str1 == "")
                        {
                            str1 += a[z, j];
                        }
                        else
                        {
                            str1 += "," + a[z, j].ToString();
                        }
                    }
                }
                if (!str1.Equals(""))
                {
                    str1 += "," + a[z,23].ToString();                   //灌溉馬達
                }
            }
            return str1;
        }

        private void SetGroupLengthToDevice()
        {
            short[] config = Chen_Farm_Config.SetConfigLength(this.m_dateTimeLength, this.m_emergencyLength);
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);
        }

        private void SetDateTimeGroupToDevice(int configIndex, DateTimeGroup group)
        {
            configIndex = configIndex + 4;
            short[] config;
            int unitLength;
            config = Chen_Farm_Config.SetConfigWaitLevelStableDuration(configIndex, group.WaitLevelStableDuration);
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);

            config = Chen_Farm_Config.SetConfigStartTime(configIndex, group.GroupStartHour, group.GroupStartMinute, 0);
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);

            for (byte i = 0; i < 4; i++)
            {
                unitLength = group.GetIoUnitLengthByProcessIndex(i);
                if (unitLength != 0)
                    config = Chen_Farm_Config.SetConfigIoGroup(configIndex, i, unitLength, Chen_Farm_Config.GetIoUnitFrameByFarmIoUnit(unitLength, group.GetIoUnitByProcessIndex(i)));
                else
                    config = Chen_Farm_Config.SetConfigIoGroup(configIndex, i, unitLength, new short[] { 0, 0 });
                this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);
            }
        }

        private void SetEmergencyGroupToDevice(int configIndex, FarmIoGroup group)
        {
            short[] config;
            int unitLength;
            config = Chen_Farm_Config.SetConfigWaitLevelStableDuration(configIndex, group.WaitLevelStableDuration);
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);

            for (byte i = 0; i < 4; i++)
            {
                unitLength = group.GetIoUnitLengthByProcessIndex(i);
                if (unitLength != 0)
                {
                    short[] tempData = Chen_Farm_Config.GetIoUnitFrameByFarmIoUnit(unitLength, group.GetIoUnitByProcessIndex(i));
                    config = Chen_Farm_Config.SetConfigIoGroup(configIndex, i, unitLength, Chen_Farm_Config.GetIoUnitFrameByFarmIoUnit(unitLength, group.GetIoUnitByProcessIndex(i)));
                }
                else
                {
                    config = Chen_Farm_Config.SetConfigIoGroup(configIndex, i, unitLength, new short[] { 0, 0 });
                }
                this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);
            }
        }

        private void SetRunningIndex()
        {
            short[] config = Chen_Farm_Config.SetConfigRunningIndex(Convert.ToInt16(this.txtRunningOrder.Text));
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);
        }
        private void SetSpecialIO()
        {
            short[] config = Chen_Farm_Config.SetSpecialIO(5, new int[5] { 17, 18, 19, 20, 21 }); //第一個變數為幾個io, 第二個變數為io的id range 1~24
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);
        }
        private void SetDeviceSystemTime()
        {
            short[] config = Chen_Farm_Config.SetSystemTime(this.m_nowTime);
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);
        }

        private void CommandDeviceToSaveSetting()
        {
            short[] config = Chen_Farm_Config.SetConfigDeviceSaveSetting();
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdWriteConfig, config, (byte)config.Length);
        }

        private void GetGroupLengthFromDevice()
        {
            short[] config = Chen_Farm_Config.GetConfigLength();
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdGetConfig, config, (byte)config.Length);
        }

        private void GetWaitLevelDuration(int configIndex)
        {
            short[] config = Chen_Farm_Config.GetConfigWaitLevelStableDuration(configIndex);
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdGetConfig, config, (byte)config.Length);
        }

        private void GetRunningIndexFromDevice()
        {
            short[] config = Chen_Farm_Config.GetConfigRunningIndex();
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdGetConfig, config, (byte)config.Length);
        }

        private void GetIoGroupFromDevice(int configIndex, int processIndex)
        {
            short[] config = Chen_Farm_Config.GetConfigIoGroup(configIndex, processIndex);
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdGetConfig, config, (byte)config.Length);
        }

        private void GetGroupStartTimeFromDevice(int configIndex)
        {
            short[] config = Chen_Farm_Config.GetConfigStartTime(configIndex);
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdGetConfig, config, (byte)config.Length);
        }

        private void GetSystemTimeFromDevice()
        {
            short[] config = Chen_Farm_Config.GetSystemTime();
            this.SendAndReceivceProtocol(this.m_hnetServer.SetCmdGetConfig, config, (byte)config.Length);
        }

        private void btnReadConfig_Click(object sender, EventArgs e)
        {
            this.m_dateTimeLength = 0;
            this.m_emergencyLength = 0;
            this.GetGroupLengthFromDevice();
            if (this.m_emergencyLength != 0)
            {
                for (int i = 0; i < this.m_emergencyLength; i++)
                {
                    this.GetWaitLevelDuration(i);
                    for (int j = 0; j < 4; j++)
                        this.GetIoGroupFromDevice(i, j);
                }
            }

            if (this.m_dateTimeLength != 0)
            {
                int index;
                for (int i = 0; i < this.m_dateTimeLength; i++)
                {
                    index = i + 4;
                    this.GetWaitLevelDuration(index);
                    for (int j = 0; j < 4; j++)
                        this.GetIoGroupFromDevice(index, j);
                    this.GetGroupStartTimeFromDevice(index);
                }
            }

            this.DemoShowGroup();
        }
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
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetOrderList();
            timer1.Enabled = true;
            timer1.Interval = 8000;       //每8秒刷新主頁面
            timer1.Start();
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            dataGridView1.DefaultCellStyle.Font = new Font("微軟正黑體", 15, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 12, FontStyle.Bold);
            dataGridView1.DataSource = GetOrderList();
            foreach (DataGridViewColumn gCol in dataGridView1.Columns)
                gCol.Width = 100;

            foreach (DataGridViewRow grow in dataGridView1.Rows)
                grow.Height = 50;
            
        }
        private void upload_Click(object sender, EventArgs e)                    //按下上傳鈕要執行的動作
        {
            //string str = Interaction.InputBox("請輸入欲開始執行編號", "選擇開始訂單", "1", 800, 400);
            try
            {
                con.Open();
            }
                catch (Exception ex)
            {

            }
            String countrecord = "select count(*) from orderlist where uID='"+userID1+"' and oId%10=0 and oID!=10";  //計算幾筆幾錄
            SqlCommand executeCR = new SqlCommand(countrecord, con);
           object obj = executeCR.ExecuteScalar();
            String b = string.Format("{0:0}", obj);
            int cnt = transform(b);                                 ///////////cnt筆資料

            this.m_dateTimeLength = Convert.ToInt32(b);             //單的筆數 (b)
            this.m_emergencyLength = Convert.ToInt32("3");            //緊急的單筆數
            this.m_dateTimeGroup = new DateTimeGroup[this.m_dateTimeLength];
            this.m_emergencyGroup = new FarmIoGroup[this.m_emergencyLength];
            for (int i = 0; i < this.m_dateTimeLength; i++)
                this.m_dateTimeGroup[i] = new DateTimeGroup();
            for (int i = 0; i < this.m_emergencyLength; i++)
                this.m_emergencyGroup[i] = new FarmIoGroup();


            int a = 1;
            int a1 = 1;
            String[,] save = new string[cnt, 19];  //存抓下來的資料
            String[,] save1 = new string[3, 19];  //存緊急單資料
            while (a <= cnt)
            {
                String selectCommand = "select top 1 * from (select top " + a + " * from orderlist where uID='"+userID1+"' and oId%10=0 AND oID!=10 order by Time asc) temp1 order by Time desc";  //把這個user select出來的相關資料撈出來
                SqlCommand cmd = new SqlCommand(selectCommand, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    for (int i = 0; i < 19; i++)        //dr.FieldCount-->dr陣列長度
                    {
                        save[a - 1, i] = dr[i + 1].ToString();   //剛剛撈出來的資料丟到save[]裡存起來(oid 不存(17個值))                                                                            
                    }
                }
                //dr.Close();
                a++;
                dr.Close();
            }
            while (a1 <= 3)
            {
                String selectCommand1 = "select top 1 * from (select top " + a1 + " * from emergency_list where  uID='"+userID1+"'  order by Time asc) temp1 order by time desc";    //同前面，但這裡撈的是emergency_list資料表的資料(每個用戶有3筆)
                SqlCommand cmd1 = new SqlCommand(selectCommand1, con);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    for (int i = 0; i < 19; i++)        //dr.FieldCount-->dr陣列長度
                    {
                        save1[a1 - 1, i] = dr1[i + 1].ToString();   //剛剛撈出來的資料丟到save1[]裡存起來(oid 不存(17個值))
                    }
                }
                //dr.Close();
                a1++;
                dr1.Close();
            }
            toFormal(save, cnt);                 //將save[]內容丟給toFormal()處理 (處理完會變成我們所需的格式)
            toFormal1(save1, 3);                 //將save1[]內容丟給toFormal1()處理 (處理完會變成我們所需的格式)
            for (int z = 0; z < cnt; z++)        //這邊請看說明文件，即把data依板子所需格式傳給板子
            {
                this.m_dateTimeGroup[z].GroupStartHour = Convert.ToByte(save[z, 0].Substring(0, 2));
                this.m_dateTimeGroup[z].GroupStartMinute = Convert.ToByte(save[z, 0].Substring(3, 2));
                this.m_dateTimeGroup[z].WaitLevelStableDuration = Convert.ToByte(10);
                this.m_dateTimeGroup[z].SetIoUnitByIndex(0, Convert.ToByte(countrelay(0, trans, z)), Chen_Farm_Config.GetIoUnitByString(countrelay(0, trans, z), openRelay(0, trans, z), openPeriod(0, trans, z)));
                this.m_dateTimeGroup[z].SetIoUnitByIndex(1, Convert.ToByte(countrelay(1, trans, z)), Chen_Farm_Config.GetIoUnitByString(countrelay(1, trans, z), openRelay(1, trans, z), openPeriod(1, trans, z)));
                this.m_dateTimeGroup[z].SetIoUnitByIndex(2, Convert.ToByte(countrelay(2, trans, z)), Chen_Farm_Config.GetIoUnitByString(countrelay(2, trans, z), openRelay(2, trans, z), openPeriod(2, trans, z)));
                this.m_dateTimeGroup[z].SetIoUnitByIndex(3, Convert.ToByte(countrelay(3, trans, z)), Chen_Farm_Config.GetIoUnitByString(countrelay(3, trans, z), openRelay(3, trans, z), openPeriod(3, trans, z)));
                
            }
            for(int z1 = 0; z1 < 3; z1++)        //一樣，但這邊是緊急單資料
            {
                this.m_emergencyGroup[z1].WaitLevelStableDuration = Convert.ToInt16("10");
                this.m_emergencyGroup[z1].SetIoUnitByIndex(0, Convert.ToByte(countrelay(0, trans1, z1)), Chen_Farm_Config.GetIoUnitByString(countrelay(0, trans1, z1), openRelay(0, trans1, z1), openPeriod(0, trans1, z1)));
                this.m_emergencyGroup[z1].SetIoUnitByIndex(1, Convert.ToByte(countrelay(1, trans1, z1)), Chen_Farm_Config.GetIoUnitByString(countrelay(1, trans1, z1), openRelay(1, trans1, z1), openPeriod(1, trans1, z1)));
                this.m_emergencyGroup[z1].SetIoUnitByIndex(2, Convert.ToByte(countrelay(2, trans1, z1)), Chen_Farm_Config.GetIoUnitByString(countrelay(2, trans1, z1), openRelay(2, trans1, z1), openPeriod(2, trans1, z1)));
                this.m_emergencyGroup[z1].SetIoUnitByIndex(3, Convert.ToByte(countrelay(3, trans1, z1)), Chen_Farm_Config.GetIoUnitByString(countrelay(3, trans1, z1), openRelay(3, trans1, z1), openPeriod(3, trans1, z1)));
            }
            
            /*for(int z = 0; z < 2; z++)
            {
                this.m_dateTimeGroup[z].GroupStartHour = Convert.ToByte(03);
                this.m_dateTimeGroup[z].GroupStartMinute = Convert.ToByte(50);
                this.m_dateTimeGroup[z].WaitLevelStableDuration = Convert.ToByte(1);
                this.m_dateTimeGroup[z].SetIoUnitByIndex(0, 20, Chen_Farm_Config.GetIoUnitByString(20, "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20", "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3"));
                this.m_dateTimeGroup[z].SetIoUnitByIndex(1, 20, Chen_Farm_Config.GetIoUnitByString(20, "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20", "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3"));
                this.m_dateTimeGroup[z].SetIoUnitByIndex(2, 20, Chen_Farm_Config.GetIoUnitByString(20, "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20", "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3"));
                this.m_dateTimeGroup[z].SetIoUnitByIndex(3, 20, Chen_Farm_Config.GetIoUnitByString(20, "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20", "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3"));
            }
            for(int z1 = 0; z1 < 1; z1++)
            {
                this.m_emergencyGroup[z1].WaitLevelStableDuration = Convert.ToInt16("1");
                this.m_emergencyGroup[z1].SetIoUnitByIndex(0, 20, Chen_Farm_Config.GetIoUnitByString(20, "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20", "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3"));
                this.m_emergencyGroup[z1].SetIoUnitByIndex(1, 20, Chen_Farm_Config.GetIoUnitByString(20, "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20", "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3"));
                this.m_emergencyGroup[z1].SetIoUnitByIndex(2, 20, Chen_Farm_Config.GetIoUnitByString(15, "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20", "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3"));
                this.m_emergencyGroup[z1].SetIoUnitByIndex(3, 20, Chen_Farm_Config.GetIoUnitByString(15, "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20", "3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3"));
            }
            */
            //if (!str.Equals(""))
            //{
            this.SetGroupLengthToDevice();

            for (int i = 0; i < this.m_emergencyLength; i++)
                this.SetEmergencyGroupToDevice(i, this.m_emergencyGroup[i]);

            for (int i = 0; i < this.m_dateTimeLength; i++)
                this.SetDateTimeGroupToDevice(i, this.m_dateTimeGroup[i]);


            //int runningindex = transform(str);
            //runningindex -= 1;
            //this.SetRunningIndex(runningindex.ToString());

            this.SetRunningIndex();
            this.SetSpecialIO();
            this.CommandDeviceToSaveSetting();
            //}
            //else
            //{
            //MessageBox.Show("請輸入欲開始訂單編號");
            //}
            con.Close();
        }
    

        private void add_Click(object sender, EventArgs e)          //新增按鈕(按了即開啟那個Form)
        {
            Form2 f2 = new Form2();
            f2.userID2 = userID1;
            f2.Show();
        }

        private void edit_Click(object sender, EventArgs e)        //編輯按鈕(按了即開啟那個Form)
        {
            Form3 f3 = new Form3();
            f3.userID3 = userID1;
            f3.Show();
        }

        private void close_Click(object sender, EventArgs e)       //關閉按鈕(按了即開啟那個Form)
        {
            this.Close();
        }

        private void delete_Click(object sender, EventArgs e)      //刪除按鈕(按了即開啟那個Form)
        {
            Form4 f4 = new Form4();
            f4.userID4 = userID1;
            f4.Show();
        }

        private void search_Click(object sender, EventArgs e)      //查詢按鈕(按了即開啟那個Form)
        {
            Form5 f5 = new Form5();
            f5.userID5 = userID1;
            f5.Show();
        }

        private void emergency_Click(object sender, EventArgs e)   //緊急按鈕(按了即開啟那個Form)
        {
            Form6 f6 = new Form6();
            f6.userID6 = userID1;
            f6.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)       //Timer設定
        {
            dataGridView1.DataSource = GetOrderList();

            foreach (DataGridViewColumn gCol in dataGridView1.Columns)
                gCol.Width = 100;

            foreach (DataGridViewRow grow in dataGridView1.Rows)
                grow.Height = 50;
            this.dataGridView1.DefaultCellStyle.Font = new Font("微軟正黑體", 15, FontStyle.Regular);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 12, FontStyle.Bold);
        }

        private void btnSetDeviceSystemTime_Click(object sender, EventArgs e)
        {
            this.SetDeviceSystemTime();
        }       //設定設定時間按鈕

        private void btnReadSystemTime_Click(object sender, EventArgs e)
        {
            this.GetSystemTimeFromDevice();
        }            //讀取設備時間按鈕
   
    }
}


