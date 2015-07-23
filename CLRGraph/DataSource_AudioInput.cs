using clojure.lang;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    public partial class DataSource_AudioInput_Config : Form
    {
        public int SelectedDeviceID = -1;
        public string SelectedDeviceName = "";

        public DataSource_AudioInput_Config()
        {
            InitializeComponent();

            int deviceCount = WaveIn.DeviceCount;
            for (int i = 0; i < deviceCount; i++)
            {
                WaveInCapabilities info = WaveIn.GetCapabilities(i);

                ListViewItem lvi = new ListViewItem(i.ToString());
                lvi.SubItems.Add(info.ProductName);
                lvi.SubItems.Add(info.Channels.ToString());

                listView_Devices.Items.Add(lvi);
            }
        }

        private void listView_Devices_DoubleClick(object sender, EventArgs e)
        {
            if (listView_Devices.SelectedItems.Count < 1)
                return;

            button_Apply_Click(sender, e);
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            if (listView_Devices.SelectedItems.Count < 1)
            {
                MessageBox.Show("Please select an audio source first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            SelectedDeviceID = int.Parse(listView_Devices.SelectedItems[0].Text);
            SelectedDeviceName = listView_Devices.SelectedItems[0].SubItems[1].Text;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }

    [DataSourceAttribute("Microphone", "Audio")]
    public class DataSource_AudioInput : DataSource
    {
        WaveIn inWave = null;
        byte[] rawData = null;
        int audioSource = 0;

        public DataSource_AudioInput(string name)
            : base(name)
        {
            SetupAudioSource();
        }

        void inWave_DataAvailable(object sender, WaveInEventArgs e)
        {
            rawData = e.Buffer;
        }

        void SetupAudioSource()
        {
            if (inWave != null)
            {
                inWave.StopRecording();
                inWave.Dispose();
                inWave = null;
            }

            inWave = new WaveIn();
            inWave.DeviceNumber = audioSource;
            inWave.DataAvailable += inWave_DataAvailable;
            inWave.StartRecording();
        }

        public override void ShowDataSeriesConfig()
        {
            DataSource_AudioInput_Config config = new DataSource_AudioInput_Config();

            if (config.ShowDialog() != DialogResult.OK)
                return;

            audioSource = config.SelectedDeviceID;
            SourceLocation = config.SelectedDeviceName;

            SetupAudioSource();
        }

        public override PersistentVector GetData(int channel = 0)
        {
            if (rawData == null)
                return null;

            GraphPoint[] points = new GraphPoint[rawData.Length / 2];

            Parallel.For(0, points.Length, (i) =>
            {
                points[i] = new GraphPoint((double)i / 32, ((double)BitConverter.ToInt16(rawData, i * 2) / 4096), 0);
            });

            return PersistentVector.create1(points);
        }
    }
}
