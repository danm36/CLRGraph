;;;; This details the use of the runtime UI
;;;; The graph will show a sine wave with parameters customizable at runtime
;;;; To modify the various properties, go to the Runtime UI tab

;; Number of points to render
;; Runtime set
(def pointcount 314)

;; Additional offset to add to the Sin function
;: Runtime set
(def offset 11)

;; Additional multiplier to apply to the points (Lower value = more compact)
;; Runtime set
(def multiplier 0.1)

;; The function to call when any of the sliders change
;; Each slider could have an independent function instead if you want
(def sliderupdate #(do
	(clear-series-points)
	(def xpoints [])
	(def ypoints [])
	(dotimes [n pointcount]
		(def xpoints (conj xpoints (* n multiplier)))
		(def ypoints (conj ypoints (CMath/Sin (+ (* n multiplier) offset)))))
	(set-data-points xpoints ypoints)))

;; And now we add the sliders
;; The format is: (add-ui-slider [Label] [Var] [Min] [Max] [Function])
;; [Label] 	The label to display next to the slider
;; [Var] 	The variable to attach to the slider. Add #' before the variable name
;; [Min] 	The minimum slider value
;; [Max] 	The maximum slider value
;; [Function] 	Optional: The optional function to run when the slider changes
;;
;; Note that if you use add-ui-slider-double instead of add-ui-slider, the
;; slider will function within the decimal ranges as well. add-ui-slider only
;; works with integers
(add-ui-slider 		"Point Count: " #'pointcount 	10 	10000	sliderupdate)
(add-ui-slider-double 	"X Offset: " 	#'offset 	0 	20 	sliderupdate)
(add-ui-slider-double 	"Multiplier: " 	#'multiplier 	0.001 	2 	sliderupdate)