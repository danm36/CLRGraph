;;;; This example shows how to create a surface just from an equation
;;;; This example also creates a runtime UI, which you should use
;;;; rather than modifying this code directly. The main exception to
;;;; this is adding new functions to graph.

;; This is the collection of functions that will be available in the
;; supplied runtime drop down button. Add more graphs using the pattern.
;; Note: The functions must be quoted as they are copied in place.
(def graph-funcs [
	"Z = X * Y" 			"#(* %1 %2)"
	"Z = X^2 * Y^2" 		"#(* (* %1 %1) (* %2 %2))"
	"Z = X^2 * Y - Y" 		"#(- (* (* %1 %1) %2) %2)"
	"Z = X * Exp(-X^2 - Y^2)"	"#(* %1 (CMath/Exp (- (* %1 %1 -1) (* %2 %2))))"
	"Z = 0.1 * Cos(Sqrt((X^2 + Y^2) * 128))" "#(* 0.1 (CMath/Cos (CMath/Sqrt (* (+ (* %1 %1) (* %2 %2)) 128))))"
	])

;; Change these to change the start and end values to feed into
;; the function
(def x-min -2)
(def x-max 2)
(def y-min -2)
(def y-max 2)

;; Change this to change how precise the graph is
;; Higher numbers = less precise
(def precision 0.02)

;; This is the function that's set by the dropdown
(def func #(* %1 %2))

;; This function creates the graph from the given points, and is called when the dropdown changes
(def update-graph #(set-data-points (make-3d-surface-fn x-min y-min x-max y-max precision func)))

;; This creates a dropdown button in the Runtime UI tab
(add-ui-dropdown "Graph Type:" #'func graph-funcs update-graph)

;; And run the update once to get an initial graph
(update-graph)



