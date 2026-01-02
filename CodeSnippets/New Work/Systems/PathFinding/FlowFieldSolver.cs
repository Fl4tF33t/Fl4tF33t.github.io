using System.Collections.Generic;
using UnityEngine;

namespace Systems {
    public static class FlowFieldSolver {
        public static void BuildIntegrationField(GridSystem2D<Cell> system, Cell destinationCell) {
            foreach (var cell in system.Grid) 
                cell.Reset();

            destinationCell.bestCost = 0;

            List<Cell> openList = new() { destinationCell };

            while (openList.Count > 0) {
                // Find cell with lowest bestCost
                Cell currentCell = openList[0];
                for (int i = 1; i < openList.Count; i++) {
                    if (openList[i].bestCost < currentCell.bestCost)
                        currentCell = openList[i];
                }

                openList.Remove(currentCell);

                var neighbours = system.GetNeighbours(
                    currentCell.x,
                    currentCell.y,
                    GridDirection.CardinalDirections
                );

                foreach (var neighbourCell in neighbours) {
                    if (neighbourCell.cost == byte.MaxValue)
                        continue;

                    ushort newBestCost = (ushort)(currentCell.bestCost + neighbourCell.cost);

                    if (newBestCost < neighbourCell.bestCost) {
                        neighbourCell.bestCost = newBestCost;

                        if (!openList.Contains(neighbourCell))
                            openList.Add(neighbourCell);
                    }
                }
            }
        }
        
        public static void BuildFlowField(GridSystem2D<Cell> system) {
            foreach (var cell in system.Grid) {
                if (cell.bestCost == 0) {
                    cell.bestDirection = GridDirection.None;
                    continue;
                }

                var neighbours = system.GetNeighbours(cell.x, cell.y, GridDirection.AllDirections);

                ushort lowestCost = cell.bestCost;
                GridDirection bestDir = GridDirection.None;

                foreach (var neighbour in neighbours) {
                    if (neighbour.cost == byte.MaxValue)
                        continue;

                    if (neighbour.bestCost < lowestCost) {
                        lowestCost = neighbour.bestCost;
                        Vector2Int dir = new Vector2Int(neighbour.x - cell.x, neighbour.y - cell.y);
                        bestDir = GridDirection.FromVector(dir);
                    }
                }

                cell.bestDirection = bestDir;
            }
        }
    }
}