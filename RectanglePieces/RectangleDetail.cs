namespace RectanglePieces
{
    public class RectangleDetail
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Size { get; set; }

        public float Y { get; set; }
        public float X { get; set; }

        public decimal FullWidth { 
            get {
                return Width * Size;
            } 
            private set { }
        }
        public decimal FullHeight
        {
            get
            {
                return Height * Size;
            }
            private set { }
        }
        public decimal Area
        {
            get
            {
                return Width * Height;
            }
            private set { }
        }

        public void SetCoordinate(int x,int y)
        {
            X = x;
            Y = y;
        }


    }
}
