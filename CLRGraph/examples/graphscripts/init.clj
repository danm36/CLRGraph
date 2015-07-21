;; Setup display how we want
(set-series-draw-mode DrawMode/PointCubes)
(set-series-line-width 1)

;; Add first souce
(start-series-poll (set-series-data-source "DataSource_1") 0.05)

;; Add other sources
(add-new-current-series)
(start-series-poll (set-series-data-source "DataSource_2") 0.05)