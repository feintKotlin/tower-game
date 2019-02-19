//棋子的基础类
using System.Collections;
using System.Collections.Generic;

public class ChessBaseBo 
{
    private string code;//棋子对象编号
    private int chessCamp;//棋子阵营
    private int chessStyle;//棋子类型
    private double chessBlood;//棋子血量
    private double hitWideth;//棋子攻击范围
    private bool enemySearch;//是否索敌
    private HashSet<string> enableChessList;//可操作棋子列表
    private HashSet<BuffBaseBo> buffReceiveList;//所受buffer列表
    private HashSet<string> goalChessList;//当前目标对象列表
    private int goalChessCamp;//目标棋子阵营
    private int goalChessNum;//目标棋子数量（-1表示攻击范围内全体攻击）
}