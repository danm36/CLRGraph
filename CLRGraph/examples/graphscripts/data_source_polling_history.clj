;;;; This exmaple describes how to have live regularly polled data sources
;;;; and a visible history of past data. This requires one 2D data source
;;;; (Preferably use the microphone source and have a microphone plugged in)
;;;; called 'DataSource_1'.
;;;; This example also provides a runtime UI

;; How often we poll the data source in seconds.
(def poll-rate 0.03)

;; Enable historic polling.
(def enable-poll-history true)

;; How far in the z axis to offset historic data.
(def history-offset 1)

;; The maximum distance of historic data groups.
;; This is so we don't run out of memory.
(def history-limit 30)

(start-series-poll (set-series-data-source "DataSource_1") poll-rate)
(set-series-draw-mode DrawMode/ConnectedLines)
(add-new-current-series)
(set-series-draw-mode DrawMode/Points)
(start-series-poll
	(series-poll-history
		(set-series-data-source "DataSource_1")
		enable-poll-history
		history-offset history-limit) poll-rate)

;; And add a UI
(def updatehistorysettings #(do
	(series-poll-history (get-series 2) enable-poll-history history-offset history-limit)
))
(add-ui-slider-double 	"History Offset:" #'history-offset 	0.01 	10 	updatehistorysettings)
(add-ui-slider 		"History Limit: " #'history-limit 	1 	100 	updatehistorysettings)