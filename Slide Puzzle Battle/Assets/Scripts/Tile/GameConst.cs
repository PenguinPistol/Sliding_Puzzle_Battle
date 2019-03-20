using UnityEngine;

public class GameConst
{
    public static int Damage_SwordTile;
    public static int Damage_ArrowTile;
    public static int Damage_BombTile;
    public static float Cooldown_InterstitialAd;
    public static float Cooldown_EnergyRecovery;
    public static int EnergyAdReward;
    public static int MaxEnergy;
    public static int SwordTileRatio;

    public static void Print()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("damage sword : ");
        sb.Append(Damage_SwordTile);
        sb.AppendLine();
        sb.Append("damage arrow : ");
        sb.Append(Damage_ArrowTile);
        sb.AppendLine();
        sb.Append("damage bomb : ");
        sb.Append(Damage_BombTile);
        sb.AppendLine();
        sb.Append("cooldown interstitial ad : ");
        sb.Append(Cooldown_InterstitialAd);
        sb.AppendLine();
        sb.Append("cooldown energy recovery : ");
        sb.Append(Cooldown_EnergyRecovery);
        sb.AppendLine();
        sb.Append("energy reward : ");
        sb.Append(EnergyAdReward);
        sb.AppendLine();
        sb.Append("max energy : ");
        sb.Append(MaxEnergy);
        sb.AppendLine();
        sb.Append("sword tile ratio : ");
        sb.Append(SwordTileRatio);
        sb.AppendLine();

        Debug.Log(sb.ToString());
    }
}
