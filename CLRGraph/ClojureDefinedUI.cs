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
                container.Location = new Point(container.Padding.Left, container.Padding.Top);
            else
                container.Location = new Point(container.Padding.Left, previous.Bounds.Bottom + previous.Padding.Bottom + container.Padding.Top);

            previous = null;
            for (int i = 0; i < controls.Length; i++)
            {
                if(previous == null)
                    controls[i].Location = new Point(controls[i].Padding.Left, controls[i].Padding.Top);
                else
                    controls[i].Location = new Point(previous.Bounds.Right + previous.Padding.Right + controls[i].Padding.Left, controls[i].Padding.Top);

                container.Controls.Add(controls[i]);
                container.Height = Math.Max(container.Height, controls[i].Height + controls[i].Padding.Vertical);
                previous = controls[i];
            }

            //AddNewClojureControl(container);
            Controls.Add(container);
            container.BringToFront();
        }

        public static Button CreateClojureButton(string label, IFn func)
        {
            Button button = new Button();
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
            Label label = new Label();
            label.AutoSize = true;
            label.Text = labelText;

            Label valLabel = new Label();
            valLabel.AutoSize = true;
            valLabel.Text = "Value: ??, Min: " + min + ", Max: " + max;

            int iMin = (int)Math.Round(min * divisor);
            int iMax = (int)Math.Round(max * divisor);
            int iValue = (int)Math.Round(double.Parse(variable.get().ToString()) * divisor);

            TrackBar trackbar = new TrackBar();
            trackbar.ValueChanged += (s, e) =>
            {
                double tbVal = (double)trackbar.Value / divisor;
                ClojureEngine.Eval("(def " + variable.sym.Name + " " + tbVal + ")");
                valLabel.Text = "Value: " + tbVal + ", Min: " + min + ", Max: " + max;

                if (func != null)
                    ClojureEngine.Log(func.invoke());
            };

            trackbar.Width = 128;
            trackbar.Height = 32;
            trackbar.Minimum = iMin;
            trackbar.Maximum = iMax;
            trackbar.Value = iValue;

            self.AddNewClojureControls(new Control[] { label, trackbar, valLabel });
        }
    }
}
