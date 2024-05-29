using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singletons/MasterManger")]
public class MasterManager : SingletonObject<MasterManager>
{
    [SerializeField]
    private GameSettings _gameSettings;
    public GameSettings GameSettings { get { return Instance._gameSettings; } }
}
