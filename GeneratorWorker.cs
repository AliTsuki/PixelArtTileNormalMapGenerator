using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using nmg = PixelArtTileNormalMapGenerator.NormalMapGeneratorForm;

namespace PixelArtTileNormalMapGenerator
{
    /// <summary>
    /// Class run in the background as a Task that analyzes the image and creates a normal map from the image.
    /// </summary>
    public static class GeneratorWorker
    {
        // Progress data
        private static int CurrentProgress = 0;
        private static int MaximumPixelsToCheck = 0;
        // Color type enum
        private enum ColorType
        {
            Background,
            Separator,
            Individual
        }

        /// <summary>
        /// Creates a normal map from a loaded bitmap image.
        /// </summary>
        /// <param name="nmgf">Reference to the parent form.</param>
        /// <param name="cToken">Reference to the cancellation token.</param>
        /// <param name="progressLabelText">Reference to progress label.</param>
        /// <param name="progressBarValue">Reference to progress bar.</param>
        /// <param name="progressLabelDetailText">Reference to progress detail label.</param>
        public static void CreateNormalMap(IProgress<string> progressLabelText, IProgress<int> progressBarValue, IProgress<string> progressLabelDetailText)
        {
            CurrentProgress = 0;
            MaximumPixelsToCheck = (nmg.ImageWidth * nmg.ImageHeight) + 1;
            progressLabelText.Report("In Progress...");
            try
            {
                CurrentProgress = 1;
                for(int x = 0; x < nmg.ImageWidth; x++)
                {
                    for(int y = 0; y < nmg.ImageHeight; y++)
                    {
                        nmg.cToken.ThrowIfCancellationRequested();
                        Color currentPixelColor = nmg.OriginalImageBitmap.GetPixel(x, y);
                        if(IsColorWithinColorDistance(currentPixelColor, ColorType.Background))
                        {
                            nmg.NormalMapImageBitmap.SetPixel(x, y, nmg.DefaultNormalMapBGColor);
                        }
                        else if(IsColorWithinColorDistance(currentPixelColor, ColorType.Separator))
                        {
                            nmg.NormalMapImageBitmap.SetPixel(x, y, nmg.DefaultNormalMapBGColor);
                        }
                        else if(IsColorWithinColorDistance(currentPixelColor, ColorType.Individual))
                        {
                            if(HasPixelAlreadyBeenAdded(x, y) == false)
                            {
                                ConvexObject co = new ConvexObject();
                                FloodFill(co, x, y);
                                co.CalculateBounds(nmg.ImageWidth, nmg.ImageHeight);
                                AddToTile(co);
                            }
                        }
                        CurrentProgress++;
                    }
                    progressBarValue.Report(CurrentProgress);
                    float percent = (float)CurrentProgress / (float)MaximumPixelsToCheck * 100f;
                    progressLabelDetailText.Report($@"{percent.ToString("00.00")}%  --- {CurrentProgress.ToString("0,0")} / {MaximumPixelsToCheck.ToString("0,0")}");
                }
                foreach(KeyValuePair<Vector2Int, Tile> tile in nmg.Tiles)
                {
                    foreach(ConvexObject co in tile.Value.ConvexObjects)
                    {
                        CreateNormalMapForConvexObject(co);
                    }
                }
                progressLabelText.Report("Finished");
                nmg.CreatingNormalMap = false;
            }
            catch(OperationCanceledException)
            {
                MessageBox.Show($@"Operation cancelled!{Environment.NewLine}");
                nmg.CreatingNormalMap = false;
                progressLabelText.Report("Stopped");
            }
            catch(Exception e)
            {
                MessageBox.Show($@"Error creating Normal Map!{Environment.NewLine}{e.ToString()}");
                Console.WriteLine(e.ToString());
                nmg.CreatingNormalMap = false;
                progressLabelText.Report("Error!");
            }
        }

        /// <summary>
        /// Determines which tile should be parent to this object based on its center, then checks if a parent tile exists at that location, if it does then it adds
        /// this object to that tile, if not then it creates a new tile at that position and adds this object to the tile.
        /// </summary>
        /// <param name="co">The Convex Object to add to a Tile.</param>
        private static void AddToTile(ConvexObject co)
        {
            Vector2Int tilePos = co.Center.ConvertToTilePosition();
            if(nmg.Tiles.ContainsKey(tilePos))
            {
                nmg.Tiles[tilePos].AddObject(co);
            }
            else
            {
                nmg.Tiles.Add(tilePos, new Tile(tilePos));
                nmg.Tiles[tilePos].AddObject(co);
            }
        }

        /// <summary>
        /// Checks if a pixel coordinate has already been added to a Convex Object.
        /// </summary>
        /// <param name="x">X coordinate of pixel.</param>
        /// <param name="y">Y coordinate of pixel.</param>
        /// <returns>Returns true if pixel coords have already been added to a Convex Object.</returns>
        private static bool HasPixelAlreadyBeenAdded(int x, int y)
        {
            Vector2Int xy = new Vector2Int(x, y);
            Vector2Int tilePos = new Vector2Int(x, y).ConvertToTilePosition();
            foreach(KeyValuePair<Vector2Int, Tile> tile in nmg.Tiles)
            {
                if(tilePos.IsWithinDistance(tile.Key, 2))
                {
                    foreach(ConvexObject co in tile.Value.ConvexObjects)
                    {
                        if(co.DoesPixelExistInThisObject(xy) == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a pixel coordinate is within the bounds of the original image.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        /// <returns>Returns true if pixel coordinates are within the bounds of the original image.</returns>
        private static bool IsPixelWithinBounds(Vector2Int xy)
        {
            if(xy.X >= 0 && xy.X < nmg.ImageWidth && xy.Y >= 0 && xy.Y < nmg.ImageHeight)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks the difference between two colors as an integer.
        /// </summary>
        /// <param name="currentColor">The color to check.</param>
        /// <param name="colorToCheckAgainst">The color to measure against.</param>
        /// <returns>Returns an integer denoting the amount of difference between 2 colors. Increasing values mean increasingly different.</returns>
        private static int GetColorDistance(Color currentColor, Color colorToCheckAgainst)
        {
            int distance = Math.Abs(currentColor.R - colorToCheckAgainst.R) + Math.Abs(currentColor.G - colorToCheckAgainst.G) + Math.Abs(currentColor.B - colorToCheckAgainst.B) + Math.Abs(currentColor.A - colorToCheckAgainst.A);
            return distance;
        }

        /// <summary>
        /// Checks if a specified pixel, by coordinate, has a color that is within the specified distance of a given color type.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        /// <param name="colorType">Color type: Separator, Individual</param>
        /// <returns>Returns true if the pixel's color is within the maximum allowed color difference.</returns>
        private static bool IsPixelColorWithinColorDistance(Vector2Int xy, ColorType colorType)
        {
            if(colorType == ColorType.Background)
            {
                if(GetColorDistance(nmg.OriginalImageBitmap.GetPixel(xy.X, xy.Y), nmg.BackgroundColor) <= nmg.BackgroundColorMaxDifference)
                {
                    return true;
                }
            }
            else if(colorType == ColorType.Separator)
            {
                if(GetColorDistance(nmg.OriginalImageBitmap.GetPixel(xy.X, xy.Y), nmg.SeparatorColor) <= nmg.SeparatorColorMaxDifference)
                {
                    return true;
                }
            }
            else if(colorType == ColorType.Individual)
            {
                if(GetColorDistance(nmg.OriginalImageBitmap.GetPixel(xy.X, xy.Y), nmg.IndividualColor) <= nmg.IndividualColorMaxDifference)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if given color is within maximum color difference.
        /// </summary>
        /// <param name="color">The color to check.</param>
        /// <param name="colorType">The color type to use maximum difference for.</param>
        /// <returns>Returns true if the color is within max difference allowed.</returns>
        private static bool IsColorWithinColorDistance(Color color, ColorType colorType)
        {
            if(colorType == ColorType.Background)
            {
                if(GetColorDistance(color, nmg.BackgroundColor) <= nmg.BackgroundColorMaxDifference)
                {
                    return true;
                }
            }
            else if(colorType == ColorType.Separator)
            {
                if(GetColorDistance(color, nmg.SeparatorColor) <= nmg.SeparatorColorMaxDifference)
                {
                    return true;
                }
            }
            else if(colorType == ColorType.Individual)
            {
                if(GetColorDistance(color, nmg.IndividualColor) <= nmg.IndividualColorMaxDifference)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds pixels adjacent to the one specified to the list of pixels needing to be checked if they meet the criteria. Used by flood fill.
        /// </summary>
        /// <param name="co">Convex Object the parent pixel belongs to.</param>
        /// <param name="x">X coordinate of pixel.</param>
        /// <param name="y">Y coordinate of pixel.</param>
        private static void CheckAdjacentPixels(ConvexObject co, int x, int y)
        {
            Vector2Int xy = new Vector2Int(x, y);
            co.AddNewIndividualPixel(xy);
            co.AddPixelAlreadyChecked(xy);
            Vector2Int[] adjacentPixelCoords = new Vector2Int[4]
            {
                new Vector2Int(x + 1, y),
                new Vector2Int(x - 1, y),
                new Vector2Int(x, y + 1),
                new Vector2Int(x, y - 1)
            };
            for(int i = 0; i < adjacentPixelCoords.Length; i++)
            {
                if(IsPixelWithinBounds(adjacentPixelCoords[i]) && co.PixelsAlreadyChecked.Contains(adjacentPixelCoords[i]) == false && co.PixelsToCheck.Contains(adjacentPixelCoords[i]) == false)
                {
                    if(IsPixelColorWithinColorDistance(adjacentPixelCoords[i], ColorType.Individual))
                    {
                        co.AddPixelToCheck(adjacentPixelCoords[i]);
                    }
                    else if(IsPixelWithinBounds(adjacentPixelCoords[i]) && IsPixelColorWithinColorDistance(adjacentPixelCoords[i], ColorType.Separator))
                    {
                        co.AddNewEdgePixel(adjacentPixelCoords[i]);
                        co.AddPixelAlreadyChecked(adjacentPixelCoords[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Runs the check adjacent pixels method on each pixel that needs to be checked. Works recursively to find all adjacent pixels for a convex object.
        /// </summary>
        /// <param name="co">Convex Object the parent pixel belongs to.</param>
        private static void FloodFill(ConvexObject co, int x, int y)
        {
            CheckAdjacentPixels(co, x, y);
            while(co.PixelsToCheck.Count > 0)
            {
                List<Vector2Int> currentPixelsToCheck = co.PixelsToCheck.ToList();
                co.ClearPixelsToCheck();
                foreach(Vector2Int pixel in currentPixelsToCheck)
                {
                    CheckAdjacentPixels(co, pixel.X, pixel.Y);
                }
            }
            co.ClearPixelsAlreadyChecked();
        }

        /// <summary>
        /// Loops through all pixels in CO and gets a UV coordinate for each, then calls SampleDefaultUV to get a final color to set this pixel on the bitmap.
        /// </summary>
        /// <param name="co">Convex Object to create a normal map for.</param>
        private static void CreateNormalMapForConvexObject(ConvexObject co)
        {
            List<UVPixel> uvPixelsEdge = new List<UVPixel>();
            List<UVPixel> uvPixelsInd = new List<UVPixel>();
            foreach(Vector2Int pixel in co.EdgePixels)
            {
                Vector2Int difference = pixel - co.Center;
                Vector2 uv = new Vector2(difference).ToUV();
                uvPixelsEdge.Add(new UVPixel(pixel, uv));
            }
            foreach(Vector2Int iPixel in co.IndividualPixels)
            {
                Vector2 interpolatedUV = new Vector2(0, 0);
                float sumOfWeights = 0f;
                foreach(UVPixel uvPixel in uvPixelsEdge)
                {
                    float weight = 1f / iPixel.Distance(uvPixel.PixelCoords);
                    interpolatedUV += uvPixel.UVCoords * weight;
                    sumOfWeights += weight;
                }
                interpolatedUV /= sumOfWeights;
                uvPixelsInd.Add(new UVPixel(iPixel, interpolatedUV));
            }
            List<UVPixel> allUVPixels = uvPixelsEdge.Concat(uvPixelsInd).ToList();
            foreach(UVPixel uvPixel in allUVPixels)
            {
                nmg.NormalMapImageBitmap.SetPixel(uvPixel.PixelCoords.X, uvPixel.PixelCoords.Y, SampleDefaultUV(co, uvPixel.UVCoords));
            }
        }

        /// <summary>
        /// Gets an averaged color based on the scale for this UV coord using the Default Normal Map image.
        /// </summary>
        /// <param name="co">Convex object from which to get the scale info from.</param>
        /// <param name="uvCoords">UV coordinates to sample from the Default Normal Map image.</param>
        /// <returns></returns>
        private static Color SampleDefaultUV(ConvexObject co, Vector2 uvCoords)
        {
            Vector2Int pixel = new Vector2Int(uvCoords * nmg.DefaultNormalMapImageSize);
            Vector2Int scale = new Vector2Int(co.ScaleVsDefault / 2f);
            List<Vector4Int> colorsToAverage = new List<Vector4Int>();
            for(int x = pixel.X - scale.X; x < pixel.X + scale.X; x++)
            {
                for(int y = pixel.Y - scale.Y; y < pixel.Y + scale.Y; y++)
                {
                    if(x >= 0 && x < nmg.DefaultNormalMapImageSize && y >= 0 && y < nmg.DefaultNormalMapImageSize)
                    {
                        colorsToAverage.Add(new Vector4Int(nmg.DefaultNormalMapImageBitmap.GetPixel(x, y)));
                    }
                }
            }
            Vector4Int avgColor = new Vector4Int(0, 0, 0, 0);
            foreach(Vector4Int color in colorsToAverage)
            {
                avgColor += color;
            }
            if(colorsToAverage.Count > 0)
            {
                avgColor /= colorsToAverage.Count;
            }
            else
            {
                Console.WriteLine($@"CO at center: {co.Center} does not have any colors to average.");
            }
            Color finalColor = avgColor.ToColor();
            return finalColor;
        }
    }
}
