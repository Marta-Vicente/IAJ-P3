using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using UnityEngine;
using System.Text;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.GOB
{
    public class GOBDecisionMaking
    {
        public bool InProgress { get; set; }
        private List<Goal> goals { get; set; }
        private List<Action> actions { get; set; }

        public Dictionary<Action,float> ActionDiscontentment { get; set; }

        public Action secondBestAction;
        public Action thirdBestAction;

        public float TotalProcessingTime = 0f;
        public static float BestDiscontentmentValue = float.MaxValue;

        // Utility based GOB
        public GOBDecisionMaking(List<Action> _actions, List<Goal> goals)
        {
            this.actions = _actions;
            this.goals = goals;
            secondBestAction = new Action("yo");
            thirdBestAction = new Action("yo too");
           
        }


        public static float CalculateDiscontentment(Action action, List<Goal> goals)
        {
            // Keep a running total
            var discontentment = 0.0f;
            var duration = action.GetDuration();

            /*
            var bigger = 0f;
            foreach (var goal in goals)
            {
                // Calculate the new value after the action
                var newValue = goal.InsistenceValue + action.GetGoalChange(goal);

                // The change rate is how much the goals changes per time
                newValue += duration * goal.ChangeRate;

                if (bigger < newValue) 
                { 
                    bigger = newValue;
                }
            }
            */


            foreach (var goal in goals)
            {
               // Calculate the new value after the action
                var newValue = goal.InsistenceValue + action.GetGoalChange(goal);

                // The change rate is how much the goals changes per time
                newValue += duration * goal.ChangeRate;

                //Here is a bug: Insistence varies between 0-10, it should be normalized
                //discontentment += goal.GetDiscontentment(newValue)/float.MaxValue * 10;
                discontentment += goal.GetDiscontentment(/*NormalizeGoalValues(newValue, 0, bigger)*/ newValue);
            }

            if (discontentment < BestDiscontentmentValue)
            {
                BestDiscontentmentValue = discontentment;
                //Debug.Log("MY DISCONTEMENT IS BIG " + discontentment);
            }
            
            return discontentment;
        }

        public Action ChooseAction()
        {
            // Find the action leading to the lowest discontentment
            this.ActionDiscontentment = new Dictionary<Action, float>();

            InProgress = true;
            Action bestAction = null;

            //bestAction = actions[0];
            float bestValue = float.MaxValue;
            /*
            if (bestAction.CanExecute())
            {
                 bestValue = CalculateDiscontentment(bestAction, goals);
            }
            */
            var secondBestValue = float.PositiveInfinity;
            var thirdBestValue = float.PositiveInfinity;
            secondBestAction = null;
            thirdBestAction = null;

            foreach(var action in actions)
            {
                if (action.CanExecute())
                {
                    var value = CalculateDiscontentment(action, goals);
                    this.ActionDiscontentment.Add(action, value);

                    if (value < bestValue)
                    {
                        thirdBestValue = secondBestValue;
                        thirdBestAction = secondBestAction;
                        secondBestValue = bestValue;
                        secondBestAction = bestAction;
                        bestValue = value;
                        bestAction = action;
                    }
                    if (value < secondBestValue && value > bestValue)
                    {
                        secondBestAction = action;
                        secondBestValue = value;
                    }
                    if (value < thirdBestValue && value > secondBestValue && value > bestValue)
                    {
                        thirdBestAction = action;
                        thirdBestValue = value;
                    }

                }

            }

            InProgress = false;

            TotalProcessingTime += Time.deltaTime;

            return bestAction;
        }

        public static float NormalizeGoalValues(float value, float min, float max)
        {
            if (value < 0) value = 0.0f;
            // Normalizing to 0-1
            var x = (value - min) / (max - min);

            // Multiplying it by 10
            x *= 10;

            Debug.Log(x);

            return x;
        }


    }
}
