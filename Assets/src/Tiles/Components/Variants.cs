namespace Tile
{
    public enum Neighbor
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

    public enum Variant
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
    
    public static class TileNeighbor 
    {
        public static Variant GetVariant(int[] neighbors, int tileId)
        {
            if (IsRightTile(neighbors, tileId))
            {
                return Variant.Right;
            }
            else if (IsLeftTile(neighbors, tileId))
            {
                return Variant.Left;
            }
            else if (IsTopTile(neighbors, tileId))
            {
                return Variant.Top;
            }
            else if (IsBottomTile(neighbors, tileId))
            {
                return Variant.Bottom;
            }

            else if (IsInnerTopRightCornerTile(neighbors, tileId))
            {
                return Variant.InnerTopRight;
            }
            else if (IsInnerTopLeftCornerTile(neighbors, tileId))
            {
                return Variant.InnerTopLeft;
            }
            else if (IsInnerBottomRightCornerTile(neighbors, tileId))
            {
                return Variant.InnerBottomRight;
            }
            else if (IsInnerBottomLeftCornerTile(neighbors, tileId))
            {
                return Variant.InnerBottomLeft;
            }


            else if (IsTopRightCornerTile(neighbors, tileId))
            {
                return Variant.OuterTopRight;
            }
            else if (IsTopLeftCornerTile(neighbors, tileId))
            {
                return Variant.OuterTopLeft;
            }
            else if (IsBottomRightCornerTile(neighbors, tileId))
            {
                return Variant.OuterBottomRight;
            }
            else if (IsBottomLeftCornerTile(neighbors, tileId))
            {
                return Variant.OuterBottomLeft;
            }

            else if (IsTipRightTile(neighbors, tileId))
            {
                return Variant.TipRight;
            }
            else if (IsTipLeftTile(neighbors, tileId))
            {
                return Variant.TipLeft;
            }
            else if (IsTipTopTile(neighbors, tileId))
            {
                return Variant.TipTop;
            }
            else if (IsTipBottomTile(neighbors, tileId))
            {
                return Variant.TipBottom;
            }

            else if (IsTipVerticalTile(neighbors, tileId))
            {
                return Variant.TipVertical;
            }
            else if (IsTipHorizontalTile(neighbors, tileId))
            {
                return Variant.TipHorizontal;
            }

            else if (IsMiddleTile(neighbors, tileId))
            {
                return Variant.Middle;
            }

            return Variant.Default;
        } 


        public static bool IsRightTile(int[] neighbors, int tileId)
        {
            return neighbors[(int)Neighbor.Top] == tileId && neighbors[(int)Neighbor.Bottom] == tileId &&
                        neighbors[(int)Neighbor.Left] == tileId && neighbors[(int)Neighbor.Right] != tileId;
        }

        public static bool IsLeftTile(int[] neighbors, int tileId)
        {
            return neighbors[(int)Neighbor.Top] == tileId && neighbors[(int)Neighbor.Bottom] == tileId &&
                        neighbors[(int)Neighbor.Right] == tileId && neighbors[(int)Neighbor.Left] != tileId;
        }

        public static bool IsTopTile(int[] neighbors, int tileId)
        {
            return neighbors[(int)Neighbor.Right] == tileId && neighbors[(int)Neighbor.Left] == tileId &&
                        neighbors[(int)Neighbor.Bottom] == tileId && neighbors[(int)Neighbor.Top] != tileId;
        }

        public static bool IsBottomTile(int[] neighbors, int tileId)
        {
            return neighbors[(int)Neighbor.Right] == tileId && neighbors[(int)Neighbor.Left] == tileId &&
                        neighbors[(int)Neighbor.Top] == tileId && neighbors[(int)Neighbor.Bottom] != tileId;
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
            return (neighbors[(int)Neighbor.Right] == tileId && neighbors[(int)Neighbor.Top] == tileId &&
                  neighbors[(int)Neighbor.Left] == tileId && neighbors[(int)Neighbor.Bottom] == tileId);
        }

        public static bool IsInnerTopRightCornerTile(int[] neighbors, int tileId)
        {
            return IsInnerTile(neighbors, tileId) && neighbors[(int)Neighbor.TopRight] != tileId;
        }

        public static bool IsInnerTopLeftCornerTile(int[] neighbors, int tileId)
        {
            return IsInnerTile(neighbors, tileId) && neighbors[(int)Neighbor.TopLeft] != tileId;
        }

        public static bool IsInnerBottomRightCornerTile(int[] neighbors, int tileId)
        {
            return IsInnerTile(neighbors, tileId) && neighbors[(int)Neighbor.BottomRight] != tileId;
        }

        public static bool IsInnerBottomLeftCornerTile(int[] neighbors, int tileId)
        {
            return IsInnerTile(neighbors, tileId) && neighbors[(int)Neighbor.BottomLeft] != tileId;
        }




        public static bool IsTopRightCornerTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Left] == tileId && neighbors[(int)Neighbor.Bottom] == tileId &&
            neighbors[(int)Neighbor.Right] != tileId && neighbors[(int)Neighbor.Top] != tileId);
        }

        public static bool IsTopLeftCornerTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Right] == tileId && neighbors[(int)Neighbor.Bottom] == tileId &&
            neighbors[(int)Neighbor.Left] != tileId && neighbors[(int)Neighbor.Top] != tileId);
        }

        public static bool IsBottomRightCornerTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Left] == tileId && neighbors[(int)Neighbor.Top] == tileId && 
            neighbors[(int)Neighbor.Right] != tileId && neighbors[(int)Neighbor.Bottom] != tileId);
        }

        public static bool IsBottomLeftCornerTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Right] == tileId && neighbors[(int)Neighbor.Top] == tileId &&
            neighbors[(int)Neighbor.Left] != tileId && neighbors[(int)Neighbor.Bottom] != tileId);
        }


        public static bool IsTipRightTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Left] == tileId &&
                neighbors[(int)Neighbor.Top] != tileId && neighbors[(int)Neighbor.Bottom] != tileId &&
                 neighbors[(int)Neighbor.Right] != tileId);
        }

        public static bool IsTipLeftTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Right] == tileId &&
                neighbors[(int)Neighbor.Top] != tileId && neighbors[(int)Neighbor.Bottom] != tileId &&
                 neighbors[(int)Neighbor.Left] != tileId);
        }

        public static bool IsTipTopTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Bottom] == tileId &&
                neighbors[(int)Neighbor.Right] != tileId && neighbors[(int)Neighbor.Left] != tileId &&
                 neighbors[(int)Neighbor.Top] != tileId);
        }

        public static bool IsTipBottomTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Top] == tileId &&
                neighbors[(int)Neighbor.Right] != tileId && neighbors[(int)Neighbor.Left] != tileId &&
                 neighbors[(int)Neighbor.Bottom] != tileId);
        }

        public static bool IsTipVerticalTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Top] == tileId && neighbors[(int)Neighbor.Bottom] == tileId &&
                neighbors[(int)Neighbor.Right] != tileId && neighbors[(int)Neighbor.Left] != tileId);
        }

        public static bool IsTipHorizontalTile(int[] neighbors, int tileId)
        {
            return (neighbors[(int)Neighbor.Right] == tileId && neighbors[(int)Neighbor.Left] == tileId &&
                neighbors[(int)Neighbor.Top] != tileId && neighbors[(int)Neighbor.Bottom] != tileId);
        }
    
    }
}