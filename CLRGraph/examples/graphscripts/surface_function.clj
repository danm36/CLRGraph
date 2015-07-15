;;;; This example shows how to create a surface just from an equation

;; Change this to change the function used in surface generation
;; %1 is the current X value
;; %2 is the current Y value
(def func #(* %1 %2))

;; Change these to change the start and end values to feed into
;; the function
(def x-min -10)
(def x-max 10)
(def y-min -10)
(def y-max 10)

;; Change this to change how precise the graph is
;; Higher numbers = less precise
(def precision 0.1)

;; And the actual suface making function. Simple, isn't it!
(set-data-points (make-3d-surface-fn x-min y-min x-max y-max precision func))