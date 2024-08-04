using System.Numerics;

public class Builder : Unit
{
    private float buildRadius;
    protected override void Awake()
    {
        base.Awake();
        buildRadius = unitSO.interactionRadius;
    }

    public void StartBuilding(Vector3 buildingPosition)
    {
        
    }
}
