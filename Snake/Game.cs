﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class Game
    {
        private static Random random = new Random();

        private int rowCount, columnCount, cellSize;
        private bool gameOver;
        private Snake snake;
        private Point food;

        public Game(int rowCount, int columnCount, int cellSize)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.cellSize = cellSize;
            Initialize();
        }

        public bool GameOver { get { return gameOver; } }

        public int GetScore()
        {
            return snake.GetLength();
        }

        public void Initialize()
        {
            gameOver = false;
            snake = new Snake(cellSize, rowCount / 2, columnCount / 2);
            GenerateFood();
        }

        public void Update()
        {
            if(gameOver)
            {
                return;
            }

            snake.Update();

            var head = snake.GetHead();
            gameOver = IsGameOver(head);
            if(head.Equals(food))
            {
                snake.Grow();
                GenerateFood();
            }
        }

        public void OnKeyPress(Keys key)
        {
            snake.OnKeyPress(key);
        }

        public void Draw(Graphics g)
        {
            g.Clear(Color.Black);
            snake.Draw(g);
            g.FillRectangle(Brushes.Fuchsia, food.X * cellSize, food.Y * cellSize, cellSize, cellSize);
            if(gameOver)
            {
                DrawCenteredString(g, Color.White, "You lose");
            }
        }

        private void DrawCenteredString(Graphics g, Color color, string text)
        {
            using (var font = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                g.DrawString(text, font, new SolidBrush(color), g.VisibleClipBounds, stringFormat);
            }
        }

        private void GenerateFood()
        {
            int row, column;
            while(true)
            {
                column = random.Next(columnCount - 1);
                row = random.Next(rowCount - 1);
                if(!snake.Intersects(column, row))
                {
                    food = new Point(column, row);
                    break;
                }
            }
        }

        private bool IsGameOver(Point head)
        {
            return snake.Intersects(head.X, head.Y)
                || head.X < 0
                || head.X > columnCount - 1
                || head.Y < 0
                || head.Y > rowCount - 1;
        }

    }
}
