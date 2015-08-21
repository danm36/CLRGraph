;;;; This renders a loud of points, then partitions them along the three axes
;;;; If do-partitions is false, only the original cloud will be shown
;;;; If partition-in-seq is true, only the points from the previous partition will be partitioned

;; The cloud mode to use. Valid values:
;; random-cloud-cubic
;; random-cloud-spherical
;; random-cloud-cylindrical
(def cloudmode random-cloud-cubic)

;; Total number of points in the cloud
(def pointcount 10000)

;; Size on each edge for the point field
(def fieldsize 10)

;; If true, partitions are done on the cloud
(def do-partitions true)

;; If true, only the previously partitioned set of points is partitioned
(def partition-in-seq false)

;; Main draw code
(def fieldstart (* (- fieldsize (/ fieldsize 2)) -1))
(set-data-points (cloudmode fieldstart fieldsize pointcount))

(if do-partitions (if (not partition-in-seq) (axis-split-series) (do
	(split-series #(>= (.y %) 0))		;; The top section
	(split-series #(>= (.x %) 0))
	(split-series #(>= (.z %) 0))
	)))