using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common
{
    public static string FighterType(int type)
    {
        switch (type)
        {
            case 1:
                return "•à•º";
            case 2:
                return "‹|•º";
            case 3:
                return "‚•º";
            case 4:
                return "‹R•º";
            default:
                return string.Empty;
        }
    }

    public static string FighterStrategy(int strategy)
    {
        switch (strategy)
        {
            case 1:
                return "UŒ‚d‹";
            case 2:
                return "‘Ï‹vd‹";
            case 3:
                return "ˆÚ“®d‹";
            default:
                return string.Empty;
        }
    }
}