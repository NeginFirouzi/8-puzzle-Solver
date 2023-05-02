using System;
using System.Collections.Generic;

namespace EightPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] startState = GenerateRandomState();
            int[,] goalState = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
            int counter = 0;

            Console.WriteLine("Starting state:");
            PrintState(startState);

            while (!IsGoalState(startState, goalState))
            {
                int[] emptySpacePosition = GetEmptySpacePosition(startState);
                List<int> validMoves = GetValidMoves(emptySpacePosition);
                int randomMove = validMoves[new Random().Next(validMoves.Count)];
                string moveName = GetMoveName(randomMove);
                MoveEmptySpace(startState, emptySpacePosition, randomMove);
                counter++;

                Console.WriteLine($"Move {counter}: {moveName}");
                PrintState(startState);
            }

            Console.WriteLine($"Goal state achieved in {counter} moves!");
        }

        static int[,] GenerateRandomState()
        {
            int[,] state = new int[3, 3];
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 };

            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    int randomIndex = new Random().Next(numbers.Count);
                    state[i, j] = numbers[randomIndex];
                    numbers.RemoveAt(randomIndex);
                }
            }

            return state;
        }

        static bool IsGoalState(int[,] state, int[,] goalState)
        {
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    if (state[i, j] != goalState[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static int[] GetEmptySpacePosition(int[,] state)
        {
            int[] position = new int[2];
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    if (state[i, j] == 0)
                    {
                        position[0] = i;
                        position[1] = j;
                        return position;
                    }
                }
            }

            return null;
        }

        static List<int> GetValidMoves(int[] emptySpacePosition)
        {
            int emptyRow = emptySpacePosition[0];
            int emptyCol = emptySpacePosition[1];
            List<int> validMoves = new List<int>();

            if (emptyRow > 0) // Can move up
            {
                validMoves.Add(1);
            }
            if (emptyRow < 2) // Can move down
            {
                validMoves.Add(2);
            }
            if (emptyCol < 2) // Can move right
            {
                validMoves.Add(3);
            }
            if (emptyCol > 0) // Can move left
            {
                validMoves.Add(4);
            }

            return validMoves;
        }

        static string GetMoveName(int move)
        {
            switch (move)
            {
                case 1:
                    return "Up";
                case 2:
                    return "Down";
                case 3:
                    return "Right";
                case 4:
                    return "Left";
                default:
                    return "";
            }
        }

        static void MoveEmptySpace(int[,] state, int[] emptySpacePosition, int move)
        {
            int emptyRow = emptySpacePosition[0];
            int emptyCol = emptySpacePosition[1];

            switch (move)
            {
                case 1: // Up
                    state[emptyRow, emptyCol] = state[emptyRow - 1, emptyCol];
                    state[emptyRow - 1, emptyCol] = 0;
                    break;
                case 2: // Down
                    state[emptyRow, emptyCol] = state[emptyRow + 1, emptyCol];
                    state[emptyRow + 1, emptyCol] = 0;
                    break;
                case 3: // Right
                    state[emptyRow, emptyCol] = state[emptyRow, emptyCol + 1];
                    state[emptyRow, emptyCol + 1] = 0;
                    break;
                case 4: // Left
                    state[emptyRow, emptyCol] = state[emptyRow, emptyCol - 1];
                    state[emptyRow, emptyCol - 1] = 0;
                    break;
            }
        }

        static void PrintState(int[,] state)
        {
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    Console.Write(state[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}