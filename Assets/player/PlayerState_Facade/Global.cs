using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Color transparent = new Color(1, 1, 1, 0);
    public static Color normal = new Color(1, 1, 1, 1);

    public enum BLOCKTYPE { xMOVE, yMOVE, zMOVE };

    public const int xRot = 0;
    public const int yRot = 1;
    public const int zRot = 2;


    public static int JumpSkill = 0;
    public static int AirJumpSkill = 1;
    public static int WallJumpSkill = 2;
    public static int DashSkill = 3;
}
