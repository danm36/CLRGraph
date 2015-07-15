;;;; Functions that aid in the division and filtering of series data

(defn sub-series
	"Create a new series based on a subset of the given series' points. The original series is untouched"
	([f]
		(sub-series f (get-current-series)))
	([f s]
		(add-new-current-series)
		(add-data-points (filter f (get-data-points s))))
)

(defn split-series
	"Splits a series in up to two series based on some predicate or function.
	If the resultant points all fit into one series, nothing happens."
	([f]
		(split-series (get-current-series) f))
	([s f]
		(if (not= (get-series s) nil)
			(let [points (get-data-points s) p1 (filter f points) p2 (remove f points)]
				(set-current-series s)
				(if (and (> (count p1) 0) (> (count p2) 0)) (do
					(set-data-points p1)
					(add-new-current-series)
					(add-data-points p2)
					(set-current-series s))))))
)

(defn axis-split-series
	"Splits the current series along all axes."
	([] (axis-split-series (get-current-series)))
	([s]
		(let [series-id (get-series-id s)]
			(split-series series-id #(>= (.z %) 0))
			(split-series series-id #(>= (.y %) 0))
			(split-series series-id #(>= (.x %) 0))
			(split-series (+ series-id 2) #(>= (.x %) 0))
			(split-series (+ series-id 1) #(>= (.y %) 0))
			(split-series (+ series-id 1) #(>= (.x %) 0))
			(split-series (+ series-id 5) #(>= (.x %) 0))
			)
	)
)

(defn filter-series
	"Removes all points that dont fit the given function."
	([f]
		(filter-series (get-current-series) f))
	([s f]
		(if (not= (get-series s) nil)
			(let [points (filter f (get-data-points s))]
				(set-current-series s)
				(set-data-points points))))
)

(defn remove-series
	"Removes all points that fit the given function."
	([f]
		(remove-series (get-current-series) f))
	([s f]
		(if (not= (get-series s) nil)
			(let [points (remove f (get-data-points s))]
				(set-current-series s)
				(set-data-points points))))
)