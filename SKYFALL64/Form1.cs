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

        int leftClick, rightClick, upClick, downClick = 0;
        bool leftDown, rightDown, upDown, downDown;

        SolidBrush greenBrush = new SolidBrush(Color.Lime);
        SolidBrush blackBrush = new SolidBrush(Color.Black);


        System.Windows.Media.MediaPlayer hit1 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer hit2 = new System.Windows.Media.MediaPlayer();

        //labyrinth variables
        string labyrinthKey = "";

        Rectangle labyrinthPlayer = new Rectangle();
        Rectangle labyrinthObjectiveBack = new Rectangle();
        Rectangle labyrinthObjectiveFront = new Rectangle();
        List<int> labyrinthTiles = new List<int>();
        int labyrinthTilesSize = 0;
        int grid = 0;
        string successfulLabyrinth = "false";
        List<int> labyrinthBot = new List<int>();

        ////labyrinth test visualized pt. 1
        //List<Rectangle> labyrinthTest = new List<Rectangle>();

        public Form1()
        {
            InitializeComponent();
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
            numHold = 0;
            shots.Clear();
            playerScore = 0;
            for(int i = 0; i < squaresActive.Count(); i++)
            {
                squaresActive[i] = 0;
            }
            gameEngine.Enabled = true;

            labyrinthKey = "";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if(gameState == "running" || gameState == "labyrinth")
                    {
                        leftDown = true;
                        leftClick++;
                    }
                    break;
                case Keys.Right:
                    if (gameState == "running" || gameState == "labyrinth")
                    {
                        rightDown = true;
                        rightClick++;
                    }
                    break;
                case Keys.Up:
                    if (gameState == "running" || gameState == "labyrinth")
                    {
                        upDown = true;
                        upClick++;
                    }
                    break;
                case Keys.Down:
                    if (gameState == "running" || gameState == "labyrinth")
                    {
                        downDown = true;
                        downClick++;
                    }
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
                case Keys.L:
                    if (gameState == "start" || gameState == "end")
                    {
                        if (labyrinthKey == "l")
                        {

                        }
                        else
                        {
                            labyrinthKey = "l";
                        }
                    }
                    break;
                case Keys.A:
                    if(labyrinthKey == "l")
                    {
                        labyrinthKey = "la";
                    }
                    else if(labyrinthKey == "la")
                    {

                    }
                    else
                    {
                        labyrinthKey = "";
                    }
                    break;
                case Keys.B:
                    if (labyrinthKey == "la")
                    {
                        labyrinthKey = "lab";
                    }
                    else if (labyrinthKey == "lab")
                    {

                    }
                    else
                    {
                        labyrinthKey = "";
                    }
                    break;
                case Keys.Y:
                    if (labyrinthKey == "lab")
                    {
                        labyrinthKey = "laby";
                    }
                    else if (labyrinthKey == "laby")
                    {

                    }
                    else
                    {
                        labyrinthKey = "";
                    }
                    break;
                case Keys.R:
                    if (labyrinthKey == "laby")
                    {
                        labyrinthKey = "labyr";
                    }
                    else if (labyrinthKey == "labyr")
                    {

                    }
                    else
                    {
                        labyrinthKey = "";
                    }
                    break;
                case Keys.I:
                    if (labyrinthKey == "labyr")
                    {
                        labyrinthKey = "labyri";
                    }
                    else if (labyrinthKey == "labyri")
                    {

                    }
                    else
                    {
                        labyrinthKey = "";
                    }
                    break;
                case Keys.N:
                    if (labyrinthKey == "labyri")
                    {
                        labyrinthKey = "labyrin";
                    }
                    else if (labyrinthKey == "labyrin")
                    {

                    }
                    else
                    {
                        labyrinthKey = "";
                    }
                    break;
                case Keys.T:
                    if (labyrinthKey == "labyrin")
                    {
                        labyrinthKey = "labyrint";
                    }
                    else if (labyrinthKey == "labyrint")
                    {

                    }
                    else
                    {
                        labyrinthKey = "";
                    }
                    break;
                case Keys.H:
                    if (labyrinthKey == "labyrint")
                    {
                        labyrinthKey = "labyrinth";
                        gameState = "labyrinth";
                        LabyrinthStart();
                    }
                    else if (labyrinthKey == "labyrinth")
                    {

                    }
                    else
                    {
                        labyrinthKey = "";
                    }
                    break;
                default:
                    labyrinthKey = "";
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
                    greenBrush = new SolidBrush(Color.Lime);
                    scoreLabel.ForeColor = Color.Lime;
                    gameState = "end";
                    gameEngine.Enabled = false;
                }
            }

            Refresh();
        }

        public void LabyrinthStart()
        {

            labyrinthKey = "";
            successfulLabyrinth = "false";
            while (successfulLabyrinth == "false")
            {
                labyrinthTiles.Clear();
                labyrinthBot.Clear();
                switch (difficulty)
                {
                    case "EASY":
                        grid = 12;
                        labyrinthTilesSize = 60;
                        LabyrinthMaker();
                        break;
                    case "MEDIUM":
                        grid = 22;
                        labyrinthTilesSize = 30;
                        LabyrinthMaker();
                        break;
                    case "HARD":
                        grid = 32;
                        labyrinthTilesSize = 20;
                        LabyrinthMaker();
                        break;
                }
            }
            ////labyrinth test visualized pt. 2
            //for (int i = 0; i < grid; i++)
            //{
            //    for (int ii = 0; ii < grid; ii++)
            //    {
            //        labyrinthTest.Add(new Rectangle(ii * labyrinthTilesSize - labyrinthTilesSize, i * labyrinthTilesSize - labyrinthTilesSize, labyrinthTilesSize, labyrinthTilesSize));
            //    }
            //}
            labyrinthPlayer = new Rectangle(0, 0, labyrinthTilesSize, labyrinthTilesSize);
            labyrinthObjectiveBack = new Rectangle((grid - 3) * labyrinthTilesSize, (grid - 3) * labyrinthTilesSize, labyrinthTilesSize, labyrinthTilesSize);
            labyrinthObjectiveFront = new Rectangle((grid - 3) * labyrinthTilesSize + 5, (grid - 3) * labyrinthTilesSize + 5, labyrinthTilesSize - 10, labyrinthTilesSize - 10);
            gameState = "labyrinth";
            labyrinthEngine.Enabled = true;
        }

        public void LabyrinthMaker()
        {
            for (int i = 0; i < grid * grid; i++)
            {
                labyrinthTiles.Add(randGen.Next(0, 2));
            }
            for (int i = 0; i < grid; i++)
            {
                labyrinthTiles[i] = 1;
                labyrinthTiles[grid * grid - 1 - i] = 1;
                labyrinthTiles[i * grid] = 1;
                labyrinthTiles[i * grid + (grid - 1)] = 1;
            }
            labyrinthTiles[grid + 1] = 2;
            labyrinthTiles[grid * grid - grid - 2] = 0;
            labyrinthBot.Add(grid + 1);
            for (int i = 0; i < labyrinthBot.Count; i++)
            {
                if (labyrinthTiles[labyrinthBot[i] + 1] == 0 && !labyrinthBot.Contains(labyrinthBot[i] + 1))
                {
                    labyrinthBot.Add(labyrinthBot[i] + 1);
                }
                if (labyrinthTiles[labyrinthBot[i] - 1] == 0 && !labyrinthBot.Contains(labyrinthBot[i] - 1))
                {
                    labyrinthBot.Add(labyrinthBot[i] - 1);
                }
                if (labyrinthTiles[labyrinthBot[i] + grid] == 0 && !labyrinthBot.Contains(labyrinthBot[i] + grid))
                {
                    labyrinthBot.Add(labyrinthBot[i] + grid);
                }
                if (labyrinthTiles[labyrinthBot[i] - grid] == 0 && !labyrinthBot.Contains(labyrinthBot[i] - grid))
                {
                    labyrinthBot.Add(labyrinthBot[i] - grid);
                }
            }
            if (labyrinthBot.Contains(grid * grid - grid - 2) || labyrinthBot.Contains(grid * (grid - 1) - grid - 1))
            {
                successfulLabyrinth = "true";
            }

            //successfulLabyrinth = "true";
        }

        private void labyrinthEngine_Tick(object sender, EventArgs e)
        {
            //move player
            if (upDown == true)
            {
                int i = 0;
                while (upClick != 0)
                {

                    if (labyrinthTiles[i] == 2 && labyrinthTiles[i - grid] == 0)
                    {
                        labyrinthTiles[i] = 0;
                        labyrinthTiles[i - grid] += 2;
                        //move player rectangle
                        labyrinthPlayer = new Rectangle(labyrinthPlayer.X, labyrinthPlayer.Y - labyrinthTilesSize, labyrinthTilesSize, labyrinthTilesSize);
                        upClick = 0;
                    }
                    if (i == labyrinthTiles.Count - 1)
                    {
                        upClick = 0;
                    }
                    i++;
                }

            }
            if (downDown == true)
            {
                int i = 0;
                while(downClick != 0)
                {
                    
                        if (labyrinthTiles[i] == 2 && labyrinthTiles[i + grid] == 0)
                        {
                            labyrinthTiles[i] = 0;
                            labyrinthTiles[i + grid] += 2;
                            //move player rectangle
                            labyrinthPlayer = new Rectangle(labyrinthPlayer.X, labyrinthPlayer.Y + labyrinthTilesSize, labyrinthTilesSize, labyrinthTilesSize);
                            downClick = 0;
                        }
                        if(i == labyrinthTiles.Count - 1)
                        {
                            downClick = 0;
                        }
                    i++;
                }
                
            }
            if (leftDown == true)
            {
                int i = 0;
                while (leftClick != 0)
                {

                    if (labyrinthTiles[i] == 2 && labyrinthTiles[i - 1] == 0)
                    {
                        labyrinthTiles[i] = 0;
                        labyrinthTiles[i - 1] += 2;
                        //move player rectangle
                        labyrinthPlayer = new Rectangle(labyrinthPlayer.X - labyrinthTilesSize, labyrinthPlayer.Y, labyrinthTilesSize, labyrinthTilesSize);
                        leftClick = 0;
                    }
                    if (i == labyrinthTiles.Count - 1)
                    {
                        leftClick = 0;
                    }
                    i++;
                }

            }
            if (rightDown == true)
            {
                int i = 0;
                while (rightClick != 0)
                {

                    if (labyrinthTiles[i] == 2 && labyrinthTiles[i + 1] == 0)
                    {
                        labyrinthTiles[i] = 0;
                        labyrinthTiles[i + 1] += 2;
                        //move player rectangle
                        labyrinthPlayer = new Rectangle(labyrinthPlayer.X + labyrinthTilesSize, labyrinthPlayer.Y, labyrinthTilesSize, labyrinthTilesSize);
                        rightClick = 0;
                    }
                    if (i == labyrinthTiles.Count - 1)
                    {
                        rightClick = 0;
                    }
                    i++;
                }

            }

            //check if player completed labyrinth
            for(int i = 0; i < labyrinthTiles.Count; i++)
            {
                if(labyrinthTiles[grid * grid - grid - 2] == 2)
                {
                    gameState = "start";
                    labyrinthEngine.Enabled = false;

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
                case "labyrinth":
                    titleLabel.Text = "";
                    subtitleLabel.Text = "";
                    difficultyLabel.Text = "";
                    scoreLabel.Text = "";

                    ////labyrinth test visualized pt.3 a) BOT
                    //for (int i = 0; i < labyrinthTiles.Count; i++)
                    //{
                    //    if (labyrinthBot.Contains(i))
                    //    {
                    //        e.Graphics.FillRectangle(greenBrush, labyrinthTest[i]);
                    //    }
                    //}
                    ////labyrinth test visualized pt.3 b) WALLS
                    //for (int i = 0; i < labyrinthTiles.Count; i++)
                    //{
                    //    if (labyrinthTiles[i] == 1)
                    //    {
                    //        e.Graphics.FillRectangle(greenBrush, labyrinthTest[i]);
                    //    }
                    //}

                    //draw player and objective
                    e.Graphics.FillRectangle(greenBrush, labyrinthPlayer);
                    e.Graphics.FillRectangle(greenBrush, labyrinthObjectiveBack);
                    e.Graphics.FillRectangle(blackBrush, labyrinthObjectiveFront);

                    break;
            }
        }
    }
}
