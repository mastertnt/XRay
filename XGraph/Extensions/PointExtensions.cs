using System;
using System.Collections.Generic;
using System.Windows;

namespace XGraph.Extensions
{
    /// <summary>
    /// Class extending the <see cref="Point"/> class.
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>
        /// Computes the shortest path from start to end.
        /// </summary>
        /// <param name="pStart">The start point.</param>
        /// <param name="pEnd">The end point.</param>
        /// <param name="The grid">The grid to consider.</param>
        /// <returns></returns>
        static public List<Point> GetShortestPath(this Point pStart, Point pEnd)
        {
            List<Point> lResult = new List<Point>();

            // First, look for the max between with and height.
            Double lHeight = Math.Abs(pEnd.Y - pStart.Y);
            Double lWidth = Math.Abs(pEnd.X - pStart.X);
            if
                (lWidth > lHeight)
            {
                if (pStart.X > pEnd.X)
                {
                    Double lNewX = pEnd.X + lWidth / 2.0;
                    lResult.Add(pStart);
                    lResult.Add(new Point(lNewX, pStart.Y));
                    lResult.Add(new Point(lNewX, pEnd.Y));
                    lResult.Add(pEnd);
                }
                else
                {
                    Double lNewX = pStart.X + lWidth / 2.0;
                    lResult.Add(pStart);
                    lResult.Add(new Point(lNewX, pStart.Y));
                    lResult.Add(new Point(lNewX, pEnd.Y));
                    lResult.Add(pEnd);
                }
            }
            else
            {
                if (pStart.Y > pEnd.Y)
                {
                    Double lNewY = pEnd.Y + lHeight / 2.0;
                    lResult.Add(pStart);
                    lResult.Add(new Point(pStart.X, lNewY));
                    lResult.Add(new Point(pEnd.X, lNewY));
                    lResult.Add(pEnd);
                }
                else
                {
                    Double lNewY = pStart.Y + lHeight / 2.0;
                    lResult.Add(pStart);
                    lResult.Add(new Point(pStart.X, lNewY));
                    lResult.Add(new Point(pEnd.X, lNewY));
                    lResult.Add(pEnd);
                }
            }

            return lResult;
        }
    }
}

