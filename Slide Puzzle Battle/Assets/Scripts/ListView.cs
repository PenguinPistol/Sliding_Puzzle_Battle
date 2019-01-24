using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 
/// </summary>
/// <typeparam name="I">List Item</typeparam>
/// <typeparam name="D">List Item Data Clas</typeparam>
public abstract class ListView<I, D> : MonoBehaviour
{
    public List<I> items;
    public GameObject listItemPrefab;
    public Transform contentView;

    /// <summary>
    /// Initialize List
    /// </summary>
    /// <param name="_data">list item data</param>
    public abstract void Init(List<D> _items);

    /// <summary>
    /// 리스트 아이템 클릭 시 처리
    /// </summary>
    /// /// <param name="_index">list item index</param>
    public abstract void SelectItem(int _index);

}
