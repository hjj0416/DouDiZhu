using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvent{
    public const int START_PANEL_ACTIVE = 0;//开始面板显示
    public const int REGIST_PANEL_ACTIVE = 1;//注册面板显示
    public const int SETTING_PANEL_ACTIVE = 2;//设置面板显示
    public const int CREATE_PANEL_ACTIVE = 3;//创建名称面板显示
    public const int MATCH_PANEL_ACTIVE = 4;//匹配面板显示

    public const int REFRESH_INFO_PANEL = 10;//刷新信息主面板
    public const int SHOW_ENTER_ROOM_BUTTON = 11;//显示进入房间按钮
    public const int SHOW_GRAB_BUTTON = 12;//显示抢地主按钮
    public const int SHOW_DEAL_BUTTON = 13;//显示出牌按钮
    public const int SHOW_OVER_PANEL = 14;//显示结束面板

    public const int SET_TABLE_CARD = 100;//设置底牌
    public const int SET_LEFT_PLAYER_DATA = 101;//设置左边角色的数据
    public const int SET_RIGHT_PLAYER_DATA=102;//设置右边角色的数据
    public const int SET_MY_PLAER_DATA = 103;//设置自身角色数据

    public const int PLAYER_READY = 1000;//角色准备
    public const int PLAYER_ENTER = 1001;//角色进入
    public const int PLAYER_LEAVE = 1002;//角色离开
    public const int PLAYER_CHAT = 1003;//角色聊天
    public const int PLAY_CHANGE_IDENTITY = 1004;//更换地主农民身份
    public const int PLAYER_HIDE_STATE = 1005;//开始游戏隐藏状态面板
    public const int PLAYER_HIDE_READY_BUTTON = 1006;//隐藏准备按钮

    public const int CHANGE_MUTIPLE = 1100;//改变倍数

    
    public const int PROMPT_MSG = 9999;
}
