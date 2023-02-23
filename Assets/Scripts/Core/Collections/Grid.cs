using System;
using System.Diagnostics;
using System.Collections.Generic;
using GG.Mathe;

namespace Rogue.Core.Collections
{
    /// <summary>
    /// Define an interface for grids.
    /// </summary>
    public interface IGrid
    {}

    /// <summary>
    /// Define a grid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Grid<T> : IGrid
    {
        /// <summary>
        /// Number of rows.
        /// </summary>
        public int Rows { get; private set; } = 0;

        /// <summary>
        /// Number of columns.
        /// </summary>
        public int Cols { get; private set; } = 0;

        /// <summary>
        /// Cells.
        /// </summary>
        protected T[] m_cells;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        public Grid(int rows, int cols)
        { 
            Debug.Assert(rows > 0, "Invalid number of rows");
            Debug.Assert(cols > 0, "Invalid number of columns");

            Rows    = rows;
            Cols    = cols;
            m_cells = new T[rows * cols];
        }

        /// <summary>
        /// Checks whether a coordinate is inside the grid or not.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <returns>True if the coordinate is inside the grid; otherwise false.</returns>
        public bool HasCoord(Vec2i coord)
        {
             return coord.Row >= 0 && coord.Row < Rows && coord.Col >= 0 && coord.Col < Cols;
        }

        /// <summary>
        /// Get the value at a position.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <returns>Item at that position.</returns>
        public T Get(Vec2i coord) => GetRef(coord);

        /// <summary>
        /// Gets a reference to the value at a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <returns>Reference to the value.</returns>
        #if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref T GetRef(Vec2i coord) => ref m_cells[GetCoordAsIndex(coord)];

        /// <summary>
        /// Set the value at a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <param name="value">Item to set.</param>
        public void Set(Vec2i coord, T value)
        {
            GetRef(coord) = value;
        }

        /// <summary>
        /// Set the value at a list of coordinates.
        /// </summary>
        /// <param name="coords">Positions.</param>
        /// <param name="value">Item to set.</param>
        public void Set(IEnumerable<Vec2i> coords, T value)
        {
            foreach (Vec2i position in coords)
            {
                GetRef(position) = value;
            }
        }

        /// <summary>
        /// Clears the grid and sets the default value for the items.
        /// </summary>
        public void Clear() => Clear(default(T));

        /// <summary>
        /// Clears the grid and sets a new value for the items.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void Clear(T value)
        {
            Array.Fill(m_cells, value);
        }

        /// <summary>
        /// Finds the first item in a coordinate that matches the conditions by the specified predicate.
        /// </summary>
        /// <param name="coord">Coordinates.</param>
        /// <param name="match">Predicate.</param>
        /// <returns>Position of the first item that matches the conditions; otherwise, null.</returns>
        public Vec2i? FindFirst(Vec2i coord, Func<T, bool> match)
        {
            if (match(GetRef(coord)))
            {
                return coord;
            }
            
            return null;
        }

        /// <summary>
        /// Finds the first item in a list of coordinates that matches the conditions by the specified predicate.
        /// </summary>
        /// <param name="coords">Coordaintes.</param>
        /// <param name="match">Predicate.</param>
        /// <returns>Position of the first item that matches the conditions; otherwise, null.</returns>
        public Vec2i? FindFirst(IEnumerable<Vec2i> coords, Func<T, bool> match)
        {
            foreach (Vec2i coord in coords)
            {
                if (match(GetRef(coord)))
                {
                    return coord;
                }
            }

            return null;
        }

        /// <summary>
        /// Performs the specified action on each value in a coordinate.
        /// </summary>
        /// <param name="coord">Coordiante.</param>
        /// <param name="action">Action.</param>
        public void ForEach(Vec2i coord, Action<T> action)
        {
            action(GetRef(coord));
        }

        /// <summary>
        /// Performs the specified action on each value in a list of coordinates.
        /// </summary>
        /// <param name="coords">Coordiantes.</param>
        /// <param name="action">Action.</param>
        public void ForEach(IEnumerable<Vec2i> coords, Action<T> action)
        {   
            foreach (Vec2i coord in coords)
            {
                action(GetRef(coord));
            }
        }

        // -------
        // HELPERS
        // -------

        /// <summary>
        /// Gets the index of a coordinate.
        /// </summary>
        /// <param name="grid">Grid.</param>
        /// <param name="coord">Coordinate.</param>
        /// <returns>Index.</returns>
        #if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #endif
        public int GetCoordAsIndex(Vec2i coord)
        {
            #if DEBUG
                if (coord.Row < 0 || coord.Row >= Rows) { throw new IndexOutOfRangeException("Row out of bounds"); }
                if (coord.Col < 0 || coord.Col >= Cols) { throw new IndexOutOfRangeException("Column out of bounds"); }
            #endif

            return coord.Row * Cols + coord.Col;
        }

        /// <summary>
        /// Get the coordinate of an index.
        /// </summary>
        /// <param name="grid">Grid.</param>
        /// <param name="index">Index.</param>
        /// <returns>Coordinate.</returns>
        #if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #endif
        public Vec2i GetIndexAsCoord(int index)
        {
            #if DEBUG
                if (index < 0 || index >= m_cells.Length) { throw new IndexOutOfRangeException("Index out of bounds"); }
            #endif

            return new Vec2i(index % Cols, index / Rows);
        }
    }
}