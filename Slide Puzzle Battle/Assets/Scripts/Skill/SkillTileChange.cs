using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class SkillTileChange : Skill
{
    public TileFactory.TileType changeType;
    public int damage;

    public override void Activate(params object[] _params)
    {
            // tile change;
        TileController tile = GameManager.Instance.currentPuzzle.GetRandomTile();

        if(changeType.Equals(TileFactory.TileType.Bomb))
        {
            GameManager.Instance.currentPuzzle.ChangeTileType(tile, changeType, GameManager.Instance.BoardSize);
        }
        else
        {
            GameManager.Instance.currentPuzzle.ChangeTileType(tile, changeType);
        }
    }
}
