using System.Collections.Generic;
using System.Linq;

namespace PixelArtTileNormalMapGenerator
{
    /// <summary>
    /// Class representing a connected group of pixels of varied shape.
    /// </summary>
    public class ConvexObject
    {
        public List<Vector2Int> EdgePixels { get; private set; }
        public List<Vector2Int> IndividualPixels { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Vector2Int Center { get; private set; }
        public List<Vector2Int> PixelsAlreadyChecked { get; private set; }
        public List<Vector2Int> PixelsToCheck { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConvexObject()
        {
            this.EdgePixels = new List<Vector2Int>();
            this.IndividualPixels = new List<Vector2Int>();
            this.Width = 0;
            this.Height = 0;
            this.Center = new Vector2Int(0, 0);
            this.PixelsAlreadyChecked = new List<Vector2Int>();
            this.PixelsToCheck = new List<Vector2Int>();
        }

        /// <summary>
        /// Adds a set of pixel coordinates to the Edge Pixels list.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        public void AddNewEdgePixel(Vector2Int xy)
        {
            this.EdgePixels.Add(xy);
        }

        /// <summary>
        /// Adds a set of pixel coordinates to the Individual Pixels list.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        public void AddNewIndividualPixel(Vector2Int xy)
        {
            this.IndividualPixels.Add(xy);
        }

        /// <summary>
        /// Adds a set of pixel coordinates to the Pixels To Check list.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        public void AddPixelToCheck(Vector2Int xy)
        {
            this.PixelsToCheck.Add(xy);
        }

        /// <summary>
        /// Adds a set of pixel coordinates to the Pixels Already Checked list.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        public void AddPixelAlreadyChecked(Vector2Int xy)
        {
            this.PixelsAlreadyChecked.Add(xy);
        }

        /// <summary>
        /// Clears the Pixels To Check list.
        /// </summary>
        public void ClearPixelsToCheck()
        {
            this.PixelsToCheck.Clear();
        }

        /// <summary>
        /// Clears the Pixels Already Checked list.
        /// </summary>
        public void ClearPixelsAlreadyChecked()
        {
            this.PixelsAlreadyChecked.Clear();
        }

        /// <summary>
        /// Gets the width, height, center, and scale by iterating through all the pixels in this object.
        /// </summary>
        /// <param name="maxWidth">Maximum width of parent image.</param>
        /// <param name="maxHeight">Maximum height of parent image.</param>
        public void CalculateBounds(int maxWidth, int maxHeight)
        {
            List<Vector2Int> allPixels = this.EdgePixels.Concat(this.IndividualPixels).ToList();
            int rightEdge = 0, lowerEdge = 0, leftEdge = maxWidth, upperEdge = maxHeight;
            foreach(Vector2Int pixel in allPixels)
            {
                int currentPixelX = pixel.X, currentPixelY = pixel.Y;
                if(currentPixelX < leftEdge)
                {
                    leftEdge = currentPixelX;
                }
                if(currentPixelX > rightEdge)
                {
                    rightEdge = currentPixelX;
                }
                if(currentPixelY > lowerEdge)
                {
                    lowerEdge = currentPixelY;
                }
                if(currentPixelY < upperEdge)
                {
                    upperEdge = currentPixelY;
                }
            }
            this.Width = rightEdge - leftEdge;
            this.Height = lowerEdge - upperEdge;
            foreach(Vector2Int pixel in allPixels)
            {
                this.Center += pixel;
            }
            this.Center /= allPixels.Count;
        }

        /// <summary>
        /// Checks if a given set of pixel coordinates has been added to the Individual Pixels list of this object.
        /// </summary>
        /// <param name="xy">The Vector2 coordinates for the pixel.</param>
        /// <returns>Returns true if the specified pixel exists in the list of this object.</returns>
        public bool DoesPixelExistInThisObject(Vector2Int xy)
        {
            if(this.IndividualPixels.Contains(xy))
            {
                return true;
            }
            return false;
        }
    }



    /// <summary>
    /// Class representing one tile on the tile palette. Stores individual Convex Objects.
    /// </summary>
    public class Tile
    {
        // Tile Dimensions
        public const int TileSize = 32;
        // Address of tile
        public Vector2Int Position { get; private set; }
        // Objects within this tiles boundary
        public List<ConvexObject> ConvexObjects { get; private set; } = new List<ConvexObject>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position">Vector2 position of tile on tile palette.</param>
        public Tile(Vector2Int position)
        {
            this.Position = position;
        }

        /// <summary>
        /// Adds a Convex Object to this Tile's Convex Object list.
        /// </summary>
        /// <param name="co">The Convex Object to add.</param>
        public void AddObject(ConvexObject co)
        {
            this.ConvexObjects.Add(co);
        }
    }



    /// <summary>
    /// Class representing a set of Pixel coordinates and UV coordinates.
    /// </summary>
    public class UVPixel
    {
        // Pixel coords and UV coords
        public Vector2Int PixelCoords { get; private set; }
        public Vector2 UVCoords { get; private set; }

        /// <summary>
        /// Constructor using a Vector2Int for pixel coordinates and a Vector2 for uv coordinates.
        /// </summary>
        /// <param name="pixelCoords">Vector2Int pixel coordinates.</param>
        /// <param name="uvCoords">Vector2 uv coordinates.</param>
        public UVPixel(Vector2Int pixelCoords, Vector2 uvCoords)
        {
            this.PixelCoords = pixelCoords;
            this.UVCoords = uvCoords;
        }
    }
}
