;;;; This script details the 3D "Mexican Hat" function

;; Defines the number of points.
;; Higher numbers mean more points, which means longer parse times.
(def count 256)

;; Higher values mean a tighter peak
(def peak-tightness 4)

;; This is the height of the peak (y scale)
(def height 64)

;; Scale the x and z axes of the graph
(def scale 1)

;; Is the hat centered on the origin
(def centered true)

;; Whether the hat is a filled surface or just point data
(def filled-surface true)

;; Used in the execution. Don't modify these
(def xvals [])
(def yvals [])
(def zvals [])
(def scaled-count (* count scale))
(def half-count (/ scaled-count 2))

;; Two loops, i for x and j for z
(loop [i 0] (when (< i scaled-count)
	(loop [j 0] (when (< j scaled-count)
		(let [
				x (float (/ (- i half-count) half-count))
				z (float (/ (- j half-count) half-count))
				t (* (CMath/Sqrt (+ (* x x) (* z z))) peak-tightness)
				y (* (- 1 (* t t)) (CMath/Exp (/ (* t t) -2)))
				r (* y height)
			]
			;; Append these values to the various lists
			(def xvals (conj xvals (if centered (- i half-count) i)))
			(def yvals (conj yvals r))
			(def zvals (conj zvals (if centered (- j half-count) j)))
		)
		(recur (+ j scale))
	))
	(recur (+ i scale))
))

;;And finally add the points to the graph
(if filled-surface
	(set-data-points (make-3d-surface count count yvals centered))
	(set-data-points-3d xvals yvals zvals))