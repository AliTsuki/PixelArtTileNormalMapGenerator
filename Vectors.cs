using System;

namespace PixelArtTileNormalMapGenerator
{
    /// <summary>
    /// Struct representing 2 integers: X, Y. Used for pixel coordinates.
    /// </summary>
    public struct Vector2Int
    {
        // X, Y
        public int X { get; private set; }
        public int Y { get; private set; }

        /// <summary>
        /// Constructor using two integers.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Vector2Int(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructor using a Vector2. Rounds float values to int.
        /// </summary>
        /// <param name="xy">Vector2 coordinates.</param>
        public Vector2Int(Vector2 xy)
        {
            this.X = (int)Math.Round(xy.X);
            this.Y = (int)Math.Round(xy.Y);
        }

        /// <summary>
        /// Adds two Vector2Int.
        /// </summary>
        /// <param name="first">First Vector2Int.</param>
        /// <param name="second">Second Vector2Int.</param>
        /// <returns>Returns the sum of the two.</returns>
        public static Vector2Int operator + (Vector2Int first, Vector2Int second)
        {
            return new Vector2Int(first.X + second.X, first.Y + second.Y);
        }

        /// <summary>
        /// Subtracts a Vector2Int from another.
        /// </summary>
        /// <param name="first">Vector2Int to subtract from.</param>
        /// <param name="second">Vector2Int amount to subtract.</param>
        /// <returns>Returns the difference of the two.</returns>
        public static Vector2Int operator - (Vector2Int first, Vector2Int second)
        {
            return new Vector2Int(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Multiplies a Vector2Int by an int.
        /// </summary>
        /// <param name="xy">Vector2Int to multiply.</param>
        /// <param name="m">Int amount to multiply by.</param>
        /// <returns>Returns the product of the two.</returns>
        public static Vector2Int operator * (Vector2Int xy, int m)
        {
            return new Vector2Int(xy.X * m, xy.Y * m);
        }

        /// <summary>
        /// Divides a Vector2Int by an int.
        /// </summary>
        /// <param name="xy">Vector2Int to divide from.</param>
        /// <param name="d">Int to divide by.</param>
        /// <returns>Returns the quotient of the two.</returns>
        public static Vector2Int operator / (Vector2Int xy, int d)
        {
            return new Vector2Int(xy.X / d, xy.Y / d);
        }

        /// <summary>
        /// Returns a string with the X and Y values.
        /// </summary>
        /// <returns>Returns a string with the X and Y values.</returns>
        public override string ToString()
        {
            return $@"(X:{this.X}, Y:{this.Y})";
        }

        /// <summary>
        /// Gets the distance between this Vector2Int and the given Vector2Int.
        /// </summary>
        /// <param name="xy">The Vector2Int to check against.</param>
        /// <returns>Returns a float representing the distance between the two Vector2Int.</returns>
        public static float Distance(Vector2Int first, Vector2Int second)
        {
            return (float)Math.Sqrt(((first.X - second.X) * (first.X - second.X)) + ((first.Y - second.Y) * (first.Y - second.Y)));
        }

        /// <summary>
        /// Checks if this Vector2 is within distance parameter of Vector2 parameter.
        /// </summary>
        /// <param name="xy">The Vector2 to check against.</param>
        /// <param name="distance">The maximum distance allowed between these 2 Vector2.</param>
        /// <returns></returns>
        public bool IsWithinDistance(Vector2Int xy, int distance)
        {
            if((((this.X - xy.X) * (this.X - xy.X)) + ((this.Y - xy.Y) * (this.Y - xy.Y))) < distance * distance)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Divides these coordinates by TileSize to get tile coordinates.
        /// </summary>
        /// <returns>Returns a new Vector2 corresponding to the tile coordinates the original coordinates belong to.</returns>
        public Vector2Int ToTilePosition()
        {
            return new Vector2Int((int)Math.Floor((float)this.X / (float)Tile.TileSize), (int)Math.Floor((float)this.Y / (float)Tile.TileSize));
        }
    }

    /// <summary>
    /// Struct representing two floats: X, Y. Used for UV coordinates.
    /// </summary>
    public struct Vector2
    {
        // X, Y
        public float X { get; private set; }
        public float Y { get; private set; }

        /// <summary>
        /// Constructor using two floats.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructor using a Vector2Int.
        /// </summary>
        /// <param name="xy">Vector2Int input.</param>
        public Vector2(Vector2Int xy)
        {
            this.X = (float)xy.X;
            this.Y = (float)xy.Y;
        }

        /// <summary>
        /// Adds two Vector2.
        /// </summary>
        /// <param name="first">First Vector2 to add.</param>
        /// <param name="second">Second Vector2 to add.</param>
        /// <returns>Returns the sum of the two.</returns>
        public static Vector2 operator + (Vector2 first, Vector2 second)
        {
            return new Vector2(first.X + second.X, first.Y + second.Y);
        }

        /// <summary>
        /// Subtracts a Vector2 from another.
        /// </summary>
        /// <param name="first">Vector2 to subtract from.</param>
        /// <param name="second">Vector2 amount to subtract.</param>
        /// <returns>Returns the difference of the two.</returns>
        public static Vector2 operator - (Vector2 first, Vector2 second)
        {
            return new Vector2(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Multiplies a Vector2 by a float.
        /// </summary>
        /// <param name="xy">Vector2 to multiply.</param>
        /// <param name="m">Float amount to multiply by.</param>
        /// <returns>Returns the product of the two.</returns>
        public static Vector2 operator * (Vector2 xy, float m)
        {
            return new Vector2(xy.X * m, xy.Y * m);
        }

        /// <summary>
        /// Divides a Vector2 by a float.
        /// </summary>
        /// <param name="xy">Vector2 to divide from.</param>
        /// <param name="d">Float amount to divide by.</param>
        /// <returns>Returns the quotient of the two.</returns>
        public static Vector2 operator / (Vector2 xy, float d)
        {
            return new Vector2(xy.X / d, xy.Y / d);
        }

        /// <summary>
        /// Returns a string with the X and Y values.
        /// </summary>
        /// <returns>Returns a string with the X and Y values.</returns>
        public override string ToString()
        {
            return $@"(X:{this.X}, Y:{this.Y})";
        }

        /// <summary>
        /// Gets the distance between two Vector2.
        /// </summary>
        /// <param name="first">First Vector2.</param>
        /// <param name="second">Second Vector2.</param>
        /// <returns>Returns a float representing the distance between the two Vector2.</returns>
        public static float Distance(Vector2 first, Vector2 second)
        {
            return (float)Math.Sqrt(((first.X - second.X) * (first.X - second.X)) + ((first.Y - second.Y) * (first.Y - second.Y)));
        }

        /// <summary>
        /// Transforms a given difference from center pixel coordinate into a UV coordinate around the outside edge of UV image. Used for edge pixels.
        /// </summary>
        /// <returns>Returns a Vector2 where both X and Y are between 0 and 1.</returns>
        public Vector2 ToUVEdge()
        {
            float tempX = this.X;
            float tempY = this.Y;
            float divisor = 0f;
            if(Math.Abs(tempX) >= Math.Abs(tempY))
            {
                divisor = tempX;
                tempX /= Math.Abs(divisor);
                tempY /= Math.Abs(divisor);
            }
            else
            {
                divisor = tempY;
                tempX /= Math.Abs(divisor);
                tempY /= Math.Abs(divisor);
            }
            tempX += 1f;
            tempY += 1f;
            tempX /= 2f;
            tempY /= 2f;
            return new Vector2(tempX, tempY);
        }

        /// <summary>
        /// Transforms a given difference from center pixel coordinate into a UV coordinate. For individual pixels that have no edge pixels.
        /// </summary>
        /// <param name="width">The width of the parent CO of this pixel.</param>
        /// <param name="height">The height of the parent CO of this pixel.</param>
        /// <returns>Returns a Vector2 where both X and Y are between 0 and 1.</returns>
        public Vector2 ToUVInd(int width, int height)
        {
            float tempX = this.X;
            float tempY = this.Y;
            tempX = (((float)width / 2f) + this.X) / (float)width;
            tempY = (((float)height / 2f) + this.Y) / (float)height;
            return new Vector2(tempX, tempY);
        }
    }
}
