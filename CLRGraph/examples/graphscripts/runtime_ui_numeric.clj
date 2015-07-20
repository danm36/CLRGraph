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

;; The function to call when any of the numeric text boxes change
;; Each numeric text boxes could have an independent function instead if you want
(def numericupdate #(do
	(clear-series-points)
	(def xpoints [])
	(def ypoints [])
	(dotimes [n pointcount]
		(def xpoints (conj xpoints (* n multiplier)))
		(def ypoints (conj ypoints (CMath/Sin (+ (* n multiplier) offset)))))
	(set-data-points xpoints ypoints)))

;; And now we add the numeric text boxes
;; The format is: (add-ui-numeric [Label] [Var] [Min] [Max] [Function])
;; [Label] 	The label to display next to the numeric text box
;; [Var] 	The variable to attach to the numeric text box. Add #' before the variable name
;; [Min] 	The minimum textbox value
;; [Max] 	The maximum textbox value
;; [Function] 	Optional: The optional function to run when the textbox changes
(add-ui-numeric		"Point Count: " #'pointcount 	10 	10000	numericupdate)
(add-ui-numeric 	"X Offset: " 	#'offset 	0 	20 	numericupdate)
(add-ui-numeric 	"Multiplier: " 	#'multiplier 	0.001 	2 	numericupdate)

;; And finally actually call the update function to get the initial points
(numericupdate)