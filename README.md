# Fractarium
A simple desktop app that lets you generate fractal images based on a variety of escape-time fractal algorithms, such as the Mandelbrot set, Julia set, Phoenix set and Burning Ship set. I made this a few years ago as an improved version of a university assignment I once had to write in Java.

It uses a far-from-optimal software rendering process based on parallelized for-loops, directly writing into an image buffer provided by the GUI framework. It works with any sensible color palette you throw at it, composable from the GUI.

## How to use

* Left-click on any pixel to zoom in. This is also sets the clicked-on pixel as the new image center. Zoom to your heart's desire until you run out of floating-point precision and the artifacts dominate.
* Right-click to zoom back out.
* Set the values in the parameter tab to change rendering properties to your desire. The midpoint and constant parameters accept complex numbers. The zoom factor determines how much you zoom in at once.
* Click on the colors in the color tab's color previews to quickly select a color and edit it.

## Examples

<p>
  <a href="https://github.com/hannes-harnisch/hannes-harnisch/blob/main/Originals/Fractarium1.png"><img src="https://github.com/hannes-harnisch/hannes-harnisch/blob/main/Fractarium1.jpg" width="49%"></a>
  <a href="https://github.com/hannes-harnisch/hannes-harnisch/blob/main/Originals/Fractarium2.png"><img src="https://github.com/hannes-harnisch/hannes-harnisch/blob/main/Fractarium2.jpg" width="49%"></a>
  <a href="https://github.com/hannes-harnisch/hannes-harnisch/blob/main/Originals/Fractarium3.png"><img src="https://github.com/hannes-harnisch/hannes-harnisch/blob/main/Fractarium3.jpg" width="49%"></a>
  <a href="https://github.com/hannes-harnisch/hannes-harnisch/blob/main/Originals/Fractarium4.png"><img src="https://github.com/hannes-harnisch/hannes-harnisch/blob/main/Fractarium4.jpg" width="49%"></a>
</p>
