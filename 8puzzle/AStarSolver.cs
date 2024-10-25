using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightPuzzleSolver
{
    class AStarSolver
    {
        static void Main(string[] args)
        {
            int[,] startState = GenerateRandomState();
            int[,] goalState = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
            Console.WriteLine("Starting state:");
            PrintState(startState, 0, "Start");

            List<Node> solution = AStar(startState, goalState);
            if (solution != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Hooray! Goal state achieved in {solution.Count - 1} moves!\n");
                Console.ResetColor();

                for (int i = 1; i < solution.Count; i++)
                {
                    string move = solution[i].Move;
                    Console.WriteLine($"State {i} (Move 0: {move}):");
                    PrintState(solution[i].State, i, move);
                }

            }
            else
            {
                Console.WriteLine("No solution found.");
            }
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

        static void PrintState(int[,] state, int stateNumber, string move)
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

        static int GetManhattanDistance(int[,] state, int[,] goalState)
        {
            int distance = 0;
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    int value = state[i, j];
                    if (value != 0)
                    {
                        int goalRow = (value - 1) / state.GetLength(0);
                        int goalCol = (value - 1) % state.GetLength(1);
                        distance += Math.Abs(i - goalRow) + Math.Abs(j - goalCol);
                    }
                }
            }
            return distance;
        }

        static List<Node> GetNeighbors(Node node, int[,] goalState)
        {
            List<Node> neighbors = new List<Node>();
            int[] emptySpacePosition = GetEmptySpacePosition(node.State);
            int emptyRow = emptySpacePosition[0];
            int emptyCol = emptySpacePosition[1];

            int[,] directions = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            string[] moveNames = new string[] { "Up", "Down", "Left", "Right" };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newRow = emptyRow + directions[i, 0];
                int newCol = emptyCol + directions[i, 1];

                if (newRow >= 0 && newRow < node.State.GetLength(0) && newCol >= 0 && newCol < node.State.GetLength(1))
                {
                    int[,] newState = (int[,])node.State.Clone();
                    newState[emptyRow, emptyCol] = newState[newRow, newCol];
                    newState[newRow, newCol] = 0;

                    neighbors.Add(new Node(newState, node, node.Cost + 1, GetManhattanDistance(newState, goalState), moveNames[i]));
                }
            }

            return neighbors;
        }

        static int[] GetEmptySpacePosition(int[,] state)
        {
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    if (state[i, j] == 0)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return null;
        }

        static List<Node> GetPath(Node node)
        {
            List<Node> path = new List<Node>();
            while (node != null)
            {
                path.Insert(0, node);
                node = node.Parent;
            }
            return path;
        }

        static List<Node> AStar(int[,] startState, int[,] goalState)
        {
            PriorityQueue<Node> openList = new PriorityQueue<Node>();
            HashSet<Node> closedList = new HashSet<Node>();

            Node startNode = new Node(startState, null, 0, GetManhattanDistance(startState, goalState), "Start");
            openList.Enqueue(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList.Dequeue();

                if (IsGoalState(currentNode.State, goalState))
                {
                    return GetPath(currentNode);
                }

                closedList.Add(currentNode);

                foreach (Node neighbor in GetNeighbors(currentNode, goalState))
                {
                    if (closedList.Contains(neighbor))
                    {
                        continue;
                    }

                    if (!openList.Contains(neighbor))
                    {
                        openList.Enqueue(neighbor);
                    }
                }
            }

            return null;
        }
    }

    class Node : IComparable<Node>
    {
        public int[,] State { get; set; }
        public Node Parent { get; set; }
        public int Cost { get; set; }
        public int Heuristic { get; set; }
        public string Move { get; set; }

        public Node(int[,] state, Node parent, int cost, int heuristic, string move)
        {
            State = state;
            Parent = parent;
            Cost = cost;
            Heuristic = heuristic;
            Move = move;
        }

        public int CompareTo(Node other)
        {
            return (Cost + Heuristic).CompareTo(other.Cost + other.Heuristic);
        }
    }

    class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> data;

        public PriorityQueue()
        {
            data = new List<T>();
        }

        public void Enqueue(T item)
        {
            data.Add(item);
            int ci = data.Count - 1;
            while (ci > 0)
            {
                int pi = (ci - 1) / 2;
                if (data[ci].CompareTo(data[pi]) >= 0) break;
                T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            int li = data.Count - 1;
            T frontItem = data[0];
            data[0] = data[li];
            data.RemoveAt(li);

            --li;
            int pi = 0;
            while (true)
            {
                int ci = pi * 2 + 1;
                if (ci > li) break;
                int rc = ci + 1;
                if (rc <= li && data[rc].CompareTo(data[ci]) < 0) ci = rc;
                if (data[pi].CompareTo(data[ci]) <= 0) break;
                T tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }

        public int Count
        {
            get { return data.Count; }
        }

        public bool Contains(T item)
        {
            return data.Contains(item);
        }
    }
}
