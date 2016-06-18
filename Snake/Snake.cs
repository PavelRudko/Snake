using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class Snake
    {
        private int cellSize, offset;
        private LinkedList<Point> nodes = new LinkedList<Point>();
        private Point direction = new Point(0, 1);
        private Point newDirection = new Point(0, 1);

        public Snake(int cellSize, int row, int column)
        {
            this.cellSize = cellSize;
            var head = new Point(column, row);
            nodes.AddFirst(head);
            nodes.AddLast(head);
        }

        public void Update()
        {
            offset += 1;
            if(offset >= cellSize)
            {
                
                offset = 0;
                nodes.RemoveLast();
                var head = nodes.First.Value;
                head.Offset(direction.X, direction.Y);
                nodes.First.Value = head;
                nodes.AddFirst(head);

                if (direction.X != -newDirection.X && direction.Y != newDirection.Y)
                {
                    direction = newDirection;
                }
            }
        }

        public int GetLength()
        {
            return nodes.Count - 1;
        }

        public void OnKeyPress(Keys key)
        {
            switch(key)
            {
                case Keys.Up:
                    newDirection = new Point(0, -1);
                    break;
                case Keys.Down:
                    newDirection = new Point(0, 1);
                    break;
                case Keys.Left:
                    newDirection = new Point(-1, 0);
                    break;
                case Keys.Right:
                    newDirection = new Point(1, 0);
                    break;
            }
        }

        public Point GetHead()
        {
            var head = nodes.First.Value;
            head.Offset(direction.X, direction.Y);
            return head;
        }

        public bool Intersects(int x, int y)
        {
            foreach(var node in nodes)
            {
                if(node.X == x && node.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public void Grow()
        {
            var head = nodes.First.Value;
            head.Offset(direction.X, direction.Y);
            nodes.First.Value = head;
            nodes.AddFirst(head);
        }

        public void Draw(Graphics g)
        {
            var node = nodes.First;

            DrawNodeWithOffset(g, node.Value, direction);

            for(node = node.Next; node != nodes.Last; node = node.Next)
            {
                var point = node.Value;
                g.FillRectangle(Brushes.Cyan, point.X * cellSize, point.Y * cellSize, cellSize, cellSize);
            }

            DrawNodeWithOffset(g, node.Value, GetTailDirection());
           
        }

        private void DrawNodeWithOffset(Graphics g, Point point, Point direction)
        {
            var position = new Point(point.X * cellSize, point.Y * cellSize);
            var pointOffset = Multiply(direction, offset);
            position.Offset(pointOffset.X, pointOffset.Y);
            g.FillRectangle(Brushes.Cyan, position.X, position.Y, cellSize, cellSize);
        }

        private Point PointToScreen(Point point)
        {
            return Multiply(point, cellSize);
        }

        private Point Multiply(Point point, float c)
        {
            return new Point((int)(point.X * c), (int)(point.Y * c));
        }

        private Point GetTailDirection()
        {
            if(nodes.Count < 3)
            {
                return direction;
            }

            var tail = nodes.Last;
            var next = tail.Previous.Value;

            return new Point(next.X - tail.Value.X, next.Y - tail.Value.Y);
        }

    }
}
