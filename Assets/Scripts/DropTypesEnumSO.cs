using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTypesEnumSO : ScriptableObject
{
    public enum DropType{
        ExtendPlayerPad = 0,
        ReducePlayerPad = 1,
        SlowPlayerPad = 2,
        FastPlayerPad = 3,
        SlowPlayerBall = 4,
        FastPlayerBall = 5,
        OneGold = 6,
        TwoGold = 7,
    }
}
