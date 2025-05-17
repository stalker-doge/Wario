// Mairaj Muhammad -> 2415831
using System.Collections.Generic;
using UnityEngine;

public class SceneDatabaseManager : MonoBehaviour
{
    public static SceneDatabaseManager Instance { get; private set; }

    [SerializeField] private SceneDatabase database;

    // Use SceneType as the dictionary key for O(1) lookup
    private Dictionary<SceneType, SceneData> sceneLookup;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeLookup();
    }

    private void InitializeLookup()
    {
        sceneLookup = new Dictionary<SceneType, SceneData>();
        foreach (var data in database.scenes)
        {
            if (!sceneLookup.ContainsKey(data.sceneType))
            {
                sceneLookup[data.sceneType] = data;
            }
            else
            {
                Debug.LogWarning($"Duplicate SceneType found: {data.sceneType}");
            }
        }
    }

    // Returns the scene name string associated with the given SceneType.
    public string GetSceneString(SceneType type)
    {
        if (sceneLookup.TryGetValue(type, out var data))
            return data.sceneName;
        return null;
    }

    // Returns the SceneData associated with the given SceneType.
    public SceneData GetSceneData(SceneType type)
    {
        if (sceneLookup.TryGetValue(type, out var data))
            return data;
        return null;
    }
}
