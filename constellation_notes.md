This file details the methods used for creating constellation outlines in the star view.

The Unity LineRenderer is used to draw the lines between star locations in the main scene. LineRenderer draws a single line between an arbitrary number of vertices.

In order to tell the constellation LineRenderer which lines to draw for each constellation, two CSV files are used.
The first contains data on the stars that make up a given constellation outline, with unique integer star ID numbers.
The second contains list contains all the node sequences required to draw the constellation, written as the sequence of star ID's seperated by dashes. For example, one of the lines that makes up Orion has the star ID sequence 6-5-2-8-3-7-1.

The DrawConstellations script reads the star ID sequences from the second file, and finds the corresponding RA/DEC values for all the nodes in that line. When the end of the sequence has been reached, the lines is drawn using the constellation_line prefab.
Note that because constellation outlines contain cycles and branches, multiple star ID sequences may be needed to fully render a constellation.

Also note that the star RA/DEC values must be entered manually. This is becuase our scene uses distance from the camera to simulate star brightness, so if the constellation lines are drawn directly between instantiated star objects, the outlines will not be a constant width.
Though this requires the manual entry of redundant star information, it is required for the desired look and feel of the outlines. Fortunately, there are only a few constellations, and each is only made of a few select stars.
