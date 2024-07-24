using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;

    //The location of where the action is gonna take place,we get its transform compnent and then we WorldToGridPosition()
    public GameObject target;

    //In case the target was not found up here, search for it by its tag to see weather it in the heirarchy or not
    public GameObject targetTag;

    public float duration = 0;
    public WorldState[] preConditions;
    public WorldState[] afterEffect;

    //The following attribut replacable once it is known what does it represent
    public NavMeshAgent agent;
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentsBeliefs;
    public bool isBeingPerformed = false;
    // Start is called before the first frame update

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        if (preConditions != null)
        {
            foreach (WorldState w in preConditions)
                preconditions.Add(w.key, w.value);
        }

        if (afterEffect != null)
        {
            foreach (WorldState w in afterEffect)
                effects.Add(w.key, w.value);
        }

    }
    public bool IsAchievable()
    {
        //The place where Unacceptable actions should be rejectd by a certain logic
        return true;
    }

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {

        foreach (KeyValuePair<string, int> p in conditions)
        {
            if (!conditions.ContainsKey(p.Key))
            {
                return false;
            }

        }
        return true;
    }

    //Force inheriting classes to implement those two methods
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
