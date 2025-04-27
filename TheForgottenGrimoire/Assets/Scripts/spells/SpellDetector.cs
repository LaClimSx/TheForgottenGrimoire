using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace spells
{
    public class SpellDetector
    {
        
        public enum SpellType 
        {
            Lightning,
            Spiral,
            Square,
            Infinity,
            Triangle
        }

        
        
        /// <summary>
        /// Detects the type of spell based on a sequence of 3D points and the headset position.
        /// </summary>
        /// <param name="listPoints">A list of <see cref="Vector3"/> points representing the motion path of the spell in 3D space.</param>
        /// <param name="headSetPosition">The position of the headset at the time the spell was cast, used for spatial context.</param>
        /// <param name="onDetect">
        /// A callback invoked when the spell is detected.
        /// The first parameter is an integer representing the spell type:
        /// 0 = Lightning, 1 = Spiral, 2 = Square, 3 = Space, 4 = Fire.
        /// The second parameter is a double between 0 and 1 representing how well the spell was cast (higher is better).
        /// </param>
        public void DetectSpell(List<Vector3> listPoints, Vector3 headSetPosition, Action<SpellType, double> onDetect)
        {
            
            int n = listPoints.Count;
        
            // We need at least 10 points to do a correct spell.
            if (n < 10)
            {
                Debug.LogWarning("Not enough points to detect spell");
                return;
            }

            // Approximation of mean of draw to project.
            float meanX = (listPoints[0].x + listPoints[1 / n].x + listPoints[2 / n].x + listPoints[3 / n].x) / 4.0f;
            float meanZ = (listPoints[0].z + listPoints[1 / n].z + listPoints[2 / n].z + listPoints[3 / n].z) / 4.0f;
            
            Vector2 direction = new Vector2(meanX, meanZ) - new Vector2(headSetPosition.x, headSetPosition.z);
            if (direction.magnitude < 1e-5)
            {
                Debug.LogWarning("Headset and mean points too close");
                return;
            }
            
            Vector2 normalizedDirection = direction.normalized;
            Vector2 rightAngleDirection = new Vector2(normalizedDirection.y, -normalizedDirection.x);
            
            // Arrays of x and y coordinates
            double[] xs = new double[n];
            double[] ys = new double[n];
            
            // inverse direction to match the shape reference
            ProjectTo2D(listPoints, n, xs, ys, rightAngleDirection);
            
        
            // Find smallest and largest x and y value.
            double minX = xs.Min();
            double maxX = xs.Max();
            double minY = ys.Min();
            double maxY = ys.Max();
            
            
            // Normalize points from 0 to 1. For axis x and y.
            double diffX = maxX - minX;
            double diffY = maxY - minY;
            
        
            // Find p1 the left most (and low) point.
            // The point satisfying the equation p1_index = argmax(-4x-y) for x,y the points in listPoints
            int p1Index = 0;
            double p1MaxValue = double.NegativeInfinity; // Smallest value to be override.
            
            for (int i = 0; i < n; i++)
            {
                // Normalize point
                xs[i] = (xs[i] - minX) / diffX;
                ys[i] = (ys[i] - minY) / diffY;
                
            
                if (-4 * xs[i] - ys[i] > p1MaxValue)
                {
                    p1Index = i;
                    p1MaxValue = -4 * xs[i] - ys[i];
                }
            }
        
            // Find p2 a point after p1 to detect directions.
            int p2Index = (p1Index + n/8) % n;
            
            
            // Check for lightning and wind if first point is above last point
            bool topBottom = ys[0] > ys[n-1];
            
        
            // For earth, space and fire, we check in which direction it goes.
            bool clockwise = xs[p2Index] - xs[p1Index] < 4*(ys[p2Index] - ys[p1Index]);
            
            double[] cumulativeErrorsSquared = {0, 0, 0, 0, 0};
            
            // Loop through all points except the first and last point.
            for (int i = 1; i < n - 1; i++)
            {
                // For each point, we check 5 different shape parametrization.
                // We evaluate each shape by checking three consecutive points: i-1, i, and i+1.

                var minErrorSquaredIn3Points = MinErrorSquaredIn3Points(i, p1Index, n, topBottom, clockwise, xs, ys);

                // After checking the three points, update cumulative error with minimum found error.
                for (int j = 0; j < 5; j++)
                {
                    cumulativeErrorsSquared[j] += minErrorSquaredIn3Points[j];
                }
            }
            
            
            // Calculate the l2_error and keep min index
            double minError = cumulativeErrorsSquared[0];
            int idxMinError = 0;
            for (int i = 0; i < 5; i++)
            {
                // Calculate the L2 score from the sum of squared error
                cumulativeErrorsSquared[i] /= n-2;
                cumulativeErrorsSquared[i] = Math.Sqrt(cumulativeErrorsSquared[i]);
                
                
                // Keep the min index
                if (cumulativeErrorsSquared[i] < minError)
                {
                    idxMinError = i;
                    minError = cumulativeErrorsSquared[i];
                }
            }

            // Create a score given how close the shape is to all references
            double[] softmaxInv = SoftmaxInverse(cumulativeErrorsSquared);
            
            onDetect?.Invoke((SpellType)idxMinError, softmaxInv[idxMinError]);
        }

        private double[] MinErrorSquaredIn3Points(int i, int p1Index, int n, bool topBottom, bool clockwise, double[] xs,
            double[] ys)
        {
            // Initialize minimum squared errors for each shape to positive infinity.
            double[] minErrorSquaredIn3Points = new double[5];
            for (int k = 0; k < 5; k++)
            {
                minErrorSquaredIn3Points[k] = double.PositiveInfinity;
            }

            // Iterate over the three points: i-1, i, and i+1.
            for (int k = -1; k <= 1; k++)
            {
                // Get parametrization for the current point.
                double[] parametrization = getParametrization(i + k, p1Index, n, topBottom, clockwise);
                double tLightningSpiral = parametrization[0];
                double tSquareInfinityTriangle = parametrization[1];


                // Select corresponding t-values for each shape.
                double[] tValues =
                {
                    tLightningSpiral, // Shape 0
                    tLightningSpiral, // Shape 1
                    tSquareInfinityTriangle, // Shape 2
                    tSquareInfinityTriangle, // Shape 3
                    tSquareInfinityTriangle  // Shape 4
                };

                // Evaluate squared error for each shape and keep the minimum over three points.
                for (int j = 0; j < 5; j++)
                {
                    double predictedX = xFunctions[j](tValues[j]);
                    double predictedY = yFunctions[j](tValues[j]);

                    double squaredError = SquaredError(predictedX, predictedY, xs[i], ys[i]);

                    if (squaredError < minErrorSquaredIn3Points[j])
                    {
                        minErrorSquaredIn3Points[j] = squaredError;
                    }
                }
            }


            return minErrorSquaredIn3Points;
        }

        private static void ProjectTo2D(List<Vector3> listPoints, int n, double[] xs, double[] ys, Vector3 direction)
        {
            // TODO change projection to right angle of vector. For now assume value is on coordinate x.
            for (int i = 0; i < n; i++)
            {
                xs[i] = listPoints[i].x;//-listPoints[i].x * direction.x - listPoints[i].z * direction.y;
                ys[i] = listPoints[i].y;
            }
        }

        private double[] getParametrization(int pointNumber, int p1Index, int numberPoint, bool topBottom, bool clockwise)
        {
            double t = (double)pointNumber / (numberPoint - 1);
            double t0 = t - (double)p1Index / numberPoint;


            return new[] { topBottom ? t : 1 - t, clockwise ? (1 + t0) % 1 : (1 - t0) % 1 };
        }

      
        
        Func<double, double>[] xFunctions = {
            LightningX,
            SpiralX,
            SquareX,
            InfinityX,
            TriangleX
        };

        Func<double, double>[] yFunctions = {
            LightningY,
            SpiralY,
            SquareY,
            InfinityY,
            TriangleY
        };

        private static double[] SoftmaxInverse(double[] t, double k=0.6)
        {
            if (t == null || t.Length == 0)
                throw new ArgumentException("Input array 't' must not be null or empty.");

            // Ensure all t >= some small epsilon to avoid division by 0
            double epsilon = 1e-8;
            double[] invT = t.Select(val => k / Math.Max(val, epsilon)).ToArray();

            // Softmax stabilization: subtract max to avoid numerical overflow
            double maxInvT = invT.Max();
            double[] exps = invT.Select(x => Math.Exp(x - maxInvT)).ToArray();
            double sumExps = exps.Sum();

            // Normalize
            double[] softmax = exps.Select(x => x / sumExps).ToArray();
            return softmax;
        }        

        private static double SquaredError(double x1, double y1, double x2, double y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }

        private static double TriangleX(double t)
        {
            //Triangle spans [0, 1] with a peak at x = 2/3
            return t < 2.0f / 3 ? t * 3.0f / 2 : 3 - 3 * t;
        }
        

        private static double TriangleY(double t)
        {
            return t switch
            {
                < 1.0 / 3.0 => 3 * t + 0.06,
                < 2.0 / 3.0 => 2.06 - 3 * t,
                _ => 0.06
            };
        }
    
        private static double SquareX(double t)
        {
            // Square padding
            const double r = 0.02f;
            const double s = 1 - r;
            const double u = 1 - 2 * r;

            double x = t switch
            {
                < 1.0f / 4 => r,
                < 2.0f / 4 => 4*u*t + 2 * r - s,
                < 3.0 / 4 => s,
                _ => -4*u*t + 4 * s - 3 * r
            };

            //Limit to border
            x = Math.Clamp(x, r, s);
        
            return x;
        }
    
        private static double SquareY(double t)
        {
            // Square padding
            const double r = 0.02f;     // 0.02
            const double s = 1 - r;     // 0.98
            const double u = 1 - 2 * r; // 0.96

            double y = t switch
            {
                < 1.0f / 4 => 4*u*t + r,
                < 2.0f / 4 => s,
                < 3.0 / 4 => -4*u*t + 3*s-2*r,
                _ => r
            };
        
            return y;
        }
    

        
        private static double InfinityX(double t)
        {
            /*
            Computes the x-coordinate of the lemniscate of Bernoulli.

            Parameters:
            - t: array-like, parameter ranging from 0 to 2π.
            - a: double, scaling factor for the size of the lemniscate.

            Returns:
            - x: array-like, x-coordinates corresponding to parameter t.
        */

            return (-(Math.Sqrt(2) * Math.Cos(t * 2 * Math.PI)) / (Math.Pow(Math.Sin(t * 2 * Math.PI),2) + 1) + Math.Sqrt(2)) /
                   (2 * Math.Sqrt(2));

        }
        
        
        private static double InfinityY(double t)
        {
            /*
            Computes the y-coordinate of the lemniscate of Bernoulli.

           Parameters:
           - t: array-like, parameter ranging from 0 to 2π.
           - a: double, scaling factor for the size of the lemniscate.

           Returns:
           - y: array-like, y-coordinates corresponding to parameter t.
        */


            return 0.5 + (0.95 * Math.Sqrt(2) * Math.Cos(t * 2 * Math.PI) * Math.Sin(t * 2 * Math.PI)) /
                (Math.Pow(Math.Sin(t * 2 * Math.PI), 2) + 1);
        }

        private static double LightningX(double t)
        {
            return t switch
            {
                < 1.0f / 3 => 0.7 - 2.1 * t,
                < 2.0 / 3 => -1 + 3.0 * t,
                _ => 2 - 1.5 * t
            };
        }

        private static double LightningY(double t)
        {
            return t switch
            {
                < 1.0f / 3 => 1 - 3.0/2*t,
                < 2.0 / 3 => 0.42 + 0.24*t,
                _ => 1.74 - 1.74*t
            };
        }


        private static double SpiralX(double t)
        {
            return ((-(1 - 0.5 * t) * Math.Sin(t * 3 * Math.PI) + 1) - 0.082) / 1.668;
        }
    
        private static double SpiralY(double t)
        {
            return (((1 - 0.5 * t) * Math.Cos(t * 3 * Math.PI) + 1) / 2 - 0.08) / (1 - 0.08);
        }
 
    }
}
