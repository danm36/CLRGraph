using clojure.lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    [ClojureClass]
    public static class DataSeries_Funcs
    {
        static List<DataSeries> AllDataSeries { get { return DataSeries.AllDataSeries; } }
        static int CurrentDataSeries { get { return DataSeries.CurrentDataSeries; } set { DataSeries.CurrentDataSeries = value; } }

        public static void UpdateSeriesInfoInUI()
        {
            if (AllDataSeries.Count > 0)
            {
                CLRGraph_MainForm.self.listview_series_disable_ItemChecked = true;
                ListViewItem[] items = new ListViewItem[AllDataSeries.Count];

                for (int i = 0; i < AllDataSeries.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Checked = !AllDataSeries[i].Hidden;
                    lvi.Tag = AllDataSeries[i];
                    lvi.UseItemStyleForSubItems = false;

                    lvi.SubItems.Add(CurrentDataSeries == i ? "✓" : "");
                    lvi.SubItems.Add((i + 1).ToString());
                    lvi.SubItems.Add("", AllDataSeries[i].DrawColor, AllDataSeries[i].DrawColor, new Font("Verdana", 1));
                    lvi.SubItems.Add(AllDataSeries[i].Name);
                    lvi.SubItems.Add(AllDataSeries[i].GetPointCount().ToString());

                    items[i] = lvi;
                }

                int topItemIndex = CLRGraph_MainForm.self.listView_series.TopItem == null ? 0 : CLRGraph_MainForm.self.listView_series.TopItem.Index;
                CLRGraph_MainForm.self.listView_series.Items.Clear();
                CLRGraph_MainForm.self.listView_series.Items.AddRange(items);
                CLRGraph_MainForm.self.listview_series_disable_ItemChecked = false;


                if (CLRGraph_MainForm.self.listView_series.Items.Count > 0)
                {
                    try { CLRGraph_MainForm.self.listView_series.TopItem = CLRGraph_MainForm.self.listView_series.Items[Math.Max(topItemIndex, 0)]; } catch { }
                }
            }
            else
            {
                CLRGraph_MainForm.self.listView_series.Items.Clear();
            }

            GLGraph.Redraw();
        }

        #region Reset and Clear
        [ClojureStaticMethod("reset-all-series", "Deletes all series and creates a new empty series.")]
        public static void ResetAll()
        {
            DataSeries.ResetAll();
        }

        [ClojureStaticMethod("clear-series-points", "Clears all points from the currently selected series.")]
        public static void ClearSeriesPoints()
        {
            DataSeries_Funcs.GetCurrentDataSeries().ClearDataPoints();
            GLGraph.self.UpdateGraphAxes();
            GLGraph.self.UpdateMatrices(true);
        }

        [ClojureStaticMethod("clear-all-series-data-points", "Clears all points from all series. The series themselves remain.")]
        public static void ClearSeriesPointsAll()
        {
            for (int i = 0; i < DataSeries.AllDataSeries.Count; i++)
                DataSeries.AllDataSeries[i].ClearDataPoints();

            GLGraph.self.UpdateGraphAxes();
            GLGraph.self.UpdateMatrices(true);
        }
        #endregion

        #region Get Series/Series ID
        [ClojureStaticMethod("get-current-series", "Returns the currently selected series.")]
        public static DataSeries GetCurrentDataSeries()
        {
            if (AllDataSeries.Count == 0)
            {
                new DataSeries();
                CurrentDataSeries = 0;
                UpdateSeriesInfoInUI();
            }

            if (CurrentDataSeries >= AllDataSeries.Count)
                CurrentDataSeries = AllDataSeries.Count - 1;
            if (CurrentDataSeries < 0)
                CurrentDataSeries = 0;

            return AllDataSeries[CurrentDataSeries];
        }

        [ClojureStaticMethod("get-series", "Returns the series object represented by the given ID or name.")]
        public static DataSeries GetSeries(DataSeries ds)
        {
            return ds;
        }

        [ClojureStaticMethod("get-series", "Returns the series object represented by the given ID or name.")]
        public static DataSeries GetSeries(int id)
        {
            if (id <= 0 || id > AllDataSeries.Count)
                return null;

            return AllDataSeries[id - 1];
        }

        [ClojureStaticMethod("get-series", "Returns the series object represented by the given ID or name.")]
        public static DataSeries GetSeries(string name)
        {
            for (int i = 0; i < AllDataSeries.Count; i++)
            {
                if (AllDataSeries[i].Name == name)
                    return AllDataSeries[i];
            }

            return null;
        }

        [ClojureStaticMethod("get-series-id", "Returns the series ID of a given series.")]
        public static int GetDataSeriesID(DataSeries series)
        {
            for (int i = 0; i < AllDataSeries.Count; i++)
            {
                if (AllDataSeries[i] == series)
                    return i + 1;
            }

            return 0;
        }
        #endregion

        #region Set Current Series
        [ClojureStaticMethod("set-current-series", "Sets the current series based on ID, name or series object.")]
        public static DataSeries SetCurrentSeries(int newCurrent)
        {
            if (newCurrent > AllDataSeries.Count)
                newCurrent = AllDataSeries.Count;
            if (newCurrent <= 0)
                newCurrent = 1;

            CurrentDataSeries = newCurrent - 1;

            if (AllDataSeries.Count == 0)
                return null;

            return AllDataSeries[CurrentDataSeries];
        }

        [ClojureStaticMethod("set-current-series", "Sets the current series based on ID, name or series object.")]
        public static DataSeries SetCurrentSeries(string newCurrentName)
        {
            for (int i = 0; i < AllDataSeries.Count; i++)
            {
                if (AllDataSeries[i].Name == newCurrentName)
                    return SetCurrentSeries(i);
            }

            return null;
        }

        [ClojureStaticMethod("set-current-series", "Sets the current series based on ID, name or series object.")]
        public static DataSeries SetCurrentSeries(DataSeries newCurrentSeries)
        {
            int indexOf = AllDataSeries.IndexOf(newCurrentSeries);

            if (indexOf < 0)
            {
                AllDataSeries.Add(newCurrentSeries);
                CurrentDataSeries = AllDataSeries.Count - 1;
            }
            else
            {
                CurrentDataSeries = indexOf;
            }

            return newCurrentSeries;
        }
        #endregion

        #region Add New Series
        [ClojureStaticMethod("add-new-series", "Creates and returns a new series, optionally with an initial set of points, supplied name and color.")]
        public static DataSeries AddNewSeries()
        {
            return AddNewSeries(null, (Color?)null);
        }

        [ClojureStaticMethod("add-new-series", "Creates and returns a new series, optionally with an initial set of points, supplied name and color.")]
        public static DataSeries AddNewSeries(string name)
        {
            return AddNewSeries(name, null);
        }

        [ClojureStaticMethod("add-new-series", "Creates and returns a new series, optionally with an initial set of points, supplied name and color.")]
        public static DataSeries AddNewSeries(string name, Color? color)
        {
            DataSeries ds = new DataSeries(name, color);
            UpdateSeriesInfoInUI();
            return ds;
        }

        [ClojureStaticMethod("add-new-current-series", "Creates and returns a new series, optionally with an initial set of points, supplied name and color.")]
        public static DataSeries AddNewSeries(PersistentVector initialPoints)
        {
            return AddNewSeries(initialPoints, null, null);
        }

        [ClojureStaticMethod("add-new-current-series", "Creates and returns a new series, optionally with an initial set of points, supplied name and color.")]
        public static DataSeries AddNewSeries(PersistentVector initialPoints, string name)
        {
            return AddNewSeries(initialPoints, name, null);
        }

        [ClojureStaticMethod("add-new-current-series", "Creates and returns a new series, optionally with a supplied name and color.")]
        public static DataSeries AddNewSeries(PersistentVector initialPoints, string name, Color? color)
        {
            DataSeries ds = new DataSeries(initialPoints, name, color);
            UpdateSeriesInfoInUI();
            return ds;
        }
        #endregion

        #region Add New Current Series
        [ClojureStaticMethod("add-new-current-series", "Creates and returns a new series, optionally with a supplied name and color. This series is also set to the current series.")]
        public static DataSeries AddNewCurrentDataSeries()
        {
            return AddNewCurrentDataSeries(null, (Color?)null);
        }

        [ClojureStaticMethod("add-new-current-series", "Creates and returns a new series, optionally with a supplied name and color. This series is also set to the current series.")]
        public static DataSeries AddNewCurrentDataSeries(string name)
        {
            return AddNewCurrentDataSeries(name, null);
        }

        [ClojureStaticMethod("add-new-current-series", "Creates and returns a new series, optionally with a supplied name and color. This series is also set to the current series.")]
        public static DataSeries AddNewCurrentDataSeries(string name, Color? color)
        {
            DataSeries ds = new DataSeries(name, color);
            CurrentDataSeries = AllDataSeries.Count - 1;
            UpdateSeriesInfoInUI();
            return ds;
        }

        [ClojureStaticMethod("add-new-current-series", "Creates and returns a new series, optionally with an initial set of points, supplied name and color. This series is also set to the current series.")]
        public static DataSeries AddNewCurrentDataSeries(PersistentVector initialPoints)
        {
            return AddNewCurrentDataSeries(initialPoints, null, null);
        }

        [ClojureStaticMethod("add-new-current-series", "Creates and returns a new series, optionally with an initial set of points, supplied name and color. This series is also set to the current series.")]
        public static DataSeries AddNewCurrentDataSeries(PersistentVector initialPoints, string name)
        {
            return AddNewCurrentDataSeries(initialPoints, name, null);
        }


        [ClojureStaticMethod("add-new-current-series", "Creates and returns a new series, optionally with a supplied name and color. This series is also set to the current series.")]
        public static DataSeries AddNewCurrentDataSeries(PersistentVector initialPoints, string name, Color? color)
        {
            DataSeries ds = new DataSeries(initialPoints, name, color);
            CurrentDataSeries = AllDataSeries.Count - 1;
            UpdateSeriesInfoInUI();
            return ds;
        }
        #endregion

        #region Delete Series
        [ClojureStaticMethod("delete-series", "Deletes the supplied series object or the series represented by the given ID or name.")]
        public static void DeleteSeries(DataSeries toDelete)
        {
            if(toDelete != null)
                toDelete.Dispose();
        }

        [ClojureStaticMethod("delete-series", "Deletes the supplied series object or the series represented by the given ID or name.")]
        public static void DeleteSeries(int toDelete)
        {
            DeleteSeries(GetSeries(toDelete));
        }

        [ClojureStaticMethod("delete-series", "Deletes the supplied series object or the series represented by the given ID or name.")]
        public static void DeleteSeries(string toDelete)
        {
            DeleteSeries(GetSeries(toDelete));
        }
        #endregion

        #region Merge Series
        [ClojureStaticMethod("merge-series", "Merges two or more series into one.")]
        public static DataSeries MergeSeries(IList seriesList)
        {
            if(seriesList.Count == 0)
            {
                ClojureEngine.Log("Supplied collection was empty. Stopping.");
                return null;
            }

            DataSeries first = null;
            for(int i = 0; i < seriesList.Count; i++)
            {
                DataSeries current = null;

                if (!(seriesList[i] is DataSeries))
                {
                    int seriesID = -1;
                    if (int.TryParse(seriesList[i].ToString(), out seriesID))
                    {
                        current = GetSeries(seriesID);
                    }
                    else if (seriesList[i] is string)
                    {
                        current = GetSeries((string)seriesList[i]);
                    }
                }
                else
                {
                    current = (DataSeries)seriesList[i];
                }

                if (current == null)
                {
                    ClojureEngine.Log("Could not merge '" + seriesList[i] + "' - It is not a series, ID or series name. Skipping.");
                    continue;
                }

                if (first == null)
                {
                    first = current;
                }
                else
                {
                    first.AddDataPoints(current.GetDataPoints());
                    DeleteSeries(current);
                }
            }

            return first;
        }
        #endregion

        #region Rename Series
        [ClojureStaticMethod("rename-series", "Renames a given series.")]
        public static DataSeries RenameSeries(string newname)
        {
            return RenameSeries(GetCurrentDataSeries(), newname);
        }

        [ClojureStaticMethod("rename-series", "Renames a given series.")]
        public static DataSeries RenameSeries(DataSeries series, string newname)
        {
            series.SetName(newname);
            return series;
        }
        #endregion

        #region Set Data Source
        [ClojureStaticMethod("set-series-data-source", "Attaches a data source to a series, optionally with a repeating poll interval.")]
        public static DataSeries SetDataSource(string sourceName)
        {
            return SetDataSource(DataSource_Funcs.GetDataSource(sourceName));
        }

        [ClojureStaticMethod("set-series-data-source", "Attaches a data source to a series, optionally with a repeating poll interval.")]
        public static DataSeries SetDataSource(string sourceName, double interval)
        {
            return SetDataSource(DataSource_Funcs.GetDataSource(sourceName), interval);
        }

        [ClojureStaticMethod("set-series-data-source", "Attaches a data source to a series, optionally with a repeating poll interval.")]
        public static DataSeries SetDataSource(string sourceName, DataSeries series)
        {
            return SetDataSource(DataSource_Funcs.GetDataSource(sourceName), series);
        }

        [ClojureStaticMethod("set-series-data-source", "Attaches a data source to a series, optionally with a repeating poll interval.")]
        public static DataSeries SetDataSource(string sourceName, DataSeries series, double interval)
        {
            return SetDataSource(DataSource_Funcs.GetDataSource(sourceName), series, interval);
        }

        [ClojureStaticMethod("set-series-data-source", "Attaches a data source to a series, optionally with a repeating poll interval.")]
        public static DataSeries SetDataSource(DataSource source)
        {
            return SetDataSource(source, GetCurrentDataSeries());
        }

        [ClojureStaticMethod("set-series-data-source", "Attaches a data source to a series, optionally with a repeating poll interval.")]
        public static DataSeries SetDataSource(DataSource source, double interval)
        {
            return SetDataSource(source, GetCurrentDataSeries(), interval);
        }

        [ClojureStaticMethod("set-series-data-source", "Attaches a data source to a series, optionally with a repeating poll interval.")]
        public static DataSeries SetDataSource(DataSource source, DataSeries series)
        {
            return series.SetDataSource(source);
        }

        [ClojureStaticMethod("set-series-data-source", "Attaches a data source to a series, optionally with a repeating poll interval.")]
        public static DataSeries SetDataSource(DataSource source, DataSeries series, double interval)
        {
            return series.SetDataSource(source, interval);
        }
        #endregion

        #region Start/Stop Source Poll Timers
        [ClojureStaticMethod("start-series-poll", "Starts the data source poll timer on a series, optionally with a new interval.")]
        public static DataSeries StartSeriesPollTimer()
        {
            return GetCurrentDataSeries().StartDataSourcePoll();
        }

        [ClojureStaticMethod("start-series-poll", "Starts the data source poll timer on a series, optionally with a new interval.")]
        public static DataSeries StartSeriesPollTimer(double interval)
        {
            return GetCurrentDataSeries().StartDataSourcePoll(interval);
        }

        [ClojureStaticMethod("start-series-poll", "Starts the data source poll timer on a series, optionally with a new interval.")]
        public static DataSeries StartSeriesPollTimer(DataSeries series)
        {
            return series.StartDataSourcePoll();
        }

        [ClojureStaticMethod("start-series-poll", "Starts the data source poll timer on a series, optionally with a new interval.")]
        public static DataSeries StartSeriesPollTimer(DataSeries series, double interval)
        {
            return series.StartDataSourcePoll(interval);
        }

        [ClojureStaticMethod("stop-series-poll", "Stops the data source poll timer on a series.")]
        public static DataSeries StopSeriesPollTimer()
        {
            return GetCurrentDataSeries().StopDataSourcePoll();
        }

        [ClojureStaticMethod("stop-series-poll", "Stops the data source poll timer on a series.")]
        public static DataSeries StopSeriesPollTimer(DataSeries series)
        {
            return series.StopDataSourcePoll();
        }
        #endregion


        #region Get Point Counts
        [ClojureStaticMethod("get-series-point-count", "Returns the number of points in the given series.")]
        public static int GetSeriesPointCount(DataSeries series)
        {
            return series.GetPointCount();
        }

        [ClojureStaticMethod("get-series-point-count", "Returns the number of points in the given series.")]
        public static int GetSeriesPointCount(int series)
        {
            return GetSeriesPointCount(GetSeries(series));
        }

        [ClojureStaticMethod("get-series-point-count", "Returns the number of points in the given series.")]
        public static int GetSeriesPointCount(string series)
        {
            return GetSeriesPointCount(GetSeries(series));
        }

        [ClojureStaticMethod("get-total-point-count", "Returns the total number of points in all series.")]
        public static int TotalPointCount()
        {
            int total = 0;

            for (int i = 0; i < AllDataSeries.Count; i++)
                total += GetSeriesPointCount(AllDataSeries[i]);

            return total;
        }
        #endregion

        #region Set Series Color
        [ClojureStaticMethod("set-series-color", "Sets the given series color. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesColor(int r, int g, int b)
        {
            return SetSeriesColor(Color.FromArgb(r, g, b));
        }

        [ClojureStaticMethod("set-series-color", "Sets the given series color. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesColor(Color col)
        {
            return SetSeriesColor(GetCurrentDataSeries(), col);
        }

        [ClojureStaticMethod("set-series-color", "Sets the given series color. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesColor(DataSeries series, int r, int g, int b)
        {
            return SetSeriesColor(series, Color.FromArgb(r, g, b));
        }

        [ClojureStaticMethod("set-series-color", "Sets the given series color. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesColor(DataSeries series, Color col)
        {
            series.SetColor(col);
            return series;
        }
        #endregion

        #region Set Color Mode
        [ClojureStaticMethod("set-series-color-mode", "Sets the given series color mode. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetColorMode(ColorMode mode)
        {
            return SetSeriesColorMode(GetCurrentDataSeries(), mode);
        }

        [ClojureStaticMethod("set-series-color-mode", "Sets the given series color mode. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesColorMode(DataSeries series, ColorMode mode)
        {
            series.SetColorMode(mode);
            return series;
        }

        [ClojureStaticMethod("set-all-series-color-mode", "Sets all series color modes.")]
        public static void SetSeriesColorModeAll(ColorMode mode)
        {
            for (int i = 0; i < DataSeries.AllDataSeries.Count; i++)
                DataSeries.AllDataSeries[i].SetColorMode(mode);
        }
        #endregion

        #region Set Draw mode
        [ClojureStaticMethod("set-series-draw-mode", "Sets the given series draw mode. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesDrawMode(DrawMode mode)
        {
            return SetSeriesDrawMode(GetCurrentDataSeries(), mode);
        }

        [ClojureStaticMethod("set-series-draw-mode", "Sets the given series draw mode. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesDrawMode(DataSeries series, DrawMode mode)
        {
            series.SetDrawMode(mode);
            return series;
        }

        [ClojureStaticMethod("set-all-series-draw-mode", "Sets all series draw modes.")]
        public static void SetSeriesDrawModeAll(DrawMode mode)
        {
            for (int i = 0; i < DataSeries.AllDataSeries.Count; i++)
                DataSeries.AllDataSeries[i].SetDrawMode(mode);
        }
        #endregion

        #region Set Transparency Mode
        [ClojureStaticMethod("set-series-transparency-mode", "Sets the given series transparency mode. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesTransparencyMode(TransparencyMode mode)
        {
            return SetSeriesTransparencyMode(GetCurrentDataSeries(), mode);
        }

        [ClojureStaticMethod("set-series-transparency-mode", "Sets the given series transparency mode. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesTransparencyMode(DataSeries series, TransparencyMode mode)
        {
            series.SetTransparencyMode(mode);
            return series;
        }

        [ClojureStaticMethod("set-all-series-transparency-mode", "Sets all series transparency modes.")]
        public static void SetSeriesTransparencyModeAll(TransparencyMode mode)
        {
            for (int i = 0; i < DataSeries.AllDataSeries.Count; i++)
                DataSeries.AllDataSeries[i].SetTransparencyMode(mode);
        }
        #endregion

        #region Set Line Width
        [ClojureStaticMethod("set-series-line-width", "Sets the given series line width. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesLineWidth(float width)
        {
            return SetSeriesLineWidth(GetCurrentDataSeries(), width);
        }

        [ClojureStaticMethod("set-series-line-width", "Sets the given series line width. If no series is supplied, the current series is used instead.")]
        public static DataSeries SetSeriesLineWidth(DataSeries series, float width)
        {
            series.SetLineWidth(width);
            return series;
        }

        [ClojureStaticMethod("set-series-line-width-all", "Sets all series line widthss.")]
        public static void SetSeriesLineWidthAll(float width)
        {
            for (int i = 0; i < DataSeries.AllDataSeries.Count; i++)
                DataSeries.AllDataSeries[i].SetLineWidth(width);
        }
        #endregion
    }
}
