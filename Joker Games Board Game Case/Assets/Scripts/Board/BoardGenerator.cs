using System;
using System.Collections;
using System.Collections.Generic;
using Data_Management;
using Game_Management;
using Item_Management;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Board
{
    public class BoardGenerator : MonoBehaviour
    {
        // Custom struct to represent a point on the grid
        // Includes coordinates (x, y), an ItemType, and an itemAmount
        [Serializable]
        public struct Point
        {
            public int x, y;
            public ItemType itemType;
            public int itemAmount;

            // Constructor for initializing Point with x, y coordinates and optional itemType and itemAmount
            public Point(int x, int y, ItemType itemType = ItemType.Empty, int itemAmount = 0)
            {
                this.x = x;
                this.y = y;
                this.itemType = itemType;
                this.itemAmount = itemAmount;
            }
        }

        private GameManager _gameManager;
        private Pool _pool;

        [SerializeField] [Range(10, 100)] private int boardSize; // Size of the grid (NxN)
        [SerializeField] private List<Point> _mapOrder; // List to store the generated path of Points

        private void OnEnable()
        {
            Actions.NewGame += NewMap;
            Actions.LoadGame += LoadMap;
        }

        private void OnDisable()
        {
            Actions.NewGame -= NewMap;
            Actions.LoadGame -= LoadMap;
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _pool = Pool.Instance;
        }

        #region Map Generation

        // Called when starting a new game, this method generates a new map
        private void NewMap()
        {
            var gridList = _gameManager.gameMap;
            if (gridList.Count > 0)
            {
                ClearExistingMap(gridList);
            }

            _mapOrder = new List<Point>();
            PlayerDataManager.PlayerData.currentGrid = 0;

            GenerateBoard(); // Trigger the board generation process when the game starts
            StartCoroutine(GenerateGridObjects());
        }

        // Called when loading a saved game, this method loads the saved map
        private void LoadMap()
        {
            if (_mapOrder.Count > 0)
            {
                Actions.GameStart?.Invoke();
            }
            else
            {
                _mapOrder = PlayerDataManager.MapOrder;

                if (_mapOrder is not { Count: > 0 })
                {
                    GenerateBoard(); // Trigger the board generation process when the game starts
                }

                StartCoroutine(GenerateGridObjects());
            }
        }

        // Method to clear existing map objects
        private void ClearExistingMap(List<Transform> gridList)
        {
            var initialCount = gridList.Count;
            for (var i = 0; i < initialCount; i++)
            {
                _pool.DeactivateObject(gridList[0].gameObject, PoolItemType.Grid);
                gridList.RemoveAt(0);
            }
        }

        #endregion

        #region Pathfinding and Board Generation

        // Main method to generate the entire board
        private void GenerateBoard()
        {
            bool isValidBoard = false;

            while (!isValidBoard)
            {
                _mapOrder = new List<Point>();
                GenerateGridOrder();

                if (_mapOrder.Count >= boardSize)
                {
                    isValidBoard = true;
                    PlayerDataManager.SaveMapOrder(_mapOrder);
                }
                else
                {
                    Debug.LogWarning("Generated grid order is smaller than board size. Restarting the process...");
                }
            }
        }

        // Method to initialize and generate the grid path
        private void GenerateGridOrder()
        {
            // Choose random starting and ending points within the grid
            var startingPoint = new Point(Random.Range(0, boardSize / 2), Random.Range(0, boardSize / 2));
            var endingPoint = new Point(Random.Range(boardSize / 2, boardSize), Random.Range(boardSize / 2, boardSize));

            var visited = new bool[boardSize, boardSize]; // Array to track visited points
            visited[startingPoint.x, startingPoint.y] = true; // Mark the starting point as visited

            FindPath(startingPoint, endingPoint, visited); // Find a path from start to end
        }

        // Method to find a path from the starting point to the target point
        private void FindPath(Point current, Point target, bool[,] visited)
        {
            int maxIterations = boardSize * boardSize; // Safeguard limit to prevent infinite loops
            int iterations = 0;

            // Assign a random itemType and itemAmount to the first Point
            current.itemType = (ItemType)Random.Range(0, Enum.GetValues(typeof(ItemType)).Length);
            current.itemAmount = Random.Range(0, 16); // Random amount between 0 and 15 inclusive

            // Continue pathfinding until reaching the target or exceeding maxIterations
            while ((current.x != target.x || current.y != target.y) && iterations < maxIterations)
            {
                // Add the current point to the path (_mapOrder)
                _mapOrder.Add(new Point(current.x, current.y, current.itemType, current.itemAmount));
                current = GetNextValidPoint(current, target, visited, boardSize); // Get the next valid point in the path
                iterations++;
            }

            // Log a warning if the safeguard limit is reached
            if (iterations >= maxIterations)
            {
                Debug.LogWarning("Pathfinding terminated due to excessive iterations.");
            }
        }

        // Method to get the next valid point in the path
        private Point GetNextValidPoint(Point current, Point target, bool[,] visited, int boardSize)
        {
            // List of possible directions (left, right, up, down)
            var directions = new List<Point>
            {
                new Point(-1, 0),
                new Point(1, 0),
                new Point(0, -1),
                new Point(0, 1)
            };

            var possibleDir = new List<Point>();

            // Check each direction to see if it's a valid move
            foreach (var dir in directions)
            {
                var newX = current.x + dir.x;
                var newY = current.y + dir.y;

                // Ensure the new position is within bounds and hasn't been visited
                if (newX >= 0 && newX < boardSize && newY >= 0 && newY < boardSize && !visited[newX, newY])
                {
                    int availableDirections = CountAvailableDirections(new Point(newX, newY), visited, boardSize);

                    // Only consider directions that lead to points with more than 2 possible next steps
                    if (availableDirections > 2)
                    {
                        possibleDir.Add(new Point(newX, newY));
                    }
                }
            }

            // If valid directions are found, pick one at random
            if (possibleDir.Count > 0)
            {
                var nextPoint = possibleDir[Random.Range(0, possibleDir.Count)];
                // Assign a random itemType and itemAmount to the nextPoint
                nextPoint.itemType = (ItemType)Random.Range(0, Enum.GetValues(typeof(ItemType)).Length);
                nextPoint.itemAmount = Random.Range(1, 16); // Random amount between 0 and 15 inclusive
                visited[nextPoint.x, nextPoint.y] = true; // Mark the point as visited
                return nextPoint;
            }

            // If no valid directions, log a warning and return the target point to exit the loop
            Debug.LogWarning("No valid directions found, terminating pathfinding.");
            return target;
        }

        // Method to count the number of available directions from a given point
        private int CountAvailableDirections(Point point, bool[,] visited, int boardSize)
        {
            var directions = new List<Point>
            {
                new Point(-1, 0),
                new Point(1, 0),
                new Point(0, -1),
                new Point(0, 1)
            };

            int count = 0;

            // Count the directions that are within bounds and haven't been visited
            foreach (var dir in directions)
            {
                var newX = point.x + dir.x;
                var newY = point.y + dir.y;

                if (newX >= 0 && newX < boardSize && newY >= 0 && newY < boardSize && !visited[newX, newY])
                {
                    count++;
                }
            }

            return count;
        }

        #endregion

        #region Grid Object Generation

        // Method to generate and place objects on the grid based on the generated path
        private IEnumerator GenerateGridObjects()
        {
            foreach (var point in _mapOrder)
            {
                yield return 0.1f.GetWait();
                
                // Spawn a grid object at the real-world position corresponding to the point
                var gridObject = Pool.Instance.SpawnObject(GetRealWorldPositionOfTheGrid(2, point), PoolItemType.Grid, null);
                if (gridObject.TryGetComponent(out Grid grid))
                {
                    grid.SetGrid(point.itemType, point.itemAmount, _mapOrder.IndexOf(point));
                }
            }

            yield return 1.0f.GetWait();
            Actions.GameStart?.Invoke();
        }

        // Convert a grid position to a real-world position in the game
        private Vector3 GetRealWorldPositionOfTheGrid(int size, Point gridPosition)
        {
            return new Vector3(gridPosition.x * size, 0, gridPosition.y * size);
        }

        #endregion
    }
}
