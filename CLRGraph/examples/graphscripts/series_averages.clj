;;;; This example creates a number of spherical clouds then connects their
;;;; average co-ordinates together, demonstrating series-average

;; The number of clouds to generate
(def cloud-count 12)

;; If true, a hull is also made around the average points
(def create-hull true)

;; Main code begins here
(set-series-draw-mode DrawMode/Points)
(rename-series "Cloud 1")
(set-data-points (random-cloud-spherical (- (rand 20) 10) (- (rand 20) 10) (- (rand 20) 10) 1 1000))
(dotimes [n (- cloud-count 1)]
	(add-new-current-series (str "Cloud " (+ n 1)))
	(set-data-points (random-cloud-spherical (- (rand 20) 10) (- (rand 20) 10) (- (rand 20) 10) 1 1000)))
(add-new-current-series "Averages")
(dotimes [n cloud-count]
	(add-data-points [(series-average (get-series (+ n 1)))]))
(set-series-draw-mode DrawMode/ConnectedLines)
(if create-hull (make-hull))