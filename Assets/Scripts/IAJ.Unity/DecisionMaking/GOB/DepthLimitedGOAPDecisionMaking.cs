using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using Assets.Scripts.Game;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.GOB
{
    public class DepthLimitedGOAPDecisionMaking
    {
        public const int MAX_DEPTH = 3;
        public int ActionCombinationsProcessedPerFrame { get; set; }
        public float TotalProcessingTime { get; set; }
        public int TotalActionCombinationsProcessed { get; set; }
        public bool InProgress { get; set; }

        public CurrentStateWorldModel InitialWorldModel { get; set; }
        private List<Goal> Goals { get; set; }
        private WorldModel[] Models { get; set; }
        private Action[] LevelAction { get; set; }
        public Action[] BestActionSequence { get; private set; }
        public Action BestAction { get; private set; }
        public float BestDiscontentmentValue { get; private set; }
        private int CurrentDepth {  get; set; }

        public DepthLimitedGOAPDecisionMaking(CurrentStateWorldModel currentStateWorldModel, List<Action> actions, List<Goal> goals)
        {
            this.ActionCombinationsProcessedPerFrame = 200;
            this.Goals = goals;
            this.InitialWorldModel = currentStateWorldModel;
        }

        public void InitializeDecisionMakingProcess()
        {
            this.InProgress = true;
            this.TotalProcessingTime = 0.0f;
            this.TotalActionCombinationsProcessed = 0;
            this.CurrentDepth = 0;
            this.Models = new WorldModel[MAX_DEPTH + 1];
            this.Models[0] = this.InitialWorldModel;
            this.LevelAction = new Action[MAX_DEPTH];
            this.BestActionSequence = new Action[MAX_DEPTH];
            this.BestAction = null;
            this.BestDiscontentmentValue = float.MaxValue;
            this.InitialWorldModel.Initialize();
        }

        public Action ChooseAction()
        {
            var processedActions = 0;

            var startTime = Time.realtimeSinceStartup;

            var currentValue = 0.0f;
            var bestValue = float.MaxValue;

            Action[] listOfBestAction = null;

            while (this.CurrentDepth >= 0)
            {
                if (this.CurrentDepth >= MAX_DEPTH)
                {
                    currentValue = this.Models[this.CurrentDepth].CalculateDiscontentment(this.Goals);

                    if (currentValue < bestValue)
                    {
                        bestValue = currentValue;
                        this.BestAction = this.BestActionSequence[0];
                        listOfBestAction = (Action[]) this.BestActionSequence.Clone();
                    }
                    CurrentDepth -= 1;
                    continue;
                }

                var nextAction = this.Models[this.CurrentDepth].GetNextAction();

                if (nextAction != null && nextAction.CanExecute(this.Models[this.CurrentDepth]))
                {
                    this.Models[CurrentDepth + 1] = this.Models[CurrentDepth].GenerateChildWorldModel();
                    nextAction.ApplyActionEffects(this.Models[CurrentDepth + 1]);
                    this.BestActionSequence[CurrentDepth] = nextAction;
                    this.CurrentDepth++;
                    processedActions++;
                }
                else
                    this.CurrentDepth--;
            }

            if(listOfBestAction == null)
            {
                listOfBestAction = this.BestActionSequence;
                this.BestAction = this.BestActionSequence[0];
            }

            this.TotalProcessingTime += Time.realtimeSinceStartup - startTime;
            this.TotalActionCombinationsProcessed = processedActions;
            this.InProgress = false;
            this.BestDiscontentmentValue = bestValue;

            this.BestActionSequence = listOfBestAction; 
            /*
            Debug.Log("+++++++++++++++++++++++");
            foreach (var action in this.BestActionSequence)
            {
                Debug.Log("\n" + action.Name);
            }
            Debug.Log("+++++++++++++++++++++++");
            */

            return this.BestAction;
        }
    }
}
