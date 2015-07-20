;;;; This details the use of the runtime UI
;;;; The graph will show a random cubic cloud of 10,000 points
;;;; Two dropdowns will be added to the Runtime UI tab allowing you to
;;;; change the color of the cloud. Feel free to add more colors.

;; Our changing value
(def color Color/Red)

;; A function to create our cloud
(def onchange #(do
	(set-series-color color)
	(set-data-points (random-cloud-cubic 10 10000))))

;; A list of valid colors and their labels. Feel free to add mode using
;; the same pattern. To ensure the proper value is set, make these strings.
(def colors [	"Red" "Color/Red"
		"Green" "Color/Green"
		"Blue" "Color/Blue"
		"Yellow" "Color/Yellow" 
		"Magenta" "(Color/FromArgb 255 0 255)"])

;; Add the dropdown.
;; The format is: "(add-ui-dropdown [Label] [Variable] [Options] [Function])"
;; [Label]	The label to display next to the dropdown.
;; [Variable]	The variable to bind to the dropdown.
;; [Options]	A key->value vector containing the dropdown options and the
;;		code to set the variable to when selected.
;; [Function]	Optional. The function to run when the dropdown is changed.
(add-ui-dropdown "Color: " #'color colors onchange)

;; Adds an alternative dropdown that supports functions per item.
;; The format is: "(add-ui-dropdown [Label] [Options])"
;; [Label]	The label to display next to the dropdown.
;; [Options]	A key->value vector containing the dropdown options and the
;;		function to run when selected.
(add-ui-dropdown-fn "Color (FN): " [	"Red" 		#(do (def color Color/Red) (onchange))
					"Green"		#(do (def color Color/Green) (onchange))
					"Blue"		#(do (def color Color/Blue) (onchange))
					"Yellow"	#(do (def color Color/Yellow) (onchange))
					"Magenta"	#(do (def color (Color/FromArgb 255 0 255)) (onchange))])

;; And finally call our change function to get the initial cloud
(onchange)