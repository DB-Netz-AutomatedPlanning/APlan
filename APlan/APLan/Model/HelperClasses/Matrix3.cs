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
    //     Represents a 3x3 double precision matrix.
    public struct Matrix3 : IEquatable<Matrix3>
    {
        private double m11;

        private double m12;

        private double m13;

        private double m21;

        private double m22;

        private double m23;

        private double m31;

        private double m32;

        private double m33;

        private bool dirty;

        private bool isIdentity;

        //
        // Summary:
        //     Gets the zero matrix.
        public static Matrix3 Zero => new Matrix3(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);

        //
        // Summary:
        //     Gets the identity matrix.
        public static Matrix3 Identity
        {
            get
            {
                Matrix3 result = new Matrix3(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);
                result.dirty = false;
                result.isIdentity = true;
                return result;
            }
        }

        //
        // Summary:
        //     Gets or sets the matrix element [0,0].
        public double M11
        {
            get
            {
                return m11;
            }
            set
            {
                m11 = value;
                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets or sets the matrix element [0,1].
        public double M12
        {
            get
            {
                return m12;
            }
            set
            {
                m12 = value;
                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets or sets the matrix element [0,2].
        public double M13
        {
            get
            {
                return m13;
            }
            set
            {
                m13 = value;
                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets or sets the matrix element [1,0].
        public double M21
        {
            get
            {
                return m21;
            }
            set
            {
                m21 = value;
                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets or sets the matrix element [1,1].
        public double M22
        {
            get
            {
                return m22;
            }
            set
            {
                m22 = value;
                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets or sets the matrix element [1,2].
        public double M23
        {
            get
            {
                return m23;
            }
            set
            {
                m23 = value;
                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets or sets the matrix element [2,0].
        public double M31
        {
            get
            {
                return m31;
            }
            set
            {
                m31 = value;
                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets or sets the matrix element [2,1].
        public double M32
        {
            get
            {
                return m32;
            }
            set
            {
                m32 = value;
                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets or sets the matrix element [2,2].
        public double M33
        {
            get
            {
                return m33;
            }
            set
            {
                m33 = value;
                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets or sets the component at the given row and column index in the matrix.
        //
        // Parameters:
        //   row:
        //     The row index of the matrix.
        //
        //   column:
        //     The column index of the matrix.
        //
        // Returns:
        //     The component at the given row and column index in the matrix.
        public double this[int row, int column]
        {
            get
            {
                return row switch
                {
                    0 => column switch
                    {
                        0 => m11,
                        1 => m12,
                        2 => m13,
                        _ => throw new ArgumentOutOfRangeException("column"),
                    },
                    1 => column switch
                    {
                        0 => m21,
                        1 => m22,
                        2 => m23,
                        _ => throw new ArgumentOutOfRangeException("column"),
                    },
                    2 => column switch
                    {
                        0 => m31,
                        1 => m32,
                        2 => m33,
                        _ => throw new ArgumentOutOfRangeException("column"),
                    },
                    _ => throw new ArgumentOutOfRangeException("row"),
                };
            }
            set
            {
                switch (row)
                {
                    case 0:
                        switch (column)
                        {
                            case 0:
                                m11 = value;
                                break;
                            case 1:
                                m12 = value;
                                break;
                            case 2:
                                m13 = value;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("column");
                        }

                        break;
                    case 1:
                        switch (column)
                        {
                            case 0:
                                m21 = value;
                                break;
                            case 1:
                                m22 = value;
                                break;
                            case 2:
                                m23 = value;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("column");
                        }

                        break;
                    case 2:
                        switch (column)
                        {
                            case 0:
                                m31 = value;
                                break;
                            case 1:
                                m32 = value;
                                break;
                            case 2:
                                m33 = value;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("column");
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException("row");
                }

                dirty = true;
            }
        }

        //
        // Summary:
        //     Gets if the actual matrix is the identity.
        //
        // Remarks:
        //     The checks to see if the matrix is the identity uses the MathHelper.Epsilon as
        //     a the threshold for testing values close to one and zero.
        public bool IsIdentity
        {
            get
            {
                if (dirty)
                {
                    dirty = false;
                    if (!MathHelper.IsOne(M11))
                    {
                        isIdentity = false;
                        return isIdentity;
                    }

                    if (!MathHelper.IsZero(M12))
                    {
                        isIdentity = false;
                        return isIdentity;
                    }

                    if (!MathHelper.IsZero(M13))
                    {
                        isIdentity = false;
                        return isIdentity;
                    }

                    if (!MathHelper.IsZero(M21))
                    {
                        isIdentity = false;
                        return isIdentity;
                    }

                    if (!MathHelper.IsOne(M22))
                    {
                        isIdentity = false;
                        return isIdentity;
                    }

                    if (!MathHelper.IsZero(M23))
                    {
                        isIdentity = false;
                        return isIdentity;
                    }

                    if (!MathHelper.IsZero(M31))
                    {
                        isIdentity = false;
                        return isIdentity;
                    }

                    if (!MathHelper.IsZero(M32))
                    {
                        isIdentity = false;
                        return isIdentity;
                    }

                    if (!MathHelper.IsOne(M33))
                    {
                        isIdentity = false;
                        return isIdentity;
                    }

                    isIdentity = true;
                    return isIdentity;
                }

                return isIdentity;
            }
        }

        //
        // Summary:
        //     Initializes a new instance of Matrix3.
        //
        // Parameters:
        //   m11:
        //     Element [0,0].
        //
        //   m12:
        //     Element [0,1].
        //
        //   m13:
        //     Element [0,2].
        //
        //   m21:
        //     Element [1,0].
        //
        //   m22:
        //     Element [1,1].
        //
        //   m23:
        //     Element [1,2].
        //
        //   m31:
        //     Element [2,0].
        //
        //   m32:
        //     Element [2,1].
        //
        //   m33:
        //     Element [2,2].
        public Matrix3(double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
            dirty = true;
            isIdentity = false;
        }

        //
        // Summary:
        //     Matrix addition.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   b:
        //     Matrix3.
        //
        // Returns:
        //     Matrix3.
        public static Matrix3 operator +(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13, a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23, a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33);
        }

        //
        // Summary:
        //     Matrix addition.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   b:
        //     Matrix3.
        //
        // Returns:
        //     Matrix3.
        public static Matrix3 Add(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13, a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23, a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33);
        }

        //
        // Summary:
        //     Matrix subtraction.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   b:
        //     Matrix3.
        //
        // Returns:
        //     Matrix3.
        public static Matrix3 operator -(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13, a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23, a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33);
        }

        //
        // Summary:
        //     Matrix subtraction.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   b:
        //     Matrix3.
        //
        // Returns:
        //     Matrix3.
        public static Matrix3 Subtract(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13, a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23, a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33);
        }

        //
        // Summary:
        //     Product of two matrices.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   b:
        //     Matrix3.
        //
        // Returns:
        //     Matrix3.
        public static Matrix3 operator *(Matrix3 a, Matrix3 b)
        {
            if (a.IsIdentity)
            {
                return b;
            }

            if (b.IsIdentity)
            {
                return a;
            }

            return new Matrix3(a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31, a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32, a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33, a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31, a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32, a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33, a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31, a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32, a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33);
        }

        //
        // Summary:
        //     Product of two matrices.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   b:
        //     Matrix3.
        //
        // Returns:
        //     Matrix3.
        public static Matrix3 Multiply(Matrix3 a, Matrix3 b)
        {
            if (a.IsIdentity)
            {
                return b;
            }

            if (b.IsIdentity)
            {
                return a;
            }

            return new Matrix3(a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31, a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32, a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33, a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31, a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32, a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33, a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31, a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32, a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33);
        }

        //
        // Summary:
        //     Product of a matrix with a vector.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   u:
        //     Vector3.
        //
        // Returns:
        //     Matrix3.
        //
        // Remarks:
        //     Matrix3 adopts the convention of using column vectors.
        public static Vector3 operator *(Matrix3 a, Vector3 u)
        {
            if (!a.IsIdentity)
            {
                return new Vector3(a.M11 * u.X + a.M12 * u.Y + a.M13 * u.Z, a.M21 * u.X + a.M22 * u.Y + a.M23 * u.Z, a.M31 * u.X + a.M32 * u.Y + a.M33 * u.Z);
            }

            return u;
        }

        //
        // Summary:
        //     Product of a matrix with a vector.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   u:
        //     Vector3.
        //
        // Returns:
        //     Matrix3.
        //
        // Remarks:
        //     Matrix3 adopts the convention of using column vectors.
        public static Vector3 Multiply(Matrix3 a, Vector3 u)
        {
            if (!a.IsIdentity)
            {
                return new Vector3(a.M11 * u.X + a.M12 * u.Y + a.M13 * u.Z, a.M21 * u.X + a.M22 * u.Y + a.M23 * u.Z, a.M31 * u.X + a.M32 * u.Y + a.M33 * u.Z);
            }

            return u;
        }

        //
        // Summary:
        //     Product of a matrix with a scalar.
        //
        // Parameters:
        //   m:
        //     Matrix3.
        //
        //   a:
        //     Scalar.
        //
        // Returns:
        //     Matrix3.
        public static Matrix3 operator *(Matrix3 m, double a)
        {
            return new Matrix3(m.M11 * a, m.M12 * a, m.M13 * a, m.M21 * a, m.M22 * a, m.M23 * a, m.M31 * a, m.M32 * a, m.M33 * a);
        }

        //
        // Summary:
        //     Product of a matrix with a scalar.
        //
        // Parameters:
        //   m:
        //     Matrix3.
        //
        //   a:
        //     Scalar.
        //
        // Returns:
        //     Matrix3.
        public static Matrix3 Multiply(Matrix3 m, double a)
        {
            return new Matrix3(m.M11 * a, m.M12 * a, m.M13 * a, m.M21 * a, m.M22 * a, m.M23 * a, m.M31 * a, m.M32 * a, m.M33 * a);
        }

        //
        // Summary:
        //     Check if the components of two matrices are equal.
        //
        // Parameters:
        //   u:
        //     Matrix3.
        //
        //   v:
        //     Matrix3.
        //
        // Returns:
        //     True if the matrix components are equal or false in any other case.
        public static bool operator ==(Matrix3 u, Matrix3 v)
        {
            return Equals(u, v);
        }

        //
        // Summary:
        //     Check if the components of two matrices are different.
        //
        // Parameters:
        //   u:
        //     Matrix3.
        //
        //   v:
        //     Matrix3.
        //
        // Returns:
        //     True if the matrix components are different or false in any other case.
        public static bool operator !=(Matrix3 u, Matrix3 v)
        {
            return !Equals(u, v);
        }

        //
        // Summary:
        //     Calculate the determinant of the actual matrix.
        //
        // Returns:
        //     Determinant.
        public double Determinant()
        {
            if (IsIdentity)
            {
                return 1.0;
            }

            return m11 * m22 * m33 + m12 * m23 * m31 + m13 * m21 * m32 - m13 * m22 * m31 - m11 * m23 * m32 - m12 * m21 * m33;
        }

        //
        // Summary:
        //     Calculates the inverse matrix.
        //
        // Returns:
        //     Inverse Matrix3.
        public Matrix3 Inverse()
        {
            if (IsIdentity)
            {
                return Identity;
            }

            double num = Determinant();
            if (MathHelper.IsZero(num))
            {
                throw new ArithmeticException("The matrix is not invertible.");
            }

            num = 1.0 / num;
            return new Matrix3(num * (m22 * m33 - m23 * m32), num * (m13 * m32 - m12 * m33), num * (m12 * m23 - m13 * m22), num * (m23 * m31 - m21 * m33), num * (m11 * m33 - m13 * m31), num * (m13 * m21 - m11 * m23), num * (m21 * m32 - m22 * m31), num * (m12 * m31 - m11 * m32), num * (m11 * m22 - m12 * m21));
        }

        //
        // Summary:
        //     Obtains the transpose matrix.
        //
        // Returns:
        //     Transpose matrix.
        public Matrix3 Transpose()
        {
            if (!IsIdentity)
            {
                return new Matrix3(m11, m21, m31, m12, m22, m32, m13, m23, m33);
            }

            return Identity;
        }

        //
        // Summary:
        //     Builds a rotation matrix for a rotation around the x-axis.
        //
        // Parameters:
        //   angle:
        //     The counter-clockwise angle in radians.
        //
        // Returns:
        //     The resulting Matrix3 instance.
        //
        // Remarks:
        //     Matrix3 adopts the convention of using column vectors to represent a transformation
        //     matrix.
        public static Matrix3 RotationX(double angle)
        {
            double num = Math.Cos(angle);
            double num2 = Math.Sin(angle);
            return new Matrix3(1.0, 0.0, 0.0, 0.0, num, 0.0 - num2, 0.0, num2, num);
        }

        //
        // Summary:
        //     Builds a rotation matrix for a rotation around the y-axis.
        //
        // Parameters:
        //   angle:
        //     The counter-clockwise angle in radians.
        //
        // Returns:
        //     The resulting Matrix3 instance.
        //
        // Remarks:
        //     Matrix3 adopts the convention of using column vectors to represent a transformation
        //     matrix.
        public static Matrix3 RotationY(double angle)
        {
            double num = Math.Cos(angle);
            double num2 = Math.Sin(angle);
            return new Matrix3(num, 0.0, num2, 0.0, 1.0, 0.0, 0.0 - num2, 0.0, num);
        }

        //
        // Summary:
        //     Builds a rotation matrix for a rotation around the z-axis.
        //
        // Parameters:
        //   angle:
        //     The counter-clockwise angle in radians.
        //
        // Returns:
        //     The resulting Matrix3 instance.
        //
        // Remarks:
        //     Matrix3 adopts the convention of using column vectors to represent a transformation
        //     matrix.
        public static Matrix3 RotationZ(double angle)
        {
            double num = Math.Cos(angle);
            double num2 = Math.Sin(angle);
            return new Matrix3(num, 0.0 - num2, 0.0, num2, num, 0.0, 0.0, 0.0, 1.0);
        }

        //
        // Summary:
        //     Build a scaling matrix.
        //
        // Parameters:
        //   value:
        //     Single scale factor for x, y, and z axis.
        //
        // Returns:
        //     A scaling matrix.
        public static Matrix3 Scale(double value)
        {
            return Scale(value, value, value);
        }

        //
        // Summary:
        //     Build a scaling matrix.
        //
        // Parameters:
        //   value:
        //     Scale factors for x, y, and z axis.
        //
        // Returns:
        //     A scaling matrix.
        public static Matrix3 Scale(Vector3 value)
        {
            return Scale(value.X, value.Y, value.Z);
        }

        //
        // Summary:
        //     Build a scaling matrix.
        //
        // Parameters:
        //   x:
        //     Scale factor for x-axis.
        //
        //   y:
        //     Scale factor for y-axis.
        //
        //   z:
        //     Scale factor for z-axis.
        //
        // Returns:
        //     A scaling matrix.
        public static Matrix3 Scale(double x, double y, double z)
        {
            return new Matrix3(x, 0.0, 0.0, 0.0, y, 0.0, 0.0, 0.0, z);
        }

        //
        // Summary:
        //     Check if the components of two matrices are approximate equal.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   b:
        //     Matrix3.
        //
        // Returns:
        //     True if the matrix components are almost equal or false in any other case.
        public static bool Equals(Matrix3 a, Matrix3 b)
        {
            return a.Equals(b, MathHelper.Epsilon);
        }

        //
        // Summary:
        //     Check if the components of two matrices are approximate equal.
        //
        // Parameters:
        //   a:
        //     Matrix3.
        //
        //   b:
        //     Matrix3.
        //
        //   threshold:
        //     Maximum tolerance.
        //
        // Returns:
        //     True if the matrix components are almost equal or false in any other case.
        public static bool Equals(Matrix3 a, Matrix3 b, double threshold)
        {
            return a.Equals(b, threshold);
        }

        //
        // Summary:
        //     Check if the components of two matrices are approximate equal.
        //
        // Parameters:
        //   other:
        //     Matrix3.
        //
        // Returns:
        //     True if the matrix components are almost equal or false in any other case.
        public bool Equals(Matrix3 other)
        {
            return Equals(other, MathHelper.Epsilon);
        }

        //
        // Summary:
        //     Check if the components of two matrices are approximate equal.
        //
        // Parameters:
        //   obj:
        //     Matrix3.
        //
        //   threshold:
        //     Maximum tolerance.
        //
        // Returns:
        //     True if the matrix components are almost equal or false in any other case.
        public bool Equals(Matrix3 obj, double threshold)
        {
            if (MathHelper.IsEqual(obj.M11, M11, threshold) && MathHelper.IsEqual(obj.M12, M12, threshold) && MathHelper.IsEqual(obj.M13, M13, threshold) && MathHelper.IsEqual(obj.M21, M21, threshold) && MathHelper.IsEqual(obj.M22, M22, threshold) && MathHelper.IsEqual(obj.M23, M23, threshold) && MathHelper.IsEqual(obj.M31, M31, threshold) && MathHelper.IsEqual(obj.M32, M32, threshold))
            {
                return MathHelper.IsEqual(obj.M33, M33, threshold);
            }

            return false;
        }

        //
        // Summary:
        //     Indicates whether this instance and a specified object are equal.
        //
        // Parameters:
        //   obj:
        //     Another object to compare to.
        //
        // Returns:
        //     True if obj and this instance are the same type and represent the same value;
        //     otherwise, false.
        public override bool Equals(object obj)
        {
            if (obj is Matrix3)
            {
                return Equals((Matrix3)obj);
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
            return M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^ M21.GetHashCode() ^ M22.GetHashCode() ^ M23.GetHashCode() ^ M31.GetHashCode() ^ M32.GetHashCode() ^ M33.GetHashCode();
        }

        //
        // Summary:
        //     Obtains a string that represents the matrix.
        //
        // Returns:
        //     A string text.
        public override string ToString()
        {
            string listSeparator = Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("|{0}{3} {1}{3} {2}|" + Environment.NewLine, m11, m12, m13, listSeparator));
            stringBuilder.Append(string.Format("|{0}{3} {1}{3} {2}|" + Environment.NewLine, m21, m22, m23, listSeparator));
            stringBuilder.Append(string.Format("|{0}{3} {1}{3} {2}|", m31, m32, m33, listSeparator));
            return stringBuilder.ToString();
        }

        //
        // Summary:
        //     Obtains a string that represents the matrix.
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
            string listSeparator = Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("|{0}{3} {1}{3} {2}|" + Environment.NewLine, m11.ToString(provider), m12.ToString(provider), m13.ToString(provider), listSeparator));
            stringBuilder.Append(string.Format("|{0}{3} {1}{3} {2}|" + Environment.NewLine, m21.ToString(provider), m22.ToString(provider), m23.ToString(provider), listSeparator));
            stringBuilder.Append(string.Format("|{0}{3} {1}{3} {2}|", m31.ToString(provider), m32.ToString(provider), m33.ToString(provider), listSeparator));
            return stringBuilder.ToString();
        }
    }
}
