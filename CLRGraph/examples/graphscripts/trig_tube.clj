;;;; This script draws four spirals using the sin and cos trigonmetry
;;;; functions. This also details the creation of new data series.
;;;; Remember to switch to a 3D rendering mode!

;; To ease referencing later on
;; Do not modify these
(def halfpi (* CMath/PI 0.5))
(def pi CMath/PI)
(def onehalfpi (* CMath/PI 1.5))

;; Other variables that control drawing.
;; Modify these as you wish.
(def trigstep 0.1)
(def xstep trigstep)
(def xstart -100)
(def xend 100)

;; The first data series is automatically created for us, so lets
;; add the first spiral!
;; Because we're using set-data-points-3d, we must supply at least
;; two collections for the y and z data (If you supply three, the
;; first will be considered x-coord data). xstart and xstep tell
;; the graph how to generate the x-coords for this graph.
(set-data-points-3d
	(CMath/Sin (range xstart xend trigstep))
	(CMath/Cos (range xstart xend trigstep))
	xstart
	xstep
)

;; Here we're creating a new series to keep this spiral seperate from
;; the one above. We could also use 'add-data-points-3d' with the same
;; parameters to add to the spiral from above, but in this particular
;; case that is not desirable.
;; If you want, change it to add-data-points-3d and comment out
;; add-new-current-data-series and see how the graph changes
(add-new-current-data-series)
(set-data-points-3d
	(CMath/Sin (range (+ xstart halfpi) (+ xend halfpi) trigstep))
	(CMath/Cos (range (+ xstart halfpi) (+  xend halfpi) trigstep))
	xstart
	xstep
)

;; And again, this time with a pi offset from the first spiral
(add-new-current-data-series)
(set-data-points-3d
	(CMath/Sin (range (+ xstart pi) (+  xend pi) trigstep))
	(CMath/Cos (range (+ xstart pi) (+  xend pi) trigstep))
	xstart
	xstep
)

;; And out last spiral 1.5PI offset from the first spiral
(add-new-current-data-series)
(set-data-points-3d
	(CMath/Sin (range (+ xstart onehalfpi) (+ xend onehalfpi) trigstep))
	(CMath/Cos (range (+ xstart onehalfpi) (+  xend onehalfpi) trigstep))
	xstart
	xstep
)