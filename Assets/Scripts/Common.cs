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
                return "����";
            case 2:
                return "�|��";
            case 3:
                return "����";
            case 4:
                return "�R��";
            default:
                return string.Empty;
        }
    }

    public static string FighterStrategy(int strategy)
    {
        switch (strategy)
        {
            case 1:
                return "�U���d��";
            case 2:
                return "�ϋv�d��";
            case 3:
                return "�ړ��d��";
            default:
                return string.Empty;
        }
    }
}