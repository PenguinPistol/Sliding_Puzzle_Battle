using UnityEngine;
using System.Collections.Generic;
using com.PlugStudio.Patterns;

public class TestManager : Singleton<TestManager>
{
    public Transform board;
    public TestObject tile;
    public List<TestObject> tiles;

    public int puzzleSize = 4;

    private void Start()
    {
        CreatePuzzle();
    }

    public void CreatePuzzle()
    {
        for (int i = 0; i < puzzleSize * puzzleSize - 1; i++)
        {
            var createdTile = Instantiate(tile, board);
            createdTile.transform.localPosition
                = new Vector2((i % puzzleSize) * 2, (-i / puzzleSize) * 2);
            createdTile.index = i;


            tiles.Add(createdTile);
        }
    }

    public TestObject GetPrevTile(int _currentIndex)
    {
        return tiles.Find(x => x.index == _currentIndex - 1);
    }
}
