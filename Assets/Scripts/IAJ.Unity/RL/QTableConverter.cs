using System.Collections.Generic;
using System;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;

[Serializable]
public class QTableDTO
{
    public List<QTableEntry> Entries { get; set; }
}

[Serializable]
public class QTableEntry
{
    public Dictionary<string, object> Key { get; set; }
    public Dictionary<string, Tuple<float, float>> Value { get; set; }
}
