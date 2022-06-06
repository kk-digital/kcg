namespace Enums
{
    public static class TileNeighborExt 
    {
        public static TileVariants GetVariant(int[] neighbors, int tileId)
        {
            if (IsRightTile(neighbors, tileId))
            {
                return TileVariants.Right;
            }
            else if (IsLeftTile(neighbors, tileId))
            {
                return TileVariants.Left;
            }
            else if (IsTopTile(neighbors, tileId))
            {
                return TileVariants.Top;
            }
            else if (IsBottomTile(neighbors, tileId))
            {
                return TileVariants.Bottom;
            }

            else if (IsInnerTopRightCornerTile(neighbors, tileId))
            {
                return TileVariants.InnerTopRight;
            }
            else if (IsInnerTopLeftCornerTile(neighbors, tileId))
            {
                return TileVariants.InnerTopLeft;
            }
            else if (IsInnerBottomRightCornerTile(neighbors, tileId))
            {
                return TileVariants.InnerBottomRight;
            }
            else if (IsInnerBottomLeftCornerTile(neighbors, tileId))
            {
                return TileVariants.InnerBottomLeft;
            }


            else if (IsTopRightCornerTile(neighbors, tileId))
            {
                return TileVariants.OuterTopRight;
            }
            else if (IsTopLeftCornerTile(neighbors, tileId))
            {
                return TileVariants.OuterTopLeft;
            }
            else if (IsBottomRightCornerTile(neighbors, tileId))
            {
                return TileVariants.OuterBottomRight;
            }
            else if (IsBottomLeftCornerTile(neighbors, tileId))
            {
                return TileVariants.OuterBottomLeft;
            }

            else if (IsTipRightTile(neighbors, tileId))
            {
                return TileVariants.TipRight;
            }
            else if (IsTipLeftTile(neighbors, tileId))
            {
                return TileVariants.TipLeft;
            }
            else if (IsTipTopTile(neighbors, tileId))
            {
                return TileVariants.TipTop;
            }
            else if (IsTipBottomTile(neighbors, tileId))
            {
                return TileVariants.TipBottom;
            }

            else if (IsTipVerticalTile(neighbors, tileId))
            {
                return TileVariants.TipVertical;
            }
            else if (IsTipHorizontalTile(neighbors, tileId))
            {
                return TileVariants.TipHorizontal;
            }

            else if (IsMiddleTile(neighbors, tileId))
            {
                return TileVariants.Middle;
            }

            return TileVariants.Default;
        }


        public static bool IsRightTile(int[] neighbors, int tileId)
        {
            return neighbors[(int)TileNeighbor.Top] == tileId && neighbors[(int)TileNeighbor.Bottom] == tileId &&
                        neighbors[(int)TileNeighbor.Left] == tileId && neighbors[(int)TileNeighbor.Right] != tileId;
        }

        public static bool IsLeftTile(int[] neighbors, int tileId)
        {
            return neighbors[(int)TileNeighbor.Top] == tileId && neighbors[(int)TileNeighbor.Bottom] == tileId &&
                        neighbors[(int)TileNeighbor.Right] == tileId && neighbors[(int)TileNeighbor.Left] != tileId;
        }

        public static bool IsTopTile(int[] neighbors, int tileId)
        {
            return neighbors[(int)TileNeighbor.Right] == tileId && neighbors[(int)TileNeighbor.Left] == tileId &&
                        neighbors[(int)TileNeighbor.Bottom] == tileId && neighbors[(int)TileNeighbor.Top] != tileId;
        }

        public static bool IsBottomTile(int[] neighbors, int tileId)
        {
            return neighbors[(int)TileNeighbor.Right] == tileId && neighbors[(int)TileNeighbor.Left] == tileId &&
                        neighbors[(int)TileNeighbor.Top] == tileId && neighbors[(int)TileNeighbor.Bottom] != tileId;
        }

        public static bool IsMiddleTile(int[] neighbors, int tileId)
        {
            bool result = true;
            foreach(int neighbor in neighbors)
            {
                if (neighbor != tileId)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }


        public static bool IsInnerTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Right] == tileId && neighbors[(int)TileNeighbor.Top] == tileId &&
                  neighbors[(int)TileNeighbor.Left] == tileId && neighbors[(int)TileNeighbor.Bottom] == tileId);
        }

        public static bool IsInnerTopRightCornerTile(int[] neighbors, int tileId)
        {
            return IsInnerTile(neighbors, tileId) && neighbors[(int)TileNeighbor.TopRight] != tileId;
        }

        public static bool IsInnerTopLeftCornerTile(int[] neighbors, int tileId)
        {
            return IsInnerTile(neighbors, tileId) && neighbors[(int)TileNeighbor.TopLeft] != tileId;
        }

        public static bool IsInnerBottomRightCornerTile(int[] neighbors, int tileId)
        {
            return IsInnerTile(neighbors, tileId) && neighbors[(int)TileNeighbor.BottomRight] != tileId;
        }

        public static bool IsInnerBottomLeftCornerTile(int[] neighbors, int tileId)
        {
            return IsInnerTile(neighbors, tileId) && neighbors[(int)TileNeighbor.BottomLeft] != tileId;
        }




        public static bool IsTopRightCornerTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Left] == tileId && neighbors[(int)TileNeighbor.Bottom] == tileId &&
            neighbors[(int)TileNeighbor.Right] != tileId && neighbors[(int)TileNeighbor.Top] != tileId);
        }

        public static bool IsTopLeftCornerTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Right] == tileId && neighbors[(int)TileNeighbor.Bottom] == tileId &&
            neighbors[(int)TileNeighbor.Left] != tileId && neighbors[(int)TileNeighbor.Top] != tileId);
        }

        public static bool IsBottomRightCornerTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Left] == tileId && neighbors[(int)TileNeighbor.Top] == tileId && 
            neighbors[(int)TileNeighbor.Right] != tileId && neighbors[(int)TileNeighbor.Bottom] != tileId);
        }

        public static bool IsBottomLeftCornerTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Right] == tileId && neighbors[(int)TileNeighbor.Top] == tileId &&
            neighbors[(int)TileNeighbor.Left] != tileId && neighbors[(int)TileNeighbor.Bottom] != tileId);
        }


        public static bool IsTipRightTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Left] == tileId &&
                neighbors[(int)TileNeighbor.Top] != tileId && neighbors[(int)TileNeighbor.Bottom] != tileId &&
                 neighbors[(int)TileNeighbor.Right] != tileId);
        }

        public static bool IsTipLeftTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Right] == tileId &&
                neighbors[(int)TileNeighbor.Top] != tileId && neighbors[(int)TileNeighbor.Bottom] != tileId &&
                 neighbors[(int)TileNeighbor.Left] != tileId);
        }

        public static bool IsTipTopTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Bottom] == tileId &&
                neighbors[(int)TileNeighbor.Right] != tileId && neighbors[(int)TileNeighbor.Left] != tileId &&
                 neighbors[(int)TileNeighbor.Top] != tileId);
        }

        public static bool IsTipBottomTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Top] == tileId &&
                neighbors[(int)TileNeighbor.Right] != tileId && neighbors[(int)TileNeighbor.Left] != tileId &&
                 neighbors[(int)TileNeighbor.Bottom] != tileId);
        }

        public static bool IsTipVerticalTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Top] == tileId && neighbors[(int)TileNeighbor.Bottom] == tileId &&
                neighbors[(int)TileNeighbor.Right] != tileId && neighbors[(int)TileNeighbor.Left] != tileId);
        }

        public static bool IsTipHorizontalTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)TileNeighbor.Right] == tileId && neighbors[(int)TileNeighbor.Left] == tileId &&
                neighbors[(int)TileNeighbor.Top] != tileId && neighbors[(int)TileNeighbor.Bottom] != tileId);
        }
    
    }

    
    public enum TileNeighbor
    {
        Right = 0,
        Left = 1,
        Top = 2,
        Bottom = 3,
        TopRight = 4,
        TopLeft = 5,
        BottomRight = 6,
        BottomLeft = 7
    }

    public enum TileVariants
    {
        Right = 0,
        Left = 1,
        Top = 2,
        Bottom = 3,
        InnerTopRight = 4,
        InnerTopLeft = 5,
        InnerBottomRight = 6,
        InnerBottomLeft = 7,
        OuterTopRight = 8,
        OuterTopLeft = 9,
        OuterBottomRight = 10,
        OuterBottomLeft = 11,
        TipRight = 12,
        TipLeft = 13,
        TipTop = 14,
        TipBottom = 15,
        TipVertical = 16,
        TipHorizontal = 17,
        Middle = 18,
        Default = 19
    }
}