using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For sorting goals
using System.Linq;

//Deviding the system into many goals, and each goal has subgoals
public class SubGoal 
{ 
    public Dictionary<string,int> sgoals;
    //This bool is only used when THIS goal is required to happen once "SPAWNING"
    //So after this goal is satisfied we don't actually need to keep it in the list of
    //the actions for this agent
    
    public bool remove;

    public SubGoal(string s, int i, bool r) {
    sgoals = new Dictionary<string,int>();
    //An i integer is for the IMPORTANCE of the goal
    sgoals.Add(s, i);
    remove = r;
    }
}
public class GAgent : MonoBehaviour
{
    public List <GAction>  actions = new List<GAction>();
    public Dictionary<SubGoal,int> goals = new Dictionary<SubGoal,int>();

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;
    
    // Start is called before the first frame update
    void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            actions.Add(a);
        }   
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (currentAction != null && currentAction.running)
        //{
           //if(currentAction.agent.hasPath && currentAction.agent.remainingDistance)
        
       // }
        if (planner == null || actionQueue == null)
        //  Agent has no plan
        {
            planner = new GPlanner();
            //Sort through the agents till the most important goal is found
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.Plan(actions , sg.Key.sgoals,/*belifs of the agent*/null);
                if (actionQueue != null) 
                {
                currentGoal = sg.Key;
                break;
                }
            }
        }

    }
}
