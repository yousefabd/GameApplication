using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Team team;
    public Team GetTeam()
    {
        return team;
    }
    public virtual Entity Spawn(Vector3 position)
    {
        Entity entity = null;
        return entity;
    }



    public ResourceType resourceType;

}
