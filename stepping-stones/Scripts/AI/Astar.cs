using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;


public class Location
{
	private int x;
	private int y;

	private static Dictionary<Tuple<int, int>, Location> locations = new Dictionary<Tuple<int, int>, Location>(); 

	public static Location at(int row, int column) {
		Tuple<int, int> position = new Tuple<int, int>(row, column); 
		if (!locations.ContainsKey(position)) locations[position] = new Location(row, column); 
		return locations[position]; 
	}

	/*Location (constructor)
	Inputs: int row, int column
	Returns: Location
	Description: Constructor of Location*/
    //public for purposes of debugging. TOOD: replace any creation of location
	public Location(int row, int column){
		x = row;
		y = column;
	}
	
	/*row
	Inputs: None
	Returns: Int
	Description: Returns row of Location instance*/
	public int row(){
		return x;
	}
	/*column
	Inputs: None
	Returns: Int
	Description: Returns column of Location instance*/
	public int column(){
		return y;
	}


	public Location left() { return new Location(x, y - 1); }
	public Location right() { return new Location(x, y + 1); }
	public Location up() { return new Location(x - 1, y); }
	public Location down() { return new Location(x + 1, y); }

	public string toString() {
		return (char)(row() + 'a') + "" + column().ToString(); 
	}

	public static Location fromString(string representation) {
		int row = representation[0] - 'a'; 
		int column = -1; 
		if (Int32.TryParse(representation.Substring(1), out column)) 
			return Location.at(row, column); 
		return Location.at(-1, -1);
	}
}

//adapted from https://www.geeksforgeeks.org/a-search-algorithm/

public class AStarAlgo
{

    //had struct "pair" that was pmuch just location

    // A structure to hold the necessary parameters
    public struct Cell
    {
        // Row and Column index of its parent
        // Note that 0 <= i <= ROW-1 & 0 <= j <= COL-1
        public int parent_i, parent_j;
        // f = g + h
        public int f, g, h;
    }

    // A Function to find the shortest path between
    // a given source cell to a destination cell according
    // to A* Search Algorithm
    public static void AStar(int[,] grid, Location src, int destCol)
    {
        int ROW = grid.GetLength(0);
        int COL = grid.GetLength(1);
        //TODO: Dest should become row/col, not location. Or set of locations
        //Probably faster if it is row, but might not be able to blocking code
        // If the source or destination is out of range
        if (!IsValid(src.row(), src.column(), ROW, COL) || !IsValid(0, destCol, ROW, COL))
        {
            Console.WriteLine("Source or destination is invalid");
            return;
        }

        // Either the source or the destination is blocked
        if (!IsUnBlocked(grid, src.row(), src.column()))
        {
            Console.WriteLine("Source or the destination is blocked");
            return;
        }

        // If the destination cell is the same as the source cell
        if (src.column() == destCol)
        {
            Console.WriteLine("We are already at the destination");
            return;
        }

        // Create a closed list and initialise it to false which
        // means that no cell has been included yet. This closed
        // list is implemented as a boolean 2D array
        bool[,] closedList = new bool[ROW, COL];

        // Declare a 2D array of structure to hold the details
        // of that cell
        Cell[,] cellDetails = new Cell[ROW, COL];

        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                cellDetails[i, j].f = int.MaxValue;
                cellDetails[i, j].g = int.MaxValue;
                cellDetails[i, j].h = int.MaxValue;
                cellDetails[i, j].parent_i = -1;
                cellDetails[i, j].parent_j = -1;
            }
        }

        // Initialising the parameters of the starting node
        int x = src.row(), y = src.column();
        cellDetails[x, y].f = 0;
        cellDetails[x, y].g = 0;
        cellDetails[x, y].h = 0;
        cellDetails[x, y].parent_i = x;
        cellDetails[x, y].parent_j = y;

        /*
            Create an open list having information as-
            <f, <i, j>>
            where f = g + h,
            and i, j are the row and column index of that cell
            Note that 0 <= i <= ROW-1 & 0 <= j <= COL-1
            This open list is implemented as a SortedSet of tuple (f, (i, j)).
            We use a custom comparer to compare tuples based on their f values.
        */
        SortedSet<(int, Location)> openList = new SortedSet<(int, Location)>(
            Comparer<(int, Location)>.Create((a, b) => a.Item1.CompareTo(b.Item1)));

        // Put the starting cell on the open list and set its
        // 'f' as 0
        openList.Add((0, new Location(x, y)));

        // We set this boolean value as false as initially
        // the destination is not reached.
        bool foundDest = false;

        while (openList.Count > 0)
        {
            (int f, Location loc) p = openList.Min;
            openList.Remove(p);

            // Add this vertex to the closed list
            //TODO MODIFY
            x = p.loc.row();
            y = p.loc.column();
            closedList[x, y] = true;

            //originally did all 8 dir, but can only do taxicab movments
            // Generating all the 4 successors of this cell
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    //TODO: THIS BROKEN
                    if (Math.Abs(i) + Math.Abs(j) != 1)
                        //if 0, not moving. Skip. Since moving in cardinal dir, either i or j should be 0
                        continue;

                    int newX = x + i;
                    int newY = y + j;

                    // If this successor is a valid cell
                    if (IsValid(newX, newY, ROW, COL))
                    {
                        // If the destination cell is the same as the
                        // current successor
                        if (IsDestination(newX, newY, destCol))
                        {
                            cellDetails[newX, newY].parent_i = x;
                            cellDetails[newX, newY].parent_j = y;
                            Console.WriteLine("The destination cell is found");
                            TracePath(cellDetails, newX, newY);
                            foundDest = true;
                            return;
                        }

                        // If the successor is already on the closed
                        // list or if it is blocked, then ignore it.
                        if (!closedList[newX, newY] && IsUnBlocked(grid, newX, newY))
                        {
                            int gNew = cellDetails[x, y].g + 1;
                            int hNew = CalculateHValue(newX, newY, destCol);
                            int fNew = gNew + hNew;

                            // If it isn’t on the open list, add it to
                            // the open list. Make the current square
                            // the parent of this square. Record the
                            // f, g, and h costs of the square cell
                            if (cellDetails[newX, newY].f == int.MaxValue || cellDetails[newX, newY].f > fNew)
                            {
                                openList.Add((fNew, new Location(newX, newY)));

                                // Update the details of this cell
                                cellDetails[newX, newY].f = fNew;
                                cellDetails[newX, newY].g = gNew;
                                cellDetails[newX, newY].h = hNew;
                                cellDetails[newX, newY].parent_i = x;
                                cellDetails[newX, newY].parent_j = y;
                            }
                        }
                    }
                }
            }
        }

        // When the destination cell is not found and the open
        // list is empty, then we conclude that we failed to
        // reach the destination cell. This may happen when the
        // there is no way to destination cell (due to
        // blockages)
        if (!foundDest)
            Console.WriteLine("Failed to find the Destination Cell");
    }

    // A Utility Function to check whether given cell (row, col)
    // is a valid cell or not.
    public static bool IsValid(int row, int col, int ROW, int COL)
    {
        // Returns true if row number and column number
        // is in range
        return (row >= 0) && (row < ROW) && (col >= 0) && (col < COL);
    }

    // A Utility Function to check whether the given cell is
    // blocked or not
    public static bool IsUnBlocked(int[,] grid, int row, int col)
    {
        // Returns true if the cell is not blocked else false
        return grid[row, col] == 1;
    }

    // A Utility Function to check whether destination cell has
    // been reached or not
    public static bool IsDestination(int row, int col, int destCol)
    {
        return (col == destCol);
    }

    // A Utility Function to calculate the 'h' heuristics.
    public static int CalculateHValue(int row, int col, int destCol)
    {
        // Return using the distance formula
        return Math.Abs(col - destCol);
    }

    // A Utility Function to trace the path from the source
    // to destination
    public static void TracePath(Cell[,] cellDetails, int arrivedX, int arrivedY)
    {
        Console.WriteLine("\nThe Path is ");
        int ROW = cellDetails.GetLength(0);
        int COL = cellDetails.GetLength(1);

        int row = arrivedX;
        int col = arrivedY;

        Stack<Location> Path = new Stack<Location>();

        while (!(cellDetails[row, col].parent_i == row && cellDetails[row, col].parent_j == col))
        {
            Path.Push(new Location(row, col));
            int temp_row = cellDetails[row, col].parent_i;
            int temp_col = cellDetails[row, col].parent_j;
            row = temp_row;
            col = temp_col;
        }

        Path.Push(new Location(row, col));
        while (Path.Count > 0)
        {
            Location p = Path.Peek();
            Path.Pop();
            Console.Write(" -> ({0},{1}) ", p.row(), p.column());
        }
    }

    // Driver method
    //TODO: Breaks after dest = 3;
    /*TODO: Does not check if dest col spot is free this is semi-intentional, as
    can push last tile out of way? Depending on ruleset. Huh*/

    public static void Main(string[] args)
    {
        /* Description of the Grid-
            1--> The cell is not blocked
            0--> The cell is blocked */
        int[,] grid =
        {
            {1, 0, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 0, 1, 1, 1, 0, 1, 1},
            {1, 1, 1, 0, 1, 1, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 0, 0, 0, 0, 1},
            {1, 1, 1, 0, 1, 1, 1, 0, 1, 0},
            {1, 0, 1, 1, 1, 1, 0, 1, 0, 0},
            {1, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {1, 0, 1, 1, 1, 1, 0, 1, 1, 1},
            {1, 1, 1, 0, 0, 0, 1, 0, 0, 1}
        };

        // Source is the left-most bottom-most corner
        Location src = new Location(8, 0);

        // Destination is the left-most top-most corner
        int dest = 9;

        AStar(grid, src, dest);
    }
}
    
