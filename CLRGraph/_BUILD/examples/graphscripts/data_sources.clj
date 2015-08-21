;;;; This example shows the use of data sources.

;;;; To set this up, first go to the Data Sources tab and click 'Add Data Source'.
;;;; Select 'Comma Seperated Values File' and find and open 'JCMB_2015_Jan.csv'.
;;;; Leave the data source name as its default (DataSource_1) and click 'Add'.

;;;; Once added, a properties menu will pop up asking you to select which columns
;;;; should be used for each axis. If you accidentally close this window, double
;;;; click the data series name in the list to open it up again. Set the X-Axis to
;;;; 'day-of-year' and the Y-Axis to 'surface temperature (C)'. Apply the changes
;;;; and then run this script.

;;;; You should then see 31 bars. These represent the ranges of the surface temperatures
;;;; for the month of January 2015 as reported by the University of Edinburgh's Weather
;;;; Station (The source CSV file). Change the Y and Z Axis columns in the data source
;;;; property window and re-run this file to see the new data and - if you also set a
;;;; Z-axis - the relation between the two sets of data when viewed in a 3D mode.

;;;; JCMB_2015_Jan.csv is property of the University of Edinburgh and is only used here
;;;; for demonstrative purposes.
;;;; Source: http://www.ed.ac.uk/schools-departments/geosciences/weather-station/download-weather-data
;;;; Modified to add the day-of-year column.

(set-series-data-source "DataSource_1")