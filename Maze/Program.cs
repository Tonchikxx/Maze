using System;
using System.Collections.Generic;
using System.Threading;

using Maze;

class Program
{
    static void Main(string[] args)
    {

        MazeGenerator mazeGame = new();

        mazeGame.RunGame();

        Console.WriteLine("\nСпасибо за игру! Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}