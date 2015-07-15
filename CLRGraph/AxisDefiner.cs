﻿using System;
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
    public partial class AxisDefiner : Form
    {
        public AxisDefiner(List<String> columns)
        {
            InitializeComponent();

            columns.Insert(0, "**NONE**");

            for (int i = 0; i < columns.Count; i++)
            {
                listBox_XAxis.Items.Add(columns[i]);
                listBox_YAxis.Items.Add(columns[i]);
                listBox_ZAxis.Items.Add(columns[i]);
            }

            listBox_XAxis.SelectedIndex = listBox_YAxis.SelectedIndex = listBox_ZAxis.SelectedIndex = 0;
        }

        public string xAxisColumn, yAxisColumn, zAxisColumn;

        private void button_Apply_Click(object sender, EventArgs e)
        {
            xAxisColumn = listBox_XAxis.SelectedIndex <= 0 ? null : listBox_XAxis.SelectedItem.ToString();
            yAxisColumn = listBox_YAxis.SelectedIndex <= 0 ? null : listBox_YAxis.SelectedItem.ToString();
            zAxisColumn = listBox_ZAxis.SelectedIndex <= 0 ? null : listBox_ZAxis.SelectedItem.ToString();

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}