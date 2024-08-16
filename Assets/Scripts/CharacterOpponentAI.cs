using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

class CharacterOpponentAI : MonoBehaviour
{
    public event EventHandler onAlmostDead;
    public event EventHandler onDoneHealing;
    public bool healing = false;
    Indices currentIndices = new Indices();
    Soldier attacker;
    public void Start()
    {
        
        gameObject.TryGetComponent<Soldier>(out attacker);
        onAlmostDead += BackOff_onAlmostDead;
        onDoneHealing += BackToPlan_onDoneHealing;
        InvokeRepeating("MakePlan", 2.00f, 3.00f);
    }

    private void BackToPlan_onDoneHealing(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        if(attacker.HealthPoints<=(attacker.unitSO.maxHealth)/2)
            onAlmostDead?.Invoke(this,EventArgs.Empty);
        if(attacker.HealthPoints >= (attacker.unitSO.maxHealth - (attacker.unitSO.maxHealth)/4))
            onDoneHealing?.Invoke(this,EventArgs.Empty);
        GridManager.Instance.WorldToGridPosition(transform.position, out currentIndices.I, out currentIndices.J);
    }
    private void BackOff_onAlmostDead(object sender, EventArgs e) {
    healing = true;
        //Path find your way to the main base
       /* PathFinder pathFinder = new PathFinder();
        GridManager.Instance.WorldToGridPosition(currentTarget.Key.transform.position, out target_indices.I, out target_indices.J);
        List<Vector3> path = pathFinder.FindPath(currentIndices, target_indices);*/

    }
    public void MakePlan()
    {
        //In case of healing ,Our only goal to heal not to do anything else unless
        if (healing)
        {
            Debug.Log("incapable of making a plan right now !");
            return;
        }
  
        GameObject[] currentPossibleTargets = GameObject.FindGameObjectsWithTag("Building or Human character");
        Debug.Log("Numbers of Entities " + currentPossibleTargets.Length);

        List<KeyValuePair<GameObject, double>> destination = new List<KeyValuePair<GameObject, double>>();
        foreach (GameObject kol in currentPossibleTargets)
        {
            Indices target_indicess;
            GridManager.Instance.WorldToGridPosition(kol.transform.position, out target_indicess.I, out target_indicess.J);

            double soldier_target_Distance = Math.Abs
                (//r for this Goblin
                (Math.Abs(kol.transform.position.x) + Math.Abs(kol.transform.position.y)) - 
                //r for this target
                (Math.Abs(transform.position.x) + Math.Abs(transform.position.y))
                 );

            destination.Add(new KeyValuePair<GameObject, double>(kol,soldier_target_Distance ));

        }
        destination.Sort((s1, s2) => s1.Value.CompareTo(s2.Value));

        int i = 0;
       
       /* while (attacker.HasTarget())
        {*/
            // Debug.Log("While loop" + ++i);
            Indices target_indices;
            KeyValuePair<GameObject, double> currentTarget = destination[0];
            Debug.Log("Current destaniation[0] is at position " + destination[0].Key.transform.position  );
            Soldier target;
            currentTarget.Key.TryGetComponent<Soldier>(out target);
            Debug.Log("Current Target Soldier is " + target.gameObject.transform.position);

            
            Debug.Log("Current Goblin Soldier is " + attacker.transform.position);

            PathFinder pathFinder = new PathFinder();
            GridManager.Instance.WorldToGridPosition(currentTarget.Key.transform.position, out target_indices.I, out target_indices.J);
            List<Vector3> path = pathFinder.FindPath(currentIndices, target_indices);

            Debug.Log("Now the path from Goblin " + attacker + " to target " + target + " is : ");
            foreach (Vector3 vector3 in path) { 
            Debug.Log(vector3);

            }

            attacker.SetPath(path);
            attacker.SetTarget(target);

            //destination.RemoveAt(0);
       // }

    }

   
     
}




