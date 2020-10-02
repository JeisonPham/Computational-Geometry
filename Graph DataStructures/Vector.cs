using System;
using System.Linq.Expressions;
using Computational_Geometry.Art_Gallery_Problem;

namespace Computational_Geometry.Graph_DataStructures
{

    public class Vector
    {
        protected bool Equals(Vector other)
        {
            return Equals(_values, other._values) && _type == other._type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_values != null ? _values.GetHashCode() : 0) * 397) ^ (int) _type;
            }
        }

        #region Static Properties
        
        // 1D
        public static Vector Right1  = new Vector(1);
        public static Vector Left1 = new Vector(-1);
        public static Vector Zero1 = new Vector(0);
        public static Vector NegativeInfinity1 = new Vector(float.NegativeInfinity);
        public static Vector PositiveInfinity1 = new Vector(float.PositiveInfinity);
        
        // 2D
        public static Vector Right2 = new Vector(1, 0);
        public static Vector Left2 = new Vector(-1, 0);
        public static Vector Up2 = new Vector(0, 1);
        public static Vector Down2 = new Vector(0, -1);
        public static Vector Zero2 = new Vector(0, 0);

        public static Vector NegativeInfinity2 = new Vector(float.NegativeInfinity, float.NegativeInfinity);
        public static Vector PositiveInfinity2 = new Vector(float.PositiveInfinity, float.PositiveInfinity);
        public static Vector One2 = new Vector(1, 1);

        #endregion

        #region Static Methods

        public static float AngleRad(Vector from, Vector to)
        {
            if(from._type != to._type) throw new ArgumentException("Vectors are not of the same type");
            var temp = from.Dot(to) / (from.magnitude * to.magnitude);

            return (float) Math.Acos(temp);
        }

        public static float SignedAngleRad(Vector from, Vector to, Vector axis = null)
        {
            if(from._type != to._type) throw new ArgumentException("Vectors are not of the same type");
            if(axis == null && from._type == VectorType.Three) throw new ArgumentException("Axis Vector needs to be of Vector3");
            var angle = AngleRad(from, to);
            if (from.CrossVal(to, axis) > 0)
            {
                return (float) (Math.PI * 2 - angle);
            }

            return angle;
        }

        #endregion

        #region Constructor and data Members

        private float[] _values; // Can create Vectors of length 1x1 2x2 3x3 4x4 etc
        private VectorType _type;

        public float x
        {
            get => _values[0];
            set => _values[0] = value;
        }
        
        public float y
        {
            get => _values[1];
            set => _values[1] = value;
        }
        
        public float z
        {
            get => _values[2];
            set => _values[2] = value;
        }
        
        public float w
        {
            get => _values[3];
            set => _values[3] = value;
        }

        public float magnitude => (float) Math.Sqrt(sqrMagnitude);

        public float sqrMagnitude
        {
            get
            {
                float mag = 0;
                for (int i = 0; i < _values.Length; i++)
                {
                    mag += _values[i] * _values[i];
                }

                return mag;
            }
        }

        public float this[int index] => _values[index];


        public Vector(params float[] array)
        {
            if (array.Length > 4)
            {
                throw new ArgumentException("There are more than 4 elements in Vector max 4, min 1");
            }
            _values = new float[array.Length];
            _values[0] = array[0];
            _type = (VectorType)array.Length;
            
            if (array.Length > 1)
            {
                _values[1] = array[1];
            }
            
            if (array.Length > 2)
            {
                _values[2] = array[2];
            }
            
            if (array.Length > 3)
            {
                _values[3] = array[3];
            }
        
        }


        #endregion

        #region Operators

        public static Vector operator + (Vector x, Vector y)
        {
            if (x._type != y._type)
            {
                throw new ArgumentException("Vector Types are not The same");
            }
            float[] temp = new float[x._values.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = x._values[i] + y._values[i];
            }

            return new Vector(temp);
        }

        public static Vector operator -(Vector x, Vector y)
        {
            if (x._type != y._type)
            {
                throw new ArgumentException("Vector Types are not The same");
            }
            float[] temp = new float[x._values.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = x._values[i] - y._values[i];
            }

            return new Vector(temp);
        }

        public static Vector operator -(Vector x)
        {
            float[] temp = new float[x._values.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = -x._values[i];
            }

            return new Vector(temp);
        }

        public static bool operator ==(Vector x, Vector y)
        {
            if (x is null && y is null) return true;
            if (x._type != y._type)
                throw new ArgumentException("Vector Types are not The same");
            
            for (int i = 0; i < x._values.Length; i++)
            {
                if (!FEquals(x._values[i], y._values[i])) return false;
            }

            return true;
        }

        public static bool operator !=(Vector x, Vector y)
        {
            return !(x == y);
        }

        public static Vector operator *(Vector x, float y)
        {
            var temp = new float[x._values.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = y * x._values[i];
            }

            return new Vector(temp);
        }

        public static Vector operator *(float y, Vector x)
        {
            return x * y;
        }

        public static Vector operator /(Vector x, float y)
        {
            return x * (1 / y);
        }

        #if UNITY_5
        
        public static implicit float(Vector x) => x.x;
        public static implicit Vector2(Vector x)  => new Vector2(x.x, x.y);
        public static implicit Vector3(Vector x) => new Vector3(x.x, x.y, x.z);
        
        public static explicit Vector(float x) => new Vector(x);
        public static explicit Vector(Vector2 x) => new Vector(x.x, x.y);
        public static explicit Vector(Vector3 x) => new Vector(x.x, x.y. x.z);
        #endif

        #endregion

        #region Private Methods

        private static bool FEquals(float x, float y)
        {
            return Math.Abs(x - y) < 0.0005f;
        }

        #endregion

        #region Public Methods

        public Vector Cross(Vector yVector)
        {
            if(this._type != yVector._type) throw new ArgumentException("Vector Types are not matching");
            switch(yVector._type)
            {
                case VectorType.One:
                    return new Vector(this.x * yVector.x);
                    break;
                case VectorType.Two:
                    return new Vector(this.x * yVector.y - this.y * yVector.x);
                    break;
                case VectorType.Three:
                    return new Vector(this.y*yVector.z - this.z*yVector.y, this.z * yVector.x - this.x * yVector.z, this.x * yVector.y - this.y * yVector.x);
                    break;
                case VectorType.Four:
                    throw new ArithmeticException("4d cross product has not yet been implemented");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public float CrossVal(Vector yVector, Vector axis = null)
        {
            if(this._type != yVector._type) throw new ArgumentException("Vector Types are not matching");
            switch(yVector._type)
            {
                case VectorType.One:
                    return this.x * yVector.x;
                    break;
                case VectorType.Two:
                    return this.x * yVector.y - this.y * yVector.x;
                    break;
                case VectorType.Three:
                    if(axis == null) throw new ArgumentException("3D Cross Product requires axis");
                    return (this.y*yVector.z - this.z*yVector.y) * axis.x + (this.z * yVector.x - this.x * yVector.z) * axis.y 
                                                                          + ( this.x * yVector.y - this.y * yVector.x) * axis.z;
                    break;
                case VectorType.Four:
                    throw new ArithmeticException("4d cross product has not yet been implemented");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public float Dot(Vector yVector)
        {
            float temp = 0;
            for (int i = 0; i < yVector._values.Length; i++)
            {
                temp += this._values[i] * yVector._values[i];
            }

            return temp;
        }

        public float Get(int i)
        {
            if(i > _values.Length) throw new ArgumentException("Passed in value is not found in the Vector");
            return _values[i];
        }

        public override string ToString()
        {
            string temp = "(";
            for (int i = 0; i < _values.Length - 1; i++)
            {
                temp += _values[i] + ", ";
            }

            temp += _values[_values.Length - 1] + ")";
            return temp;

        }

        public void Normalize()
        {
            var mag = magnitude;
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = _values[i] / mag;
            }
        }

        public void Set(Vector x)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = x._values[i];
            }
        }

        public float Distance(Vector x)
        {
            double temp = 0;
            for (int i = 0; i < x._values.Length; i++)
            {
                temp += Math.Pow(this._values[i] - x._values[i], 2);
            }

            return (float) Math.Sqrt(temp);
        }

        #endregion

        #region Other

        private enum VectorType
        {
            One  = 1, Two = 2, Three = 3, Four = 4
        }
        
        

        #endregion
    }
}