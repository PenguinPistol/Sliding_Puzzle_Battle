using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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

    /// <summary>
    /// Initialize List
    /// </summary>
    /// <param name="_items">list item data</param>
    public abstract IEnumerator Init(List<TData> _items);

    /// <summary>
    /// 리스트 아이템 클릭 시 처리
    /// </summary>
    /// /// <param name="_index">list item index</param>
    public abstract void SelectItem(int _index);

}
