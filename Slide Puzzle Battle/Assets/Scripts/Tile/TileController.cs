using UnityEngine;
using System.Collections;
using TMPro;

public class TileController : MonoBehaviour
{
    private const float MOVE_SPEED = 20.0f;

    public Tile data;
    public Animator animator;
    public SpriteRenderer sprite;
    public TextMeshPro text;

    public int Index { get { return data.index; } }
    public System.Type Type { get { return data.GetType(); } }

    private void Update()
    {
        if(data != null)
        {
            data.Execute();
        }
    }

    public void SetData(Tile _data, Vector3 _posotion, Vector3 _scale)
    {
        data = _data;
        data.controller = this;

        sprite.sprite = data.sprite;
        transform.localPosition = _posotion;
        transform.localScale = _scale;
    }

    public IEnumerator MoveCoroutine(Vector3 _direction)
    {
        // 크기 = 이동거리
        SoundManager.Instance.PlaySE(11);
        Vector3 target = transform.localPosition + _direction * data.size * 2;

        while (Vector3.Distance(transform.localPosition, target) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Time.deltaTime * MOVE_SPEED);
            yield return null;
        }

        transform.localPosition = target;
    }
    
    public void PlayAnimation(string _name)
    {
        animator.Play(_name);
    }
}
