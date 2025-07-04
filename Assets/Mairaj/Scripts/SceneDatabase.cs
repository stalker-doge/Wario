//Mairaj Muhammad ->2415831
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneDatabase", menuName = "ScriptableObjects/Scene Database")]
public class SceneDatabase : ScriptableObject
{
    public List<SceneData> scenes;
}

[System.Serializable]
public class SceneData
{
    public string sceneName;
    public SceneType sceneType;
}

public enum SceneType
{
    MainMenu,
    Options,
    EndScene,
    Loading,
    Credits,
    Leaderboard,
    MatchTwoCardsVariant1,
    MatchTwoCardsVariant2,
    MatchTwoCardsVariant3,
    RollingBall,
    MazeGame,
    AimAndShootVariant1,
    AimAndShootVariant2,
    AimAndShootVariant3,
    AimAndShootVariant4,
    AimAndShootOnline
}

