using clojure.lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRGraph
{
    /// <summary>
    /// Proxy class to handle Clojure types
    /// </summary>
    [ClojureClass]
    public static class CMath
    {
        //Proxy values
        public const double PI = Math.PI;
        public const double E = Math.E;

        public const double TwoPI = Math.PI * 2;
        public const double HalfPI = Math.PI / 2;
        public const double ThreeHalfPI = Math.PI / 2 * 3;

        /// <summary>
        /// Runs a delegate on each entry in a given enumarable or the value supplied
        /// </summary>
        /// <param name="d">Function to run (One parameter of double, returns double)</param>
        /// <param name="val">IEnumerable or numeric object</param>
        /// <returns></returns>
        static object RunMathFunc(Func<double, double> d, object val)
        {
            object returnVal = null;
            List<object> results = new List<object>();

            if (val is IEnumerable)
            {
                IEnumerable enumerable = (IEnumerable)val;
                double dbl = 0;
                ulong count = 0;

                foreach (object obj in enumerable)
                {
                    if (double.TryParse(obj.ToString(), out dbl))
                    {
                        results.Add(d.Invoke(dbl));
                    }
                    else
                    {
                        results.Add(obj);
                        ClojureEngine.Log("Could not mathematically operate on value '" + obj.ToString() + "'");
                    }

                    ++count;

                    if (count > ClojureEngine.ClojureMaxIterations)
                    {
                        ClojureEngine.Log("Supplied collection exceeded '" + ClojureEngine.ClojureMaxIterations + "' values. Stopping processing.");
                        break;
                    }
                }
            }
            else
            {
                return d.Invoke(double.Parse(val.ToString()));
            }

            returnVal = PersistentVector.create(results.ToArray());
            return returnVal;
        }

        /// <summary>
        /// Runs a delegate on each entry in a given enumarables or values supplied
        /// </summary>
        /// <param name="d">Function to run (Two parameters of double, returns double)</param>
        /// <param name="val1">IEnumerable or numeric object</param>
        /// <param name="val2">IEnumerable or numeric object</param>
        /// <returns></returns>
        static object RunMathFunc(Func<double, double, double> d, object val1, object val2)
        {
            object returnVal = null;
            List<object> results = new List<object>();

            double dbl1 = 0;
            double dbl2 = 0;
            int count = 0;

            if (val1 is IList && val2 is IList)
            {
                IList enumerable1 = (IList)val1;
                IList enumerable2 = (IList)val2;

                foreach (object obj in enumerable1)
                {
                    try
                    {
                        if (double.TryParse(obj.ToString(), out dbl1))
                        {
                            if (double.TryParse(enumerable2[count].ToString(), out dbl2))
                            {
                                results.Add(d.Invoke(dbl1, dbl2));
                            }
                            else
                            {
                                results.Add(obj);
                                ClojureEngine.Log("Could not mathematically operate on value '" + enumerable2[count].ToString() + "' in second collection");
                            }
                        }
                        else
                        {
                            results.Add(obj);
                            ClojureEngine.Log("Could not mathematically operate on value '" + obj.ToString() + "' in first collection");
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }

                    ++count;

                    if (count > ClojureEngine.ClojureMaxIterations)
                    {
                        ClojureEngine.Log("Supplied collection exceeded '" + ClojureEngine.ClojureMaxIterations + "' values. Stopping processing.");
                        break;
                    }
                }
            }
            else if (val1 is IList)
            {
                IList enumerable1 = (IList)val1;

                try
                {
                    dbl2 = double.Parse(val2.ToString());
                }
                catch
                {
                    ClojureEngine.Log("Error: Second parameter '" + val2.ToString() + "' is not numeric. Stopping processing.");
                    return val1;
                }

                foreach (object obj in enumerable1)
                {
                    if (double.TryParse(obj.ToString(), out dbl1))
                    {
                        results.Add(d.Invoke(dbl1, dbl2));
                    }
                    else
                    {
                        results.Add(obj);
                        ClojureEngine.Log("Could not mathematically operate on value '" + obj.ToString() + "' in first collection.");
                    }

                    ++count;

                    if (count > ClojureEngine.ClojureMaxIterations)
                    {
                        ClojureEngine.Log("Supplied collection exceeded '" + ClojureEngine.ClojureMaxIterations + "' values. Stopping processing.");
                        break;
                    }
                }
            }
            else if (val2 is IList)
            {
                IList enumerable2 = (IList)val2;

                try
                {
                    dbl1 = double.Parse(val1.ToString());
                }
                catch
                {
                    ClojureEngine.Log("Error: First parameter '" + val1.ToString() + "' is not numeric. Stopping processing.");
                    return val1;
                }

                foreach (object obj in enumerable2)
                {
                    if (double.TryParse(obj.ToString(), out dbl2))
                    {
                        results.Add(d.Invoke(dbl1, dbl2));
                    }
                    else
                    {
                        results.Add(obj);
                        ClojureEngine.Log("Could not mathematically operate on value '" + obj.ToString() + "' in second collection");
                    }

                    ++count;

                    if (count > ClojureEngine.ClojureMaxIterations)
                    {
                        ClojureEngine.Log("Supplied collection exceeded '" + ClojureEngine.ClojureMaxIterations + "' values. Stopping processing.");
                        break;
                    }
                }
            }
            else
            {
                return d.Invoke(double.Parse(val1.ToString()), double.Parse(val2.ToString()));
            }


            returnVal = PersistentVector.create(results.ToArray());
            return returnVal;
        }

        public static object Abs(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Abs(x); }), val); }
        public static object Acos(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Acos(x); }), val); }
        public static object Asin(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Asin(x); }), val); }
        public static object Atan(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Atan(x); }), val); }
        public static object Atan2(object val1, object val2) { return RunMathFunc(new Func<double, double, double>((x, y) => { return Math.Atan2(x, y); }), val1, val2); }
        public static object Ceiling(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Ceiling(x); }), val); }
        public static object Ceil(object val) { return Ceiling(val); }
        public static object Cos(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Cos(x); }), val); }
        public static object Cosh(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Cosh(x); }), val); }
        public static object Exp(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Exp(x); }), val); }
        public static object Floor(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Floor(x); }), val); }
        public static object Log(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Log(x); }), val); }
        public static object Log(object val1, object val2) { return RunMathFunc(new Func<double, double, double>((x, y) => { return Math.Log(x, y); }), val1, val2); }
        public static object Max(object val1, object val2) { return RunMathFunc(new Func<double, double, double>((x, y) => { return Math.Max(x, y); }), val1, val2); }
        public static object Min(object val1, object val2) { return RunMathFunc(new Func<double, double, double>((x, y) => { return Math.Min(x, y); }), val1, val2); }
        public static object Pow(object val1, object val2) { return RunMathFunc(new Func<double, double, double>((x, y) => { return Math.Pow(x, y); }), val1, val2); }
        public static object Round(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Round(x); }), val); }
        public static object Round(object val1, object val2) { return RunMathFunc(new Func<double, double, double>((x, y) => { return Math.Round(x, (int)y); }), val1, val2); }
        public static object Sign(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Sign(x); }), val); }
        public static object Sin(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Sin(x); }), val); }
        public static object Sinh(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Sinh(x); }), val); }
        public static object Sqrt(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Sqrt(x); }), val); }
        public static object Tan(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Tan(x); }), val); }
        public static object Tanh(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Tanh(x); }), val); }
        public static object Truncate(object val) { return RunMathFunc(new Func<double, double>((x) => { return Math.Truncate(x); }), val); }
    }
}
