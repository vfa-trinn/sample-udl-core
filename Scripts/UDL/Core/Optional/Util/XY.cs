using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Core
{
    [Serializable]
    public struct XY
    {

        public static readonly int DEFAULT_NUMBER = 10000000;
        public static int GetHashCodeByXY(int x, int y)
        {
            return x * DEFAULT_NUMBER + y;
        }
        public static int GetHashCodeByXY(XY address)
        {
            return address.x * DEFAULT_NUMBER + address.y;
        }


        public int x;
        public int y;

        public XY(int x, int y)
        {
            this.x = x;
            this.y = y;
            //this.defined = true;
            _hashCode = int.MinValue;
        }

        public static XY zero
        {
            get { return new XY(0, 0); }
        }

        public static XY Empty
        {
            get { return new XY(int.MinValue, int.MinValue); }
        }

        public bool isEmpty
        {
            get
            {
                return this.Equals(XY.Empty);
            }
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }

        public override bool Equals(object obj)
        {
            if(obj is XY == false)
            {
                return false;
            }
            XY xy = (XY) obj;
            return (xy.x == this.x && xy.y == this.y);
        }

        public bool Adjacent(XY xy)
        {
            if (this.Equals(xy))
                return false;

            if (this.x == xy.x && this.y == xy.y + 1)
                return true;
            if (this.x == xy.x && this.y == xy.y - 1)
                return true;
            if (this.y == xy.y && this.x == xy.x + 1)
                return true;
            if (this.y == xy.y && this.x == xy.x - 1)
                return true;

            return false;
        }

        private int _hashCode;
        public override int GetHashCode()
        {
            if (_hashCode == int.MinValue)
            {
                _hashCode = XY.GetHashCodeByXY(this);
            }
            return _hashCode;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, 0, y);
        }

        public List<XY> GetGrid(int rangeHeightFront, int rangeHeightBack, int rangeWidth)
        {
            List<XY> list = new List<XY>();
            for (int i = -rangeHeightFront; i < rangeHeightBack; i++)
            {
                for (int j = -rangeWidth; j < rangeWidth; j++)
                {

                    list.Add(new XY(i + x, j + y));
                }
            }
            return list;
        }

        public List<XY> GetGrid(int rangeHeightFront, int rangeHeightBack, int rangeWidth, XY min, XY max, bool includeMax = true)
        {

            if (!includeMax)
            {
                max.x -= 1;
                max.y -= 1;
            }

            List<XY> list = new List<XY>();
            for (int i = -rangeHeightFront; i < rangeHeightBack; i++)
            {
                for (int j = -rangeWidth; j < rangeWidth; j++)
                {
                    int xx = this.x + i;
                    int yy = this.y + j;
                    if (xx >= min.x && xx <= max.x && yy >= min.y && yy <= max.y)
                    {
                        list.Add(new XY(xx, yy));
                    }
                }
            }
            return list;
        }

        public static XY Delta(XY xy1, XY xy2)
        {
            return new XY(Math.Abs(xy1.x - xy2.x), Math.Abs(xy1.y - xy2.y));
        }

        public static int Step(XY xy1, XY xy2)
        {
            if (xy1.isEmpty || xy2.isEmpty) return -1;

            XY delta = XY.Delta(xy1, xy2);
            return delta.x + delta.y;
        }

        public static XY operator +(XY xy1, XY xy2)
        {
            return new XY(xy1.x + xy2.x, xy1.y + xy2.y);
        }

        public static bool operator ==(XY c1, XY c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(XY c1, XY c2)
        {
            return c1.Equals(c2) == false;
        }

        public static List<XY> SurroundingXYs(XY xy)
        {
            return new List<XY>()
            {
                new XY(xy.x + 0, xy.y - 1),
                new XY(xy.x + 1, xy.y - 1),
                new XY(xy.x + 1, xy.y + 0),
                new XY(xy.x + 1, xy.y + 1),
                new XY(xy.x + 0, xy.y + 1),
                new XY(xy.x - 1, xy.y + 1),
                new XY(xy.x - 1, xy.y + 0),
                new XY(xy.x - 1, xy.y - 1)
            };
        }

        
    }
}
