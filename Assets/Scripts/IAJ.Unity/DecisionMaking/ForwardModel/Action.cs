using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel
{
    [Serializable]
    public class Action /*: ISerializable*/
    {
        private static int ActionID = 0;
        public string Name { get; set; }
        public int ID { get; set; }
        private Dictionary<Goal, float> GoalEffects { get; set; }
        public float Duration { get; set; }

        public Action(string name)
        {
            this.ID = Action.ActionID++;
            this.Name = name;
            this.GoalEffects = new Dictionary<Goal, float>();
            this.Duration = 0.0f;

        }

        public void AddEffect(Goal goal, float goalChange)
        {
            this.GoalEffects[goal] = goalChange;
        }

        // Used for GOB Decison Making
        public virtual float GetGoalChange(Goal goal)
        {
            if (this.GoalEffects.ContainsKey(goal))
            {
                return this.GoalEffects[goal];
            }
            else return 0.0f;
        }

        public virtual float GetDuration()
        {
            return this.Duration;
        }

        public virtual float GetDuration(WorldModel worldModel)
        {
            return this.Duration;
        }

        public virtual float GetDuration(WorldModelFEAR worldModel)
        {
            return this.Duration;
        }

        public virtual bool CanExecute(WorldModel woldModel)
        {
            return true;
        }

        public virtual bool CanExecute(WorldModelFEAR woldModel)
        {
            return true;
        }


        public virtual bool CanExecute()
        {
            return true;
        }

        public virtual void Execute()
        {
        }

        // Used for GOAP Decison Making
        public virtual void ApplyActionEffects(WorldModel worldModel)
        {
        }

        public virtual void ApplyActionEffects(WorldModelFEAR worldModel)
        {
        }

        // Used for MCTS Biased
        public virtual float GetHValue(WorldModel worldModel)
        {
            return 0.0f;
        }

        public override bool Equals(object obj)
        {
            try
            {
                Action A = obj as Action;
                if(A.Name.Equals(this.Name)) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        /*
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("ID", ID, typeof(int));
            info.AddValue("GoalEffects", GoalEffects, typeof(Dictionary<Goal, float>));
            info.AddValue("Duration", Duration, typeof(float));
        }

        protected Action(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            ID = (int)info.GetValue("ID", typeof(int));
            GoalEffects = (Dictionary<Goal, float>)info.GetValue("GoalEffects", typeof(Dictionary<Goal, float>));
            Duration = (float)info.GetValue("Duration", typeof(float));
        }
        */

    }
}
