using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour
{
    public event EventHandler onResourceGathering;
    public event EventHandler onUnitProduction;

    STRATEGIES strategy = new STRATEGIES();

    List<KeyValuePair<GameObject, double>>prioritizedNodes = new List<KeyValuePair<GameObject, double>>();

    Indices lastPoinInMap = new Indices(80, 80);
    Indices lastSearchingPoinInMap = new Indices(0, 0);

    int mainBuildingPriority = 10;

    //BuildingSOList computerBuildings = new BuildingSOList();

    private void Start()
    {
        strategy = STRATEGIES.NONE;
        //Invoke("AssigningMainBuilding", 2);
    }

    private void Update()
    {
      
        if (strategy != STRATEGIES.RESOURCE_GATHERING && (Time.time <= 120.0f /*&&!ResourcesStausGood()*/)/*||Regather()*/)
        {
            Debug.Log("Condition is is satisfied!");
            strategy = STRATEGIES.RESOURCE_GATHERING;
            onResourceGathering += ComputerPlayer_onResourceGathering;
            onResourceGathering?.Invoke(this, EventArgs.Empty);
        }
        /*if (Time.time > 120.0f *//*&&ResourcesGood()*//*)
        {
            strategy = STRATEGIES.UNIT_PRODUCTION;
            onUnitProduction += ComputerPlayer_onUnitProduction;
            onUnitProduction?.Invoke(this, EventArgs.Empty);
        }*/
    }


    private void ComputerPlayer_onUnitProduction(object sender, EventArgs e)
    {
        if (onResourceGathering != null)
        {
            CancelInvoke("GatherResources");
            onResourceGathering -= ComputerPlayer_onUnitProduction;
        }
        InvokeRepeating("UnitProduction",5,20);
    }


    private void ComputerPlayer_onResourceGathering(object sender, EventArgs e)
    {
        InvokeRepeating("GatherResources", 1, 13);
    }


    //Called Once at the beggining of the battle
    private void FindAndPrioritzeResourcesNodes() 
    {
        if (strategy != STRATEGIES.RESOURCE_GATHERING)
        {
            
            return ;
        }
        //Points counter
        int n = 0;

        //Distance to that Resource node
        float distance=0.0f;

        Vector3 goblinMainBuilding = GameManager.Instance.GetGoblinsMainBuildingAsGameObject().transform.position;
        Collider2D[] resourcesNodes =Physics2D.OverlapAreaAll(GridManager.Instance.GridToWorldPosition(lastPoinInMap), GridManager.Instance.GridToWorldPosition(lastSearchingPoinInMap));

        if (resourcesNodes == null)
        {
            Debug.Log("No resources Nodes Available !");
            return;
        }
        Vector3 colPosition;
        foreach (Collider2D col in resourcesNodes)
        {
            colPosition = col.gameObject.transform.position;
            if (col.gameObject.tag.Equals("Resource Node"))
            {
                distance = Vector3.Distance(goblinMainBuilding, colPosition);
                //Prioritizing resources nodes types
                if (col.gameObject.TryGetComponent<Wood>(out Wood wood))
                { n = 1; }  

                else if (col.gameObject.TryGetComponent<Stone>(out Stone stone))
                { n = 4; } 

                else if (col.gameObject.TryGetComponent<Gold>(out Gold gold))
                { n = 6; }  

                    prioritizedNodes.Add(new KeyValuePair<GameObject, double>(col.gameObject, distance + n));
            }
        }
        prioritizedNodes.Sort((s1, s2) => s1.Value.CompareTo(s2.Value));
        Debug.Log("Prioritized Nodes are : ");
        foreach (KeyValuePair<GameObject, double> kvp in prioritizedNodes) 
        {
            Debug.Log(kvp);
        
        }
    }
    private bool ResourcesStausGood() { return true; }

    public void GatherResources() 
    {
        Debug.Log("Now i am at GatherResources");
        if (prioritizedNodes.Count == 0)
        {
            Debug.Log("Now prioritizedNodes count = 0");
            FindAndPrioritzeResourcesNodes();
            Debug.Log("Found and Prioitzed node successfully !");
        }

        KeyValuePair<GameObject, double> currentNode;
        KeyValuePair<GameObject, double> nextNode;

        BuildingSO resourceHarvester;

        Vector3 harvesterPosition;
        Indices harvesterIndices;
        

        if (prioritizedNodes.Count != 0)
        {
            currentNode = prioritizedNodes.ElementAt(0);
                
                //Is it wood?
                if (currentNode.Key.TryGetComponent<Wood>(out Wood wood))
                {
                    //The needed harvester is Wood Miner                
                     resourceHarvester = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/WoodMiner");
                    
                //Check if there is another node in the list
                if (prioritizedNodes.Count >= 2)
                {
                    nextNode=prioritizedNodes.ElementAt(1);

                    //Check if it is the same resource type
                    if (nextNode.Key.TryGetComponent<Wood>(out Wood wood1)/*&& Distance between two nodes<=Wood Harvester radius*/)
                    {   
                        //Harevester position in between those nodes
                        harvesterPosition = Vector3.Lerp(currentNode.Key.transform.position, nextNode.Key.transform.position, 0.5f);

                        GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                        BuildingManager.Instance.placeBuilding
                                            (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                        prioritizedNodes.RemoveAt(0);
                        prioritizedNodes.RemoveAt(1);
                        return;
                    }
                   
                }
                    harvesterPosition = currentNode.Key.transform.position;
                    GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                    BuildingManager.Instance.placeBuilding
                                            (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                    prioritizedNodes.RemoveAt(0);
                    return;
                }
                //Or is it stone?
                else if (currentNode.Key.TryGetComponent<Stone>(out Stone stone))
                {
                    //The needed harvester is Stone Miner
                    resourceHarvester = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/StoneMiner");

                //Check if there is another node in the list
                if (prioritizedNodes.Count >= 2)
                {
                    nextNode = prioritizedNodes.ElementAt(1);

                    //Check if it is the same resource type
                    if (nextNode.Key.TryGetComponent<Stone>(out Stone stone1)/*&& Distance between two nodes<=Stone Harvester radius*/)
                    {
                        //Harevester position in between those nodes
                        harvesterPosition = Vector3.Lerp(currentNode.Key.transform.position, nextNode.Key.transform.position, 0.5f);

                        GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                        BuildingManager.Instance.placeBuilding
                                                (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                        prioritizedNodes.RemoveAt(0);
                        prioritizedNodes.RemoveAt(1);
                        return;
                    }
                    
                }
                    harvesterPosition = currentNode.Key.transform.position;
                    GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                    BuildingManager.Instance.placeBuilding
                                            (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                    prioritizedNodes.RemoveAt(0);
                    return;

                }

                //Or is It Gold?
                else if (currentNode.Key.TryGetComponent<Gold>(out Gold gold))
                {
                    //The needed harvester is Stone Miner
                    resourceHarvester = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/GoldMiner");

                //Check if there is another node in the list
                if (prioritizedNodes.Count >= 2)
                {
                    nextNode = prioritizedNodes.ElementAt(1);

                    //Check if it is the same resource type
                    if (nextNode.Key.TryGetComponent<Gold>(out Gold gold1)/*&& Distance between two nodes<=Gold Harvester radius*/)
                    {
                        //Harevester position in between those nodes
                        harvesterPosition = Vector3.Lerp(currentNode.Key.transform.position, nextNode.Key.transform.position, 0.5f);

                        GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                        BuildingManager.Instance.placeBuilding
                                                (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                        prioritizedNodes.RemoveAt(0);
                        prioritizedNodes.RemoveAt(1);
                        return;
                    }
                   
                }
                    harvesterPosition = currentNode.Key.transform.position;
                    GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                    BuildingManager.Instance.placeBuilding
                                            (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                    prioritizedNodes.RemoveAt(0);

                    return;

                }
            return;
        }
        Debug.Log("No resources nodes are available!");
        return;
    }

    /* public void AssigningMainBuilding() 
     {
         Debug.Log("From computerPlayer : " + GameManager.Instance.GetGoblinsMainBuilding());
         Debug.Log(computerBuildings.buildingSOList.Count);
         computerBuildings.buildingSOList.Add(GameManager.Instance.GetGoblinsMainBuilding());
     }*/
    public void UnitProduction() 
    {
        BuildingSO buildingSO;
        int luckyNumber = UnityEngine.Random.Range(0, 1);

        //CheckHighestBuildingPrioity();
        
        if (luckyNumber == 0)
        {
            buildingSO = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/DefenseBuilding");

            //It has to be somewhere next to the main building or harvesters
            
        }
        else buildingSO = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/GoblinCamp");

        //BuildingManager.Instance.placeBuilding(buildingSO,
    }
    public int CheckHighestMainBuildingPrioity()
    {
        List<KeyValuePair<GameObject,int>>buildingsPriority = new List<KeyValuePair<GameObject,int>>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Resource Harvester");
        GameObject[] goblinsHarvesters;
        foreach (GameObject go in temp)
        {
            if (go.TryGetComponent<Building>(out Building building))
            {
               // if(building.buildingSO.team == Team.GOBLINS)

            }
        
        }
        return mainBuildingPriority; 
    }
    public void Attack() { }
    public void Defense() { }
}
