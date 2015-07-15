;;;; A Pacman diorama made using some random cloud algorithms
;;;; This example is purely for fun and serves little mathematical merit

;; The number of dots in front of Pacman
(def dotcount 20)

;; Main code begins here
;; Sets the scene
(set-background-color Color/Black)
(axis-display false)

;; Create Pacman himself
;; Alter the second value in CMath/Pow to change how open Pacman's mouth is
;; (or make something completely different
(rename-series "Pacman")
(set-data-points (random-cloud-spherical 10 50000))
(filter-series #(< (+ (.x %) (.y %)) (CMath/Pow (.y %) 2)))
(set-series-color Color/Yellow)

;; Add the dots based on the dot count set above
(add-new-current-series "Dots" Color/White)
(dotimes [n dotcount] (add-data-points (random-cloud-spherical (* (+ n 1) 15) 0 0 1 1000)))

;; Now for the walls. random-cloud-cubic allows different x, y and z dimensions
(add-new-current-series "Walls" Color/Blue)
(add-data-points (random-cloud-cubic -60 15 -10 (+ (* 15 dotcount) 90) 1 20 5000))
(add-data-points (random-cloud-cubic -60 -15 -10 (+ (* 15 dotcount) 90) 1 20 5000))

;; Blinky's eyes are rendered first to avoid a current depth sorting bug
(add-new-current-series "Blinky_Eyes" Color/White)
(add-data-points (random-cloud-spherical -22 2 -5 2 500))
(add-data-points (random-cloud-spherical -22 2 5 2 500))

;; And finally draw Blinky himself
(add-new-current-series "Blinky" Color/Red)
(add-data-points (random-cloud-spherical -30 0 0 10 10000))
(add-data-points (random-cloud-cylindrical -30 -5 0 10 10 2500))
(add-data-points (random-cloud-spherical -23 -10 0 2 1000))
(add-data-points (random-cloud-spherical -28 -10 -5 2 1000))
(add-data-points (random-cloud-spherical -33 -10 -5 2 1000))
(add-data-points (random-cloud-spherical -28 -10 5 2 1000))
(add-data-points (random-cloud-spherical -33 -10 5 2 1000))
(add-data-points (random-cloud-spherical -38 -10 0 2 1000))

