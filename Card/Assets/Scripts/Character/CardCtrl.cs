using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌控制类
/// </summary>
public class CardCtrl : MonoBehaviour {

    //数据
    public CardDto cardDto { get; private set; }
    //卡牌是否被选中
    public bool selected { get; set; }

    private SpriteRenderer spriteRenderer;
    private bool isMine;


	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    /// <summary>
    /// 初始化卡牌数据
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    /// <param name="isMine"></param>
    public void Init(CardDto card,int index,bool isMine)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.cardDto = card;
        this.isMine = isMine;
        //为了重用
        if (selected==true)
        {
            selected = false;
            transform.localPosition -= new Vector3(0,0.3f,0);
        }
        string resPath = string.Empty;
        if(isMine)
        {
            resPath = "Poker/" + card.Name;
        }
        else
        {
            resPath = "Poker/CardBack";
        }
        Sprite sp = Resources.Load<Sprite>(resPath);
        spriteRenderer.sprite = sp;
        spriteRenderer.sortingOrder = index;
    }

    /// <summary>
    /// 当鼠标点击时候调用
    /// </summary>
    private void OnMouseDown()
    {
        if (isMine == false)
            return;
        this.selected = !selected;
        if(selected==true)
        {
            transform.localPosition += new Vector3(0, 0.3f, 0);
        }
        else
        {
            transform.localPosition -= new Vector3(0, 0.3f, 0);
        }
    }

    public void SelectState()
    {
        if(selected==false)
        {
            this.selected = true;
            transform.localPosition += new Vector3(0, 0.3f, 0);
        }

    }
}
