using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;

public sealed class GWorld
{

    //Singleton pattern
    private static readonly GWorld instance = new GWorld();

    private static WorldStates world;
    static GWorld()
    {
        world = new WorldStates();
    }
    private GWorld()
    {
    }

    public static GWorld Instance 
    { 
        get { return instance; } 
    }    

    public  WorldStates GetWorld()
    {
        return world;
    }
}


