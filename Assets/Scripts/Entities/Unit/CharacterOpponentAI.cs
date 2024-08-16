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
    //public event EventHandler onDoneHealing;

    private bool healing = false;
   

    private float maxHealth = 0.0f;

    Indices currentGoblinIndices = new Indices();
    Indices target_indices;
    Indices goblinHomeIndices;

    Soldier thisGoblin;

    GameObject goblinHome;
    GameObject[] currentPossibleTargets;

    PathFinder pathFinder = new PathFinder();
    List<KeyValuePair<GameObject, double>> destinations = new List<KeyValuePair<GameObject, double>>();

    Vector3 tempo = Vector3.zero;

    public void Start()
    {
        gameObject.TryGetComponent<Soldier>(out thisGoblin);
        maxHealth = thisGoblin.unitSO.maxHealth;

        InvokeRepeating("MakePlan", 2, 3);
        InvokeRepeating("SetGoblinHome", 5, 120);
    }

    public void Update()
    {
        //Debug.Log(thisGoblin.HealthPoints);
        if (thisGoblin.HealthPoints <= (maxHealth) / 4 &&!healing)
        {
            Debug.Log("Condition is satisfied and current health is : " + thisGoblin.HealthPoints);
            onAlmostDead += BackOff_onAlmostDead;
            onAlmostDead?.Invoke(this, EventArgs.Empty);
        }
        /*if (thisGoblin.HealthPoints >= (maxHealth - (maxHealth) / 4))
        {
            onDoneHealing?.Invoke(this, EventArgs.Empty);
            onDoneHealing += BackToPlan_onDoneHealing;
        }*/
        GridManager.Instance.WorldToGridPosition(transform.position, out currentGoblinIndices.I, out currentGoblinIndices.J);
        //Debug.Log("Gobling Indeices : "+currentGoblinIndices.I + " " +currentGoblinIndices.J);
    }

    private void BackOff_onAlmostDead(object sender, EventArgs e)
    {
        healing = true;
        Debug.Log("Healing is activated");
        //Path-find your way to the main base
        GridManager.Instance.WorldToGridPosition(goblinHome.transform.position, out goblinHomeIndices.I, out goblinHomeIndices.J);
        List<Vector3> path = pathFinder.FindPath(currentGoblinIndices, goblinHomeIndices);
        thisGoblin.SetPath(path);
    }
    private void BackToPlan_onDoneHealing(object sender, EventArgs e)
    {
        healing = false;
        onAlmostDead -= BackOff_onAlmostDead;
    }
    private void SetGoblinHome() 
    {
        goblinHome = GameObject.FindGameObjectWithTag("Goblin Base");
        goblinHome.TryGetComponent<Building>(out Building  building);
        Debug.Log("Goblin home is :" + goblinHome + " with building Sctipt attached " +building +"BUILDINGSO = " + building.buildingSO);
    }

    public void MakePlan()
    {
        //In case of healing ,Our only goal to heal not to do anything else unless
        if (healing)
        {
            Debug.Log("incapable of making a plan right now !");
            return;
        }

        currentPossibleTargets = GameObject.FindGameObjectsWithTag("Building or Human character");

        if (currentPossibleTargets.Length == 0)
        {
           // Debug.Log("No targets to make a plan on");
            return;
        }
        //Debug.Log("Numbers of Entities " + currentPossibleTargets.Length);

        double soldier_target_Distance;
        foreach (GameObject kol in currentPossibleTargets)
        {
            tempo = kol.transform.position;
            GridManager.Instance.WorldToGridPosition(tempo, out target_indices.I, out target_indices.J);

            soldier_target_Distance = Vector3.Distance(tempo, transform.position);

            destinations.Add(new KeyValuePair<GameObject, double>(kol, soldier_target_Distance));
        }

        destinations.Sort((s1, s2) => s1.Value.CompareTo(s2.Value));

       
        KeyValuePair<GameObject, double> currentTarget = destinations[0];
        tempo = destinations[0].Key.transform.position;
        /*Debug.Log("Current destaniation[0] is at position " + destinations[0].Key.transform.position);
        Debug.Log("Current Target is a building " + currentTarget.Key.transform.position);
        Debug.Log("Current Goblin Soldier is " + goblinAttacker.transform.position);*/

       
        GridManager.Instance.WorldToGridPosition(tempo, out target_indices.I, out target_indices.J);
        List<Vector3> path = pathFinder.FindPath(currentGoblinIndices, target_indices);
        thisGoblin.SetPath(path);

        if (currentTarget.Key.TryGetComponent<Soldier>(out Soldier soldier))
            thisGoblin.SetTarget(soldier);
        if (currentTarget.Key.TryGetComponent<Building>(out Building building))
            thisGoblin.SetTarget(building);

        currentPossibleTargets = null;
        destinations.Clear();
        return;
    }
}




