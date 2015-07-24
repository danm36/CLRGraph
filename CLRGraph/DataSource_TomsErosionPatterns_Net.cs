﻿using clojure.lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    public partial class DataSource_TomsErosionPatterns_Net_Config : Form
    {
        public int port = 5000;

        public DataSource_TomsErosionPatterns_Net_Config(int nPort)
        {
            InitializeComponent();

            numericUpDown_port.Value = port = nPort;
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            port = (int)numericUpDown_port.Value;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }

    [DataSourceAttribute("Tom's Erosion Pattern Network", "TCP Network")]
    public class DataSource_TomsErosionPatterns_Net : DataSource
    {
        List<GraphPoint> landPoints = new List<GraphPoint>();
        List<GraphPoint> waterPoints = new List<GraphPoint>();

        string pendingFile = "";

        TcpListener listener = null;
        TcpClient myClient = null;
        StreamReader sr = null;
        int myPort = 5000;

        bool needLandData = false;
        bool needWaterData = false;

        public DataSource_TomsErosionPatterns_Net(string name)
            : base(name)
        {
            listener = new TcpListener(IPAddress.Any, 10169);
            listener.Start();
        }

        public override bool NeedToGetNewData(int channel)
        {
            try
            {
                if (listener.Pending())
                {
                    if (myClient != null)
                        myClient.Close();

                    myClient = listener.AcceptTcpClient();
                    sr = new StreamReader(myClient.GetStream());
                    ClojureEngine.Log("New client connected: " + myClient.Client.ToString());
                }

                if (myClient != null && myClient.Available > 0)
                {
                    char[] buffer = new char[myClient.Available];
                    sr.Read(buffer, 0, buffer.Length);

                    pendingFile += new string(buffer);
                    
                    pendingFile = pendingFile.Substring(pendingFile.LastIndexOf("E1"));
                    if (pendingFile.IndexOf(";") >= 0)
                    {
                        ClojureEngine.Log("Beginning read of recieved data.");

                        string checkingFile = pendingFile.Substring(0, pendingFile.IndexOf(";")).Replace("\r", "");
                        pendingFile = pendingFile.Substring(pendingFile.IndexOf(";") + 1);

                        string[] lines = checkingFile.Split('\n');

                        if (lines[0] != "E1")
                        {
                            MessageBox.Show("File is not a correct erosion type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        landPoints.Clear();
                        waterPoints.Clear();

                        string[] splLine = lines[2].Split(' ');

                        int xCount = int.Parse(splLine[0]);
                        int yCount = int.Parse(splLine[1]);
                        int zCount = int.Parse(splLine[2]);

                        for (int z = 0; z < zCount; z++)
                        {
                            for (int y = 0; y < yCount; y++)
                            {
                                for (int x = 0; x < xCount; x++)
                                {
                                    if (lines[3][x + y * xCount + z * yCount * xCount] == '1')
                                        landPoints.Add(new GraphPoint(x, y, z));
                                    else
                                        waterPoints.Add(new GraphPoint(x, y, z));
                                }
                            }
                        }

                        ClojureEngine.Log("Recieved data parsed.");

                        needLandData = true;
                        needWaterData = true;
                    }
                }
            }
            catch (Exception e)
            {
                ClojureEngine.Log(e.ToString());
            }

            return channel == 1 ? needWaterData : needLandData;
        }

        public override PersistentVector GetData(int channel = 0)
        {
            if (channel == 1)
                needWaterData = false;
            else
                needLandData = false;
            return PersistentVector.create1(channel == 1 ? waterPoints : landPoints);
        }

        public override void ShowDataSeriesConfig()
        {
            DataSource_TomsErosionPatterns_Net_Config cfg = new DataSource_TomsErosionPatterns_Net_Config(myPort);

            if (cfg.ShowDialog() != DialogResult.OK)
                return;

            myPort = cfg.port;

            StartListener();
        }

        private void StartListener()
        {
            if (myClient != null)
            {
                myClient.Close();
                myClient = null;
            }

            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }

            try
            {
                listener = new TcpListener(IPAddress.Any, myPort);
                listener.Start();
                ClojureEngine.Log("TCP Listener started on port " + myPort);
            }
            catch
            {
                ClojureEngine.Log("ERROR: Could not start TCP Listener on " + myPort);
            }
        }
    }
}