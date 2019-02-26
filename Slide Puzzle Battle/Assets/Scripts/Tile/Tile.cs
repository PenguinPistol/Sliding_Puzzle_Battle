using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile
{

    /*
    private TileData data;
    private bool isMoved;

    public Image    sprite;
    public Text     rate;
    public int      index;
    public Animator animator;

    public TileData.TileType Type
    {
        get { return data.type; }
    }

    public bool IsAttack
    {
        get { return data.isAttack; }
    }

    public Vector2[] AttackRange
    {
        get { return data.attackRange; }
    }

    public RectTransform rectTransform;

    private void Update()
    {
        data.Execute();
    }

    public void InitData(TileData _data, Vector2 _position, float _size, int _index)
    {
        data = _data;
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();

        if (data.icon != null)
        {
            sprite.enabled = true;
            sprite.sprite = data.icon;
        }

        if(data.rate != 0)
        {
            rate.enabled = true;
            rate.text = string.Format("{0}", data.rate);
        }

        rectTransform.sizeDelta = new Vector3(_size, _size);
        StartCoroutine(StartCreateAnimation(_position));
        //rectTransform.localPosition = _position;

        index = _index;
        isMoved = false;
    }

    public void MovePosition(Vector3 _position)
    {
        if (isMoved == false)
        {
            isMoved = true;
            StartCoroutine(MoveTarget(_position));
        }
    }

    public void Attack()
    {
        var range = GameManager.Instance.GetRangeTiles(index);

        StartCoroutine(data.AttackAnimation(animator, range));
    }

    private IEnumerator MoveTarget(Vector3 _target)
    {
        while(Vector3.Distance(rectTransform.localPosition, _target) > 0.1f)
        {
            rectTransform.localPosition = Vector3.MoveTowards(rectTransform.localPosition, _target, 20);
            yield return null;
        }

        rectTransform.localPosition = _target;
        isMoved = false;
    }

    private IEnumerator StartCreateAnimation(Vector3 _position)
    {
        rectTransform.localPosition = _position + (Vector3.up * 400f);

        while (Vector3.Distance(rectTransform.localPosition, _position) > 0.1f)
        {
            rectTransform.localPosition = Vector3.MoveTowards(rectTransform.localPosition, _position, 100);
            yield return null;
        }

        rectTransform.localPosition = _position;

        GetComponent<Button>().onClick.AddListener(() => {
            if(GameManager.Instance.IsAttacked)
            {
                return;
            }

            GameManager.Instance.MoveTile(index);
        });
    }

    public void Damaged(float _damage)
    {
        Debug.Log("damageds");
        data.rate -= _damage;
    }

    */

    public float size; // 정사각형 가로
    public int index;
    public TileController controller;

    public Sprite sprite;


    public Tile(Sprite _sprite, float _size)
    {
        this.sprite = _sprite;
        size = _size;
    }

    public Tile(Tile _other)
    {
        sprite = _other.sprite;
        size = _other.size;
        index = _other.index;
    }

    /// <summary>
    /// TileController.Update() 에서 호출
    /// </summary>
    public virtual void Execute()
    {
    }
}
