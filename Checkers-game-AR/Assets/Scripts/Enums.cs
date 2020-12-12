using System;

namespace Enums
{
    [Serializable]
    public enum ColorChecker
    {
        Blue =  1,
        Red  = -1
    }

    [Serializable]
    public enum BoardPosition
    {
        A1 = 11, A3 = 13, A5 = 15, A7 = 17,
        B2 = 22, B4 = 24, B6 = 26, B8 = 28,
        C1 = 31, C3 = 33, C5 = 35, C7 = 37,
        D2 = 42, D4 = 44, D6 = 46, D8 = 48,
        E1 = 51, E3 = 53, E5 = 55, E7 = 57,
        F2 = 62, F4 = 64, F6 = 66, F8 = 68,
        G1 = 71, G3 = 73, G5 = 75, G7 = 77,
        H2 = 82, H4 = 84, H6 = 86, H8 = 88,
    }
}
