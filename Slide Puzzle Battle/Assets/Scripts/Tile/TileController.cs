using UnityEngine;
using System.Collections;

public class TileController : MonoBehaviour
{
    public Tile data;
    public Animator animator;

    public int Index { get { return data.index; } }

    private void Update()
    {
        if(data == null)
        {
            return;
        }

        data.Execute();
    }

    public void SetData(Tile _data, Vector3 _posotion, Vector3 _scale)
    {
        data = _data;
        data.controller = this;

        GetComponent<SpriteRenderer>().sprite = data.sprite;
        transform.localPosition = _posotion;
        transform.localScale = _scale;
    }

    public void Move(Vector3 _direction)
    {
        //int x = data.index % 4;
        //int y = data.index / 4;
        // 빈인덱스가 

        StartCoroutine(MoveCoroutine(_direction));
    }

    public IEnumerator MoveCoroutine(Vector3 _direction)
    {
        // 크기 = 이동거리
        Vector3 target = transform.localPosition + _direction * data.size * 2;

        while (Vector3.Distance(transform.localPosition, target) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Time.deltaTime * 10f);
            yield return null;
        }

        transform.localPosition = target;
    }

    public void SelectTile()
    {
        GameManager.Instance.puzzle.MoveTile(Index);
    }

    public void PlayAnimation(string _name)
    {
        animator.Play(_name);
    }

    public void Attack()
    {

    }
}
