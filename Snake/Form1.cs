using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging;

namespace Snake
{
    public partial class Snake : Form
    {
        private List<Circle> snake = new List<Circle>();
        private Circle food = new Circle();

        int maxWidth;
        int maxHeight;

        int score;
        int highscore;

        Random rand = new Random();
        bool goLeft, goRight, goUp, goDown;

        public Snake()
        {
            InitializeComponent();
            new Settings();
        }    

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Left && Settings.Directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.Directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.Directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.Directions != "up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I scored: " + score + " and my Highscore is: " + highscore + ".";
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.DarkBlue;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game by Lio";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp,new Rectangle(0,0,width,height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (goLeft)
            {
                Settings.Directions = "left";
            }
            if (goRight)
            {
                Settings.Directions = "right";
            }
            if (goUp)
            {
                Settings.Directions = "up";
            }
            if (goDown)
            {
                Settings.Directions = "down";
            }

            for(int i = snake.Count-1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.Directions)
                    {
                        case "left":
                            snake[i].X--;
                            break;
                        case "right":
                            snake[i].X++;
                            break;
                        case "up":
                            snake[i].Y--;
                            break;
                        case "down":
                            snake[i].Y++;
                            break;
                    }
                    if (snake[i].X < 0) { snake[i].X = maxWidth; }
                    if (snake[i].X > maxWidth) { snake[i].X = 0; }
                    if (snake[i].Y < 0) { snake[i].Y = maxHeight; }
                    if (snake[i].Y > maxHeight) { snake[i].Y = 0; }

                    if(snake[i].X==food.X && snake[i].Y == food.Y)
                    {
                        EatFood();
                    }
                    for(int j = 1; j < snake.Count; j++)
                    {
                        if(snake[i].X==snake[j].X && snake[i].Y == snake[j].Y)
                        {
                            GameOver();
                        }
                    }
                }
                else
                {
                    snake[i].X = snake[i - 1].X;
                    snake[i].Y = snake[i - 1].Y;
                }
            }

            picCanvas.Invalidate();

        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            Brush snakeColor;
            for(int i = 0; i < snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColor = Brushes.Red;
                }
                else
                {
                    snakeColor = Brushes.Orange;
                }

                canvas.FillEllipse(snakeColor,new Rectangle(
                    snake[i].X*Settings.Width,
                    snake[i].Y * Settings.Height,
                    Settings.Width,
                    Settings.Height
                    ));
            }
            canvas.FillEllipse(Brushes.Navy, new Rectangle(
                    food.X * Settings.Width,
                    food.Y * Settings.Height,
                    Settings.Width,
                    Settings.Height
                    ));
        }
       
        private void RestartGame()
        {
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;

            snake.Clear();

            Start_Button.Enabled = false;
            Snap_Button.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;

            Circle head = new Circle(10, 5);
            snake.Add(head);
            for(int i = 0; i < 5; i++)
            {
                Circle body = new Circle();
                snake.Add(body);
            }
            food = new Circle(rand.Next(2, maxWidth), rand.Next(2, maxHeight));
            Gametimer.Start();
        }

        private void EatFood()
        {
            score += 1;
            txtScore.Text = "Score: " + score;
            Circle body = new Circle(snake[snake.Count - 1].X, snake[snake.Count - 1].Y);
            snake.Add(body);
            food = new Circle(rand.Next(2, maxWidth), rand.Next(2, maxHeight));
        }

        private void GameOver()
        {
            Gametimer.Stop();
            Start_Button.Enabled = true;
            Snap_Button.Enabled = true;

            if (score > highscore)
            {
                highscore = score;
                txtHighscore.Text = "High Score: " + Environment.NewLine + highscore;
                txtHighscore.ForeColor = Color.Magenta;
                txtHighscore.TextAlign = ContentAlignment.MiddleCenter;
                  
            }
        }


    }
}
