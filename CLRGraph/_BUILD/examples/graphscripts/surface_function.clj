;;;; This example shows how to create a surface just from an equation
;;;; This example also creates a runtime UI, which you should use
;;;; rather than modifying this code directly. The main exception to
;;;; this is adding new functions to graph.

;; This is the collection of functions that will be available in the
;; supplied runtime drop down button. Add more graphs using the pattern.
;; Note: The functions must be quoted as they are copied in place.
;; Note: x and y are valid to use in these functions
(def graph-funcs [
	"Z = X * Y" 			"#(* x y)"
	"Z = X^2 * Y^2" 		"#(* (* x x) (* y y))"
	"Z = X^2 * Y - Y" 		"#(- (* (* x x) y) y)"
	"Z = X * Exp(-X^2 - Y^2)"	"#(* x (CMath/Exp (- (* x x -1) (* y y))))"
	"[Waves] Z = 0.1 * Cos(Sqrt((X^2 + Y^2) * 128))" "#(* 0.1 (CMath/Cos (CMath/Sqrt (* (+ (* x x) (* y y)) 128))))"
	"[Droplet] Z = 0.5 * (1 / Exp(Sqrt((X^2 + Y^2) * 128))) * Cos(Sqrt((X^2 + Y^2) * 128))" "#(* 0.5 (/ 1 (CMath/Exp (CMath/Sqrt (* (+ (* x x) (* y y)) 4)))) (CMath/Cos (CMath/Sqrt (* (+ (* x x) (* y y)) 128))))"
	"[Inverted Waves] Z = Sin(x * y * 32) * 0.1"  "#(* (CMath/Sin (* x y 32)) 0.1)"
     ])

;; Change these to change the start and end values to feed into
;; the function
(def x-min -2)
(def x-max 2)
(def y-min -2)
(def y-max 2)

;; Change this to change how precise the graph is
;; Higher numbers = less precise
(def precision 0.2)

;; This is the function that's set by the dropdown
(def func #(* x y))

;; This function creates the graph from the given points, and is called when the dropdown changes
(def update-graph #(set-data-points (make-3d-surface-fn x-min y-min x-max y-max precision func)))

;; This creates a dropdown button in the Runtime UI tab
(add-ui-dropdown "Graph Type:" #'func graph-funcs update-graph)

;; Add sliders for the min and max values
(add-ui-slider-double "Min X:" #'x-min -200 200 update-graph)
(add-ui-slider-double "Max X:" #'x-max -200 200 update-graph)
(add-ui-slider-double "Min Y:" #'y-min -200 200 update-graph)
(add-ui-slider-double "Max Y:" #'y-max -200 200 update-graph)

;; Adds a slider for precision
(add-ui-slider-double "Precision:" #'precision 0.01 2 update-graph)

;; And run the update once to get an initial graph
(update-graph)



