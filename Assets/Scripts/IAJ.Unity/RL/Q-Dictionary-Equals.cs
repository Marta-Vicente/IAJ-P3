using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System.Collections.Generic;

public class Q_Dictionary_Equals : IEqualityComparer<WorldModel>
{
    public bool Equals(WorldModel x, WorldModel y)
    {
        if (x.EvaluateHP() != y.EvaluateHP()) return false;
        if (x.EvaluateMana() != y.EvaluateMana()) return false;
        if (x.EvaluateShieldHP() != y.EvaluateShieldHP()) return false;
        if (x.EvaluateXP() != y.EvaluateXP()) return false;
        if (x.EvaluateLevel() != y.EvaluateLevel()) return false;
        if (x.EvaluateTime() != y.EvaluateTime()) return false;
        if (x.EvaluateMoney() != y.EvaluateMoney()) return false;
        if (x.EvaluatePosition() != y.EvaluatePosition()) return false;

        return true;
    }

    public int GetHashCode(WorldModel obj)
    {
        return obj.EvaluateHP().GetHashCode() + obj.EvaluateMana().GetHashCode() + obj.EvaluateShieldHP().GetHashCode() +
            obj.EvaluateXP().GetHashCode() + obj.EvaluateLevel().GetHashCode() + obj.EvaluateTime().GetHashCode() +
            obj.EvaluateMana().GetHashCode() + obj.EvaluatePosition().GetHashCode();
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
