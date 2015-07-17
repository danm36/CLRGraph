using clojure.lang;
using CLRGraph.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    public static class ClojureEngine
    {
        private class ClojureMethodInfo
        {
            public Dictionary<int, ClojureArityInfo> ArityCode = new Dictionary<int, ClojureArityInfo>();
            public string DocString = "";

            public ClojureMethodInfo(string docString)
            {
                DocString = docString;
            }

            public void AddArity(int arity, string arityCode, List<string> paramNames)
            {
                if (ArityCode.ContainsKey(arity))
                    ArityCode[arity].Merge(arityCode, paramNames);
                else
                    ArityCode.Add(arity, new ClojureArityInfo(arity, arityCode, paramNames));
            }
        }

        private class ClojureArityInfo
        {
            public string ExecCode;
            public List<string> ParamNames = new List<string>();

            public ClojureArityInfo(int arity, string execCode, List<string> paramNames)
            {
                ExecCode = execCode;
                ParamNames = paramNames;

                for(int i = ParamNames.Count; i < arity; i++)
                {
                    ParamNames.Add("p" + (i + 1));
                }
            }

            public void Merge(string execCode, List<string> paramNames)
            {
                ExecCode = execCode;
                for (int i = 0; i < ParamNames.Count && i < paramNames.Count; i++)
                {
                    ParamNames[i] += "-or-" + paramNames[i];
                }
            }
        }

        public const uint ClojureMaxIterations = int.MaxValue;

        #region Variables
        static Var RT_ReadString, RT_Eval;
        const string outstrval_macro = "" +
@"(defmacro with-out-str-and-value
    [& body]
    `(let [s# (new System.IO.StringWriter)]
        (binding [*out* s#]
            (let [v# ~@body]
                (vector (str s#)
                    v#
                )
            )
        )
    )
)";
        const string Clojure_EvalString = "" +
@"(with-out-str-and-value
    (do
        {0}
    )
)";
        const string Clojure_REPLReset = "(map #(ns-unmap *ns* %) (keys (ns-interns *ns*)))";
        const string Clojure_InitCompleteString = "Clojure/CLRGraph Interop Initialized";
        
        static TextBox logControl;

        static HashSet<Type> runtimeImports = new HashSet<Type>()
        {
            typeof(System.Drawing.Color)
        };
        static Dictionary<string, ClojureMethodInfo> runtimeMethods = new Dictionary<string, ClojureMethodInfo>();
        #endregion

        public static void Initialize(TextBox newLogControl)
        {
            SetLogControl(newLogControl);

            //Setup clojure system
            RT_ReadString = RT.var("clojure.core", "read-string");
            RT_Eval = RT.var("clojure.core", "eval");
            RT_Eval.invoke(RT_ReadString.invoke(outstrval_macro)); //Allows correct returns for logging

            Eval("(use 'clojure.repl)");
            //Load clojure functions from this runtime
            LoadClojureFuncsFromAllAssemblies();
            InitRuntimeMethods();

            //Load all the clojure function files
            foreach (string file in Directory.GetFiles("functions"))
            {
                Eval(File.ReadAllText(file), true);
            }

            ClearLog();
            Eval("(println \"" + Clojure_InitCompleteString + "\")", true);
        }

        #region Runtime Clojure Method Creation
        private static void LoadClojureFuncsFromAllAssemblies()
        {
            runtimeMethods.Clear();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; i++)
            {
                LoadClojureFuncsFromAssembly(assemblies[i]);
            }
        }

        private static void LoadClojureFuncsFromAssembly(Assembly assembly)
        {
            Type[] types = (from type in assembly.GetTypes()
                     where Attribute.IsDefined(type, typeof(ClojureImport))
                     select type).ToArray();

            for (int i = 0; i < types.Length; i++)
                runtimeImports.Add(types[i]);

            types = (from type in assembly.GetTypes()
                           where Attribute.IsDefined(type, typeof(ClojureClass))
                           select type).ToArray();

            for (int i = 0; i < types.Length; ++i)
            {
                runtimeImports.Add(types[i]);

                MethodInfo[] methods = (from method in types[i].GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                                        where Attribute.IsDefined(method, typeof(ClojureStaticMethod))
                                        select method).ToArray();

                for (int j = 0; j < methods.Length; ++j)
                {
                    ClojureStaticMethod attr = methods[j].GetCustomAttribute<ClojureStaticMethod>();
                    ParameterInfo[] parameters = methods[j].GetParameters();

                    string clojureCode = types[i].Name + "/" + methods[j].Name;

                    List<string> paramNames = new List<string>();

                    for (int p = 0; p < parameters.Length; p++)
                    {
                        if(p == parameters.Length - 1 && typeof(ISeq).IsAssignableFrom(parameters[p].ParameterType))
                            paramNames.Add("& " + parameters[p].Name);
                        else
                            paramNames.Add(parameters[p].Name);
                    }

                    AddRuntimeMethod(attr.Name, attr.DocString, parameters.Length, clojureCode, paramNames);
                }
            }
        }

        private static void AddRuntimeMethod(string methodName, string docString, int arity, string clojurecode, List<string> paramNames)
        {
            if (!runtimeMethods.ContainsKey(methodName))
                runtimeMethods.Add(methodName, new ClojureMethodInfo(docString));

            runtimeMethods[methodName].AddArity(arity, clojurecode, paramNames);
        }

        private static void InitRuntimeMethods()
        {
            StringBuilder clojureImports = new StringBuilder();

            foreach (Type type in runtimeImports)
                clojureImports.Append("(import " + type.Namespace + "." + type.Name + ")");

            Eval(clojureImports.ToString(), true);

            StringBuilder clojureMethods = new StringBuilder();

            foreach (KeyValuePair<string, ClojureMethodInfo> method in runtimeMethods)
            {
                clojureMethods.Append("(defn " + method.Key + " \"" + method.Value.DocString + "\" ");

                foreach (KeyValuePair<int, ClojureArityInfo> arities in method.Value.ArityCode)
                {
                    clojureMethods.Append("([");

                    string parametersIn = "";
                    string parametersOut = "";
                    for (int i = 0; i < arities.Value.ParamNames.Count; i++)
                    {
                        parametersIn += arities.Value.ParamNames[i] + " ";

                        if (arities.Value.ParamNames[i][0] == '&')
                            parametersOut += arities.Value.ParamNames[i].Substring(1).Trim();
                        else
                            parametersOut += arities.Value.ParamNames[i];

                        parametersOut += " ";
                    }

                    clojureMethods.Append(parametersIn.Trim() + "] (" + arities.Value.ExecCode + " " + parametersOut.Trim() + "))");

                }
                clojureMethods.Append(") ");
            }

            Eval(clojureMethods.ToString(), true);
        }
        #endregion

        public static void SetLogControl(TextBox newLogControl)
        {
            logControl = newLogControl;
        }

        public static void Log(object toLog)
        {
            if (toLog == null)
                return;

            string logstr = toLog.ToString();
            if(logstr == "")
                return;

            logControl.AppendText(logstr);
            if (!logstr.EndsWith("\r\n"))
                logControl.AppendText("\r\n");
        }

        public static void ClearLog()
        {
            logControl.Clear();
        }

        public static bool Eval(string toEval, bool dontShowUserInpot = false, bool resetREPL = false)
        {
            try
            {
                if (resetREPL)
                {
                    ClearLog();
                    //Log(ClojureFunc_Eval.invoke(ClojureFunc_ReadString.invoke(Clojure_REPLReset)).ToString());
                    Log("NOTICE: REPL has (not really) been reset");
                }

                if (!dontShowUserInpot)
                {
                    string[] splEval = toEval.Split('\n');
                    for (int i = 0; i < splEval.Length; i++)
                    {
                        //Todo: Actual namespace
                        Log("user=>" + splEval[i]);
                    }
                }

                PersistentVector evalResult = (PersistentVector)RT_Eval.invoke(RT_ReadString.invoke(String.Format(Clojure_EvalString, toEval)));

                if (evalResult != null)
                {
                    for (int i = 0; i < evalResult.Count; i++)
                    {
                        if (evalResult[i] == null)
                        {
                            Log("nil");
                        }
                        else if(evalResult[i] is LazySeq)
                        {
                            LazySeq ls = (LazySeq)evalResult[i];
                            StringBuilder logString = new StringBuilder();
                            int cur = 0;

                            logString.Append("(");
                            foreach (object obj in ls)
                            {
                                if (cur > 0)
                                    logString.Append(" ");
                                if(obj == null)
                                    logString.Append("nil");
                                else
                                    logString.Append(obj.ToString());
                                cur++;

                                if (cur > ushort.MaxValue)
                                {
                                    logString.Append(" ...");
                                    break;
                                }
                            }
                            logString.Append(")");

                            Log(logString.ToString());
                        }
                        else
                        {
                            Log(evalResult[i].ToString());
                        }
                    }
                }

                GLGraph.Redraw();
                return true;
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                GLGraph.Redraw();
                return false;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class ClojureImport : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ClojureClass : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ClojureStaticMethod : Attribute
    {
        public string Name;
        public string DocString = "";

        public ClojureStaticMethod(string name, string docString)
        {
            Name = name;
            DocString = docString;
        }
    }
}
