/****************************************************************************\
 * 
 * This is an example plugin for CLRGraph, showing the various classes you can
 * extend.
 * 
\****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLRGraph;

namespace ExamplePlugin
{
    /// <summary>
    /// This class describes a data source that can be used in CLRGraph.
    /// This data source can support any data that can visualized in up to
    /// three dimensions, or more if you extend GraphPoint to support additional
    /// data.
    /// 
    /// The DataSource attribute is required for the source to appear in the
    /// data source selector
    /// </summary>
    [DataSource("Example Data Source", "Example source")]
    public class ExampleDataSource : DataSource
    {
        public override List<GraphPoint> GetData(int channel, double elapsedTime)
        {
            return new List<GraphPoint>() { new GraphPoint(1, 1, 1) };
        }
    }

    /// <summary>
    /// This class contains static methods that are converted into valid
    /// clojure functions.
    /// 
    /// The attribute 'ClojureClass' is required for any class that includes
    /// clojure functions. Alternatively, you can use the ClojureImport
    /// attribute to add a type to Clojure without adding any methods.
    /// ClojureImport is useful for adding enums.
    /// </summary>
    [ClojureClass]
    public static class ExampleClojureFuncs
    {
        /// <summary>
        /// This attribute discribes the function name within the clojure
        /// runtime, as well as providing a doc string to be used with
        /// clojure's "doc" function.
        /// </summary>
        [ClojureStaticMethod("example-plugin-method", "An example method used to explain the plugin system. Prints a message")]
        public static void ExampleClojureFunc()
        {
            ClojureEngine.Log("Example method called");
        }

        /// <summary>
        /// This clojure function correctly supports one parameter and
        /// returns a string, which can then be handled via other clojure
        /// functions. No additional properties need to be added to the
        /// attribute.
        /// </summary>
        [ClojureStaticMethod("example-plugin-method-params", "An example method used to explain the plugin system. Takes in a string and reverses it. Both prints and returns the result.")]
        public static string ExampleClojureFuncParams(string toReverse)
        {
            string reversed = new string(toReverse.ToCharArray().Reverse().ToArray());

            ClojureEngine.Log("Reversing '" + toReverse + "' => '" + reversed + "'");
            return reversed;
        }
    }
}
