using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SimetraCustomLib
{
    namespace GeneralUtils
    {
        public enum GridPivot
        {
            Center,
            BottomLeft
        }

        /// <summary>
        /// The class that contains methods related to camera
        /// </summary>
        public static class CameraJobs
        {
            static float originalOrthographicSize;
            static bool orthographicConstantWidth = false;

            /// <summary>
            /// Keeps height constant for all screen ratios in orthographic mode. Unity keeps height constant by default
            /// </summary>
            public static void KeepOrthographicHeightConstant(Camera cam)
            {
                if (!orthographicConstantWidth)
                    return;

                cam.orthographicSize = originalOrthographicSize;

                orthographicConstantWidth = false;
            }

            /// <summary>
            /// Keeps width constant for all screen ratios in orthographic mode
            /// </summary>
            public static void KeepOrthographicWidthConstant
                (Camera cam, int refScrWidthInPixels, int refScrHeightInPixels)
            {
                if (orthographicConstantWidth)
                    return;

                if (refScrHeightInPixels <= 0)
                    throw new ArgumentException("Reference height must be positive", "refScrHeightInPixels");

                if (refScrWidthInPixels <= 0)
                    throw new ArgumentException("Reference width must be positive", "refScrWidthInPixels");

                float camSize = cam.orthographicSize * refScrWidthInPixels *
                    cam.pixelHeight / refScrHeightInPixels / cam.pixelWidth;

                originalOrthographicSize = cam.orthographicSize;
                cam.orthographicSize = camSize;

                orthographicConstantWidth = true;
            }

            /// <summary>
            /// 
            /// </summary>
            public static float FixXandYPositionOnScreen()
            {
                return -1;
            }

            /// <summary>
            /// 
            /// </summary>
            public static float FixXPositionOnScreen()
            {
                return -1;
            }

            /// <summary>
            /// Fixes the y position in case screen height differs from reference 
            /// </summary>
            public static float FixYPositionOnScreen
                (Camera cam, float yPositionInPixels, int refScrHeightInPixels)
            {
                if (yPositionInPixels < 0)
                    throw new ArgumentException("The y position can not be negative", "yPositionInPixels");

                if (refScrHeightInPixels <= 0)
                    throw new ArgumentException("Reference screen height must be positive",
                        "refScrHeightInPixels");

                return yPositionInPixels / refScrHeightInPixels * cam.pixelHeight;
            }
        }

        /// <summary>
        /// Create powerful grid systems with this flexible class (for 2d)
        /// </summary>
        public class GenericGrid<GridObjectType>
        {
            int width; // grid width
            int height; // grid height
            Vector3 origin; // origin of grid (bottom left corner)
            float cellSize; // size of each cell

            GridObjectType[,] gridArray;
            TextMeshPro[,] debugTextArray;

            // ctor
            public GenericGrid(int width, int height, float cellSize, Vector3 spawnLocation = default(Vector3), GridPivot pivot = GridPivot.BottomLeft, bool debug = false, Func<int, int, GridObjectType> createGridObject = null)
            {
                if (width <= 0)
                    throw new ArgumentException("width", "width must be positive");
                if (height <= 0)
                    throw new ArgumentException("height", "height must be positive");

                this.width = width;
                this.height = height;
                this.cellSize = cellSize;

                switch (pivot)
                {
                    case GridPivot.Center:
                        origin = spawnLocation - new Vector3(width, height) * cellSize / 2f;
                        break;
                    case GridPivot.BottomLeft:
                        origin = spawnLocation;
                        break;
                    default:
                        break;
                }

                gridArray = new GridObjectType[width, height];

                if (createGridObject == null)
                    createGridObject = (int x, int y) => Activator.CreateInstance<GridObjectType>();

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        gridArray[x, y] = createGridObject(x, y);

                if (debug)
                {
                    Transform debugTextParent = new GameObject("Debug text").transform;
                    debugTextArray = new TextMeshPro[width, height];

                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            debugTextArray[x, y] = WorldObjects.CreateWorldText(gridArray[x, y]?.ToString(),
                                debugTextParent.transform, GridToWorldPosition(x, y) +
                                new Vector3(.5f, .5f) * cellSize, 40, Color.white,
                                TextAlignmentOptions.Center);

                            Debug.DrawLine(GridToWorldPosition(x, y), GridToWorldPosition(x, y + 1),
                                Color.white, 100f);
                            Debug.DrawLine(GridToWorldPosition(x, y), GridToWorldPosition(x + 1, y),
                                Color.white, 100f);
                        }
                    }

                    Debug.DrawLine(GridToWorldPosition(0, height), GridToWorldPosition(width, height),
                        Color.white, 100f);
                    Debug.DrawLine(GridToWorldPosition(width, 0), GridToWorldPosition(width, height),
                        Color.white, 100f);
                }
            }

            /// <summary>
            /// Convert grid x, y to world position
            /// </summary>
            public Vector3 GridToWorldPosition(int x, int y)
            {
                return new Vector3(x + .5f, y + .5f) * cellSize + origin;
            }

            /// <summary>
            /// Convert grid x, y to world position
            /// </summary>
            public Vector3 GridToWorldPosition(Vector2Int gridXY)
            {
                return GridToWorldPosition(gridXY.x, gridXY.y);
            }

            /// <summary>
            /// Convert world pos to grid pos (can yield negative values)
            /// </summary>
            public bool WorldToGridPosition(Vector3 worldPos, out int x, out int y)
            {
                x = Mathf.FloorToInt((worldPos - origin).x / cellSize);
                y = Mathf.FloorToInt((worldPos - origin).y / cellSize);

                if (x < 0 || y < 0 || x >= width || y >= height)
                    return false;

                return true;
            }

            // set a value for a cell in grid given by its x and y coords
            public void SetCellObject(int x, int y, GridObjectType value)
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                    return;

                _SetCellObject(x, y, value);
            }
            //
            public void SetCellObject(Vector3 worldPos, GridObjectType value)
            {
                int x, y;

                if (WorldToGridPosition(worldPos, out x, out y))
                    _SetCellObject(x, y, value);
            }

            private void _SetCellObject(int x, int y, GridObjectType value)
            {
                gridArray[x, y] = value;

                if (debugTextArray.GetLength(0) != 0) // if debug text array exists
                    debugTextArray[x, y].text = value?.ToString();
            }

            /// <summary>
            /// Returns object in grid
            /// </summary>
            public GridObjectType GetCellObject(int x, int y)
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                    return default(GridObjectType);

                return gridArray[x, y];
            }

            /// <summary>
            /// Returns object in grid
            /// </summary>
            public GridObjectType GetCellObject(Vector3 worldPos)
            {
                int x, y;

                if (WorldToGridPosition(worldPos, out x, out y))
                    return gridArray[x, y];
                else
                    return default(GridObjectType);
            }

            /// <summary>
            /// Returns object in grid. Does not check for index out of range exception!
            /// </summary>
            public GridObjectType GetCellObjectImmediate(int x, int y)
            {
                return gridArray[x, y];
            }
        }

        /// <summary>
        /// Easily create world objects
        /// </summary>
        public static class WorldObjects
        {
            public static TextMeshPro CreateWorldText(string text = "New text", Transform parent = null, Vector3 localPosition = default(Vector3), float fontSize = 16, Color? color = null, TextAlignmentOptions alignmentOption = TextAlignmentOptions.TopLeft, int sortingOrder = 0)
            {
                GameObject textObject = new GameObject("New World Text", typeof(TextMeshPro));
                TextMeshPro textMesh = textObject.GetComponent<TextMeshPro>();

                textObject.transform.SetParent(parent, false);
                textObject.transform.localPosition = localPosition;

                textMesh.alignment = alignmentOption;

                textMesh.text = text;
                textMesh.fontSize = fontSize;
                textMesh.color = color == null ? Color.white : (Color)color;
                textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

                return textMesh;
            }
        }
    }
}