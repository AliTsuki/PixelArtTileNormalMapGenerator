# Pixel Art Tile Normal Map Generator
Pixel Art Tile Normal Map Generator is a standalone application designed to convert a pixel art tilemap image to a corresponding normal map image for use with 3D style lighting in a game engine.

It is currently set up to work on two or three tone images, but there are plans for future versions to use more colors in the process.

Designed to be built in Microsoft Visual Studio using .NET Framework 4.7.1.

Pre built executable located under Releases tab.

## Instructions:
1. Click "Load Image" and select an image file (Preferably a PNG texture image file of 1024x1024 or less pixels).
2. Click "Select Background Color" and use the color seleciton tool to select the background color. For best results click "Define Custom Colors >>>" and type in the exact RGB values (0-255) and press OK.
   1. The background color is which color in your tilemap you wish to appear flat in the normal map. For example if your tilemap is raised stones set in a flat background.
3. (Optional) Click "Select Separator Color" and use the color seleciton tool to select the separator color. For best results click "Define Custom Colors >>>" and type in the exact RGB values (0-255) and press OK.
   1. The separator color is which color in your tilemap you are using to separate the raised parts of your image from the flat parts of your image. It is optional as it will work with only a background color and individual color if your image is two tone. For example if you use a darker color border to define the shape of the raised parts of your image.
4. Click "Select Individual Color" and use the color seleciton tool to select the individual color. For best results click "Define Custom Colors >>>" and type in the exact RGB values (0-255) and press OK.
   1. The individual color is which color in your tilemap you wish to appear raised up and convex in the normal map. For example the individual color would be the color of any raised stones in the tilemap that you want to appear round.
5. Beneath each color select is a number box that represents the maximum color difference to check for each color. If your image has any noise and for example your background color should be the specified color and any color within a specific margin you would select that margin here. The calculation is an integer that is the sum of the absolute values minus the sum of the selected values.   
   1. For example the color (255, 250, 250) is a margin of 10 total away from (255, 255, 255).
6. Click "Generate Normal Map" and the process will begin. It takes 30 seconds to a minute depending on the size of the image. The progress bar at the bottom will inform you of the progress of this operation. When it is complete the normal map image will appear in the picture box to the right side.
7. Click "Save Normal Map" to open a file save dialog and save the normal map image to disk as a PNG image file.

The "Cancel" button can be pressed to stop an in-progress normal map generation.

The "Reset" button can be pressed to reset all values to their defaults.
