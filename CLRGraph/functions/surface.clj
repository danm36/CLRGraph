;;;; Assistant functions for surface management

(defn make-3d-surface-fn
	"Creates a surface from the supplied function, with optional x and y ranges and precision.
	If no ranges are supplied, the surface is generated from -1 to 1.
	If upper ranges are supplied, the surface begins from 0.
	If no precision is supplied, the default precision is 1."
	([f] (make-3d-surface-fn -1 -1 1 1 1 f))
	([max-x max-y f] (make-3d-surface-fn 0 0 max-x max-y 1 f))
	([max-x max-y precision f] (make-3d-surface-fn 0 0 max-x max-y precision f))
	([min-x min-y max-x max-y f] (make-3d-surface-fn min-x min-y max-x max-y 1 f))
	([min-x min-y max-x max-y precision f]
		(def zvals [])
        (def tmax-y (+ max-y precision))
        (def tmax-x (+ max-x precision))
        (def range-x (- max-x min-x))
        (def range-y (- max-y min-y))
        (def trange-x (- tmax-x min-x))
        (def trange-y (- tmax-y min-y))
		(doseq [y (range min-y max-y precision)			
				x (range min-x max-x precision)]
			(def zvals (conj zvals (apply f [x y])))
		)
		(make-3d-surface min-x min-y trange-x trange-y (/ trange-x precision) (/ trange-y precision) zvals)
	)
)