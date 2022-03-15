using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RectanglePieces
{
    public partial class RectangleResult : Form
    {
        float _mainWidth;
        float _mainHeight;
        float begin_x=0;
        float begin_y=0;
        float lastX=0;
        float lastY=0;

        bool start = true;

        List<RectangleDetail> _rectangles;
        List<RectangleDetail> freeRectangles = new List<RectangleDetail>();
        List<Point> acceptedPoints = new List<Point>();
        public RectangleResult(decimal mainWidth,decimal mainHeight,List<RectangleDetail> rectangles)
        {
            _mainHeight =(float) mainHeight;
            _mainWidth = (float)mainWidth;
            _rectangles = rectangles;
            InitializeComponent();
        }
        public Boolean IsFree(ref Point point,bool advancedSearch=false)
        {
            if (point != null)
            {   
                    if (IsCrossed(point))
                    {
                      if(!SearchFreeCoordinate(ref point) && advancedSearch==false)
                    {
                        
                        return false;  //there is no free space
                    }
                      if(!AdvansedSearchCoordinate(ref point) && advancedSearch)
                    {
                        return false;
                    }
                    }
                acceptedPoints.Add(point);
                ColorHelper.ResetColor();
                return true;
            }
           
            return false;
        }
        public  bool Intersects(Point mainPoint,Point point )
        {
            if (point == null)
                return false;

            var r1 = new { Left = mainPoint.X, Right = mainPoint.X + mainPoint.Width, Bottom = mainPoint.Y, Top = mainPoint.Y + mainPoint.Height };
            var r2 = new { Left = point.X, Right = point.X + point.Width, Bottom = point.Y, Top = point.Y + point.Height };

            return r2.Right > r1.Left && r2.Bottom < r1.Top && r2.Top > r1.Bottom && r2.Left < r1.Right;
        }
        private bool IsCrossed(Point point2)
        {
            if (point2.End_X > _mainWidth || point2.X < 0 || point2.Y < 0 || point2.End_Y > _mainHeight) //out of Main Rectangle
            {
                return true;
            }
                foreach (var point1 in acceptedPoints)
                {
                    if (Intersects(point1, point2))
                    {
                        return true;
                    }
                }
            return false;
        }
        private bool SearchFreeCoordinate(ref Point point) 
        {
            Point localPoint = point;
            List<Point> possiblePoints = new List<Point>();
            var orderedAcceptedPoints= (localPoint.Height > localPoint.Width)?acceptedPoints.OrderBy(x => x.Y).ThenBy(x => x.X):acceptedPoints.OrderBy(x => x.X).ThenBy(x => x.Y);
            
            foreach(var rec in orderedAcceptedPoints) //by x
            {
                
                var rightExist = acceptedPoints.Any(x =>( x.X == rec.End_X || rec.End_X==_mainWidth) && x.Y == rec.Y);  //right Free
                if (!rightExist) //empty space //needed spase x1,y1   x2,y2
                {
                    //left r would be r.X-point.width
                    var newPoint = new Point(rec.End_X, rec.Y, rec.End_X+point.Width, point.Height, point.Width, point.Height);
                    if (!IsCrossed(newPoint))
                    {
                        point = newPoint;
                        return true;
                        break;
                    }
                }

                var leftExist = acceptedPoints.Any(x => (x.End_X == rec.X || rec.X == 0) && x.Y == rec.Y);  //left Free
                if (!leftExist) //empty space //needed spase x1,y1   x2,y2
                {
                    //left r would be r.X-point.width
                    var newPoint = new Point(rec.X - point.Width, rec.Y, rec.X, point.Height, point.Width, point.Height);
                    if (!IsCrossed(newPoint))
                    {
                        point = newPoint;
                        return true;
                        break;

                        //have freeSpace
                    }
                }


                var downExist = acceptedPoints.Any(x => (x.Y == rec.End_Y || rec.End_Y == _mainHeight) && x.X == rec.X);  //down Free
                if (!downExist) //empty space //needed spase x1,y1   x2,y2
                {
                    //left r would be r.X-point.width
                    var newPoint = new Point(rec.X, rec.End_Y, point.Width, rec.End_Y+point.Height, point.Width, point.Height);
                    if (!IsCrossed(newPoint))
                    {
                        point = newPoint;
                        return true;
                        break;
                    }
                }

                var upExist = acceptedPoints.Any(x => (x.End_Y == rec.Y || rec.Y == 0) && x.X == rec.X);  //up Free
                if (!upExist) //empty space //needed spase x1,y1   x2,y2
                {
                    //left r would be r.X-point.width
                    var newPoint = new Point(rec.X, rec.Y - point.Height, point.Width, rec.Y , point.Width, point.Height);
                    if (!IsCrossed(newPoint))
                    {
                        point = newPoint;
                        return true;
                        break;
                    }
                }
            }
            return false;
        }  
        private bool AdvansedSearchCoordinate(ref Point point)    //search begins from (maxX,maxY)
        {
            for (int x =(int) _mainWidth; x>= 0; x--)
            {
                lastX = x;
                begin_x = x - point.Width;
                for (int h = (int)_mainHeight; h >= 0; h--)
                {
                    lastY = h;
                    begin_y = h - point.Height;
                    var tempPoint = new Point(begin_x, begin_y, lastX, lastY, point.Width, point.Height);
                    var result = IsCrossed(tempPoint);
                    if (!result)
                    {
                        point = tempPoint;
                        return true;
                        break;
                    }
                }
            }

            return false;
        }
        private void PlaceRectanglesByX(Graphics g)   
        {
            var longerThan70Percent = _rectangles.Where(x => (float)x.Width > ((_mainWidth * 70) / 100)).ToList();
            if(longerThan70Percent != null && longerThan70Percent.Count() > 0)
            {
                begin_x = 0;
                begin_y = _mainHeight;
                start = true;
                foreach(var recX in longerThan70Percent)  //coordinate (0,maxY)
                {
                    for (int i = 1; i <= recX.Size; i++)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(ColorHelper.Alpha, ColorHelper.Red,ColorHelper.Green,ColorHelper.Blue)))
                        {
                            var newPoint = CheckPosition((float)recX.Width, (float)recX.Height, DecreaseByY);
                            if (newPoint != null)
                            {
                                g.FillRectangle(brush, newPoint.X, newPoint.Y, (float)recX.Width, (float)recX.Height);
                            }
                            else
                            {
                                AddToFreeRectangle(recX);
                            }
                        }
                    }
                    _rectangles.Remove(recX);
                }
            }
        }
        private void PlaceRectanglesByY(Graphics g) //highest
        {
            var longerThan70Percent = _rectangles.Where(x => (float)x.Height > ((_mainHeight * 70) / 100)).ToList();
            if (longerThan70Percent != null && longerThan70Percent.Count() > 0)
            {
                begin_y = 0;
                begin_x = _mainWidth;
                start = true;
                foreach (var recY in longerThan70Percent)  //coordinate (0,maxY)
                {
                    for (int i = 1; i <= recY.Size; i++)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(ColorHelper.Alpha, ColorHelper.Red, ColorHelper.Green, ColorHelper.Blue)))
                        {
                            var newPoint = CheckPosition((float)recY.Width, (float)recY.Height, DecreaseByX);
                            if (newPoint != null)
                            {
                                g.FillRectangle(brush, newPoint.X, newPoint.Y, (float)recY.Width, (float)recY.Height);
                            }
                            else
                            {
                                AddToFreeRectangle(recY);
                            }
                        }
                    }
                    _rectangles.Remove(recY);
                }
            }

        }
        private void PlaceAllRectangles(Graphics g)
        {
            var remainRectangles = _rectangles.ToList();
           if (remainRectangles != null && remainRectangles.Count() > 0)
            {
                begin_x = 0;
                begin_y = 0;
                start = true;
                foreach (var remRec in remainRectangles)
                {
                    for (int i = 1; i <= remRec.Size; i++)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(ColorHelper.Alpha, ColorHelper.Red, ColorHelper.Green, ColorHelper.Blue)))
                        {
                            var newPoint = CheckPosition((float)remRec.Width, (float)remRec.Height, EncreaseByX);
                            if (newPoint != null)
                            {
                                g.FillRectangle(brush, newPoint.X, newPoint.Y, (float)remRec.Width, (float)remRec.Height);
                            }
                            else
                            {
                                AddToFreeRectangle(remRec);
                            }
                        }
                    }
                    _rectangles.Remove(remRec);
                }
            }
        }
        private void PlaceRemaindedRectangles(Graphics g)
        {
            var localFreeRectangles = freeRectangles.OrderByDescending(x=>x.Area).ToList();
            if (localFreeRectangles != null && localFreeRectangles.Count() > 0)
            {
                foreach (var free in localFreeRectangles)
                {
                    for (int i = 1; i <= free.Size; i++)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(ColorHelper.Alpha, ColorHelper.Red, ColorHelper.Green, ColorHelper.Blue)))
                        {
                            var point = new Point(begin_x, begin_y, lastX, lastY, (float)free.Width, (float)free.Height);
                            var result = IsFree(ref point,true);
                            if (result == false)
                            {
                                MessageBox.Show("Not enough space!");
                                break;
                            }
                            else
                            {
                                  if (point != null)
                                    {
                                    g.FillRectangle(brush, point.X, point.Y, (float)free.Width, (float)free.Height);
                                    }
                            }
                        }
                    }
                    freeRectangles.Remove(free);
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            using (Pen selPen = new Pen(Color.Blue))
            {
                g.DrawRectangle(selPen, begin_x+1F, begin_y+1F, (float)_mainWidth, (float)_mainHeight);   //main rectangle
                PlaceRectanglesByX(g);
                PlaceRectanglesByY(g);
                PlaceAllRectangles(g);
                PlaceRemaindedRectangles(g);
            }
        }
        private Point  DecreaseByY(float width, float height)
        {
            Point point = null;
            lastX = width;
            if (start)
            {
                start = false;
                lastY -=height;
                point= new Point(begin_x, lastY, lastX, begin_y, width, height);
            }
            else
            {
                var lastPoint = acceptedPoints.LastOrDefault();
                if (lastPoint != null)
                {
                    lastY = lastPoint.Y - height;
                    //lastX -= lastPoint.End_X;
                    if (lastY + height > _mainHeight)   //no space,free space
                    {
                        var leftedWidth = _mainWidth - lastX;

                        //freePoints.Add(new Point(lastX, begin_y, leftedWidth, Height));      //doesnot fited position

                       if(!SearchFreeCoordinate(ref point))
                        {
                            //freePoints.Add(point);
                            return null;
                        }
                    }
                    else
                    {
                        begin_y = lastY;
                        point = new Point(begin_x, begin_y, lastX, lastPoint.Y, width, height);

                    }
                }
            }
            if (IsFree(ref point))  //else error
            {
                return point;
            }
            return null;
        }
        private Point EncreaseByY(float width, float height)
        {
            Point point = null;
            lastY = height;
            if (start)
            {
                start = false;
                lastX -= width;
                point= new Point(begin_x, begin_y, lastX, lastY, width, height);
            }
            else
            {
                var lastPoint = acceptedPoints.LastOrDefault();
                if (lastPoint != null)
                {
                    lastX -= lastPoint.End_X;
                    if (lastX + width > _mainWidth)   //no space,free space
                    {
                        var leftedWidth = _mainWidth - lastX;

                        //freePoints.Add(new Point(lastX, begin_y, leftedWidth, Height));      //doesnot fited position


                    }
                    else
                    {
                        begin_x = lastX;

                    }
                    point = new Point(begin_x, begin_y, lastX, lastPoint.End_Y, width, height);
                }
            }
            if (IsFree(ref point))  //else error
            {
                return point;
            }
            return null;
        }
        private Point DecreaseByX(float width, float height)
        {
            Point point = new Point(begin_x, begin_y, lastX, lastY, _mainWidth, _mainHeight); ;

            lastY = height;
            if (start)
            {
                start = false;
                lastX -= width;
                point= new Point(lastX, begin_y, begin_x, lastY, width, height);
            }
            else
            {
                var lastPoint = acceptedPoints.LastOrDefault();
                if (lastPoint != null)
                {
                    lastX -= lastPoint.End_X;
                    if (lastX + width > _mainWidth)   //no space,free space
                    {
                        if (!SearchFreeCoordinate(ref point))
                        {
                            return null;
                        }
                    }
                    else
                    {
                        begin_x = lastX;
                        point = new Point(lastX, begin_y, lastX - width, lastPoint.End_Y, width, height);
                    }
                }
            }
            
            if (IsFree(ref point))  //else error
            {
                return point;
            }
            return null;
        }
        private Point EncreaseByX(float width,float height) //X encreasing,Y doesnt
        {
            Point point =new  Point(begin_x, begin_y, lastX, lastY, _mainWidth, _mainHeight);
            lastY = height;
            if (start)
            {
                start = false;
                lastX += width;
                point= new Point(begin_x, begin_y, lastX, lastY, width, height);
            }
            else
            {
                var lastPoint = acceptedPoints.LastOrDefault();
                if (lastPoint != null)
                {
                    lastX = lastPoint.End_X;
                    if (lastX + width > _mainWidth)   //no space,free space
                    {
                        if (!SearchFreeCoordinate(ref point))  //no coordinate,return null
                        {
                            return null;
                        }
                    }
                    else
                    {
                        begin_x = lastX;
                        point = new Point(begin_x, begin_y, lastX + width, lastPoint.End_Y, width, height);
                    }
                }
            }
            if (IsFree(ref point))  //else error
            {
                return point;
            }
            return null;

        }
        public Point CheckPosition(float nextWidth,float Height,Func<float,float,Point> func)
        {
             lastX = begin_x;
             lastY = begin_y;
            var point = func((float)nextWidth, (float)Height);
            
            return point;
        }
        private void AddToFreeRectangle(RectangleDetail rectangle)
        {
            var exist = freeRectangles.Any(x => x.Width == rectangle.Width && x.Height == rectangle.Height);
            if (exist)
            {
                freeRectangles.Where(x => x.Width == rectangle.Width && x.Height == rectangle.Height).Select(c => { c.Size+=1; return c; }).ToList();
            }
            else
            {
                
                freeRectangles.Add(new RectangleDetail {Size=1,Width=rectangle.Width,Height=rectangle.Height });
            }
        }
        private void RestangleResult_Load_1(object sender, EventArgs e)
        {

        }
    }
}
