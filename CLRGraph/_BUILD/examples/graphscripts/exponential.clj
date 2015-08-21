;;;; This script shows an exponential curve from xstart < x < xend

(def xstart -100)
(def xend 100)
(def xstep 0.1)

(set-data-points (CMath/Exp (range xstart xend xstep)) xstart xstep)