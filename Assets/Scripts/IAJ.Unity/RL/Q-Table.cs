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

        public static Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, float>>> Table
            = new Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, float>>>(new Q_Dictionary_Equals());


        public Q_Table() { }

        public Tuple<float, float> FindOrAdd(WorldModel State, Action Action)
        {
            var copy = GetQTablePropertiesCopy(State.Properties);
            if (State == null || Action == null) return null;
            if (Table.ContainsKey(copy))
            {
                if (Table[copy].ContainsKey(Action.Name)) return Table[copy][Action.Name];
                else
                {
                    Table[copy].Add(Action.Name, new Tuple<float, float>(0f, 0f));
                    return Table[copy][Action.Name];
                }
            }
            else
            {
                Table.Add(copy, new Dictionary<string, Tuple<float, float>>());
                if (Table[copy].ContainsKey(Action.Name)) return Table[copy][Action.Name];
                else
                {
                    Table[copy].Add(Action.Name, new Tuple<float, float>(0f, 0f));
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
                    Table[copy][Action.Name] = new Tuple<float, float>(Q, 0f);
                else
                {
                    Table[copy].Add(Action.Name, new Tuple<float, float>(Q, 0f));
                }
            }
            else
            {
                Table.Add(copy, new Dictionary<string, Tuple<float, float>>());
                Table[copy].Add(Action.Name, new Tuple<float, float>(Q, 0f));
            }
        }


        public Action GetBest(WorldModel State, Action[] actions)
        {
            if (actions == null) return null;
            Action best = null;
            float bestQ = float.MinValue;

            foreach (var action in actions)
            {
                Tuple<float, float> t = FindOrAdd(State, action);
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
                    QTableDTO newTable = JsonConvert.DeserializeObject<QTableDTO>(json);
                    Table = FromDTO(newTable);

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
        public static Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, float>>> FromDTO(QTableDTO qTableDTO)
        {
            Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, float>>> NewTable
                = new Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<float, float>>>(new Q_Dictionary_Equals());

            
            foreach (var entry in qTableDTO.Entries)
            {
                int mana = Convert.ToInt32((object)entry.Key[Properties.MANA]);
                int maxHP = Convert.ToInt32((object)entry.Key[Properties.MAXHP]);
                int hp = Convert.ToInt32((object)entry.Key[Properties.HP]);
                int shieldHP = Convert.ToInt32((object)entry.Key[Properties.ShieldHP]);
                int money = Convert.ToInt32((object)entry.Key[Properties.MONEY]);
                float time = Convert.ToSingle((object)entry.Key[Properties.TIME]);
                int level = Convert.ToInt32((object)entry.Key[Properties.LEVEL]);
                int maxmana = Convert.ToInt32((object)entry.Key[Properties.MAXMANA]);
                string postition = Convert.ToString((object)entry.Key[Properties.POSITION]);
                int xp = Convert.ToInt32((object)entry.Key[Properties.XP]);

                Dictionary<string, object> subDictionary = new Dictionary<string, object>();
                subDictionary.Add(Properties.MANA, mana);
                subDictionary.Add(Properties.MAXHP, maxHP);
                subDictionary.Add(Properties.HP, hp);
                subDictionary.Add(Properties.ShieldHP, shieldHP);
                subDictionary.Add(Properties.MONEY, money);
                subDictionary.Add(Properties.TIME, time);
                subDictionary.Add(Properties.LEVEL, level);
                subDictionary.Add(Properties.MAXMANA, maxmana);
                subDictionary.Add(Properties.POSITION, postition);
                subDictionary.Add(Properties.XP, xp);


                Dictionary<string, Tuple<float, float>> value = new Dictionary<string, Tuple<float, float>>(entry.Value);
                NewTable.Add(subDictionary, value);
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