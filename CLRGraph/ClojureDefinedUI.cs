using clojure.lang;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    public class ClojureDefinedUI : UserControl
    {
        static ClojureDefinedUI self;

        public ClojureDefinedUI()
        {
            self = this;

            Font = new Font("Consolas", 8.0f);
            if(Font.Name != "Consolas")
                Font = new Font(FontFamily.GenericMonospace, 8.0f);
        }

        public static void Reset()
        {
            self.Controls.Clear();
        }

        private Control GetLastControl()
        {
            if (Controls.Count > 0)
            {
                return Controls[Controls.Count - 1];
            }

            return null;
        }

        private void AddNewClojureControl(Control control)
        {
            AddNewClojureControls(new Control[] { control });
        }

        private void AddNewClojureControls(Control[] controls)
        {
            Panel container = new Panel();
            container.Dock = DockStyle.Top;
            container.Height = 1;
            Control previous = GetLastControl();

            if (previous == null)
                container.Location = new Point(container.Margin.Left, container.Margin.Top);
            else
                container.Location = new Point(container.Margin.Left, previous.Bounds.Bottom + previous.Margin.Bottom + container.Margin.Top);

            previous = null;
            for (int i = 0; i < controls.Length; i++)
            {
                if(previous == null)
                    controls[i].Location = new Point(controls[i].Margin.Left + container.Padding.Left, controls[i].Margin.Top);
                else
                    controls[i].Location = new Point(previous.Bounds.Right + previous.Margin.Right + controls[i].Margin.Left, controls[i].Margin.Top);

                controls[i].Font = new System.Drawing.Font(Font, FontStyle.Regular);
                container.Controls.Add(controls[i]);
                container.Height = Math.Max(container.Height, controls[i].Height + controls[i].Margin.Vertical + container.Padding.Vertical);
                previous = controls[i];
            }

            for (int i = 0; i < container.Controls.Count; i++)
            {
                if (!(container.Controls[i] is Label))
                    continue;

                int lblWidth = container.Controls[i].Width;
                ((Label)container.Controls[i]).AutoSize = false;
                container.Controls[i].Width = lblWidth;
                container.Controls[i].Height = container.Height - container.Padding.Vertical - container.Controls[i].Margin.Vertical;
                ((Label)container.Controls[i]).TextAlign = ContentAlignment.MiddleLeft;
            }


            //AddNewClojureControl(container);
            Controls.Add(container);
            container.BringToFront();
        }

        public static Button CreateClojureButton(string label, IFn func)
        {
            Button button = new Button();
            button.AutoSize = true;
            button.AutoSizeMode = AutoSizeMode.GrowOnly;
            button.Text = label;
            button.Click += (s, e) =>
                {
                    ClojureEngine.Log(func.invoke());
                };

            return button;
        }

        public static void AddClojureButton(string label, IFn func)
        {
            self.AddNewClojureControl(CreateClojureButton(label, func));
        }

        public static void AddClojureButtons(PersistentVector vec)
        {
            List<Button> buttons = new List<Button>();

            for (int i = 0; i < vec.Count; i += 2)
            {
                if (!(vec[i + 1] is IFn))
                    continue;
                buttons.Add(CreateClojureButton(vec[i].ToString(), (IFn)vec[i + 1]));
            }

            ClojureDefinedUI.self.AddNewClojureControls(buttons.ToArray());
        }

        public static void AddClojureSlider(string labelText, Var variable, double min, double max, int divisor, IFn func)
        {
            double variableVal = 0;
            string variableSymbolName = variable.sym.Name;

            Label label = new Label();
            label.AutoSize = true;
            label.Text = labelText;

            if (!double.TryParse(variable.get().ToString(), out variableVal))
            {
                label.Text = "Error: Could not convert variable '" + variableSymbolName + "' value of '" + variable.get().ToString() + "' to a numeric type";
                self.AddNewClojureControl(label);
                return;
            }

            int iMin = (int)Math.Round(min * divisor);
            int iMax = (int)Math.Round(max * divisor);
            int iValue = (int)Math.Round(variableVal * divisor);

            NumericUpDown numericUpDown = new NumericUpDown();
            TrackBar trackbar = new TrackBar();
            trackbar.AutoSize = false;
            trackbar.Width = 128;
            trackbar.Height = 24;
            trackbar.Minimum = iMin;
            trackbar.Maximum = iMax;
            trackbar.Value = iValue;
            trackbar.ValueChanged += (s, e) =>
            {
                double tbVal = (double)trackbar.Value / divisor;
                ClojureEngine.Eval("(def " + variableSymbolName + " " + tbVal + ")");
                numericUpDown.Value = (decimal)tbVal;

                if (func != null)
                {
                    try
                    {
                        object res = func.invoke();
                        ClojureEngine.Log(res);
                    }
                    catch (Exception ex)
                    {
                        ClojureEngine.Log(ex.ToString());
                    }
                }
            };

            Label valLabel = new Label();
            valLabel.AutoSize = true;
            valLabel.Text = "Value: ";

            numericUpDown.DecimalPlaces = (int)Math.Log10(divisor) + 1;
            numericUpDown.Minimum = (decimal)min;
            numericUpDown.Maximum = (decimal)max;
            numericUpDown.Value = (decimal)trackbar.Value;
            numericUpDown.ValueChanged += (s, e) =>
            {
                trackbar.Value = (int)numericUpDown.Value * divisor;
            };

            Label minmaxLabel = new Label();
            minmaxLabel.AutoSize = true;
            minmaxLabel.Text = "Min: " + min + ", Max: " + max;


            self.AddNewClojureControls(new Control[] { label, trackbar, valLabel, numericUpDown, minmaxLabel });
        }

        public static void AddClojureNumericUpDown(string labelText, Var variable, double min, double max, IFn func)
        {
            double variableVal = 0;
            string variableSymbolName = variable.sym.Name;

            Label label = new Label();
            label.AutoSize = true;
            label.Text = labelText;

            if (!double.TryParse(variable.get().ToString(), out variableVal))
            {
                label.Text = "Error: Could not convert variable '" + variableSymbolName + "' value of '" + variable.get().ToString() + "' to a numeric type";
                self.AddNewClojureControl(label);
                return;
            }

            NumericUpDown numericUpDown = new NumericUpDown();
            numericUpDown.DecimalPlaces = 8;
            numericUpDown.Increment = 0.01m;
            numericUpDown.Minimum = (decimal)min;
            numericUpDown.Maximum = (decimal)max;
            numericUpDown.Value = (decimal)variableVal;
            numericUpDown.ValueChanged += (s, e) =>
                {
                    double tbVal = (double)numericUpDown.Value;
                    ClojureEngine.Eval("(def " + variableSymbolName + " " + tbVal + ")");

                    if (func != null)
                        ClojureEngine.Log(func.invoke());
                };

            self.AddNewClojureControls(new Control[] { label, numericUpDown });
        }

        public static void AddClojureDropDownBox(string labelText, Var variable, PersistentVector vals, IFn func)
        {
            string variableSymbolName = null;
            
            if(variable != null)
                variableSymbolName = variable.sym.Name;

            Label label = new Label();
            label.AutoSize = true;
            label.Text = labelText;

            Dictionary<string, object> labelCodeAssoc = new Dictionary<string, object>();

            ComboBox comboBox = new ComboBox();
            for (int i = 0; i < vals.Count; i += 2)
            {
                labelCodeAssoc.Add(vals[i].ToString(), vals[i + 1]);
                comboBox.Items.Add(vals[i]);
            }

            comboBox.SelectedIndex = 0;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.SelectedIndexChanged += (s, e) =>
                {
                    if (comboBox.SelectedItem == null)
                        return;

                    object code = labelCodeAssoc[comboBox.SelectedItem.ToString()];

                    if (code is IFn)
                    {
                        ClojureEngine.Log(((IFn)code).invoke());
                    }
                    else if(variableSymbolName != null)
                    {
                        string newVal = code.ToString();
                        ClojureEngine.Eval("(def " + variableSymbolName + " " + newVal + ")");
                    }

                    if (func != null)
                        ClojureEngine.Log(func.invoke());
                };

            self.AddNewClojureControls(new Control[] { label, comboBox });
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ClojureDefinedUI
            // 
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ClojureDefinedUI";
            this.ResumeLayout(false);

        }
    }
}
