using System;
using UnityEngine;

public abstract class ListViewItem<T> : MonoBehaviour 
{
    protected T data;
    protected int index;

    public T Data { get { return data; } }
    public int Index { get { return index; } }

    public abstract void Init(T _data, int _index);
}
