using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// </summary>
/// <typeparam name="TItem">List Item</typeparam>
/// <typeparam name="TData">List Item Data Clas</typeparam>
public abstract class ListView<TItem, TData> : MonoBehaviour
    where TItem : ListViewItem<TData>
{
    public List<TItem> items;
    public TItem listItemPrefab;
    public Transform contentView;
    public ScrollRect scrollRect;

    /// <summary>
    /// Initialize List
    /// </summary>
    /// <param name="_items">list item data</param>
    public abstract IEnumerator Init(List<TData> _items);

    /// <summary>
    /// 리스트 아이템 클릭 시 처리
    /// </summary>
    /// <param name="_index">list item index</param>
    public abstract void SelectItem(int _index);


    /// <summary>
    /// 리스트 스크롤 위치 설정
    /// </summary>
    /// <param name="_targetIndex">target index</param>
    /// <param name="_isVertial">scroll direction.(true -> vertical / false -> holizontal)</param>
    public void SetScrollPosition(int _targetIndex, bool _isVertial)
    {
        if(scrollRect == null)
        {
            Debug.LogFormat("listview({0})::SetScrollRectPosition() >> scrollRect is null", this.GetType().Name);
            return;
        }

        if(items == null)
        {
            Debug.LogFormat("listview({0})::SetScrollRectPosition() >> items is null", this.GetType().Name);
            return;
        }

        float targetPosition = 1f - (float)_targetIndex / items.Count;

        StartCoroutine(Scroll(targetPosition, _isVertial));
    }

    private IEnumerator Scroll(float _targetPosition, bool _isVertial)
    {
        float currentPosition = 1f;

        while(currentPosition > _targetPosition)
        {
            currentPosition -= Time.deltaTime;

            if (_isVertial)
            {
                scrollRect.verticalNormalizedPosition = currentPosition;
            }
            else
            {
                scrollRect.horizontalNormalizedPosition = currentPosition;
            }

            yield return null;
        }

        scrollRect.verticalNormalizedPosition = _targetPosition;
    }
}
