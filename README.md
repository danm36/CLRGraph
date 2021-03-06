# CLRGraph
CLRGraph is a C# and Clojure powered plotter and grapher, initially developed by Daniel Masterson as part of an internship at Hull University in 2015.

## Required Dependencies
* ClojureCLR: http://clojure.org/clojureclr
 * This is for the runtime scripting engine.
* OpenTK: http://www.opentk.com/
 * This is for graph rendering.
* NAudio: https://github.com/naudio/NAudio
 * This is to read audio input. 

## Current Features
* Full live scripting capabilities via both a core script file and a REPL. Use the main scripting window to define and set up your graph, then use the REPL to make fine adjustments.
 * Note: Upon running Clojure from the main scripting window, the entire graph will be reset. This will not happen in the REPL.
* Virtual unlimited point count based on your current maximum RAM (Averages 1GB usage for around 2.5 million points)
* Render as lines, triangles or quads based on point edges.
* 2D, 3D Orthographic and 3D Perspective rendering modes.
* Import arbitrary data, including [upcoming] live sensor data in real time.
 * Using the Data Sources system, data can be 'latent' and only streamed in when requested, rather than loaded in all at once. Whatever method used depends on the data source type.
* Generate convex hulls from a given point set and [upcoming] export them to 3D model and printer formats.
* A wide array of point, series and graph parsing functions.
* Create new clojure functions in C# via reflection.
* Send data to CLRGraph over a TCP network connection.
 * Current in testing via Tom's Erosion Patterns
* Animated graphing (From sensor data or large data sets like audio).

## Upcoming Features
* Create data importers and exported via plugins.
* More graph functions.
* Image representation.
* Improved hull generation (Preventing internal triangles).
* Improved polygon rendering speed.
* Concave hull generation utilizing the convex hull generation.
