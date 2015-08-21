;;;; This details the use of the runtime UI
;;;; This example utilizes all available UI elements

(def point-count 10000)
(def size-x 10)
(def size-y 10)
(def size-z 10)
(def offset-x -5)
(def offset-y -5)
(def offset-z -5)
(def cloud-type "cubic")

(def onchange #(do
	(set-data-points
		(case cloud-type
			"cubic" (random-cloud-cubic offset-x offset-y offset-z size-x size-y size-z point-count)
			"spherical" (random-cloud-spherical offset-x offset-y offset-z size-x point-count)
			"cylindrical" (random-cloud-cylindrical offset-x offset-y offset-z size-x size-y point-count)
		)
	)
))

(add-ui-slider 		"Point count: "		#'point-count	1	100000 	onchange)
(add-ui-numeric		"Size X/Radius: "	#'size-x	0.01	1000 	onchange)
(add-ui-numeric		"Size Y/Height: "	#'size-y	0.01	1000 	onchange)
(add-ui-numeric		"Size Z       : "	#'size-z	0.01	1000 	onchange)
(add-ui-slider		"Offset X: "	#'offset-x	-100	100 	onchange)
(add-ui-slider		"Offset Y: "	#'offset-y	-100	100 	onchange)
(add-ui-slider		"Offset Z: "	#'offset-z	-100	100 	onchange)
(add-ui-dropdown	"Cloud Type: "	#'cloud-type	["Cubic" "\"cubic\"" "Spherical" "\"spherical\"" "Cylindrical" "\"cylindrical\""] 	onchange)
(add-ui-buttons		["Axis Partition"	#(axis-split-series)
			"Make Hull"		#(make-hull)
			"Reset Graph"		#(do (reset-all-series) (onchange))])

(onchange)