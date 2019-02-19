using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBaseBo
{
    private int bufferType;//buff类型
    private double bufferData;//buff数值
    private double bufferRate;//buff频率
    private double buffTime;//buff持续时间（负数表示一直持续）
    private List<int> buffChessStyleList;//可受buff棋子类型
}
