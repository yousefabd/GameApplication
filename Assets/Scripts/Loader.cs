using UnityEngine.SceneManagement;

public static class Loader
{


    public enum Scene
    {
        MainMenuScene,
        test,
        LoadingScene,
        TowerDefenseMode
    }


    private static Scene targetScene;



    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }

}