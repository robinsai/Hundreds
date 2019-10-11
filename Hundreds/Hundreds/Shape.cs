using System.Drawing;

namespace Hundreds
{


    public class Shape
    {

        //
        //Size size;
        public Point location;
        public Size sizes;

        public int speedx;
        public int speedy;
        public bool grow;
        int radius;
        //bool that keeps track of if the shape is growing

        private void CheckForWalls(int clientWidth, int clientHeight)
        {
            if (this.location.X < 0 || this.location.X + this.sizes.Width > clientWidth)
            {
                speedx *= -1;
            }
            if (this.location.Y < 0 || this.location.Y + this.sizes.Height > clientHeight)
            {
                speedy *= -1;
            }
        }
        private void GiveSpeed()
        {
            this.location.X += speedx;
            this.location.Y += speedy;
        }
        public void Update(int ClientWidth, int ClientHeight)
        {
            CheckForWalls(ClientWidth, ClientHeight);
            GiveSpeed();
            IncreaseSize();
        }
        public Point DoMathBro()
        {
            int x = ((int)location.X + this.sizes.Width) + location.X;
            int y = ((int)location.Y + this.sizes.Height) + location.Y;
            Point newPoint = new Point((x / 2) - 7, (y / 2) - 7);
            return newPoint;
        }
        public Point MiddleOftheCircle
        {

            get
            {
                return DoMathBro();
            }


        }
        public int CircleLabelTextValue;
       
        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)location.X, (int)location.Y, sizes.Width, sizes.Height);
            }
        }

        //add hitbox
        public Shape(Point position, Size sizes)
        {
            location = position;
            this.sizes = sizes;
            CircleLabelTextValue = sizes.Width/30;
            speedx = 1;
            speedy = 1;
            radius = (this.location.X + this.sizes.Width) / 2;
            grow = false;
            // hitbox = new Rectangle(location, sizes);

        }
        public int Radius
            {
            get { return radius; }
            }
       public void IncreaseSize()
        {
            if (grow)
            {
              
                this.sizes.Width++;
                this.sizes.Height++;
                CircleLabelTextValue++;
            }
            grow = false;
        }
        
        public void Draw(Graphics gfx)
        {
            
            Font font = new Font("Arial", 10);

            gfx.DrawString(CircleLabelTextValue.ToString(), font, Brushes.Black, MiddleOftheCircle);
            gfx.DrawEllipse(Pens.Black, this.Hitbox);
        }


    }
}
