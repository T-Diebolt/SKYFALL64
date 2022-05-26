using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace SKYFALL64
{
    public partial class Form1 : Form
    {
        Random randGen = new Random();
        int numHold = 0;

        Rectangle coverBack = new Rectangle(450, 0, 150, 600);
        Rectangle coverLine = new Rectangle(450, 0, 5, 600);
        Rectangle[] player = new Rectangle[4];
        int playerSpeed = 50;

        List<Rectangle> shots = new List<Rectangle>();
        int shotSpeed = 50;

        Rectangle[] squares = new Rectangle[99];
        int obstacleCount = 0;
        int obstacleSpeed = 0;
        int[] squaresActive = new int[99];

        int playerScore = 0;

        string difficulty = "MEDIUM";
        string gameState = "start";

        int leftClick, rightClick, upClick = 0;
        bool leftDown, rightDown, upDown;

        SolidBrush greenBrush = new SolidBrush(Color.Lime);
        SolidBrush blackBrush = new SolidBrush(Color.Black);


        System.Windows.Media.MediaPlayer hit1 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer hit2 = new System.Windows.Media.MediaPlayer();

        public Form1()
        {
            InitializeComponent();
            player[0] = new Rectangle(205, 505, 40, 40);
            player[1] = new Rectangle(155, 555, 40, 40);
            player[2] = new Rectangle(205, 555, 40, 40);
            player[3] = new Rectangle(255, 555, 40, 40);

            for (int i = 0; i < 11; i++)
            {
                for (int ii = 0; ii < 9; ii++)
                {
                    squares[numHold] = new Rectangle(ii * 50 + 5, i * 50 + 5, 40, 40);
                    numHold++;
                }
            }

            hit1.Open(new Uri(Application.StartupPath + "/Resources/softHit.wav"));
            hit2.Open(new Uri(Application.StartupPath + "/Resources/hardHit.wav"));
        }

        public void GameStart()
        {
            player[0] = new Rectangle(205, 505, 40, 40);
            player[1] = new Rectangle(155, 555, 40, 40);
            player[2] = new Rectangle(205, 555, 40, 40);
            player[3] = new Rectangle(255, 555, 40, 40);

            gameState = "running";
            leftClick = 0;
            rightClick = 0;
            upClick = 0;
            obstacleCount = 0;
            shots.Clear();
            playerScore = 0;
            for(int i = 0; i < squaresActive.Count(); i++)
            {
                squaresActive[i] = 0;
            }
            gameEngine.Enabled = true;

            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = true;
                    leftClick++;
                    break;
                case Keys.Right:
                    rightDown = true;
                    rightClick++;
                    break;
                case Keys.Up:
                    upDown = true;
                    upClick++;
                    break;
                case Keys.Space:
                    if (gameState == "start" || gameState == "end")
                    {
                        gameState = "running";
                        GameStart();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "start" || gameState == "end")
                    {
                        Application.Exit();
                    }
                    break;
                case Keys.D:
                    if(gameState == "start" || gameState == "end")
                    {
                        switch (difficulty)
                        {
                            case "MEDIUM":
                                difficulty = "HARD";
                                break;
                            case "HARD":
                                difficulty = "EASY";
                                break;
                            default:
                                difficulty = "MEDIUM";
                                break;
                        }
                    }
                    Refresh();
                    break;
            }
        }

        private void gameEngine_Tick(object sender, EventArgs e)
        {
            //establish speeds
            switch (difficulty)
            {
                case "MEDIUM":
                    obstacleSpeed = 30;
                    break;
                case "HARD":
                    obstacleSpeed = 18;
                    break;
                default:
                    obstacleSpeed = 42;
                    break;
            }

            //move player
            for (int i = 0; i < player.Length; i++)
            {
                if (leftDown == true && player[3].X > 55 && leftClick == 1)
                {
                    int x = player[i].X - playerSpeed;
                    player[i] = new Rectangle(x, player[i].Y, 40, 40);
                }

                if (rightDown == true && player[3].X < this.Width - player[i].Width - 105 && rightClick == 1)
                {
                    int x = player[i].X + playerSpeed;
                    player[i] = new Rectangle(x, player[i].Y, 40, 40);
                }
            }

            //player shoots new square
            if (upDown == true && upClick == 1)
            {
                shots.Add(new Rectangle(player[0].X, player[0].Y - 50, 40, 40));
            }

            //make buttons click not hold
            leftClick = 0;
            rightClick = 0;
            upClick = 0;

            //obstacles
            obstacleCount++;

            if (obstacleCount == obstacleSpeed)
            {
                //make obstacles move
                for (int i = 98; i > 8; i--)
                {
                    squaresActive[i] = squaresActive[i - 9];
                }

                //make obstacles appear
                for (int i = 0; i < 9; i++)
                {
                    numHold = randGen.Next(1, 21);
                    if (numHold < 12)
                    {
                        squaresActive[i] = 1;
                    }
                    else
                    {
                        squaresActive[i] = 0;
                    }
                }


                obstacleCount = 0;
            }

            //shot stops if necessary
            for (int i = 0; i < squares.Length; i++)
            {
                for (int ii = 0; ii < shots.Count; ii++)
                {
                    if (shots[ii].X == squares[i].X && shots[ii].Y == squares[i].Y + 50 && squaresActive[i] == 1)
                    {
                        squaresActive[i + 9] = 1;
                        shots.Remove(shots[ii]);
                    }
                    else if (shots[ii].Y == 5 && shots[ii].X == squares[i].X)
                    {
                        squaresActive[i] = 1;
                        shots.Remove(shots[ii]);
                    }
                }
            }

            //shot moves
            if (shots.Count > 0)
            {
                for (int i = 0; i < shots.Count; i++)
                {
                    int y = shots[i].Y - shotSpeed;
                    shots[i] = new Rectangle(shots[i].X, y, 40, 40);
                }

            }

            

            //check to see if player has cleared row
            for (int i = 0; i < 11; i++)
            {
                numHold = 0;
                for(int ii = 0; ii < 9; ii++)
                {
                    if(squaresActive[i * 9 + ii] == 1)
                    {
                        numHold++;
                    }
                }
                //clear row
                if(numHold == 9)
                {
                    for(int ii = 0; ii < 9; ii++)
                    {
                        squaresActive[i * 9 + ii] = 0;
                        
                        for(int iii = 0; iii < 10 - i; iii++)
                        {
                            squaresActive[i * 9 + ii + iii * 9] = squaresActive[i * 9 + ii + iii * 9 + 9];
                        }
                        
                    }
                    playerScore = playerScore + 100;
                    var pointSound = new System.Windows.Media.MediaPlayer();
                    pointSound.Open(new Uri(Application.StartupPath + "/Resources/Point Get.wav"));
                    pointSound.Play();
                }

            }

            //check to see if player has died
            for(int i = 0; i < 9; i++)
            {
                if (squaresActive[98 - i] == 1)
                {
                    Refresh();
                    hit1.Stop();
                    hit1.Play();
                    Thread.Sleep(1500);
                    greenBrush = new SolidBrush(Color.Red);
                    scoreLabel.ForeColor = Color.Red;
                    hit2.Stop();
                    hit2.Play();
                    Refresh();
                    Thread.Sleep(1500);
                    greenBrush = new SolidBrush(Color.Red);
                    scoreLabel.ForeColor = Color.Lime;
                    gameState = "end";
                    gameEngine.Enabled = false;
                }
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            switch (gameState)
            {
                case "start":
                    //write titles
                    titleLabel.Text = "SKYFALL64";
                    subtitleLabel.Text = "Press space to play,\nd to change difficulty,\nor escape to exit.";
                    difficultyLabel.Text = $"Difficulty: {difficulty}";
                    scoreLabel.Text = "";
                    break;
                case "running":
                    ////test draw squares
                    //for(int i = 0; i < 99; i++)
                    //{
                    //    e.Graphics.FillRectangle(greenBrush, squares[i]);
                    //}

                    //draw squares
                    for (int i = 0; i < 99; i++)
                    {
                        if (squaresActive[i] == 1)
                        {
                            e.Graphics.FillRectangle(greenBrush, squares[i]);
                        }
                        else
                        {
                            e.Graphics.FillRectangle(blackBrush, squares[i]);
                        }
                    }

                    //draw player
                    for (int i = 0; i < player.Length; i++)
                    {
                        e.Graphics.FillRectangle(greenBrush, player[i]);
                    }
                    
                    //draw shots
                    for(int i = 0; i < shots.Count; i++)
                    {
                        e.Graphics.FillRectangle(greenBrush, shots[i]);
                    }

                    //draw cover
                    e.Graphics.FillRectangle(blackBrush, coverBack);
                    e.Graphics.FillRectangle(greenBrush, coverLine);

                    //erase titles
                    titleLabel.Text = "";
                    subtitleLabel.Text = "";
                    difficultyLabel.Text = "";
                    scoreLabel.Text = $"Score:\n{playerScore}";

                    break;
                case "end":
                    //write titles
                    titleLabel.Text = $"SCORE: {playerScore}";
                    subtitleLabel.Text = "Press space to play,\nd to change difficulty,\nor escape to exit.";
                    difficultyLabel.Text = $"Difficulty: {difficulty}";
                    scoreLabel.Text = "";
                    break;
            }
        }
    }
}
