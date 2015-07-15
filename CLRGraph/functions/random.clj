;;;; Functions with a focus on randomness

(defn random-cloud-cubic
	"Creates a random cubic cloud of points"
	([count] (random-cloud-cubic 0 0 0 1 1 1 count))
	([extents count] (random-cloud-cubic 0 0 0 extents extents extents count))
	([origin extents count] (random-cloud-cubic origin origin origin extents extents extents count))
	([ex ey ez count] (random-cloud-cubic 0 0 0 ex ey ez count))
	([ox oy oz ex ey ez count]
		(def xvals [])
		(def yvals [])
		(def zvals [])
		(dotimes [n count]
			(def xvals (conj xvals (+ (rand ex) ox)))
			(def yvals (conj yvals (+ (rand ey) oy)))
			(def zvals (conj zvals (+ (rand ez) oz)))
		)
		(make-data-points-3d xvals yvals zvals)
	)
)

;;FIXME: Axis bias
(defn random-cloud-spherical
	"Creates a random spherical cloud of points"
	([count] (random-cloud-spherical 0 0 0 1 count))
	([dist count] (random-cloud-spherical 0 0 0 dist count))
	([origin dist count] (random-cloud-spherical origin origin origin dist count))
	([ox oy oz dist count]
		(def xvals [])
		(def yvals [])
		(def zvals [])
		(dotimes [n count]
			(let [
					yaw (rand CMath/TwoPI)
					pitch (rand CMath/TwoPI)
					randdist (rand dist)
				]
				(def xvals (conj xvals (+ (* (* (CMath/Sin yaw) (CMath/Cos pitch)) randdist) ox)))
				(def yvals (conj yvals (+ (* (CMath/Sin pitch) randdist) oy)))
				(def zvals (conj zvals (+ (* (* (CMath/Cos yaw) (CMath/Cos pitch)) randdist) oz)))
			)
		)
		(make-data-points-3d xvals yvals zvals)
	)
)

;; FIXME: Axis bias
(defn random-cloud-cylindrical
	"Creates a random cylindrical cloud of points"
	([count] (random-cloud-cylindrical 0 0 0 1 1 count))
	([dist count] (random-cloud-cylindrical 0 0 0 dist dist count))
	([dist height count] (random-cloud-cylindrical 0 0 0 dist height count))
	([origin dist height count] (random-cloud-cylindrical origin origin origin dist height count))
	([ox oy oz dist height count]
		(def xvals [])
		(def yvals [])
		(def zvals [])
		(dotimes [n count]
			(let [
					yaw (rand CMath/TwoPI)
					randheight (rand height)
				]
				(def xvals (conj xvals (+ (* (CMath/Sin yaw) (rand dist)) ox)))
				(def yvals (conj yvals (+ (- randheight (/ height 2)) oy)))
				(def zvals (conj zvals (+ (* (CMath/Cos yaw) (rand dist)) oz)))
			)
		)
		(make-data-points-3d xvals yvals zvals)
	)
)