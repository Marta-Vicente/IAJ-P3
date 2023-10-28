using System;
using System.Runtime.Serialization;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel
{
    [Serializable]
    public class Goal /*: ISerializable*/
    {
        public string Name { get; private set; }
        public float InsistenceValue { get; set; }
        public float ChangeRate { get; set; }
        public float Weight { get; private set; }

        public Goal(string name, float weight)
        {
            this.Name = name;
            this.Weight = weight;
        }

        public override bool Equals(object obj)
        {
            var goal = obj as Goal;
            if (goal == null) return false;
            else return this.Name.Equals(goal.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public float GetDiscontentment()
        {
            var insistence = this.InsistenceValue;
            if (insistence <= 0) return 0.0f;
            return this.Weight * insistence * insistence;
        }

        public float GetDiscontentment(float goalValue)
        {
            if (goalValue <= 0) return 0.0f;
            return this.Weight*goalValue*goalValue;
        }

        /*
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("InsistenceValue", InsistenceValue, typeof(float));
            info.AddValue("ChangeRate", ChangeRate, typeof(float));
            info.AddValue("Weight", Weight, typeof(float));
        }

        protected Goal(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            InsistenceValue = (float)info.GetValue("InsistenceValue", typeof(float));
            ChangeRate = (float)info.GetValue("ChangeRate", typeof(float));
            Weight = (float)info.GetValue("Weight", typeof(float));
        }
        */

    }
}
