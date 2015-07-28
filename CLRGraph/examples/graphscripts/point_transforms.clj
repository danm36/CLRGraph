;;;; This example takes in data from a data source and then transforms
;;;; it in multiple ways

(set-series-data-source "DataSource_1")
(translate-series 0.02 -0.032 0)
(scale-series 100)
(rotate-series 0 CMath/PI 0)