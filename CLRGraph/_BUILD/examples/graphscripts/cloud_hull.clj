;;;; This renders a cloud of points, then generates a convex hull from them
;;;; The more points there are, the more like the general cloud shape the
;;;; hull will be
;;;; For more interesting hulls, reduce the point count
;;;; For more accurate hulls, increse the iteration count

;; The cloud mode to use. Valid values:
;; random-cloud-cubic
;; random-cloud-spherical
;; random-cloud-cylindrical
(def cloudmode random-cloud-cubic)

;; Total number of points in the cloud
(def pointcount 10000)

;; Size on each edge for the point field
(def fieldsize 10)

;; Size on each edge for the point field
(def hulliterationcount 10)

;; Main draw code
(def fieldstart (* (- fieldsize (/ fieldsize 2)) -1))
(set-data-points (cloudmode fieldstart fieldsize pointcount))
(make-hull hulliterationcount)