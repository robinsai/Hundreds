using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hundreds
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Graphics gfx;
        Bitmap canvas;
        List<Shape> Circles;
        public List<Label> CircleValueLabels;
       public int gameLevel = 1;
        public Size defaultCircleSize = new Size();
        public Point defaultCircleLocation = new Point();
        Shape Circle;
        int circleSize =0;
        Random ran;
       
        public Label circleValueLabel;
        public bool generateValueLabels = true;
       public enum AfterGameScenes
        {
            YouWin,
            YouLose,
        }
        public AfterGameScenes gameOver;
        
        /*
        game automatically moves onto next round when you press the start button and says that you win 10/10 
   */
        void GameStart()
        {
            startButton.Visible = false;
            startButton.Enabled = false;
            timer1.Enabled = true;
            //timer1.Start();
        }
        public Shape CircleBlueprint(Random random)
        {
            circleSize = random.Next(30, 40);
            defaultCircleLocation.X = ran.Next(ClientSize.Width - 100);
            defaultCircleLocation.Y = ran.Next(ClientSize.Height - 100);
            defaultCircleSize.Width = circleSize;
            defaultCircleSize.Height = circleSize;
            Circle = new Shape(defaultCircleLocation, defaultCircleSize);
            
            return Circle;
        }
      public bool MathForIntersects(Shape circle1,Shape circle2)
        {
            //xDistance to the power of 2 + yDistance to the power of 2 then square root the sum.
            //compare the squareRooted < totalRadius of the two
            float radius = circle1.Radius + circle2.Radius;
            float xDifference = circle1.location.X - circle2.location.X;
            float yDifference = circle1.location.Y - circle2.location.Y;
            float newX = xDifference * xDifference;
            float newY = yDifference * yDifference;
            float sum = newX + newY;
            double SqrRootSum = Math.Sqrt(sum);
            if(SqrRootSum < radius)
            {
                return true;
            }
            return false;
        }
        void InternalCollision(List<Shape> circles)
        {
            for (int x = 0; x < circles.Count; x++)
            {


                for (int i = 0; i < circles.Count; i++)
                {
                    if (circles[x].Hitbox.IntersectsWith(circles[i].Hitbox) && x != i)
                    {
                        if (MathForIntersects(circles[x], circles[i]) )
                        {
                            if (circles[x].grow == true || circles[i].grow == true)
                            {
                                gameOver = AfterGameScenes.YouLose;
                                //swapping speeds when hitting each other.
                            }
                            else
                            {

                                int tempX = circles[x].speedx;
                                int tempY = circles[i].speedy;
                                circles[x].speedx = circles[i].speedx;
                                circles[x].speedy = circles[i].speedy;
                                circles[i].speedx = tempX;
                                circles[i].speedy = tempY;

                            }
                        }
                    }
                }
            }
        }
        
        public void LevelGenerator()
        {
            Circles.Clear();
           
            int amountOfCircleGenerated = gameLevel;
            for (int i =0; i < amountOfCircleGenerated; i++)
            {
                Circles.Add(CircleBlueprint(ran));
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CircleValueLabels = new List<Label>();
            Circles = new List<Shape>();
            ran = new Random();

            circleValueLabel = new Label();
            Controls.Add(circleValueLabel);
            Circle = new Shape(defaultCircleLocation, defaultCircleSize);
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            gfx = Graphics.FromImage(canvas);
            LevelGenerator();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            
           
            if (gameLevel <= 1)
            {
                levelLabel.Text = $"Level {gameLevel}";
                levelLabel.Visible = true;
            }
            else 
            {
          
                LevelGenerator();
                generateValueLabels = true;
            }
        GameStart();
        }
       
     public void GenerateValueLabelsFnction()
        {


            if (generateValueLabels)
            {
                for (int i = 0; i < gameLevel; i++)
                {
                    CircleValueLabels.Add(circleValueLabel);
                    CircleValueLabels[i].Visible = true;

                }
                generateValueLabels = false;
            }
            for (int i = 0; i < gameLevel; i++)
            {
                CircleValueLabels[i].Location = Circles[i].location;




                CircleValueLabels[i].Text = 50.ToString();
            }
        }
        public void WinOrLoseChecker()
        {

            int totalSize = 0;

            for (int i = 0; i < Circles.Count; i++)
            {

                totalSize += Circles[i].CircleLabelTextValue;
            }
            if (totalSize >= 100)
            {
                gameOver = AfterGameScenes.YouWin;
            }
            
            switch(gameOver)
            {
                case AfterGameScenes.YouLose:
                    timer1.Enabled = false;
                    label2.Text = "you lose ";
                    levelLabel.Text = "playAgain?";
                    break;
                  
                case AfterGameScenes.YouWin:
                    timer1.Enabled = false;
                    if (gameLevel >= 10)
                    {
                        label2.Text = "you win the game!";
                       

                    }
                    else
                    { 
                    gameLevel++;
                    label2.Text = "you win ";
                    levelLabel.Text = $"Next Round :{gameLevel}";
                    startButton.Visible = true;
                    startButton.Enabled = true;
                    startButton.Text = $"Round {gameLevel}";
                    

                        }
                    break;

            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
           // GenerateValueLabelsFnction();
            
            gfx.Clear(Color.HotPink);
          /*
         if(Circle.sizes.Width >= 100)
            {
                gameLevel++;
                label2.Text = "you win ";
                levelLabel.Text = $"Next Round :{gameLevel}";
                
                startButton.Visible = true;
                startButton.Enabled = true;
                startButton.Text = $"Round {gameLevel}";
                timer1.Enabled = false;
            }
            */ 
            Circle.Update(ClientSize.Width,ClientSize.Height);

            for (int i = 0; i < Circles.Count; i++)
            {
                Circles[i].Draw(gfx);
            }
            Circle.Draw(gfx);
            WinOrLoseChecker();
            pictureBox.Image = canvas;

        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            for(int i =0; i < Circles.Count;i++)
            { 
                if (Circles[i].Hitbox.Contains(e.Location))
                {
                    Circles[i].grow = true;
                }
            }
        }

  

    }
}
