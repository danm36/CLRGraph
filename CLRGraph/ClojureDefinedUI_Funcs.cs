﻿using clojure.lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    [ClojureClass]
    public static class ClojureDefinedUI_Funcs
    {
        [ClojureStaticMethod("add-ui-button", "Creates a UI button in the runtime interface that - when clicked - runs the specified function.")]
        public static void CreateButton(string label, IFn func)
        {
            ClojureDefinedUI.AddClojureButton(label, func);
        }

        [ClojureStaticMethod("add-ui-buttons", "Creates a group of UI buttons in the runtime interface that - when clicked - runs their respective functions.")]
        public static void CreateButton(PersistentVector vec)
        {
            ClojureDefinedUI.AddClojureButtons(vec);
        }


        [ClojureStaticMethod("add-ui-slider", "Creates a UI slider in the runtime interface that alters the supplied clojure variable. Uses the integer type.")]
        public static void CreateSlider(string label, Var var, int min, int max)
        {
            ClojureDefinedUI.AddClojureSlider(label, var, min, max, 1, null);
        }

        [ClojureStaticMethod("add-ui-slider", "Creates a UI slider in the runtime interface that alters the supplied clojure variable and optionally runs a supplied function on change. Uses the integer type.")]
        public static void CreateSlider(string label, Var var, int min, int max, IFn func)
        {
            ClojureDefinedUI.AddClojureSlider(label, var, min, max, 1, func);
        }


        [ClojureStaticMethod("add-ui-slider-double", "Creates a UI slider in the runtime interface that alters the supplied clojure variable and optionally runs a supplied function on change. Uses the double type.")]
        public static void CreateSliderDouble(string label, Var var, double min, double max)
        {
            ClojureDefinedUI.AddClojureSlider(label, var, min, max, 10000, null);
        }

        [ClojureStaticMethod("add-ui-slider-double", "Creates a UI slider in the runtime interface that alters the supplied clojure variable and optionally runs a supplied function on change. Uses the double type.")]
        public static void CreateSliderDouble(string label, Var var, double min, double max, IFn func)
        {
            ClojureDefinedUI.AddClojureSlider(label, var, min, max, 10000, func);
        }

        [ClojureStaticMethod("params-test", "TEST")]
        public static void ParamsTest(ArraySeq_object obj)
        {
            for (int i = 0; i < obj.Count; i++)
                ClojureEngine.Log(obj[i].ToString());
        }
    }
}
