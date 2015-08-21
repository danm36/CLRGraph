;;;; This details the use of the runtime UI
;;;; The graph will show a random cubic cloud of 10,000 points
;;;; Five buttons will be added to the Runtime UI tab allowing you to
;;;; change the color of the cloud. Feel free to add more buttons.

;; Create our cloud
(set-data-points (random-cloud-cubic 10 10000))

;; Add the buttons.
;; The format is: "(add-ui-button [Label] [Function])"
;; [Label]	The label to display on the button
;; [Function]	The function to run when the button is clicked
(add-ui-button "Red" #(set-series-color Color/Red))
(add-ui-button "Green" #(set-series-color Color/Green))
(add-ui-button "Blue" #(set-series-color Color/Blue))
(add-ui-button "Yellow" #(set-series-color Color/Yellow))
(add-ui-button "Magenta" #(set-series-color 255 0 255)) 	; Note that you can supply RGB too

;; You can also add the buttons using add-ui-buttons, as detailed below
;; The format is very similar to add-ui-button, except all buttons are
;; in a vector as a label/function pair
;; Note that in this way, all the buttons are grouped together on a row
(add-ui-buttons ["Red" #(set-series-color Color/Red)
		"Green" #(set-series-color Color/Green)
		"Blue" #(set-series-color Color/Blue)
		"Yellow" #(set-series-color Color/Yellow)
		"Magenta" #(set-series-color 255 0 255)]) ; Note that you can supply RGB too