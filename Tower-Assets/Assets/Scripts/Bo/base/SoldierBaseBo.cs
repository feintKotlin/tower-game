using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBaseBo : ChessBaseBo
{
    //驻扎型
    private object moveGoalPoint;//移动目标点
    private double moveSpeed;//移动速度
    //行进型
    private List<Object> movePointList;//移动目标点的集合
    private bool move;//是否行进
}
