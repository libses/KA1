using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace KA1
{
    public enum Figure
    {
        Empty,
        Solider,
        Horse
    }

    public class VectorWrapper
    {
        public Vector2 Vector;
        public List<Vector2> Path;

        public VectorWrapper(Vector2 vector, List<Vector2> path)
        {
            Vector = vector;
            Path = path;
        }
    }

    internal class Program
    {
        public static List<Vector2> moves = new() { new Vector2(1, 2),
        new Vector2(-1, 2), new Vector2(-2, 1), new Vector2(-2, -1),
        new Vector2(-1, -2), new Vector2(1, -2), new Vector2(2, -1),
        new Vector2(2, 1)};
        static void Main(string[] args)
        {
            Figure[,] board = new Figure[8, 8];
            var first = Convert(Console.ReadLine());
            var second = Convert(Console.ReadLine());
            board[first.Item1, first.Item2] = Figure.Horse;
            board[second.Item1, second.Item2] = Figure.Solider;
            var path = BFS(new Vector2(first.Item1, first.Item2), board, new Vector2(second.Item1, second.Item2));
            Console.WriteLine();
            foreach (var node in path)
            {
                Console.WriteLine(Convert(node));
            }
        }

        public static IEnumerable<Vector2> GetValidMoves(Vector2 start, Figure[,] board)
        {
            return moves.Select(x => start + x).Where(x => IsMoveValid(x, board));
        }

        public static bool IsMoveValid(Vector2 vector, Figure[,] board)
        {
            var right = vector + new Vector2(1, 1);
            var left = vector + new Vector2(-1, 1);
            if (IsInsideBoard(right) && board[(int)right.X, (int)right.Y] == Figure.Solider) return false;
            if (IsInsideBoard(left) && board[(int)left.X, (int)left.Y] == Figure.Solider) return false;
            return IsInsideBoard(vector);
        }

        public static bool IsInsideBoard(Vector2 vector)
        {
            return vector.X < 8 && vector.Y < 8 && vector.X > -1 && vector.Y > -1;
        }

        public static (int, int) Convert(string chessNotation)
        {
            return (chessNotation[0] - 97, chessNotation[1] - 49);
        }

        public static string Convert(Vector2 vectorNotation)
        {
            return new string(new char[2] { (char)((int)vectorNotation.X + 97), (char)((int)vectorNotation.Y + 49) });
        }

        public static List<Vector2> BFS(Vector2 start, Figure[,] board, Vector2 target)
        {
            var queue = new Queue<VectorWrapper>();
            queue.Enqueue(new VectorWrapper(start, new List<Vector2>() { start}));
            while (queue.Count > 0)
            {
                var currentWrapper = queue.Dequeue();
                if (currentWrapper.Vector == target)
                {
                    return currentWrapper.Path;
                }

                var moves = GetValidMoves(currentWrapper.Vector, board);
                var validMoves = moves.Where(x => !currentWrapper.Path.Contains(x));
                foreach (var move in validMoves)
                {
                    queue.Enqueue(new VectorWrapper(move, currentWrapper.Path.Append(move).ToList()));
                }
            }

            return null;
        }
    }
}
