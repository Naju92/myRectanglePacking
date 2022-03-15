using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectanglePieces
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float End_Y { get; set; }
        public float End_X { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }
        //public Point(float x, float y)
        //{
        //    X = x;
        //    Y = y;
        //}
        public Point(float x,float y,float end_x,float end_y,float width,float height)

        {
            X = x;
            Y = y;
            End_Y = end_y;
            End_X = end_x;
            Height = height;
            Width = width;

        }
    }
}
