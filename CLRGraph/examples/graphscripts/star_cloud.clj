;;;; This renders a cubic cloud of points
;;;; If realistic is true, then all points will be white and the background will be black
;;;; If realistic is false, points will have the colors associated with their series

;; The cloud mode to use. Valid values:
;; random-cloud-cubic
;; random-cloud-spherical
;; random-cloud-cylindrical
(def cloudmode random-cloud-cubic)

;; Total number of stars in the system
(def starcount 10000)

;; Size on each edge for the star field
(def fieldsize 10)

;; If true, renders white stars on a black background
;; If false, renders points in their default series color on a white background
(def realistic true)

;; Main draw code
(def fieldstart (* (- fieldsize (/ fieldsize 2)) -1))
(def clustercount (/ starcount 8))
(if realistic (set-background-color Color/Black) (set-background-color Color/White))

(set-data-points (cloudmode fieldstart fieldsize clustercount))
(if realistic (set-series-color Color/White))
(add-new-current-series)
(set-data-points (cloudmode fieldstart fieldsize clustercount))
(if realistic (set-series-color Color/White))
(add-new-current-series)
(set-data-points (cloudmode fieldstart fieldsize clustercount))
(if realistic (set-series-color Color/White))
(add-new-current-series)
(set-data-points (cloudmode fieldstart fieldsize clustercount))
(if realistic (set-series-color Color/White))
(add-new-current-series)
(set-data-points (cloudmode fieldstart fieldsize clustercount))
(if realistic (set-series-color Color/White))
(add-new-current-series)
(set-data-points (cloudmode fieldstart fieldsize clustercount))
(if realistic (set-series-color Color/White))
(add-new-current-series)
(set-data-points (cloudmode fieldstart fieldsize clustercount))
(if realistic (set-series-color Color/White))
(add-new-current-series)
(set-data-points (cloudmode fieldstart fieldsize clustercount))
(if realistic (set-series-color Color/White))
