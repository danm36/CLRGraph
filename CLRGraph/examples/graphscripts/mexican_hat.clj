;;;; This script details the 3D "Mexican Hat" function

;; Defines the number of points.
;; Higher numbers mean more points, which means longer parse times.
(def count 256.0)

(def xvals [])
(def yvals [])
(def zvals [])
(def halfCount (/ count 2.0))

(loop [i 0.0] (when (< i count)
	(loop [j 0.0] (when (< j count)
		(let [
				x (float (/ (- i halfCount) halfCount))
				y (float (/ (- j halfCount) halfCount))
				t (* (CMath/Sqrt (+ (* x x) (* y y))) 4)
				z (* (- 1 (* t t)) (CMath/Exp (/ (* t t) 2)))
			]
			(def xvals (conj xvals i))
			(def yvals (conj yvals z))
			(def zvals (conj zvals j))
		)
		(recur (+ j 1.0))
	))
	(recur (+ i 1.0))
))

(set-data-points-3d xvals yvals zvals)