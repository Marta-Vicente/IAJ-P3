using Assets.Scripts.Game;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.Action;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.Runtime.Serialization;
using Assets.Scripts.IAJ.Unity.DecisionMaking.RL;
using Newtonsoft.Json;


namespace Assets.Scripts.IAJ.Unity.DecisionMaking.RL
{
    [Serializable]
    public class Q_Table
    {

        public static Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, WorldModel>>> Table
            = new Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, WorldModel>>>(new Q_Dictionary_Equals());


        public Q_Table() { }

        public Tuple<float, WorldModel> FindOrAdd(WorldModel State, Action Action)
        {
            var copy = GetQTablePropertiesCopy(State.Properties);
            if (State == null || Action == null) return null;
            if (Table.ContainsKey(copy))
            {
                if (Table[copy].ContainsKey(Action.Name)) return Table[copy][Action.Name];
                else
                {
                    Table[copy].Add(Action.Name, new Tuple<float, WorldModel>(0f, null));
                    return Table[copy][Action.Name];
                }
            }
            else
            {
                Table.Add(copy, new Dictionary<string, Tuple<float, WorldModel>>());
                if (Table[copy].ContainsKey(Action.Name)) return Table[copy][Action.Name];
                else
                {
                    Table[copy].Add(Action.Name, new Tuple<float, WorldModel>(0f, null));
                    return Table[copy][Action.Name];
                }
            }
        }

        public void UpdateOrAdd(WorldModel State, Action Action, float Q, WorldModel newState)
        {
            var copy = GetQTablePropertiesCopy(State.Properties);
            if (State == null || Action == null) return;
            if (Table.ContainsKey(copy))
            {
                if (Table[copy].ContainsKey(Action.Name))
                    Table[copy][Action.Name] = new Tuple<float, WorldModel>(Q, newState);
                else
                {
                    Table[copy].Add(Action.Name, new Tuple<float, WorldModel>(Q, newState));
                }
            }
            else
            {
                Table.Add(copy, new Dictionary<string, Tuple<float, WorldModel>>());
                Table[copy].Add(Action.Name, new Tuple<float, WorldModel>(Q, newState));
            }
        }


        public Action GetBest(WorldModel State, Action[] actions)
        {
            if (actions == null) return null;
            Action best = null;
            float bestQ = float.MinValue;

            foreach (var action in actions)
            {
                Tuple<float, WorldModel> t = FindOrAdd(State, action);
                if (t.Item1 >= bestQ)
                {
                    bestQ = t.Item1;
                    best = action;
                }
            }

            return best;
        }


        public Dictionary<string, object> GetQTablePropertiesCopy(Dictionary<string, object> initial)
        {
            Dictionary<string, object> copy = new Dictionary<string, object>(initial);
            copy[Properties.POSITION] = copy[Properties.POSITION].ToString();
            return copy;
        }


        /*
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Serialize the Table dictionary and its sub-dictionaries
            var serializedTable = new Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>>();
            foreach (var entry in Table)
            {
                var subDictionary = new Dictionary<Action, Tuple<float, WorldModel>>(new Q_Dictionary_Equals_Actions());
                foreach (var subEntry in entry.Value)
                {
                    subDictionary[subEntry.Key] = subEntry.Value;
                }
                serializedTable[entry.Key] = subDictionary;
            }
            info.AddValue("Table", serializedTable, typeof(Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>>));
        }

        protected Q_Table(SerializationInfo info, StreamingContext context)
        {
            // Deserialize the Table dictionary and its sub-dictionaries
            var serializedTable = (Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>>)info.GetValue("Table", typeof(Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>>));
            Table = new Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>>(new Q_Dictionary_Equals());
            foreach (var entry in serializedTable)
            {
                var subDictionary = new Dictionary<Action, Tuple<float, WorldModel>>(entry.Value);
                Table[entry.Key] = subDictionary;
            }
        }
        */

        public static void SaveQTableToFile(Q_Table qTable, string filePath)
        {
            try
            {
                //string json = JsonConvert.SerializeObject(Q_Table.Table, Formatting.Indented);
                string json = JsonConvert.SerializeObject(ToDTO());
                System.IO.File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error saving Q_Table: " + ex.Message);
            }
        }

        public static void LoadQTableFromFile(string filePath)
        {

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    string json = System.IO.File.ReadAllText(filePath);
                    //qTable = new Q_Table();
                    //Q_Table.Table = JsonConvert.DeserializeObject<Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, WorldModel>>>>(json);
                    //Q_Table.Table = JsonUtility.FromJson<Dictionary<WorldModel, Dictionary<Action, Tuple<float, WorldModel>>>>(json);
                    QTableDTO qTableDTO = JsonConvert.DeserializeObject<QTableDTO>(json);
                    Table = FromDTO(qTableDTO);

                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading Q_Table: " + ex.Message);
            }
        }


        public static QTableDTO ToDTO()
        {
            var entries = new List<QTableEntry>();

            foreach (var entry in Table)
            {
                var key = entry.Key;
                var value = entry.Value;
                entries.Add(new QTableEntry
                {
                    Key = key,
                    Value = value
                });
            }

            return new QTableDTO { Entries = entries };
        }
        public static Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, WorldModel>>> FromDTO(QTableDTO qTableDTO)
        {
            Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, WorldModel>>> NewTable
                = new Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, WorldModel>>>(new Q_Dictionary_Equals());

            foreach (var entry in qTableDTO.Entries)
            {
                NewTable[entry.Key] = entry.Value;
            }

            return NewTable;
        }
        

    }
}

    public static class Q_Table_Instance
    {
        public static Q_Table Table = new Q_Table();

        public static void Q_table_Parse(Q_Table q_table)
        {
            Table = q_table;
        }
    }