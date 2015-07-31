(set-series-draw-mode DrawMode/PointCubes)
(start-series-poll (set-series-data-source-channel (set-series-data-source "DataSource_1") 0) 0.03)
(add-new-current-series)
(start-series-poll (set-series-data-source-channel (set-series-data-source "DataSource_1") 1) 0.03)