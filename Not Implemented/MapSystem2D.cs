using System;
using Utility.Astronomy;

namespace Mapsys.Core2D
{
    public class MapSystem2D
    {
        #region Fields & Autoprops

        private const int _gridBaseXY = 10; // Amount of tiles on XY as base, before any zoomin and scaling
        private bool[,] _worldGrid = null; // The grid created thus. the numbers repreesent the tile number tthat is seppoused to be inserted.
        /// <summary>
        /// This is measured from a center point to each side. Thuink... Radius.
        /// </summary>
        private int _visibleGridSize = 5; // The grid that actualy shows images.
        private Tuple<int, int>[,] _visibleGrid = null; // A tuple numbered values from the great grid.

        private readonly Resolution _imageResolution = Resolution.K4; // Constant for now
        private byte _scale = 1;
        private byte _maxScale = 20;

        public Utility.Geography.GeoPoint centerPoint = new Utility.Geography.GeoPoint();

        #endregion

        #region Properties

        public byte Scale { get { return _scale; } set { _scale = value; UpdateWorldGrid(); } } // Updates the grid when setting scale

        private float CurrentTileGeoLength { get { return Earth.equatorialCircumference / TotalTilesRow; } }

        private float CurrentPixelGeoLength { get { return CurrentTileGeoLength / (int)_imageResolution; } }

        private int TotalTilesRow { get { return _gridBaseXY * (int)Math.Pow(2, _scale); } }

        private int TotalTiles { get { return _gridBaseXY * _gridBaseXY * (int)Math.Pow(2, _scale); } }

        #endregion

        #region ctors

        public MapSystem2D (Utility.Geography.GeoPoint point)
        {
            centerPoint = point;
            _visibleGrid = new Tuple<int, int>[_visibleGridSize, _visibleGridSize];
            UpdateWorldGrid();
            UpdateVisibleGrid();
        }

        public MapSystem2D(float lat, float lon, float alt) : this(new Utility.Geography.GeoPoint(lat, lon, alt)) { }

        #endregion

        #region Methods

        public void PlaceImages()
        {
            var width = _visibleGridSize;
            var midX = (int)PointToGridPos()[0];
            var midY = (int)PointToGridPos()[1];

            //_visibleGrid = new int[3,3];
            throw new NotImplementedException();
        }

        public void InsertVisibleGrid(Direction direction)
        {
            throw new NotImplementedException();
        }

        //public void Scaler()
        //{
        //    UpdateMaxGrid();
        //    throw new NotImplementedException();
        //}

        //public void Zoom()
        //{

        //}

        private void UpdateWorldGrid()
        {
            _worldGrid = new bool[TotalTilesRow, TotalTilesRow];

            for (int i = 0; i < _worldGrid.Length; i++)
            {
                for (int j = 0; j < _worldGrid.Length; j++)
                {
                    _worldGrid[i, j] = false;
                }
            }
        }

        private void UpdateVisibleGrid()
        {
            var mid = PointToGridPos();
            var midX = (int)mid[0];
            var midY = (int)mid[1];
            var limit = _visibleGridSize * 2 + 1; // since it acts like a radius from the midpoint

            for (int i = 0; i < limit; i++)
            {
                for (int j = 0; j < limit; j++)
                {
                    _visibleGrid[i, j] = new Tuple<int, int>(midX - _visibleGridSize + i, midY - _visibleGridSize + j);
                }
            }
        }

        private float[] PointToGridPos()
        {
            var latNorm = (centerPoint.Lat + 90) / 180;
            var lonNorm = (centerPoint.Lon + 180) / 360;

            return new float[] { lonNorm * TotalTilesRow, latNorm * TotalTilesRow };
        }

        #endregion

        #region Nested & Enums

        public enum Resolution
        {
            K1 = 1024,
            K2 = 2048,
            K4 = 4096
        }

        [Flags]
        public enum Direction
        {
            North = 0,
            East = 90,
            South = 180,
            West = 270,
        }

        #endregion
    }
}