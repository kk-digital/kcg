
using Enums.Tile;
using System;

namespace PlanetTileMap
{


    public static class SpriteRule_R3
    {
        // neighbor positions 
        // power of 2 numbers 
        // we need them to do bitwise operations
        public static int BitField_Right = 1 << 0; // 1
        public static int BitField_Left = 1 << 1; // 2
        public static int BitField_Up = 1 << 2; // 4
        public static int BitField_Down = 1 << 3; // 8
        public static int BitField_UpRight = 1 << 4; // 16
        public static int BitField_UpLeft = 1 << 5; // 32
        public static int BitField_DownRight = 1 << 6; // 64
        public static int BitField_DownLeft = 1 << 7; // 128
        
        
        // these are the bits associated with Position 12 in the tile sheet
        public static int R3_P12 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpRight | BitField_UpLeft | BitField_DownRight | BitField_DownLeft;
        
        // these are the bits associated with Position 17 in the tile sheet
        public static int R3_P17 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpRight | BitField_UpLeft | BitField_DownRight;
        
        // these are the bits associated with Position 19 in the tile sheet
        public static int R3_P19 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpRight | BitField_UpLeft; 
        
        // these are the bits associated with Position 16 in the tile sheet
        public static int R3_P16 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpRight | BitField_UpLeft | BitField_DownLeft; 
        
        // these are the bits associated with Position 27 in the tile sheet
        public static int R3_P27 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpLeft | BitField_DownRight | BitField_DownLeft;
        
        // these are the bits associated with Position 30 in the tile sheet
        public static int R3_P30 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_DownRight | BitField_DownLeft;
        
        // these are the bits associated with Position 28 in the tile sheet
        public static int R3_P28 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpRight | BitField_DownRight | BitField_DownLeft;
        
        // these are the bits associated with Position 49 in the tile sheet
        public static int R3_P49 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpLeft | BitField_DownLeft;
        
        // these are the bits associated with Position 50 in the tile sheet
        public static int R3_P50 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpRight | BitField_DownRight;
        
        // these are the bits associated with Position 9 in the tile sheet
        public static int R3_P9 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpLeft | BitField_DownRight;
        
        // these are the bits associated with Position 20 in the tile sheet
        public static int R3_P20 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpRight | BitField_DownLeft;
        
        // these are the bits associated with Position 52 in the tile sheet
        public static int R3_P52 = BitField_Right | BitField_Left | BitField_Up | BitField_Down;
        
        // these are the bits associated with Position 32 in the tile sheet
        public static int R3_P32 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_DownLeft;
        
        // these are the bits associated with Position 31 in the tile sheet
        public static int R3_P31 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_DownRight;
        
        // these are the bits associated with Position 43 in the tile sheet
        public static int R3_P43 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpLeft;
        
        // these are the bits associated with Position 42 in the tile sheet
        public static int R3_P42 = BitField_Right | BitField_Left | BitField_Up | BitField_Down |
            BitField_UpRight;
        
        
        // these are the bits associated with Position 34 in the tile sheet
        public static int R3_P34_1 = BitField_Right | BitField_Left;
        public static int R3_P34_2 = BitField_Right | BitField_Left | BitField_UpRight;
        public static int R3_P34_3 = BitField_Right | BitField_Left | BitField_UpLeft;
        public static int R3_P34_4 = BitField_Right | BitField_Left | BitField_DownRight;
        public static int R3_P34_5 = BitField_Right | BitField_Left | BitField_DownLeft;
        public static int R3_P34_6 = BitField_Right | BitField_Left | BitField_UpRight | 
            BitField_UpLeft;
        public static int R3_P34_7 = BitField_Right | BitField_Left | BitField_UpRight | 
            BitField_DownRight;
        public static int R3_P34_8 = BitField_Right | BitField_Left | BitField_UpRight | 
            BitField_DownLeft;
        public static int R3_P34_9 = BitField_Right | BitField_Left | BitField_UpRight | 
            BitField_UpLeft | BitField_DownRight;
        public static int R3_P34_10 = BitField_Right | BitField_Left | BitField_UpRight | 
            BitField_DownRight | BitField_DownLeft;
        public static int R3_P34_11 = BitField_Right | BitField_Left | BitField_UpRight | 
            BitField_UpLeft | BitField_DownLeft;
        public static int R3_P34_12 = BitField_Right | BitField_Left | BitField_UpRight
            | BitField_UpLeft | BitField_DownRight | BitField_DownLeft;    
        public static int R3_P34_13 = BitField_Right | BitField_Left | BitField_UpLeft |
            BitField_DownRight;
        public static int R3_P34_14 = BitField_Right | BitField_Left | BitField_UpLeft |
            BitField_DownLeft;      
        public static int R3_P34_15 = BitField_Right | BitField_Left | BitField_UpLeft |
            BitField_DownRight | BitField_DownLeft;     
        public static int R3_P34_16 = BitField_Right | BitField_Left | BitField_DownRight |
            BitField_DownLeft;
        
        
        // these are the bits associated with Position 14 in the tile sheet
        public static int R3_P14_1 = BitField_Up | BitField_Down;
        public static int R3_P14_2 = BitField_Up | BitField_Down | BitField_UpRight;
        public static int R3_P14_3 = BitField_Up | BitField_Down | BitField_UpLeft;
        public static int R3_P14_4 = BitField_Up | BitField_Down | BitField_DownRight;
        public static int R3_P14_5 = BitField_Up | BitField_Down | BitField_DownLeft;    
        public static int R3_P14_6 = BitField_Up | BitField_Down | BitField_UpRight | 
            BitField_UpLeft;
        public static int R3_P14_7 = BitField_Up | BitField_Down | BitField_UpRight | 
            BitField_DownRight;
        public static int R3_P14_8 = BitField_Up | BitField_Down | BitField_UpRight | 
            BitField_DownLeft;
        public static int R3_P14_9 = BitField_Up | BitField_Down | BitField_UpRight | 
            BitField_UpLeft | BitField_DownRight;
        public static int R3_P14_10 = BitField_Up | BitField_Down | BitField_UpRight | 
            BitField_DownRight | BitField_DownLeft;
        public static int R3_P14_11 = BitField_Up | BitField_Down | BitField_UpRight | 
            BitField_UpLeft | BitField_DownLeft;
        public static int R3_P14_12 = BitField_Up | BitField_Down | BitField_UpRight
            | BitField_UpLeft | BitField_DownRight | BitField_DownLeft;
        public static int R3_P14_13 = BitField_Up | BitField_Down | BitField_UpLeft |
            BitField_DownRight;
        public static int R3_P14_14 = BitField_Up | BitField_Down | BitField_UpLeft |
            BitField_DownLeft;
        public static int R3_P14_15 = BitField_Up | BitField_Down | BitField_UpLeft |
            BitField_DownRight | BitField_DownLeft;
        public static int R3_P14_16 = BitField_Up | BitField_Down | BitField_DownRight |
            BitField_DownLeft;
        
        
        // these are the bits associated with Position 33 in the tile sheet
        public static int R3_P33_1 = BitField_Right;
        public static int R3_P33_2 = BitField_Right | BitField_UpRight;
        public static int R3_P33_3 = BitField_Right | BitField_DownRight;
        public static int R3_P33_4 = BitField_Right | BitField_UpRight | BitField_DownRight;
        public static int R3_P33_5 = BitField_Right | BitField_UpRight | BitField_DownRight |
            BitField_UpLeft;
        public static int R3_P33_6 = BitField_Right | BitField_UpRight | BitField_DownRight |
            BitField_DownLeft;
        public static int R3_P33_7 = BitField_Right | BitField_UpRight | BitField_DownRight | 
            BitField_UpLeft | BitField_DownLeft;
        public static int R3_P33_8 = BitField_Right | BitField_DownRight | BitField_UpLeft;
        public static int R3_P33_9 = BitField_Right | BitField_DownRight | BitField_DownLeft;
        public static int R3_P33_10 = BitField_Right | BitField_DownRight | BitField_UpLeft |
            BitField_DownLeft;
        public static int R3_P33_11 = BitField_Right | BitField_UpRight | BitField_UpLeft;
        public static int R3_P33_12 = BitField_Right | BitField_UpRight | BitField_DownLeft;
        public static int R3_P33_13 = BitField_Right | BitField_UpRight | BitField_UpLeft |
            BitField_DownLeft;
        public static int R3_P33_14 = BitField_Right | BitField_UpLeft;
        public static int R3_P33_15 = BitField_Right | BitField_DownLeft;
        public static int R3_P33_16 = BitField_Right | BitField_UpLeft |
            BitField_DownLeft;
        
        
        
        // these are the bits associated with Position 35 in the tile sheet
        public static int R3_P35_1 = BitField_Left;
        public static int R3_P35_2 = BitField_Left | BitField_UpRight;
        public static int R3_P35_3 = BitField_Left | BitField_DownRight;
        public static int R3_P35_4 = BitField_Left | BitField_UpRight | BitField_DownRight;
        
        public static int R3_P35_5 = BitField_Left | BitField_UpRight | BitField_DownRight |
            BitField_UpLeft;
        public static int R3_P35_6 = BitField_Left | BitField_UpRight | BitField_DownRight |
            BitField_DownLeft;
        public static int R3_P35_7 = BitField_Left | BitField_UpRight | BitField_DownRight | 
            BitField_UpLeft | BitField_DownLeft;
        
        public static int R3_P35_8 = BitField_Left | BitField_DownRight | BitField_UpLeft;
        public static int R3_P35_9 = BitField_Left | BitField_DownRight | BitField_DownLeft;
        public static int R3_P35_10 = BitField_Left | BitField_DownRight | BitField_UpLeft |
            BitField_DownLeft;
        
        public static int R3_P35_11 = BitField_Left | BitField_UpRight | BitField_UpLeft;
        public static int R3_P35_12 = BitField_Left | BitField_UpRight | BitField_DownLeft;
        public static int R3_P35_13 = BitField_Left | BitField_UpRight | BitField_UpLeft |
            BitField_DownLeft;
        
        public static int R3_P35_14 = BitField_Left | BitField_UpLeft;
        public static int R3_P35_15 = BitField_Left | BitField_DownLeft;
        public static int R3_P35_16 = BitField_Left | BitField_UpLeft |
            BitField_DownLeft;
        
        // these are the bits associated with Position 25 in the tile sheet
        public static int R3_P25_1 = BitField_Up;
        public static int R3_P25_2 = BitField_Up | BitField_UpRight;
        public static int R3_P25_3 = BitField_Up | BitField_DownRight;
        public static int R3_P25_4 = BitField_Up | BitField_UpLeft;
        public static int R3_P25_5 = BitField_Up | BitField_DownLeft;  
        public static int R3_P25_6 = BitField_Up | BitField_UpRight | BitField_DownRight;
        public static int R3_P25_7 = BitField_Up | BitField_UpRight | BitField_UpLeft;
        public static int R3_P25_8 = BitField_Up | BitField_UpRight | BitField_DownLeft;
        public static int R3_P25_9 = BitField_Up | BitField_DownRight | BitField_UpLeft;
        public static int R3_P25_10 = BitField_Up | BitField_DownRight | BitField_DownLeft;
        public static int R3_P25_11 = BitField_Up | BitField_UpLeft | BitField_DownLeft;
        public static int R3_P25_12 = BitField_Up | BitField_UpRight | BitField_DownRight |
            BitField_UpLeft;
        public static int R3_P25_13 = BitField_Up | BitField_UpRight | BitField_DownRight |
            BitField_DownLeft;
        public static int R3_P25_14 = BitField_Up | BitField_DownRight |
            BitField_DownLeft | BitField_UpLeft;
        public static int R3_P25_15 = BitField_Up | BitField_UpRight |
            BitField_DownLeft | BitField_UpLeft;
        public static int R3_P25_16 = BitField_Up | BitField_UpRight | BitField_DownRight |
            BitField_DownLeft | BitField_UpLeft;
        
        
        // these are the bits associated with Position 3 in the tile sheet
        public static int R3_P3_1 = BitField_Down;
        public static int R3_P3_2 = BitField_Down | BitField_UpRight;
        public static int R3_P3_3 = BitField_Down | BitField_DownRight;
        public static int R3_P3_4 = BitField_Down | BitField_UpRight | BitField_DownRight;
        public static int R3_P3_5 = BitField_Down | BitField_UpRight | BitField_DownRight |
            BitField_UpLeft;
        public static int R3_P3_6 = BitField_Down | BitField_UpRight | BitField_DownRight |
            BitField_DownLeft;
        public static int R3_P3_7 = BitField_Down | BitField_UpRight | BitField_DownRight | 
            BitField_UpLeft | BitField_DownLeft;
        public static int R3_P3_8 = BitField_Down | BitField_DownRight | BitField_UpLeft;
        public static int R3_P3_9 = BitField_Down | BitField_DownRight | BitField_DownLeft;
        public static int R3_P3_10 = BitField_Down | BitField_DownRight | BitField_UpLeft |
            BitField_DownLeft;
        public static int R3_P3_11 = BitField_Down | BitField_UpRight | BitField_UpLeft;
        public static int R3_P3_12 = BitField_Down | BitField_UpRight | BitField_DownLeft;
        public static int R3_P3_13 = BitField_Down | BitField_UpRight | BitField_UpLeft |
            BitField_DownLeft;
        public static int R3_P3_14 = BitField_Down | BitField_UpLeft;
        public static int R3_P3_15 = BitField_Down | BitField_DownLeft;
        public static int R3_P3_16 = BitField_Down | BitField_UpLeft |
            BitField_DownLeft;
        
        
        
        // these are the bits associated with Position 0 in the tile sheet
        public static int R3_P0_1 = BitField_Right | BitField_Down | BitField_DownRight;
        public static int R3_P0_2 = BitField_Right | BitField_Down | BitField_DownRight |
            BitField_UpRight;
        public static int R3_P0_3 = BitField_Right | BitField_Down | BitField_DownRight |
            BitField_UpLeft;
        public static int R3_P0_4 = BitField_Right | BitField_Down | BitField_DownRight |
            BitField_DownLeft;
        public static int R3_P0_5 = BitField_Right | BitField_Down | BitField_DownRight |
            BitField_UpRight | BitField_UpLeft;
        public static int R3_P0_6 = BitField_Right | BitField_Down | BitField_DownRight |
            BitField_UpRight | BitField_DownLeft;
        public static int R3_P0_7 = BitField_Right | BitField_Down | BitField_DownRight |
            BitField_DownLeft | BitField_UpLeft;
        public static int R3_P0_8 = BitField_Right | BitField_Down | BitField_DownRight |
            BitField_UpRight | BitField_UpLeft | BitField_DownLeft;
        
        
        // these are the bits associated with Position 4 in the tile sheet
        public static int R3_P4_1 = BitField_Right | BitField_Down;
        public static int R3_P4_2 = BitField_Right | BitField_Down | BitField_UpRight;
        public static int R3_P4_3 = BitField_Right | BitField_Down | BitField_UpLeft;
        public static int R3_P4_4 = BitField_Right | BitField_Down | BitField_DownLeft;
        public static int R3_P4_5 = BitField_Right | BitField_Down | BitField_UpRight |
            BitField_UpLeft;
        public static int R3_P4_6 = BitField_Right | BitField_Down | BitField_UpRight|
            BitField_DownLeft;
        public static int R3_P4_7 = BitField_Right | BitField_Down | BitField_DownLeft |
            BitField_UpLeft;
        public static int R3_P4_8 = BitField_Right | BitField_Down | BitField_UpRight |
            BitField_UpLeft | BitField_DownLeft;
        
        // these are the bits associated with Position 2 in the tile sheet
        public static int R3_P2_1 = BitField_Left | BitField_Down | BitField_DownLeft;
        public static int R3_P2_2 = BitField_Left | BitField_Down | BitField_DownLeft |
            BitField_DownRight;
        public static int R3_P2_3 = BitField_Left | BitField_Down | BitField_DownLeft|
            BitField_UpRight;
        public static int R3_P2_4 = BitField_Left | BitField_Down | BitField_DownLeft |
            BitField_UpLeft;
        public static int R3_P2_5 = BitField_Left | BitField_Down | BitField_DownLeft |
            BitField_DownRight | BitField_UpRight;
        public static int R3_P2_6 = BitField_Left | BitField_Down | BitField_DownLeft |
            BitField_DownRight | BitField_UpLeft;
        public static int R3_P2_7 = BitField_Left | BitField_Down | BitField_DownLeft |
            BitField_DownRight | BitField_UpRight | BitField_UpLeft;
        public static int R3_P2_8 = BitField_Left | BitField_Down | BitField_DownLeft |
            BitField_DownRight | BitField_UpLeft;
        public static int R3_P2_9 = BitField_Left | BitField_Down | BitField_DownLeft |
            BitField_UpRight | BitField_UpLeft;
        
        // these are the bits associated with Position 7 in the tile sheet
        public static int R3_P7_1 = BitField_Left | BitField_Down;
        public static int R3_P7_2 = BitField_Left | BitField_Down |
            BitField_DownRight;
        public static int R3_P7_3 = BitField_Left | BitField_Down |
            BitField_UpRight;
        public static int R3_P7_4 = BitField_Left | BitField_Down |
            BitField_UpLeft;
        public static int R3_P7_5 = BitField_Left | BitField_Down |
            BitField_DownRight | BitField_UpRight;
        public static int R3_P7_6 = BitField_Left | BitField_Down |
            BitField_DownRight | BitField_UpLeft;
        public static int R3_P7_7 = BitField_Left | BitField_Down |
            BitField_DownRight | BitField_UpRight | BitField_UpLeft;
        public static int R3_P7_8 = BitField_Left | BitField_Down |
            BitField_DownRight | BitField_UpLeft;
        public static int R3_P7_9 = BitField_Left | BitField_Down |
            BitField_UpRight | BitField_UpLeft;
        
        
        // these are the bits associated with Position 22 in the tile sheet
        public static int R3_P22_1 = BitField_Right | BitField_Up | BitField_UpRight;
        public static int R3_P22_2 = BitField_Right | BitField_Up | BitField_UpRight |
            BitField_UpLeft;
        public static int R3_P22_3 = BitField_Right | BitField_Up | BitField_UpRight |
            BitField_DownRight;
        public static int R3_P22_4 = BitField_Right | BitField_Up | BitField_UpRight |
            BitField_DownLeft;
        public static int R3_P22_5 = BitField_Right | BitField_Up | BitField_UpRight |
            BitField_UpLeft | BitField_DownRight;
        public static int R3_P22_6 = BitField_Right | BitField_Up | BitField_UpRight |
            BitField_UpLeft | BitField_DownLeft;
        public static int R3_P22_7 = BitField_Right | BitField_Up | BitField_UpRight |
            BitField_DownRight | BitField_DownLeft;
        public static int R3_P22_8 = BitField_Right | BitField_Up | BitField_UpRight |
            BitField_UpLeft | BitField_DownRight | BitField_DownLeft;
        public static int R3_P22_9 = BitField_Right | BitField_Up | BitField_UpRight |
            BitField_DownRight | BitField_DownLeft;
        
        
        // these are the bits associated with Position 37 in the tile sheet
        public static int R3_P37_1 = BitField_Right | BitField_Up;
        public static int R3_P37_2 = BitField_Right | BitField_Up|
            BitField_UpLeft;
        public static int R3_P37_3 = BitField_Right | BitField_Up|
            BitField_DownRight;
        public static int R3_P37_4 = BitField_Right | BitField_Up|
            BitField_DownLeft;
        public static int R3_P37_5 = BitField_Right | BitField_Up|
            BitField_UpLeft | BitField_DownRight;
        public static int R3_P37_6 = BitField_Right | BitField_Up|
            BitField_UpLeft | BitField_DownLeft;
        public static int R3_P37_7 = BitField_Right | BitField_Up|
            BitField_DownRight | BitField_DownLeft;
        public static int R3_P37_8 = BitField_Right | BitField_Up |
            BitField_UpLeft | BitField_DownRight | BitField_DownLeft;
        public static int R3_P37_9 = BitField_Right | BitField_Up|
            BitField_DownRight | BitField_DownLeft;
        
        
        
        // these are the bits associated with Position 24 in the tile sheet
        public static int R3_P24_1 = BitField_Left | BitField_Up | BitField_UpLeft;
        public static int R3_P24_2 = BitField_Left | BitField_Up | BitField_UpLeft |
            BitField_UpRight;
        public static int R3_P24_3 = BitField_Left | BitField_Up | BitField_UpLeft | 
            BitField_DownRight;
        public static int R3_P24_4 = BitField_Left | BitField_Up | BitField_UpLeft | 
            BitField_DownLeft;
        public static int R3_P24_5 = BitField_Left | BitField_Up | BitField_UpLeft |
            BitField_UpRight | BitField_DownRight;
        public static int R3_P24_6 = BitField_Left | BitField_Up | BitField_UpLeft |
            BitField_UpRight | BitField_DownLeft;  
        public static int R3_P24_7 = BitField_Left | BitField_Up | BitField_UpLeft |
            BitField_DownLeft | BitField_DownRight;
        public static int R3_P24_8 = BitField_Left | BitField_Up | BitField_UpLeft |
            BitField_UpRight | BitField_DownLeft | BitField_DownRight;
        public static int R3_P24_9 = BitField_Left | BitField_Up | BitField_UpLeft |
            BitField_DownLeft | BitField_DownRight;
        
        
        // these are the bits associated with Position 40 in the tile sheet
        public static int R3_P40_1 = BitField_Left | BitField_Up;
        public static int R3_P40_2 = BitField_Left | BitField_Up|
            BitField_UpRight;
        public static int R3_P40_3 = BitField_Left | BitField_Up | 
            BitField_DownRight;
        public static int R3_P40_4 = BitField_Left | BitField_Up | 
            BitField_DownLeft;
        public static int R3_P40_5 = BitField_Left | BitField_Up |
            BitField_UpRight | BitField_DownRight; 
        public static int R3_P40_6 = BitField_Left | BitField_Up |
            BitField_UpRight | BitField_DownLeft;
        public static int R3_P40_7 = BitField_Left | BitField_Up |
            BitField_DownLeft | BitField_DownRight;
        public static int R3_P40_8 = BitField_Left | BitField_Up |
            BitField_UpRight | BitField_DownLeft | BitField_DownRight;
        public static int R3_P40_9 = BitField_Left | BitField_Up |
            BitField_DownLeft | BitField_DownRight;
        
        
        // these are the bits associated with Position 11 in the tile sheet
        public static int R3_P11_1 = BitField_Right | BitField_UpRight |
            BitField_DownRight | BitField_Up | BitField_Down;
        public static int R3_P11_2 = BitField_Right | BitField_UpRight |
            BitField_DownRight | BitField_Up | BitField_Down | BitField_UpLeft;
        public static int R3_P11_3 = BitField_Right | BitField_UpRight |
            BitField_DownRight | BitField_Up | BitField_Down | BitField_DownLeft;
        public static int R3_P11_4 = BitField_Right | BitField_UpRight |
            BitField_DownRight | BitField_Up | BitField_Down | BitField_UpLeft |
            BitField_DownLeft;
        
        // these are the bits associated with Position 15 in the tile sheet
        public static int R3_P15_1 = BitField_Right | BitField_UpRight |
            BitField_Up | BitField_Down;
        public static int R3_P15_2 = BitField_Right | BitField_UpRight |
            BitField_Up | BitField_Down | BitField_UpLeft;
        public static int R3_P15_3 = BitField_Right | BitField_UpRight |
            BitField_Up | BitField_Down | BitField_DownLeft;
        public static int R3_P15_4 = BitField_Right | BitField_UpRight |
            BitField_Up | BitField_Down | BitField_UpLeft |
            BitField_DownLeft;
        
        // these are the bits associated with Position 26 in the tile sheet
        public static int R3_P26_1 = BitField_Right | BitField_Up | BitField_Down |
            BitField_DownRight;
        public static int R3_P26_2 = BitField_Right | BitField_Up| BitField_Down |
            BitField_DownRight | BitField_UpLeft;
        public static int R3_P26_3 = BitField_Right | BitField_Up| BitField_Down |
            BitField_DownRight | BitField_DownRight;
        public static int R3_P26_4 = BitField_Right | BitField_Up | BitField_Down |
            BitField_DownRight | BitField_DownLeft;
        public static int R3_P26_5 = BitField_Right | BitField_Up | BitField_Down |
            BitField_DownRight | BitField_UpLeft | BitField_DownRight;
        public static int R3_P26_6 = BitField_Right | BitField_Up | BitField_Down |
            BitField_DownRight | BitField_UpLeft | BitField_DownLeft;
        public static int R3_P26_7 = BitField_Right | BitField_Up | BitField_Down |
            BitField_DownRight | BitField_DownRight | BitField_DownLeft;
        public static int R3_P26_8 = BitField_Right | BitField_Up | BitField_Down |
            BitField_DownRight | BitField_UpLeft | BitField_DownRight | BitField_DownLeft;
        public static int R3_P26_9 = BitField_Right | BitField_Up | BitField_Down |
            BitField_DownRight | BitField_DownRight | BitField_DownLeft;
        
        
        // these are the bits associated with Position 48 in the tile sheet
        public static int R3_P48_1 = BitField_Right | BitField_Up | BitField_Down;
        public static int R3_P48_2 = BitField_Right | BitField_Up | BitField_Down |
            BitField_UpLeft | BitField_DownLeft;
        public static int R3_P48_3 = BitField_Right | BitField_Up | BitField_Down |
            BitField_UpLeft;
        public static int R3_P48_4 = BitField_Right | BitField_Up | BitField_Down |
            BitField_DownLeft;
        
        // these are the bits associated with Position 13 in the tile sheet
        public static int R3_P13_1 = BitField_Left | BitField_UpLeft | BitField_DownLeft |
            BitField_Up | BitField_Down;
        public static int R3_P13_2 = BitField_Left | BitField_UpLeft | BitField_DownLeft |
            BitField_Up | BitField_Down | BitField_UpRight;
        public static int R3_P13_3 = BitField_Left | BitField_UpLeft | BitField_DownLeft |
            BitField_Up | BitField_Down | BitField_DownRight;
        public static int R3_P13_4 = BitField_Left | BitField_UpLeft | BitField_DownLeft |
            BitField_Up | BitField_Down | BitField_UpRight | BitField_DownRight;
        
        
        // these are the bits associated with Position 18 in the tile sheet
        public static int R3_P18_1 = BitField_Left | BitField_UpLeft |
            BitField_Up | BitField_Down;
        public static int R3_P18_2 = BitField_Left | BitField_UpLeft |
            BitField_Up | BitField_Down | BitField_UpRight;
        public static int R3_P18_3 = BitField_Left | BitField_UpLeft |
            BitField_Up | BitField_Down | BitField_DownRight;
        public static int R3_P18_4 = BitField_Left | BitField_UpLeft |
            BitField_Up | BitField_Down | BitField_UpRight | BitField_DownRight;
        
        // these are the bits associated with Position 29 in the tile sheet
        public static int R3_P29_1 = BitField_Left | BitField_DownLeft |
            BitField_Up | BitField_Down;
        public static int R3_P29_2 = BitField_Left | BitField_DownLeft |
            BitField_Up | BitField_Down | BitField_UpRight;
        public static int R3_P29_3 = BitField_Left | BitField_DownLeft |
            BitField_Up | BitField_Down | BitField_DownRight;
        public static int R3_P29_4 = BitField_Left | BitField_DownLeft |
            BitField_Up | BitField_Down | BitField_UpRight | BitField_DownRight;
        

        // these are the bits associated with Position 51 in the tile sheet
        public static int R3_P51_1 = BitField_Left | BitField_Up | BitField_Down;
        public static int R3_P51_2 = BitField_Left | BitField_Up | BitField_Down | 
            BitField_UpRight;
        public static int R3_P51_3 = BitField_Left | BitField_Up | BitField_Down | 
            BitField_DownRight;
        public static int R3_P51_4 = BitField_Left | BitField_Up | BitField_Down |
            BitField_UpRight | BitField_DownRight;
        
        // these are the bits associated with Position 23 in the tile sheet
        public static int R3_P23_1 = BitField_Up | BitField_UpLeft | BitField_UpRight |
            BitField_Right | BitField_Left;
        public static int R3_P23_2 = BitField_Up | BitField_UpLeft | BitField_UpRight |
            BitField_Right | BitField_Left | BitField_DownRight;
        public static int R3_P23_3 = BitField_Up | BitField_UpLeft | BitField_UpRight |
            BitField_Right | BitField_Left | BitField_DownLeft;
        public static int R3_P23_4 = BitField_Up | BitField_UpLeft | BitField_UpRight |
            BitField_Right | BitField_Left |  BitField_DownRight | BitField_DownLeft;
        
        
        // these are the bits associated with Position 38 in the tile sheet
        public static int R3_P38_1 = BitField_Up | BitField_Right | BitField_Left |
            BitField_UpLeft;
        public static int R3_P38_2 = BitField_Up | BitField_Right | BitField_Left |
            BitField_UpLeft | BitField_DownRight;
        public static int R3_P38_3 = BitField_Up | BitField_Right | BitField_Left |
            BitField_UpLeft | BitField_DownLeft;
        public static int R3_P38_4 = BitField_Up | BitField_Right | BitField_Left |
            BitField_UpLeft | BitField_DownRight | BitField_DownLeft;
        

        // these are the bits associated with Position 39 in the tile sheet
        public static int R3_P39_1 = BitField_Up | BitField_Right | BitField_Left |
            BitField_UpRight | BitField_DownRight;
        public static int R3_P39_2 = BitField_Up | BitField_Right | BitField_Left |
            BitField_UpRight | BitField_DownLeft;
        public static int R3_P39_3 = BitField_Up | BitField_Right | BitField_Left |
            BitField_UpRight;
        public static int R3_P39_4 = BitField_Up | BitField_Right | BitField_Left |
            BitField_UpRight | BitField_DownRight | BitField_DownLeft;
        

        // these are the bits associated with Position 41 in the tile sheet
        public static int R3_P41_1 = BitField_Up | BitField_Left | BitField_Right;
        public static int R3_P41_2 = BitField_Up | BitField_Left | BitField_Right |
            BitField_DownRight;
        public static int R3_P41_3 = BitField_Up | BitField_Left | BitField_Right |
            BitField_DownLeft;
        public static int R3_P41_4 = BitField_Up | BitField_Left | BitField_Right |
            BitField_DownRight | BitField_DownLeft;
        
        // these are the bits associated with Position 1 in the tile sheet
        public static int R3_P1_1 = BitField_Down | BitField_DownLeft | BitField_DownRight |
            BitField_Right | BitField_Left;
        public static int R3_P1_2 = BitField_Down | BitField_DownLeft | BitField_DownRight |
            BitField_Right | BitField_Left | BitField_UpRight;
        public static int R3_P1_3 = BitField_Down | BitField_DownLeft | BitField_DownRight |
            BitField_Right | BitField_Left | BitField_UpLeft;
        public static int R3_P1_4 = BitField_Down | BitField_DownLeft | BitField_DownRight |
            BitField_Right | BitField_Left | BitField_UpLeft | BitField_UpRight;
        
        
        // these are the bits associated with Position 5 in the tile sheet
        public static int R3_P5_1 = BitField_Down | BitField_DownLeft |
            BitField_Right | BitField_Left;
        public static int R3_P5_2 = BitField_Down | BitField_DownLeft |
            BitField_Right | BitField_Left | BitField_UpRight;
        public static int R3_P5_3 = BitField_Down | BitField_DownLeft |
            BitField_Right | BitField_Left | BitField_UpLeft;
        public static int R3_P5_4 = BitField_Down | BitField_DownLeft |
            BitField_Right | BitField_Left | BitField_UpLeft | BitField_UpRight;
        
        // these are the bits associated with Position 6 in the tile sheet
        public static int R3_P6_1 = BitField_Down | BitField_DownRight |
            BitField_Right | BitField_Left;
        public static int R3_P6_2 = BitField_Down | BitField_DownRight |
            BitField_Right | BitField_Left | BitField_UpRight;
        public static int R3_P6_3 = BitField_Down | BitField_DownRight |
            BitField_Right | BitField_Left | BitField_UpLeft;
        public static int R3_P6_4 = BitField_Down | BitField_DownRight |
            BitField_Right | BitField_Left | BitField_UpLeft | BitField_UpRight;
        

        // these are the bits associated with Position 8 in the tile sheet
        public static int R3_P8_1 = BitField_Down | BitField_Right | 
            BitField_Left;
        public static int R3_P8_2 = BitField_Down | BitField_Right | BitField_Left |
            BitField_UpRight;
        public static int R3_P8_3 = BitField_Down | BitField_Right | BitField_Left | 
            BitField_UpLeft;
        public static int R3_P8_4 = BitField_Down | BitField_Right | BitField_Left |
            BitField_UpLeft | BitField_UpRight;
        
        
        
        
        public static int[] R3_Map;
        static SpriteRule_R3()
        {
            // creating the static Rule array
            R3_Map = new int[256];

            // empty cells will default to tile position 36
            for(int i = 0; i < 256; i++)
            {
                R3_Map[i] = 36; 
            } 
            
            // each position in the array must be 
            // set to a tile position

            R3_Map[R3_P12] = 12;
            R3_Map[R3_P17] = 17;
            R3_Map[R3_P19] = 19;
            R3_Map[R3_P16] = 16;
            R3_Map[R3_P27] = 27;
            R3_Map[R3_P30] = 30;
            R3_Map[R3_P28] = 28;
            R3_Map[R3_P49] = 49;
            R3_Map[R3_P50] = 50;
            R3_Map[R3_P9] = 9;
            R3_Map[R3_P20] = 20;
            R3_Map[R3_P52] = 52;
            R3_Map[R3_P32] = 32;
            R3_Map[R3_P31] = 31;
            R3_Map[R3_P43] = 43;
            R3_Map[R3_P42] = 42;
            
            R3_Map[R3_P34_1] = 34;
            R3_Map[R3_P34_2] = 34;
            R3_Map[R3_P34_3] = 34;
            R3_Map[R3_P34_4] = 34;
            R3_Map[R3_P34_5] = 34;
            R3_Map[R3_P34_6] = 34;
            R3_Map[R3_P34_7] = 34;
            R3_Map[R3_P34_8] = 34;
            R3_Map[R3_P34_9] = 34;
            R3_Map[R3_P34_10] = 34;
            R3_Map[R3_P34_11] = 34;
            R3_Map[R3_P34_12] = 34;
            R3_Map[R3_P34_13] = 34;
            R3_Map[R3_P34_14] = 34;
            R3_Map[R3_P34_15] = 34;
            R3_Map[R3_P34_16] = 34;
            
            R3_Map[R3_P14_1] = 14;
            R3_Map[R3_P14_2] = 14;
            R3_Map[R3_P14_3] = 14;
            R3_Map[R3_P14_4] = 14;
            R3_Map[R3_P14_5] = 14;
            R3_Map[R3_P14_6] = 14;
            R3_Map[R3_P14_7] = 14;
            R3_Map[R3_P14_8] = 14;
            R3_Map[R3_P14_9] = 14;
            R3_Map[R3_P14_10] = 14;
            R3_Map[R3_P14_11] = 14;
            R3_Map[R3_P14_12] = 14;
            R3_Map[R3_P14_13] = 14;
            R3_Map[R3_P14_14] = 14;
            R3_Map[R3_P14_15] = 14;
            R3_Map[R3_P14_16] = 14;
            
            R3_Map[R3_P33_1] = 33;
            R3_Map[R3_P33_2] = 33;
            R3_Map[R3_P33_3] = 33;
            R3_Map[R3_P33_4] = 33;
            R3_Map[R3_P33_5] = 33;
            R3_Map[R3_P33_6] = 33;
            R3_Map[R3_P33_7] = 33;
            R3_Map[R3_P33_8] = 33;
            R3_Map[R3_P33_9] = 33;
            R3_Map[R3_P33_10] = 33;
            R3_Map[R3_P33_11] = 33;
            R3_Map[R3_P33_12] = 33;
            R3_Map[R3_P33_13] = 33;
            R3_Map[R3_P33_14] = 33;
            R3_Map[R3_P33_15] = 33;
            R3_Map[R3_P33_16] = 33;
            
            
            R3_Map[R3_P35_1] = 35;
            R3_Map[R3_P35_2] = 35;
            R3_Map[R3_P35_3] = 35;
            R3_Map[R3_P35_4] = 35;
            R3_Map[R3_P35_5] = 35;
            R3_Map[R3_P35_6] = 35;
            R3_Map[R3_P35_7] = 35;
            R3_Map[R3_P35_8] = 35;
            R3_Map[R3_P35_9] = 35;
            R3_Map[R3_P35_10] = 35;
            R3_Map[R3_P35_11] = 35;
            R3_Map[R3_P35_12] = 35;
            R3_Map[R3_P35_13] = 35;
            R3_Map[R3_P35_14] = 35;
            R3_Map[R3_P35_15] = 35;
            R3_Map[R3_P35_16] = 35;
            
            R3_Map[R3_P25_1] = 25;
            R3_Map[R3_P25_2] = 25;
            R3_Map[R3_P25_3] = 25;
            R3_Map[R3_P25_4] = 25;
            R3_Map[R3_P25_5] = 25;
            R3_Map[R3_P25_6] = 25;
            R3_Map[R3_P25_7] = 25;
            R3_Map[R3_P25_8] = 25;
            R3_Map[R3_P25_9] = 25;
            R3_Map[R3_P25_10] = 25;
            R3_Map[R3_P25_11] = 25;
            R3_Map[R3_P25_12] = 25;
            R3_Map[R3_P25_13] = 25;
            R3_Map[R3_P25_14] = 25;
            R3_Map[R3_P25_15] = 25;
            R3_Map[R3_P25_16] = 25;
            
            R3_Map[R3_P3_1] = 3;
            R3_Map[R3_P3_2] = 3;
            R3_Map[R3_P3_3] = 3;
            R3_Map[R3_P3_4] = 3;
            R3_Map[R3_P3_5] = 3;
            R3_Map[R3_P3_6] = 3;
            R3_Map[R3_P3_7] = 3;
            R3_Map[R3_P3_8] = 3;
            R3_Map[R3_P3_9] = 3;
            R3_Map[R3_P3_10] = 3;
            R3_Map[R3_P3_11] = 3;
            R3_Map[R3_P3_12] = 3;
            R3_Map[R3_P3_13] = 3;
            R3_Map[R3_P3_14] = 3;
            R3_Map[R3_P3_15] = 3;
            R3_Map[R3_P3_16] = 3;
            
            R3_Map[R3_P0_1] = 0;
            R3_Map[R3_P0_2] = 0;
            R3_Map[R3_P0_3] = 0;
            R3_Map[R3_P0_4] = 0;
            R3_Map[R3_P0_5] = 0;
            R3_Map[R3_P0_6] = 0;
            R3_Map[R3_P0_7] = 0;
            R3_Map[R3_P0_8] = 0;
            
            R3_Map[R3_P4_1] = 4;
            R3_Map[R3_P4_2] = 4;
            R3_Map[R3_P4_3] = 4;
            R3_Map[R3_P4_4] = 4;
            R3_Map[R3_P4_5] = 4;
            R3_Map[R3_P4_6] = 4;
            R3_Map[R3_P4_7] = 4;
            R3_Map[R3_P4_8] = 4;
            
            R3_Map[R3_P2_1] = 2;
            R3_Map[R3_P2_2] = 2;
            R3_Map[R3_P2_3] = 2;
            R3_Map[R3_P2_4] = 2;
            R3_Map[R3_P2_5] = 2;
            R3_Map[R3_P2_6] = 2;
            R3_Map[R3_P2_7] = 2;
            R3_Map[R3_P2_8] = 2;
            R3_Map[R3_P2_9] = 2;
            
            R3_Map[R3_P7_1] = 7;
            R3_Map[R3_P7_2] = 7;
            R3_Map[R3_P7_3] = 7;
            R3_Map[R3_P7_4] = 7;
            R3_Map[R3_P7_5] = 7;
            R3_Map[R3_P7_6] = 7;
            R3_Map[R3_P7_7] = 7;
            R3_Map[R3_P7_8] = 7;
            R3_Map[R3_P7_9] = 7;
            
            R3_Map[R3_P22_1] = 22;
            R3_Map[R3_P22_2] = 22;
            R3_Map[R3_P22_3] = 22;
            R3_Map[R3_P22_4] = 22;
            R3_Map[R3_P22_5] = 22;
            R3_Map[R3_P22_6] = 22;
            R3_Map[R3_P22_7] = 22;
            R3_Map[R3_P22_8] = 22;
            R3_Map[R3_P22_9] = 22;
            
            R3_Map[R3_P37_1] = 37;
            R3_Map[R3_P37_2] = 37;
            R3_Map[R3_P37_3] = 37;
            R3_Map[R3_P37_4] = 37;
            R3_Map[R3_P37_5] = 37;
            R3_Map[R3_P37_6] = 37;
            R3_Map[R3_P37_7] = 37;
            R3_Map[R3_P37_8] = 37;
            R3_Map[R3_P37_9] = 37;
            
            R3_Map[R3_P24_1] = 24;
            R3_Map[R3_P24_2] = 24;
            R3_Map[R3_P24_3] = 24;
            R3_Map[R3_P24_4] = 24;
            R3_Map[R3_P24_5] = 24;
            R3_Map[R3_P24_6] = 24;
            R3_Map[R3_P24_7] = 24;
            R3_Map[R3_P24_8] = 24;
            R3_Map[R3_P24_9] = 24;
            
            R3_Map[R3_P40_1] = 40;
            R3_Map[R3_P40_2] = 40;
            R3_Map[R3_P40_3] = 40;
            R3_Map[R3_P40_4] = 40;
            R3_Map[R3_P40_5] = 40;
            R3_Map[R3_P40_6] = 40;
            R3_Map[R3_P40_7] = 40;
            R3_Map[R3_P40_8] = 40;
            R3_Map[R3_P40_9] = 40;
            
            
            R3_Map[R3_P11_1] = 11;
            R3_Map[R3_P11_2] = 11;
            R3_Map[R3_P11_3] = 11;
            R3_Map[R3_P11_4] = 11;
            
            R3_Map[R3_P15_1] = 15;
            R3_Map[R3_P15_2] = 15;
            R3_Map[R3_P15_3] = 15;
            R3_Map[R3_P15_4] = 15;
            
            R3_Map[R3_P26_1] = 26;
            R3_Map[R3_P26_2] = 26;
            R3_Map[R3_P26_3] = 26;
            R3_Map[R3_P26_4] = 26;
            R3_Map[R3_P26_5] = 26;
            R3_Map[R3_P26_6] = 26;
            R3_Map[R3_P26_7] = 26;
            R3_Map[R3_P26_8] = 26;
            R3_Map[R3_P26_9] = 26;
            
            R3_Map[R3_P48_1] = 48;
            R3_Map[R3_P48_2] = 48;
            R3_Map[R3_P48_3] = 48;
            R3_Map[R3_P48_4] = 48;
            
            R3_Map[R3_P13_1] = 13;
            R3_Map[R3_P13_2] = 13;
            R3_Map[R3_P13_3] = 13;
            R3_Map[R3_P13_4] = 13;
            
            R3_Map[R3_P18_1] = 18;
            R3_Map[R3_P18_2] = 18;
            R3_Map[R3_P18_3] = 18;
            R3_Map[R3_P18_4] = 18;
            
            R3_Map[R3_P29_1] = 29;
            R3_Map[R3_P29_2] = 29;
            R3_Map[R3_P29_3] = 29;
            R3_Map[R3_P29_4] = 29;
            
            R3_Map[R3_P51_1] = 51;
            R3_Map[R3_P51_2] = 51;
            R3_Map[R3_P51_3] = 51;
            R3_Map[R3_P51_4] = 51;
            
            R3_Map[R3_P23_1] = 23;
            R3_Map[R3_P23_2] = 23;
            R3_Map[R3_P23_3] = 23;
            R3_Map[R3_P23_4] = 23;
            
            R3_Map[R3_P38_1] = 38;
            R3_Map[R3_P38_2] = 38;
            R3_Map[R3_P38_3] = 38;
            R3_Map[R3_P38_4] = 38;
            
            R3_Map[R3_P39_1] = 39;
            R3_Map[R3_P39_2] = 39;
            R3_Map[R3_P39_3] = 39;
            R3_Map[R3_P39_4] = 39;
            
            R3_Map[R3_P41_1] = 41;
            R3_Map[R3_P41_2] = 41;
            R3_Map[R3_P41_3] = 41;
            R3_Map[R3_P41_4] = 41;
            
            R3_Map[R3_P1_1] = 1;
            R3_Map[R3_P1_2] = 1;
            R3_Map[R3_P1_3] = 1;
            R3_Map[R3_P1_4] = 1;
            
            R3_Map[R3_P5_1] = 5;
            R3_Map[R3_P5_2] = 5;
            R3_Map[R3_P5_3] = 5;
            R3_Map[R3_P5_4] = 5;
            
            R3_Map[R3_P6_1] = 6;
            R3_Map[R3_P6_2] = 6;
            R3_Map[R3_P6_3] = 6;
            R3_Map[R3_P6_4] = 6;
            
            R3_Map[R3_P8_1] = 8;
            R3_Map[R3_P8_2] = 8;
            R3_Map[R3_P8_3] = 8;
            R3_Map[R3_P8_4] = 8;
            
        }

        public static void UpdateBackSprite(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetBackTile(x, y);
            ref TileMaterial tileMaterial = ref GameState.TileCreationApi.GetMaterial(tile.MaterialType);

            int neighborsBitField = 0;

            // check if Right Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x + 1, y);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Right;
                }
            }

            // check if Left Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x - 1, y);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Left;
                }
            }

            // check if Up Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x, y + 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Up;
                }
            }

            // check if Down Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x, y - 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Down;
                }
            }

            // check if Up Right Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x + 1 < tileMap.MapSize.X && y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x + 1, y + 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_UpRight;
                }
            }

            // check if Up Left Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x - 1 >= 0 && y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x - 1, y + 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_UpLeft;
                }
            }

            // check if Down Right Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x + 1 < tileMap.MapSize.X && y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x + 1, y - 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_DownRight;
                }
            }

            // check if Down Left Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x - 1 >= 0 && y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x - 1, y - 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_DownLeft;
                }
            }





            int tilePosition = R3_Map[neighborsBitField];





            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.TileID = tileMaterial.StartTileIndex + tilePosition;
        }







        public static void UpdateMidSprite(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetMidTile(x, y);
            ref TileMaterial tileMaterial = ref GameState.TileCreationApi.GetMaterial(tile.MaterialType);

            int neighborsBitField = 0;

            // check if Right Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x + 1, y);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Right;
                }
            }

            // check if Left Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x - 1, y);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Left;
                }
            }

            // check if Up Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x, y + 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Up;
                }
            }

            // check if Down Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x, y - 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Down;
                }
            }

            // check if Up Right Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x + 1 < tileMap.MapSize.X && y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x + 1, y + 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_UpRight;
                }
            }

            // check if Up Left Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x - 1 >= 0 && y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x - 1, y + 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_UpLeft;
                }
            }

            // check if Down Right Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x + 1 < tileMap.MapSize.X && y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x + 1, y - 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_DownRight;
                }
            }

            // check if Down Left Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x - 1 >= 0 && y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x - 1, y - 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_DownLeft;
                }
            }





            int tilePosition = R3_Map[neighborsBitField];





            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.TileID = tileMaterial.StartTileIndex + tilePosition;
        }












        public static void UpdateFrontSprite(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetFrontTile(x, y);
            ref TileMaterial tileMaterial = ref GameState.TileCreationApi.GetMaterial(tile.MaterialType);

            int neighborsBitField = 0;

            // check if Right Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x + 1, y);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Right;
                }
            }

            // check if Left Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x - 1, y);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Left;
                }
            }

            // check if Up Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x, y + 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Up;
                }
            }

            // check if Down Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x, y - 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_Down;
                }
            }

            // check if Up Right Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x + 1 < tileMap.MapSize.X && y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x + 1, y + 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_UpRight;
                }
            }

            // check if Up Left Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x - 1 >= 0 && y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x - 1, y + 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_UpLeft;
                }
            }

            // check if Down Right Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x + 1 < tileMap.MapSize.X && y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x + 1, y - 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_DownRight;
                }
            }

            // check if Down Left Neighbor Exist and add the bit position 
            // only if the tile and the neighbor have the same id
            if (x - 1 >= 0 && y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x - 1, y - 1);
                if (neighborTile.MaterialType == tile.MaterialType)
                {
                    neighborsBitField |= BitField_DownLeft;
                }
            }





            int tilePosition = R3_Map[neighborsBitField];





            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.TileID = tileMaterial.StartTileIndex + tilePosition;
        }
    }
}