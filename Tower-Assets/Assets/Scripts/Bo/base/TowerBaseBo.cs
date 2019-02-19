using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBaseBo : ChessBaseBo
{
    private int level;//塔的等级
    private List<int> upgradeCost;//升级花费
    //弹道塔
    private double hitSpeed;//攻击速度
    private int bullet;//当前子弹类型
    private List<int> bulletList;//与等级对应的子弹集合

    //光环塔
    private int bufferSend;//发出的buffer
    private List<BuffBaseBo> bufferSendtList;//与等级对应的buffer集合

    //训练塔
    private int productChessType;//训练的棋子类型
    private string standLocation;//驻扎点坐标
    private List<SoldierBaseBo> productChessTypeList;//与等级对应的训练棋子的集合
}
