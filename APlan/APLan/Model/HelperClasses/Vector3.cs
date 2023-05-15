using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APLan.Model.HelperClasses
{
    //
    // Summary:
    //     Represent a three component vector of double precision.
    public struct Vector3 : IEquatable<Vector3>
    {
        private double x;

        private double y;

        private double z;

        private bool isNormalized;

        //
        // Summary:
        //     Zero vector.
        public static Vector3 Zero => new Vector3(0.0, 0.0, 0.0);

        //
        // Summary:
        //     Unit X vector.
        public static Vector3 UnitX
        {
            get
            {
                Vector3 result = new Vector3(1.0, 0.0, 0.0);
                result.isNormalized = true;
                return result;
            }
        }

        //
        // Summary:
        //     Unit Y vector.
        public static Vector3 UnitY
        {
            get
            {
                Vector3 result = new Vector3(0.0, 1.0, 0.0);
                result.isNormalized = true;
                return result;
            }
        }

        //
        // Summary:
        //     Unit Z vector.
        public static Vector3 UnitZ
        {
            get
            {
                Vector3 result = new Vector3(0.0, 0.0, 1.0);
                result.isNormalized = true;
                return result;
            }
        }

        //
        // Summary:
        //     Represents a vector with not a number components.
        public static Vector3 NaN => new Vector3(double.NaN, double.NaN, double.NaN);

        //
        // Summary:
        //     Gets or sets the X component.
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                isNormalized = false;
            }
        }

        //
        // Summary:
        //     Gets or sets the Y component.
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                isNormalized = false;
            }
        }

        //
        // Summary:
        //     Gets or sets the Z component.
        public double Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
                isNormalized = false;
            }
        }

        //
        // Summary:
        //     Gets or sets a vector element defined by its index.
        //
        // Parameters:
        //   index:
        //     Index of the element.
        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    2 => z,
                    _ => throw new ArgumentOutOfRangeException("index"),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("index");
                }

                isNormalized = false;
            }
        }

        //
        // Summary:
        //     Gets if the vector has been normalized.
        public bool IsNormalized => isNormalized;

        //
        // Summary:
        //     Initializes a new instance of Vector3.
        //
        // Parameters:
        //   value:
        //     X, Y, Z component.
        public Vector3(double value)
        {
            x = value;
            y = value;
            z = value;
            isNormalized = false;
        }

        //
        // Summary:
        //     Initializes a new instance of Vector3.
        //
        // Parameters:
        //   x:
        //     X component.
        //
        //   y:
        //     Y component.
        //
        //   z:
        //     Z component.
        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            isNormalized = false;
        }

        //
        // Summary:
        //     Initializes a new instance of Vector3.
        //
        // Parameters:
        //   array:
        //     Array of three elements that represents the vector.
        public Vector3(double[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (array.Length != 3)
            {
                throw new ArgumentOutOfRangeException("array", array.Length, "The dimension of the array must be three.");
            }

            x = array[0];
            y = array[1];
            z = array[2];
            isNormalized = false;
        }

        //
        // Summary:
        //     Returns a value indicating if any component of the specified vector evaluates
        //     to a value that is not a number System.Double.NaN.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        // Returns:
        //     Returns true if any component of the specified vector evaluates to System.Double.NaN;
        //     otherwise, false.
        public static bool IsNaN(Vector3 u)
        {
            if (!double.IsNaN(u.X) && !double.IsNaN(u.Y))
            {
                return double.IsNaN(u.Z);
            }

            return true;
        }

        //
        // Summary:
        //     Obtains the dot product of two vectors.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     The dot product.
        public static double DotProduct(Vector3 u, Vector3 v)
        {
            return u.X * v.X + u.Y * v.Y + u.Z * v.Z;
        }

        //
        // Summary:
        //     Obtains the cross product of two vectors.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     Vector3.
        public static Vector3 CrossProduct(Vector3 u, Vector3 v)
        {
            double num = u.Y * v.Z - u.Z * v.Y;
            double num2 = u.Z * v.X - u.X * v.Z;
            double num3 = u.X * v.Y - u.Y * v.X;
            return new Vector3(num, num2, num3);
        }

        //
        // Summary:
        //     Obtains the distance between two points.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     Distance.
        public static double Distance(Vector3 u, Vector3 v)
        {
            return Math.Sqrt(SquareDistance(u, v));
        }

        //
        // Summary:
        //     Obtains the square distance between two points.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     Square distance.
        public static double SquareDistance(Vector3 u, Vector3 v)
        {
            return (u.X - v.X) * (u.X - v.X) + (u.Y - v.Y) * (u.Y - v.Y) + (u.Z - v.Z) * (u.Z - v.Z);
        }

        //
        // Summary:
        //     Obtains the angle between two vectors.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     Angle in radians.
        public static double AngleBetween(Vector3 u, Vector3 v)
        {
            double num = DotProduct(u, v) / (u.Modulus() * v.Modulus());
            if (num >= 1.0)
            {
                return 0.0;
            }

            if (num <= -1.0)
            {
                return Math.PI;
            }

            return Math.Acos(num);
        }

        //
        // Summary:
        //     Obtains the midpoint.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     Vector3.
        public static Vector3 MidPoint(Vector3 u, Vector3 v)
        {
            return new Vector3((v.X + u.X) * 0.5, (v.Y + u.Y) * 0.5, (v.Z + u.Z) * 0.5);
        }

        //
        // Summary:
        //     Checks if two vectors are perpendicular.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     True if are perpendicular or false in any other case.
        public static bool ArePerpendicular(Vector3 u, Vector3 v)
        {
            return ArePerpendicular(u, v, MathHelper.Epsilon);
        }

        //
        // Summary:
        //     Checks if two vectors are perpendicular.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        //   threshold:
        //     Tolerance used.
        //
        // Returns:
        //     True if are perpendicular or false in any other case.
        public static bool ArePerpendicular(Vector3 u, Vector3 v, double threshold)
        {
            return MathHelper.IsZero(DotProduct(u, v), threshold);
        }

        //
        // Summary:
        //     Checks if two vectors are parallel.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     True if are parallel or false in any other case.
        public static bool AreParallel(Vector3 u, Vector3 v)
        {
            return AreParallel(u, v, MathHelper.Epsilon);
        }

        //
        // Summary:
        //     Checks if two vectors are parallel.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        //   threshold:
        //     Tolerance used.
        //
        // Returns:
        //     True if are parallel or false in any other case.
        public static bool AreParallel(Vector3 u, Vector3 v, double threshold)
        {
            Vector3 vector = CrossProduct(u, v);
            if (!MathHelper.IsZero(vector.X, threshold))
            {
                return false;
            }

            if (!MathHelper.IsZero(vector.Y, threshold))
            {
                return false;
            }

            if (!MathHelper.IsZero(vector.Z, threshold))
            {
                return false;
            }

            return true;
        }

        //
        // Summary:
        //     Rounds the components of a vector.
        //
        // Parameters:
        //   u:
        //     Vector to round.
        //
        //   numDigits:
        //     Number of decimal places in the return value.
        //
        // Returns:
        //     The rounded vector.
        public static Vector3 Round(Vector3 u, int numDigits)
        {
            return new Vector3(Math.Round(u.X, numDigits), Math.Round(u.Y, numDigits), Math.Round(u.Z, numDigits));
        }

        //
        // Summary:
        //     Normalizes the vector.
        //
        // Parameters:
        //   u:
        //     Vector to normalize
        //
        // Returns:
        //     A normalized vector.
        public static Vector3 Normalize(Vector3 u)
        {
            if (u.isNormalized)
            {
                return u;
            }

            double num = u.Modulus();
            if (MathHelper.IsZero(num))
            {
                return NaN;
            }

            double num2 = 1.0 / num;
            Vector3 result = new Vector3(u.x * num2, u.y * num2, u.z * num2);
            result.isNormalized = true;
            return result;
        }

        //
        // Summary:
        //     Check if the components of two vectors are equal.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     True if the three components are equal or false in any other case.
        public static bool operator ==(Vector3 u, Vector3 v)
        {
            return Equals(u, v);
        }

        //
        // Summary:
        //     Check if the components of two vectors are different.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     True if the three components are different or false in any other case.
        public static bool operator !=(Vector3 u, Vector3 v)
        {
            return !Equals(u, v);
        }

        //
        // Summary:
        //     Adds two vectors.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     The addition of u plus v.
        public static Vector3 operator +(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X + v.X, u.Y + v.Y, u.Z + v.Z);
        }

        //
        // Summary:
        //     Adds two vectors.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     The addition of u plus v.
        public static Vector3 Add(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X + v.X, u.Y + v.Y, u.Z + v.Z);
        }

        //
        // Summary:
        //     Subtracts two vectors.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     The subtraction of u minus v.
        public static Vector3 operator -(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X - v.X, u.Y - v.Y, u.Z - v.Z);
        }

        //
        // Summary:
        //     Subtracts two vectors.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     The subtraction of u minus v.
        public static Vector3 Subtract(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X - v.X, u.Y - v.Y, u.Z - v.Z);
        }

        //
        // Summary:
        //     Negates a vector.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        // Returns:
        //     The negative vector of u.
        public static Vector3 operator -(Vector3 u)
        {
            Vector3 result = new Vector3(0.0 - u.X, 0.0 - u.Y, 0.0 - u.Z);
            result.isNormalized = u.IsNormalized;
            return result;
        }

        //
        // Summary:
        //     Negates a vector.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        // Returns:
        //     The negative vector of u.
        public static Vector3 Negate(Vector3 u)
        {
            Vector3 result = new Vector3(0.0 - u.X, 0.0 - u.Y, 0.0 - u.Z);
            result.isNormalized = u.IsNormalized;
            return result;
        }

        //
        // Summary:
        //     Multiplies a vector with an scalar (same as a*u, commutative property).
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   a:
        //     Scalar.
        //
        // Returns:
        //     The multiplication of u times a.
        public static Vector3 operator *(Vector3 u, double a)
        {
            return new Vector3(u.X * a, u.Y * a, u.Z * a);
        }

        //
        // Summary:
        //     Multiplies a vector with an scalar (same as a*u, commutative property).
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   a:
        //     Scalar.
        //
        // Returns:
        //     The multiplication of u times a.
        public static Vector3 Multiply(Vector3 u, double a)
        {
            return new Vector3(u.X * a, u.Y * a, u.Z * a);
        }

        //
        // Summary:
        //     Multiplies a scalar with a vector (same as u*a, commutative property).
        //
        // Parameters:
        //   a:
        //     Scalar.
        //
        //   u:
        //     Vector3.
        //
        // Returns:
        //     The multiplication of u times a.
        public static Vector3 operator *(double a, Vector3 u)
        {
            return new Vector3(u.X * a, u.Y * a, u.Z * a);
        }

        //
        // Summary:
        //     Multiplies a scalar with a vector (same as u*a, commutative property).
        //
        // Parameters:
        //   a:
        //     Scalar.
        //
        //   u:
        //     Vector3.
        //
        // Returns:
        //     The multiplication of u times a.
        public static Vector3 Multiply(double a, Vector3 u)
        {
            return new Vector3(u.X * a, u.Y * a, u.Z * a);
        }

        //
        // Summary:
        //     Multiplies two vectors component by component.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     The multiplication of u times v.
        public static Vector3 operator *(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X * v.X, u.Y * v.Y, u.Z * v.Z);
        }

        //
        // Summary:
        //     Multiplies two vectors component by component.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     The multiplication of u times v.
        public static Vector3 Multiply(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X * v.X, u.Y * v.Y, u.Z * v.Z);
        }

        //
        // Summary:
        //     Divides an scalar with a vector.
        //
        // Parameters:
        //   a:
        //     Scalar.
        //
        //   u:
        //     Vector3.
        //
        // Returns:
        //     The division of u times a.
        public static Vector3 operator /(Vector3 u, double a)
        {
            double num = 1.0 / a;
            return new Vector3(u.X * num, u.Y * num, u.Z * num);
        }

        //
        // Summary:
        //     Divides a vector with an scalar.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   a:
        //     Scalar.
        //
        // Returns:
        //     The division of u times a.
        public static Vector3 Divide(Vector3 u, double a)
        {
            double num = 1.0 / a;
            return new Vector3(u.X * num, u.Y * num, u.Z * num);
        }

        //
        // Summary:
        //     Divides two vectors component by component.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     The multiplication of u times v.
        public static Vector3 operator /(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X / v.X, u.Y / v.Y, u.Z / v.Z);
        }

        //
        // Summary:
        //     Divides two vectors component by component.
        //
        // Parameters:
        //   u:
        //     Vector3.
        //
        //   v:
        //     Vector3.
        //
        // Returns:
        //     The multiplication of u times v.
        public static Vector3 Divide(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X / v.X, u.Y / v.Y, u.Z / v.Z);
        }

        //
        // Summary:
        //     Normalizes the current vector.
        public void Normalize()
        {
            if (!isNormalized)
            {
                double num = Modulus();
                if (MathHelper.IsZero(num))
                {
                    this = NaN;
                }
                else
                {
                    double num2 = 1.0 / num;
                    x *= num2;
                    y *= num2;
                    z *= num2;
                }

                isNormalized = true;
            }
        }

        //
        // Summary:
        //     Obtains the modulus of the vector.
        //
        // Returns:
        //     Vector modulus.
        public double Modulus()
        {
            if (isNormalized)
            {
                return 1.0;
            }

            return Math.Sqrt(DotProduct(this, this));
        }

        //
        // Summary:
        //     Returns an array that represents the vector.
        //
        // Returns:
        //     Array.
        public double[] ToArray()
        {
            return new double[3] { x, y, z };
        }

        //
        // Summary:
        //     Check if the components of two vectors are approximate equal.
        //
        // Parameters:
        //   a:
        //     Vector3.
        //
        //   b:
        //     Vector3.
        //
        // Returns:
        //     True if the three components are almost equal or false in any other case.
        public static bool Equals(Vector3 a, Vector3 b)
        {
            return a.Equals(b, MathHelper.Epsilon);
        }

        //
        // Summary:
        //     Check if the components of two vectors are approximate equal.
        //
        // Parameters:
        //   a:
        //     Vector3.
        //
        //   b:
        //     Vector3.
        //
        //   threshold:
        //     Maximum tolerance.
        //
        // Returns:
        //     True if the three components are almost equal or false in any other case.
        public static bool Equals(Vector3 a, Vector3 b, double threshold)
        {
            return a.Equals(b, threshold);
        }

        //
        // Summary:
        //     Check if the components of two vectors are approximate equal.
        //
        // Parameters:
        //   other:
        //     Vector3.
        //
        // Returns:
        //     True if the three components are almost equal or false in any other case.
        public bool Equals(Vector3 other)
        {
            return Equals(other, MathHelper.Epsilon);
        }

        //
        // Summary:
        //     Check if the components of two vectors are approximate equal.
        //
        // Parameters:
        //   other:
        //     Vector3.
        //
        //   threshold:
        //     Maximum tolerance.
        //
        // Returns:
        //     True if the three components are almost equal or false in any other case.
        public bool Equals(Vector3 other, double threshold)
        {
            if (MathHelper.IsEqual(other.X, x, threshold) && MathHelper.IsEqual(other.Y, y, threshold))
            {
                return MathHelper.IsEqual(other.Z, z, threshold);
            }

            return false;
        }

        //
        // Summary:
        //     Indicates whether this instance and a specified object are equal.
        //
        // Parameters:
        //   other:
        //     Another object to compare to.
        //
        // Returns:
        //     True if obj and this instance are the same type and represent the same value;
        //     otherwise, false.
        public override bool Equals(object other)
        {
            if (other is Vector3)
            {
                return Equals((Vector3)other);
            }

            return false;
        }

        //
        // Summary:
        //     Returns the hash code for this instance.
        //
        // Returns:
        //     A 32-bit signed integer that is the hash code for this instance.
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        //
        // Summary:
        //     Obtains a string that represents the vector.
        //
        // Returns:
        //     A string text.
        public override string ToString()
        {
            return string.Format("{0}{3} {1}{3} {2}", x, y, z, Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
        }

        //
        // Summary:
        //     Obtains a string that represents the vector.
        //
        // Parameters:
        //   provider:
        //     An IFormatProvider interface implementation that supplies culture-specific formatting
        //     information.
        //
        // Returns:
        //     A string text.
        public string ToString(IFormatProvider provider)
        {
            return string.Format("{0}{3} {1}{3} {2}", x.ToString(provider), y.ToString(provider), z.ToString(provider), Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
        }
    }
}
