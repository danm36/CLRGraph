;;;; Assistant functions for plotting
(defn plot
    "Plots a graph based on the given function, with optional range and precision.
    If no ranges are supplied, the plot is generated from -1 to 1
    If no precision is supplied, the precision defaults to 0.1
    Sets the current series to the resulting points"
    ([f] (plot -1 1 0.1 f))
    ([min max f] (plot min max 0.1 f))
    ([min max precision f]
        (set-data-points (make-plot-fn min max precision f))
    )
)

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
        (def range-x (- tmax-x min-x))
        (def range-y (- tmax-y min-y))
		(doseq [seqy (range min-y max-y precision)			
				seqx (range min-x max-x precision)]
            ;(def zvals (conj zvals (apply f [x y])))
            (def x seqx)
            (def y seqy)
            (def zvals (conj zvals (f)))
		)
		(make-3d-surface min-x min-y range-x range-y (/ range-x precision) (/ range-y precision) zvals)
	)
)