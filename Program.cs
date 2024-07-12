using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;

namespace Snake
{
    class Program
    {
        private static Random rand = new Random();

        private static int score = 5;

        private static Pixel head;
        private static Pixel berry;

        private static List<Pixel> body = new List<Pixel>();

        private static Direction currentMovement = Direction.Right;

        private static bool gameOver = false;

        static void Main()
        {
            InitializeGame();
            while (!gameOver)
            {
                Clear();
                DrawBorder();
                CheckCollisions();
                UpdateGame();
            }
            DisplayGameOver();
        }

        static void InitializeGame()
        {
            WindowHeight = 16;
            WindowWidth = 32;
            head = new Pixel(WindowWidth / 2, WindowHeight / 2, ConsoleColor.Red);
            berry = new Pixel(rand.Next(1, WindowWidth - 2), rand.Next(1, WindowHeight - 2), ConsoleColor.Cyan);
        }

        static void CheckCollisions()
        {
            gameOver |= (head.XPos == WindowWidth - 1 || head.XPos == 0 || head.YPos == WindowHeight - 1 || head.YPos == 0);

            if (berry.XPos == head.XPos && berry.YPos == head.YPos)
            {
                score++;
                berry = new Pixel(rand.Next(1, WindowWidth - 2), rand.Next(1, WindowHeight - 2), ConsoleColor.Cyan);
            }

            foreach (var segment in body)
            {
                gameOver |= (segment.XPos == head.XPos && segment.YPos == head.YPos);
                DrawPixel(segment);
            }

            DrawPixel(head);
            DrawPixel(berry);
        }

        static void UpdateGame()
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds <= 500)
            {
                currentMovement = ReadMovement(currentMovement);
            }

            body.Add(new Pixel(head.XPos, head.YPos, ConsoleColor.Green));
            MoveHead();

            if (body.Count > score)
            {
                body.RemoveAt(0);
            }
        }

        static void MoveHead()
        {
            switch (currentMovement)
            {
                case Direction.Up: head.YPos--; break;
                case Direction.Down: head.YPos++; break;
                case Direction.Left: head.XPos--; break;
                case Direction.Right: head.XPos++; break;
            }
        }

        static Direction ReadMovement(Direction movement)
        {
            if (KeyAvailable)
            {
                var key = ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow && movement != Direction.Down) movement = Direction.Up;
                else if (key == ConsoleKey.DownArrow && movement != Direction.Up) movement = Direction.Down;
                else if (key == ConsoleKey.LeftArrow && movement != Direction.Right) movement = Direction.Left;
                else if (key == ConsoleKey.RightArrow && movement != Direction.Left) movement = Direction.Right;
            }
            return movement;
        }

        static void DrawPixel(Pixel pixel)
        {
            SetCursorPosition(pixel.XPos, pixel.YPos);
            ForegroundColor = pixel.ScreenColor;
            Write("■");
            SetCursorPosition(0, 0);
        }

        static void DrawBorder()
        {
            for (int i = 0; i < WindowWidth; i++)
            {
                DrawBorderPixel(i, 0);
                DrawBorderPixel(i, WindowHeight - 1);
            }

            for (int i = 0; i < WindowHeight; i++)
            {
                DrawBorderPixel(0, i);
                DrawBorderPixel(WindowWidth - 1, i);
            }
        }

        static void DrawBorderPixel(int x, int y)
        {
            SetCursorPosition(x, y);
            Write("■");
        }

        static void DisplayGameOver()
        {
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            WriteLine($"Game over, Score: {score - 5}");
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 1);
            ReadKey();
        }

        struct Pixel
        {
            public Pixel(int xPos, int yPos, ConsoleColor color)
            {
                XPos = xPos;
                YPos = yPos;
                ScreenColor = color;
            }
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ScreenColor { get; set; }
        }

        enum Direction
        {
            Up,
            Down,
            Right,
            Left
        }
    }
}
