using clojure.lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRGraph
{
    [ClojureClass]
    public static class DataSource_Funcs
    {
        [ClojureStaticMethod("get-data-source", "Returns a datasource object from the given name, or nil if not found.")]
        public static DataSource GetDataSource(string name)
        {
            if (DataSource.DataSources.ContainsKey(name))
                return DataSource.DataSources[name];

            return null;
        }

        [ClojureStaticMethod("get-data-source-points", "Returns the current set of points available in the data source, or nil if the data source is not found.")]
        public static PersistentVector GetDataSourcePoints(string name)
        {
            return GetDataSourcePoints(GetDataSource(name));
        }

        [ClojureStaticMethod("get-data-source-points", "Returns the current set of points available in the data source, or nil if the data source is not found.")]
        public static PersistentVector GetDataSourcePoints(DataSource source)
        {
            if (source == null)
                return null;

            return source.GetData();
        }
    }
}
