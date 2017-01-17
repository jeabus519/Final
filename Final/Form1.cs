using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Final
{
    public partial class Form1 : Form
    {
        //image declarations and stuff
        Image upStand = Properties.Resources.up_stand;
        Image rightStand = Properties.Resources.right_stand;
        Image leftStand = Properties.Resources.left_stand;
        Image downStand = Properties.Resources.down_stand;
        Image upStep = Properties.Resources.up;
        Image rightStep = Properties.Resources.right;
        Image leftStep = Properties.Resources.left;
        Image downStep = Properties.Resources.down;

        //initial starting values for Hero character and other stuff
        int xHero = 336;
        int yHero = 216;
        int speedHero = 3;
        int widthHero = 40;
        int heightHero = 40;
        bool moving = false;
        bool legal = true;
        string direction = "down";
        int timer;

        //sets rectangle around the obstacles
        Rectangle leftObstacle = new Rectangle(168, 168, 94, 144);
        Rectangle rightObstacle = new Rectangle(456, 168, 94, 144);
        Rectangle legalSpace = new Rectangle(111, 111, 501, 261);

        //determines whether a key is being pressed or not - DO NOT CHANGE
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown;

        //create graphic objects
        SolidBrush drawBrush = new SolidBrush(Color.Black);

        public Form1()
        {
            InitializeComponent();

            //start the timer when the program starts
            gameTimer.Enabled = true;
            gameTimer.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    moving = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    moving = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    moving = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    moving = true;
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    moving = false;
                    timer = 0;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    moving = false;
                    timer = 0;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    moving = false;
                    timer = 0;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    moving = false;
                    timer = 0;
                    break;
                default:
                    break;
            }
        }

        public void checkCollision(Rectangle player)
        {
            if (!(player.IntersectsWith(legalSpace))
                || player.IntersectsWith(leftObstacle)
                || player.IntersectsWith(rightObstacle))
                
            {
                legal = false;
            }
            else
            {
                legal = true;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //sets player hitbox
            Rectangle player; 

            //

            #region move character based on key presses

            if (leftArrowDown == true)
            {
                xHero = xHero - speedHero;

                player = new Rectangle(xHero, yHero, widthHero, heightHero);
                checkCollision(player);
              
                if (!legal)
                {
                    xHero = xHero + speedHero;
                }
                direction = "left";
            }

            if (downArrowDown == true)
            {
                yHero = yHero + speedHero;
                player = new Rectangle(xHero, yHero, widthHero, heightHero);
                checkCollision(player);

                if (!legal)
                {
                    yHero = yHero - speedHero;
                }
                direction = "down";
            }

            if (rightArrowDown == true)
            {
                xHero = xHero + speedHero;
                player = new Rectangle(xHero, yHero, widthHero, heightHero);
                checkCollision(player);

                if (!legal)
                {
                    xHero = xHero - speedHero;
                }
                direction = "right";
            }

            if (upArrowDown == true)
            {
                yHero = yHero - speedHero;
                player = new Rectangle(xHero, yHero, widthHero, heightHero);
                checkCollision(player);

                if (!legal)
                {
                    yHero = yHero + speedHero;
                }
                direction = "up";
            }

            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw sprite
            if (direction == "up")
            {
                if(timer <= 10)
                {
                    e.Graphics.DrawImage(upStand, xHero + 2, yHero - 4, 36, 48);
                }
                else if (timer <= 19)
                {
                    e.Graphics.DrawImage(upStep, xHero + 2, yHero - 4, 36, 48);
                }
                else
                {
                    e.Graphics.DrawImage(upStep, xHero + 2, yHero - 4, 36, 48);
                    timer = 0;
                }
            }
            else if (direction == "right")
            {
                if (timer <= 10)
                {
                    e.Graphics.DrawImage(rightStand, xHero + 2, yHero - 4, 36, 48);
                }
                else if (timer <= 19)
                {
                    e.Graphics.DrawImage(rightStep, xHero + 2, yHero - 4, 36, 48);
                }
                else
                {
                    e.Graphics.DrawImage(rightStep, xHero + 2, yHero - 4, 36, 48);
                    timer = 0;
                }
            }
            else if (direction == "left")
            {
                if (timer <= 10)
                {
                    e.Graphics.DrawImage(leftStand, xHero + 2, yHero - 4, 36, 48);
                }
                else if (timer <= 19)
                {
                    e.Graphics.DrawImage(leftStep, xHero + 2, yHero - 4, 36, 48);
                }
                else
                {
                    e.Graphics.DrawImage(leftStep, xHero + 2, yHero - 4, 36, 48);
                    timer = 0;
                }
            }
            else
            {
                if (timer <= 10)
                {
                    e.Graphics.DrawImage(downStand, xHero + 2, yHero - 4, 36, 48);
                }
                else if (timer <= 19)
                {
                    e.Graphics.DrawImage(downStep, xHero + 2, yHero - 4, 36, 48);
                }
                else
                {
                    e.Graphics.DrawImage(downStep, xHero + 2, yHero - 4, 36, 48);
                    timer = 0;
                }
            }
            if (moving)
            {
                timer++;
            }
        }
    }
}
