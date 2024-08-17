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

    Indices lastPoinInMap = new Indices(80, 80);
    Indices lastSearchingPoinInMap = new Indices(0, 0);

    int k = 2;

    BuildingSO BB;
    private void Start()
    {
        strategy = STRATEGIES.NONE;
        //InvokeRepeating("SpawnGoblins",5,7)
    }

    private void Update()
    {

        //When to gather resources 
        if (strategy != STRATEGIES.RESOURCE_GATHERING && (Time.time<=10.0f))
        {
            Debug.Log("Resource Gathering Condition is satisfied");
            strategy = STRATEGIES.RESOURCE_GATHERING;
            onResourceGathering += ComputerPlayer_onResourceGathering;
            onResourceGathering?.Invoke(this, EventArgs.Empty);
        }

        //When to Produce unit
        if (strategy != STRATEGIES.UNIT_PRODUCTION&&(Time.time > 10.0f) )
        {
            Debug.Log("Unit production Condition is satisfied");
            strategy = STRATEGIES.UNIT_PRODUCTION;
            onUnitProduction += ComputerPlayer_onUnitProduction;
            onUnitProduction?.Invoke(this, EventArgs.Empty);
        }
    }
    public void SpawnGoblins(BuildingSO building) 
    {
        Vector3 B = new Vector3(building.buildingPrefab.gameObject.transform.position.x+1, building.buildingPrefab.gameObject.transform.position.y-1);
        Cell unitCell = GridManager.Instance.GetgridMap().GetValue(B);
        Indices indices = unitCell.GetIndices();
        Unit unit;
        int r = UnityEngine.Random.Range(0, 3);
        BuildingSO b = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/GoblinCamp");
        unit = unitCell.SpawnUnit(b.unitGenerationData[r], GridManager.Instance.GridToWorldPositionCentered(indices));
        unitCell.SetEntity(unit);
        GridManager.Instance.GetgridMap().UpdateValues();
    }


    private void ComputerPlayer_onUnitProduction(object sender, EventArgs e)
    {
        Debug.Log("I AM AT : ComputerPlayer_onUnitProduction");
        if (onResourceGathering != null)
        {
            Debug.Log("I CANCELED SUBSCRIPTION FOR onResourceGathering");
            CancelInvoke("GatherResources");
            onResourceGathering -= ComputerPlayer_onUnitProduction;
        }
        InvokeRepeating("UnitProduction",3,10);
        Debug.Log("Invoke InvokeRepeating(\"UnitProduction\",3,10); was called");
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
    if (ResourceManager.Instance.getGoblinStoneResource() >= 50 || ResourceManager.Instance.getGoblinWoodResource() >= 200 || ResourceManager.Instance.getGoldResource() >= 10000)
            return true;
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
                     resourceHarvester = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/WoodMinerGoblin");

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
                        if (!Affordable(resourceHarvester))
                            return;
                            BuildingManager.Instance.placeBuilding
                                            (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                            ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.wood);
                            ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.stone);
                            ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.price);

                            prioritizedNodes.RemoveAt(0);
                            prioritizedNodes.RemoveAt(1);
                            return;
                    }
                  
                }
                    harvesterPosition = currentNode.Key.transform.position;

                    GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);
                harvesterIndices.I += k;
                harvesterIndices.J+= k;
                //Check if you have enough resources,PLace and update resources and then add the harvester to the list of buildings ,else return and resort the list
                if (!Affordable(resourceHarvester))
                    return;
                    BuildingManager.Instance.placeBuilding
                (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.wood);
                ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.stone);
                ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.price);

                prioritizedNodes.RemoveAt(0);
                    return;
                
                
                }



                //Or is it stone?

                else if (currentNode.Key.TryGetComponent<Stone>(out Stone stone))
                {
                    //The needed harvester is Stone Miner
                    resourceHarvester = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/StoneMinerGoblin");

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
                        if (!Affordable(resourceHarvester))
                            return;
                            BuildingManager.Instance.placeBuilding
                                            (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                            ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.wood);
                            ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.stone);
                            ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.price);

                            prioritizedNodes.RemoveAt(0);
                            prioritizedNodes.RemoveAt(1);
                            return;
                    }
                    
                }
                    harvesterPosition = currentNode.Key.transform.position;
                    GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);
                
                harvesterIndices.I += k;
                harvesterIndices.J += k;
                if (!Affordable(resourceHarvester))
                    return;

                    BuildingManager.Instance.placeBuilding
                (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.wood);
                ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.stone);
                ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.price);

                prioritizedNodes.RemoveAt(0);
                    
                    return;               
            }



            //Or is It Gold?


            else if (currentNode.Key.TryGetComponent<Gold>(out Gold gold))
                {
                    //The needed harvester is Stone Miner
                    resourceHarvester = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/GoldMinerGoblin");

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
                        if (!Affordable(resourceHarvester))
                            return;
                            BuildingManager.Instance.placeBuilding
                                            (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                            ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.wood);
                            ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.stone);
                            ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.price);

                            prioritizedNodes.RemoveAt(0);
                            prioritizedNodes.RemoveAt(1);
                            return;
                    }
                   
                }
                    harvesterPosition = currentNode.Key.transform.position;
                    GridManager.Instance.WorldToGridPosition(harvesterPosition, out harvesterIndices.I, out harvesterIndices.J);
                k = 2;
                harvesterIndices.I += k;
                harvesterIndices.J += k;

                if (!Affordable(resourceHarvester))
                    return;
                    BuildingManager.Instance.placeBuilding
                (resourceHarvester, harvesterIndices, harvesterIndices.I - 1, harvesterIndices.I + 1, harvesterIndices.J - 1, harvesterIndices.J + 1);
                ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.wood);
                ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.stone);
                ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -resourceHarvester.price);
                prioritizedNodes.RemoveAt(0);
                    
                    return;
               

                }
            return;
        }
        Debug.Log("No resources nodes are available!");
        strategy = STRATEGIES.NONE;
        return;
    }

    public void UnitProduction() 
    {
        BuildingSO attackingBuilding;
        Indices attackingBuildingIndices = new Indices();

        GameObject theBuildingToDefend;
        int luckyNumber = UnityEngine.Random.Range(0, 15);
        Debug.Log("Lucky number is " + luckyNumber);
        if (luckyNumber == 0)
        {
            Debug.Log("We are gonna build DefenseBuilding next to the Goblin Base ");
            attackingBuilding = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/DefenseBuildingGoblins");
            theBuildingToDefend = GameObject.FindGameObjectWithTag("Goblin Base");
        }
        else 
        {
            Debug.Log("We are gonna build GoblinCamp next to the Resource Harvester ");
            attackingBuilding = Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/GoblinCamp");

            theBuildingToDefend = GameObject.FindGameObjectWithTag("Resource Harvester");
            if (theBuildingToDefend == null)
            {
                Debug.Log("But there is no Resource Harvester ");
                theBuildingToDefend = GameObject.FindGameObjectWithTag("Goblin Base");
            }
        }
        if (!Affordable(attackingBuilding))
        {  return; }
            
            GridManager.Instance.WorldToGridPosition(theBuildingToDefend.transform.position, out attackingBuildingIndices.I, out attackingBuildingIndices.J);
         k = UnityEngine.Random.Range(-3, +3);
        while (k == 0) { k = UnityEngine.Random.Range(-3, +3); }
        attackingBuildingIndices.I += k;
        attackingBuildingIndices.J -= k;
        BuildingManager.Instance.placeBuilding
            (attackingBuilding, attackingBuildingIndices, attackingBuildingIndices.I - 1, attackingBuildingIndices.I + 1, attackingBuildingIndices.J - 1, attackingBuildingIndices.J + 1);
        ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -attackingBuilding.wood);
        ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -attackingBuilding.stone);
        ResourceManager.Instance.updateResourceGoblin(ResourceType.WOOD, -attackingBuilding.price);
        if (attackingBuilding == Resources.Load<BuildingSO>("ScriptableObjects/BuildingTypes/GoblinCamp"))
        { 
            BB = attackingBuilding;
            InvokeRepeating("MethodInBetween", 1, 10);
        }
        Debug.Log(attackingBuilding + "Is instantiated");
        return;
        
    }
    public void MethodInBetween() { SpawnGoblins(BB); }
    private bool Affordable(BuildingSO building) 
    {
        Debug.Log("Goblin gold : " + ResourceManager.Instance.getGoblinGoldResource());
        Debug.Log("Goblin wood : " + ResourceManager.Instance.getGoblinWoodResource());
        Debug.Log("Goblin stone : " + ResourceManager.Instance.getGoblinStoneResource());
        if (building.wood<=ResourceManager.Instance.getGoblinWoodResource() && building.stone <= ResourceManager.Instance.getGoblinStoneResource()
            &&building.price<=ResourceManager.Instance.getGoblinGoldResource())
        return true;
        return false;
    }

    public void Attack() { }
    public void Defense() { }
}
