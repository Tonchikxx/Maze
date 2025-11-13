using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class MazeGenerator
    {
        private const int width = 51;
        private const int height = 21;
        private const char wall = '█';
        private const char path = ' ';
        private const char player = 'P';
        private const char exit = 'E';

        private char[,] maze;
        private Random random;
        private (int x, int y) playerPos;
        private (int x, int y) exitPos;

        public MazeGenerator()
        {
            maze = new char[width, height];
            random = new Random();
        }

        public void GenerateMaze()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maze[x, y] = wall;
                }
            }

            int startX = 1;
            int startY = 1;

            CarvePassage(startX, startY);

            playerPos = (1, 1);
            maze[1, 1] = path;
            maze[0, 1] = path; 


            exitPos = (width - 2, height - 2);
            maze[exitPos.x, exitPos.y] = exit;
            maze[exitPos.x + 1, exitPos.y] = path; 
        }

        private void CarvePassage(int x, int y)
        {
            maze[x, y] = path;


            int[] dx = { 0, 2, 0, -2 };
            int[] dy = { -2, 0, 2, 0 };

            List<int> directions = new List<int> { 0, 1, 2, 3 };
            ShuffleDirections(directions);

            foreach (int direction in directions)
            {
                int newX = x + dx[direction];
                int newY = y + dy[direction];

                if (IsInBounds(newX, newY) && maze[newX, newY] == wall)
                {
                    maze[x + dx[direction] / 2, y + dy[direction] / 2] = path;
                    CarvePassage(newX, newY);
                }
            }
        }

        private void ShuffleDirections<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private static bool IsInBounds(int x, int y) 
        {
            return x > 0 && x < width - 1 && y > 0 && y < height - 1;
        }

        public void DisplayMaze()
        {
            Console.Clear();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == playerPos.x && y == playerPos.y)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(player);
                        Console.ResetColor();
                    }
                    else if (x == exitPos.x && y == exitPos.y)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(exit);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(maze[x, y]);
                    }
                }
                Console.WriteLine();
            }

        }

        public bool MovePlayer(ConsoleKey key)
        {
            (int x, int y) = playerPos; 

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    y--;
                    break;
                case ConsoleKey.DownArrow:
                    y++;
                    break;
                case ConsoleKey.LeftArrow:
                    x--;
                    break;
                case ConsoleKey.RightArrow:
                    x++;
                    break;
                default:
                    return false;
            }

            if (x >= 0 && x < width && y >= 0 && y < height && maze[x, y] != wall)
            {
                playerPos = (x, y);
                return true;
            }

            return false;
        }

        public bool CheckWin()
        {
            return playerPos.x == exitPos.x && playerPos.y == exitPos.y;
        }

        public void RunGame()
        {
            bool playing = true;

            while (playing)
            {
                GenerateMaze();
                DisplayMaze();
                bool levelCompleted = false;

                while (!levelCompleted && playing)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.Escape:
                                Console.WriteLine("\nИгра завершена!");
                                playing = false;
                                break;

                            case ConsoleKey.N:

                                levelCompleted = true;
                                Console.WriteLine("\nГенерируем новый лабиринт...");
                                Thread.Sleep(1000);
                                break;

                            default:
                                if (MovePlayer(keyInfo.Key))
                                {
                                    DisplayMaze();

                                    if (CheckWin())
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("\nПоздравляем! Вы нашли выход!");
                                        Console.WriteLine("Нажмите N для нового лабиринта или Esc для выхода");
                                        Console.ResetColor();
                                        levelCompleted = true;
                                    }
                                }
                                break;
                        }
                    }

                    Thread.Sleep(50);
                }

                if (playing && levelCompleted)
                {
                    bool waitingForInput = true;
                    while (waitingForInput && playing)
                    {
                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                            switch (keyInfo.Key)
                            {
                                case ConsoleKey.N:
                                    waitingForInput = false;
                                    Console.WriteLine("\nГенерируем новый лабиринт...");
                                    Thread.Sleep(1000);
                                    break;

                                case ConsoleKey.Escape:
                                    Console.WriteLine("\nИгра завершена!");
                                    playing = false;
                                    break;
                            }
                        }
                        Thread.Sleep(50);
                    }
                }
            }
        }
    }
}
