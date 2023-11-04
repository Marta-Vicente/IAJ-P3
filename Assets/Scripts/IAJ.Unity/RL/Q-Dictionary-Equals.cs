using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System.Collections.Generic;
using System.Numerics;
using Assets.Scripts.Game;

public class Q_Dictionary_Equals : IEqualityComparer<Dictionary<string, object>>
{
    /*
    public bool Equals(WorldModel x, WorldModel y)
    {
        if (EvaluateHP() != y.EvaluateHP()) return false;
        if (EvaluateMana() != y.EvaluateMana()) return false;
        if (EvaluateShieldHP() != y.EvaluateShieldHP()) return false;
        if (EvaluateXP() != y.EvaluateXP()) return false;
        if (EvaluateLevel() != y.EvaluateLevel()) return false;
        if (EvaluateTime() != y.EvaluateTime()) return false;
        if (EvaluateMoney() != y.EvaluateMoney()) return false;
        if (EvaluatePosition() != y.EvaluatePosition()) return false;

        return true;
    }

    public int GetHashCode(WorldModel obj)
    {
        return obj.EvaluateHP().GetHashCode() + obj.EvaluateMana().GetHashCode()*10 + obj.EvaluateShieldHP().GetHashCode()*100 +
            obj.EvaluateXP().GetHashCode()*1000 + obj.EvaluateLevel().GetHashCode()*10000 + obj.EvaluateTime().GetHashCode()*100000 +
            obj.EvaluateMana().GetHashCode()*1000000 + obj.EvaluatePosition().GetHashCode()*10000000;
    }
    */

    public bool Equals(Dictionary<string, object> x, Dictionary<string, object> y)
    {
        try
        {
            if (EvaluateHP((int)x[Properties.HP]) != EvaluateHP((int)y[Properties.HP])) return false;
            if (EvaluateMana((int)x[Properties.MANA]) != EvaluateMana((int)y[Properties.MANA])) return false;
            if (EvaluateShieldHP((int)x[Properties.ShieldHP]) != EvaluateShieldHP((int)y[Properties.ShieldHP])) return false;
            if (EvaluateXP((int)x[Properties.XP]) != EvaluateXP((int)y[Properties.XP])) return false;
            if (EvaluateLevel((int)x[Properties.LEVEL]) != EvaluateLevel((int)y[Properties.LEVEL])) return false;
            if (EvaluateTime((float)x[Properties.TIME]) != EvaluateTime((float)y[Properties.TIME])) return false;
            if (EvaluateMoney((int)x[Properties.MONEY]) != EvaluateMoney((int)y[Properties.MONEY])) return false;
            if (EvaluatePosition((string)x[Properties.POSITION]) != EvaluatePosition((string)y[Properties.POSITION])) return false;
            return true;
        }
        catch
        {
            if (EvaluateHP((int)x[Properties.HP]) != EvaluateHP((int)y[Properties.HP])) return false;
            if (EvaluateMana((int)x[Properties.MANA]) != EvaluateMana((int)y[Properties.MANA])) return false;
            if (EvaluateShieldHP((int)x[Properties.ShieldHP]) != EvaluateShieldHP((int)y[Properties.ShieldHP])) return false;
            if (EvaluateXP((int)x[Properties.XP]) != EvaluateXP((int)y[Properties.XP])) return false;
            if (EvaluateLevel((int)x[Properties.LEVEL]) != EvaluateLevel((int)y[Properties.LEVEL])) return false;
            if (EvaluateTime((float)x[Properties.TIME]) != EvaluateTime((float)y[Properties.TIME])) return false;
            if (EvaluateMoney((int)x[Properties.MONEY]) != EvaluateMoney((int)y[Properties.MONEY])) return false;
            if (EvaluatePosition(((UnityEngine.Vector3)x[Properties.POSITION]).ToString()) != 
                EvaluatePosition(((UnityEngine.Vector3)y[Properties.POSITION]).ToString())) return false;
            return true;
        }
    }

    public int GetHashCode(Dictionary<string, object> obj)
    {
        try
        {
            return EvaluateHP((int)obj[Properties.HP]).GetHashCode() +
            EvaluateMana((int)obj[Properties.MANA]).GetHashCode() * 10 +
            EvaluateShieldHP((int)obj[Properties.ShieldHP]).GetHashCode() * 100 +
            EvaluateXP((int)obj[Properties.XP]).GetHashCode() * 1000 +
            EvaluateLevel((int)obj[Properties.LEVEL]).GetHashCode() * 10000 +
            EvaluateTime((float)obj[Properties.TIME]).GetHashCode() * 100000 +
            EvaluateMoney((int)obj[Properties.MONEY]).GetHashCode() * 1000000 +
            EvaluatePosition((string)obj[Properties.POSITION]).GetHashCode() * 10000000;
        }
        catch
        {
            return EvaluateHP((int)obj[Properties.HP]).GetHashCode() +
            EvaluateMana((int)obj[Properties.MANA]).GetHashCode() * 10 +
            EvaluateShieldHP((int)obj[Properties.ShieldHP]).GetHashCode() * 100 +
            EvaluateXP((int)obj[Properties.XP]).GetHashCode() * 1000 +
            EvaluateLevel((int)obj[Properties.LEVEL]).GetHashCode() * 10000 +
            EvaluateTime((float)obj[Properties.TIME]).GetHashCode() * 100000 +
            EvaluateMoney((int)obj[Properties.MONEY]).GetHashCode() * 1000000 +
            EvaluatePosition(((UnityEngine.Vector3)obj[Properties.POSITION]).ToString()).GetHashCode() * 10000000;  
        };
    }

    
    public static int EvaluateHP(int HP)
    {
        if (HP <= 2)
            return 0;
        else if (2 < HP && HP <= 6)
            return 1;
        else if (6 < HP && HP <= 12)
            return 2;
        else
            return 3;
    }

    public static int EvaluateMana(int mana)
    {

        if (0 <= mana && mana <= 2)
            return 0;
        else if (2 < mana && mana <= 5)
            return 1;
        else
            return 2;
    }

    public static int EvaluateShieldHP(int shieldHP)
    {

        if (0 <= shieldHP && shieldHP <= 2)
            return 0;
        else
            return 1;
    }

    public static int EvaluateXP(int xp)
    {
        if (0 <= xp && xp <= 50)
            return 0;
        else if (50 < xp && xp <= 300)
            return 1;
        else if (300 < xp && xp <= 700)
            return 2;
        else
            return 3;
    }

    public static int EvaluateLevel(int lvl)
    {
        if (lvl == 1)
            return 0;
        else
            return 1;
    }

    public static int EvaluateTime(float time)
    {
        if (0 <= time && time <= 15)
            return 0;
        else if (15 < time && time <= 30)
            return 1;
        else if (30 < time && time <= 45)
            return 2;
        else if (45 < time && time <= 60)
            return 3;
        else if (60 < time && time <= 75)
            return 4;
        else if (75 < time && time <= 90)
            return 5;
        else if (90 < time && time <= 105)
            return 6;
        else if (105 < time && time <= 120)
            return 7;
        else if (120 < time && time <= 135)
            return 8;
        else
            return 9;
    }

    public static int EvaluateMoney(int money)
    {
        if (money == 0)
            return 0;
        else if (money == 5)
            return 1;
        else if (money == 10)
            return 2;
        else if (money == 15)
            return 3;
        else if (money == 20)
            return 4;
        else
            return 5;
    }

    public static int EvaluatePosition(string posStr)
    {
        UnityEngine.Vector3 pos = ParseVector3(posStr);
        if (pos.x < 33 && pos.y < 33)
            return 0;
        else if (33 <= pos.x && pos.x < 66 && pos.y < 33)
            return 1;
        else if (66 <= pos.x && pos.y < 33)
            return 2;

        else if (pos.x < 33 && 33 <= pos.y && pos.y < 66)
            return 3;
        else if (33 <= pos.x && pos.x < 66 && 33 <= pos.y && pos.y < 66)
            return 4;
        else if (66 <= pos.x && 33 <= pos.y && pos.y < 66)
            return 5;

        else if (pos.x < 33 && 66 <= pos.y)
            return 6;
        else if (33 <= pos.x && pos.x < 66 && 66 <= pos.y)
            return 7;
        else if (66 <= pos.x && 66 <= pos.y)
            return 8;
        else
            return 9;

    }
    

    /*
    public static int EvaluateHP(int HP)
    {
        if (HP <= 2)
            return 0;
        else if (2 < HP && HP <= 6)
            return 1;
        else if (6 < HP && HP <= 9)
            return 2;
        else if (9 < HP && HP <= 12)
            return 3;
        else
            return 3;
    }

    public static int EvaluateMana(int mana)
    {

        if (0 <= mana && mana <= 2)
            return 0;
        else if (2 < mana && mana <= 5)
            return 1;
        else if (5 < mana && mana <= 7)
            return 2;
        else
            return 3;
    }

    public static int EvaluateShieldHP(int shieldHP)
    {

        if (0 <= shieldHP && shieldHP <= 2)
            return 0;
        else
            return 1;
    }

    public static int EvaluateXP(int xp)
    {
        if (0 <= xp && xp <= 10)
            return 0;
        else if (10 < xp && xp <= 20)
            return 1;
        else if (20 < xp && xp <= 30)
            return 2;
        else if (30 < xp && xp <= 40)
            return 3;
        else
            return 4;
    }

    public static int EvaluateLevel(int lvl)
    {
        if (lvl == 1)
            return 0;
        else
            return 1;
    }

    public static int EvaluateTime(float time)
    {
        if (0 <= time && time <= 15)
            return 0;
        else if (15 < time && time <= 30)
            return 1;
        else if (30 < time && time <= 45)
            return 2;
        else if (45 < time && time <= 60)
            return 3;
        else if (60 < time && time <= 75)
            return 4;
        else if (75 < time && time <= 90)
            return 5;
        else if (90 < time && time <= 105)
            return 6;
        else if (105 < time && time <= 120)
            return 7;
        else if (120 < time && time <= 135)
            return 8;
        else
            return 9;
    }

    public static int EvaluateMoney(int money)
    {
        if (money == 0)
            return 0;
        else if (money == 5)
            return 1;
        else if (money == 10)
            return 2;
        else if (money == 15)
            return 3;
        else if (money == 20)
            return 4;
        else
            return 5;
    }

    public static int EvaluatePosition(string posStr)
    {
        UnityEngine.Vector3 pos = ParseVector3(posStr);
        if (pos.x < 20 && pos.y < 20)
            return 0;
        else if (20 <= pos.x && pos.x < 40 && pos.y < 20)
            return 1;
        else if (40 <= pos.x && pos.x < 60 && pos.y < 20)
            return 2;
        else if (60 <= pos.x && pos.x < 80 && pos.y < 20)
            return 3;
        else if (80 <= pos.x && pos.x <= 100 && pos.y < 20)
            return 4;

        else if (pos.x < 20 && 20 <= pos.y && pos.y < 40)
            return 5;
        else if (20 <= pos.x && pos.x < 40 && 20 <= pos.y && pos.y < 40)
            return 6;
        else if (40 <= pos.x && pos.x < 60 && 20 <= pos.y && pos.y < 40)
            return 7;
        else if (60 <= pos.x && pos.x < 80 && 20 <= pos.y && pos.y < 40)
            return 8;
        else if (80 <= pos.x && pos.x <= 100 && 20 <= pos.y && pos.y < 40)
            return 9;

        else if (pos.x < 20 && 40 <= pos.y && pos.y < 60)
            return 10;
        else if (20 <= pos.x && pos.x < 40 && 40 <= pos.y && pos.y < 60)
            return 11;
        else if (40 <= pos.x && pos.x < 60 && 40 <= pos.y && pos.y < 60)
            return 12;
        else if (60 <= pos.x && pos.x < 80 && 40 <= pos.y && pos.y < 60)
            return 13;
        else if (80 <= pos.x && pos.x <= 100 && 40 <= pos.y && pos.y < 60)
            return 14;

        else if (pos.x < 20 && 60 <= pos.y && pos.y < 80)
            return 15;
        else if (20 <= pos.x && pos.x < 40 && 60 <= pos.y && pos.y < 80)
            return 16;
        else if (40 <= pos.x && pos.x < 60 && 60 <= pos.y && pos.y < 80)
            return 17;
        else if (60 <= pos.x && pos.x < 80 && 60 <= pos.y && pos.y < 80)
            return 18;
        else if (80 <= pos.x && pos.x <= 100 && 60 <= pos.y && pos.y < 80)
            return 19;

        else if (pos.x < 20 && 80 <= pos.y && pos.y <= 100)
            return 20;
        else if (20 <= pos.x && pos.x < 40 && 80 <= pos.y && pos.y <= 100)
            return 21;
        else if (40 <= pos.x && pos.x < 60 && 80 <= pos.y && pos.y <= 100)
            return 22;
        else if (60 <= pos.x && pos.x < 80 && 80 <= pos.y && pos.y <= 100)
            return 23;
        else if (80 <= pos.x && pos.x <= 100 && 80 <= pos.y && pos.y <= 100)
            return 24;
        else
            return 25; // If the position is outside the 5x5 grid.

    }
    */

    public static UnityEngine.Vector3 ParseVector3(string vectorString)
    {
        string[] components = vectorString.Replace("(", "").Replace(")", "").Split(',');

        if (components.Length == 3)
        {
            float x, y, z;

            if (float.TryParse(components[0], out x) &&
                float.TryParse(components[1], out y) &&
                float.TryParse(components[2], out z))
            {
                return new UnityEngine.Vector3(x, y, z);
            }
        }

        return UnityEngine.Vector3.zero;
    }


}


public class Q_Dictionary_Equals_Actions : IEqualityComparer<Action>
{
    public bool Equals(Action x, Action y)
    {
        return x.Name.Equals(y.Name);
    }

    public int GetHashCode(Action obj)
    {
        return obj.Name.GetHashCode();
    }
}
