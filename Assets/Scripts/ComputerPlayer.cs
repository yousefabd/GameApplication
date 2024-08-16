using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour
{
    public event EventHandler onResourceGathering;
    public event EventHandler onUnitProduction;

    STRATEGIES strategy = new STRATEGIES();

    List<KeyValuePair<GameObject, double>>prioritizedNodes = new List<KeyValuePair<GameObject, double>>();
    List<KeyValuePair<GameObject,double>> prioritzedBuildings = new List<KeyValuePair<GameObject,double>>();

    Indices lastPoinInMap = new Indices(80, 80);
    Indices lastSearchingPoinInMap = new Indices(0, 0);

    int mainBuildingPriority = 10;
    bool defendingNow;

    private void Start()
    {
        strategy = STRATEGIES.NONE;
        Invoke("AssigningMainBuilding", 2);
    }

    private void Update()
    {

        //When to gather resources 
        if (strategy != STRATEGIES.RESOURCE_GATHERING && !ResourcesStausGood())
        {
            Debug.Log("Condition is satisfied!");
            strategy = STRATEGIES.RESOURCE_GATHERING;
            onResourceGathering += ComputerPlayer_onResourceGathering;
            onResourceGathering?.Invoke(this, EventArgs.Empty);
        }

        //When to Produce unit
        if (strategy != STRATEGIES.UNIT_PRODUCTION && ResourcesStausGood())
        {
            strategy = STRATEGIES.UNIT_PRODUCTION;
            onUnitProduction += ComputerPlayer_onUnitProduction;
            onUnitProduction?.Invoke(this, EventArgs.Empty);
        }
    }


    private void ComputerPlayer_onUnitProduction(object sender, EventArgs e)
    {
        if (onResourceGathering != null)
        {
            CancelInvoke("GatherResources");
            onResourceGathering -= ComputerPlayer_onUnitProduction;
        }
        InvokeRepeating("UnitProduction",3,10);
    }


    private void ComputerPlayer_onResourceGathering(object sender, EventArgs e)
    {
        InvokeRepeating("GatherResources", 1, 13);
    }


    //Called Once at the beggining of the battle
    private void FindAndPrioritzeResourcesNodes() 
    {
        if (strategy != STRATEGIES.RESOURCE_GATHERING)
            return ;
        
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
    private bool ResourcesStausGood() 
    {
        Dictionary<ResourceType,int> tempo = ResourceManager.Instance.GetGoblinsResources();
        int woodValue = 0 ;int stoneValue = 0;int goldValue = 0;
        foreach (KeyValuePair<ResourceType, int> kvp in tempo)
        {
            if (kvp.Key == ResourceType.WOOD){woodValue = kvp.Value;}
       else if (kvp.Key == ResourceType.STONE){stoneValue = kvp.Value; }
       else if (kvp.Key == ResourceType.GOLD){goldValue = kvp.Value; }
        }
        if (woodValue >= 200 || stoneValue >= 50 || goldValue >= 10000)
        {
            return true;
        }
        return false;
    }

    public void GatherResources() 
    {
        
        if (prioritizedNodes.Count == 0)
        {
            FindAndPrioritzeResourcesNodes();
        }
        int u = 0;
        foreach (KeyValuePair<GameObject,double> resourceNode in prioritizedNodes) 
        {
            if (resourceNode.Key.IsDestroyed())
                prioritizedNodes.RemoveAt(u);
            u++;
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
                if (prioritizedNodes.Count >= 2 && resourceHarvester.buildingPrefab.TryGetComponent<BoxCollider2D>(out BoxCollider2D boxCollider))
                {
                    nextNode = prioritizedNodes.ElementAt(1);

                    //Check if it is the same resource type
                    if (nextNode.Key.TryGetComponent<Wood>(out Wood wood1) && 
                        ( 0.3 * (boxCollider.size.x)) <= (0.5 * Vector3.Distance(currentNode.Key.transform.position,nextNode.Key.transform.position)))
                    {
                        //Harevester position in between those nodes
                        harvesterPosition = Vector3.Lerp(currentNode.Key.transform.position, nextNode.Key.transform.position, 0.5f);

                        GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                        //Check if you have enough resources,PLace and update resources and then add the harvester to the list of buildings ,else return and resort the list
                        BuildingManager.Instance.placeBuilding
                                            (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                        prioritizedNodes.RemoveAt(0);
                        prioritizedNodes.RemoveAt(1);
                        //prioritzedBuildings.Add(new KeyValuePair <typeof resourceHarvester, >);
                        return;
                    }
                  
                }
                    harvesterPosition = currentNode.Key.transform.position;

                    GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                //Check if you have enough resources,PLace and update resources and then add the harvester to the list of buildings ,else return and resort the list
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
                if (prioritizedNodes.Count >= 2 && resourceHarvester.buildingPrefab.TryGetComponent<BoxCollider2D>(out BoxCollider2D boxCollider))
                {
                    nextNode = prioritizedNodes.ElementAt(1);

                    //Check if it is the same resource type
                    if (nextNode.Key.TryGetComponent<Stone>(out Stone stone1) &&
                        (0.3 * (boxCollider.size.x)) <= (0.5 * Vector3.Distance(currentNode.Key.transform.position, nextNode.Key.transform.position)))
                    {
                        //Harevester position in between those nodes
                        harvesterPosition = Vector3.Lerp(currentNode.Key.transform.position, nextNode.Key.transform.position, 0.5f);

                        GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                        //Check if you have enough resources,PLace and update resources and then add the harvester to the list of buildings ,else return and resort the list
                        BuildingManager.Instance.placeBuilding
                                                (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                        prioritizedNodes.RemoveAt(0);
                        prioritizedNodes.RemoveAt(1);
                        return;
                    }
                    
                }
                    harvesterPosition = currentNode.Key.transform.position;
                    GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                //Check if you have enough resources,PLace and update resources and then add the harvester to the list of buildings ,else return and resort the list
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
                if (prioritizedNodes.Count >= 2 && resourceHarvester.buildingPrefab.TryGetComponent<BoxCollider2D>(out BoxCollider2D boxCollider))
                {
                    nextNode = prioritizedNodes.ElementAt(1);

                    //Check if it is the same resource type
                    if (nextNode.Key.TryGetComponent<Gold>(out Gold gold1)&&
                        (0.3 * (boxCollider.size.x)) <= (0.5 * Vector3.Distance(currentNode.Key.transform.position, nextNode.Key.transform.position)))
                    {
                        //Harevester position in between those nodes
                        harvesterPosition = Vector3.Lerp(currentNode.Key.transform.position, nextNode.Key.transform.position, 0.5f);

                        GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);

                        //Check if you have enough resources,PLace and update resources and then add the harvester to the list of buildings ,else return and resort the list
                        BuildingManager.Instance.placeBuilding
                                                (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                        prioritizedNodes.RemoveAt(0);
                        prioritizedNodes.RemoveAt(1);
                        return;
                    }
                   
                }
                    harvesterPosition = currentNode.Key.transform.position;
                    GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);
                //Check if you have enough resources,PLace and update resources and then add the harvester to the list of buildings ,else return and resort the list
                BuildingManager.Instance.placeBuilding
                                            (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                    prioritizedNodes.RemoveAt(0);

                    return;

                }
            return;
        }
        Debug.Log("No resources nodes are available!");
        strategy = STRATEGIES.NONE;
        return;
    }

     public void AssigningMainBuilding() 
     {
        KeyValuePair<GameObject, double> tempo = new KeyValuePair<GameObject, double>(GameManager.Instance.GetGoblinsMainBuildingAsGameObject(),1);
        prioritzedBuildings.Add(tempo);
     }
    public void UnitProduction() 
    {
        BuildingSO attackingBuilding;
        Indices attackingBuildingIndices = new Indices();

        GameObject theBuildingToDefend;
        int luckyNumber = UnityEngine.Random.Range(0, 1);

        if (luckyNumber == 0)
        {
            attackingBuilding = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/DefenseBuilding");
            theBuildingToDefend = GameObject.FindGameObjectWithTag("Goblin base");
        }
        else 
        { 
            attackingBuilding = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/GoblinCamp");
            theBuildingToDefend = GameObject.FindGameObjectWithTag("Resource Harvester");
        }
        GridManager.Instance.WorldToGridPosition(theBuildingToDefend.transform.position, out attackingBuildingIndices.I,out attackingBuildingIndices.J);
        //Check if it is
        if(Affordable(attackingBuilding))
        BuildingManager.Instance.placeBuilding
        (attackingBuilding, attackingBuildingIndices, attackingBuildingIndices.I - 1, attackingBuildingIndices.I + 1, attackingBuildingIndices.J - 1, attackingBuildingIndices.J + 1);
        return;
    }
    private bool Affordable(BuildingSO building) 
    {
        if(building.wood<=ResourceManager.Instance.getGoblinWoodResource() && building.stone <= ResourceManager.Instance.getGoblinStoneResource()
            /*&building.gold<=ResourceManager.Instance.getGoblinGoldResource()&*/)
        return true;
        return false;
    }
    public void Attack() { }
    public void Defense() { }
}
