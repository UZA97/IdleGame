using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string _gameVersion = "0.0.1";
    public string GameVersion { get { return _gameVersion; } }
    [SerializeField] private string _nickName = "UZA";
    public string NickName
    {
        get
        {
            int value = Random.Range(0, 9999);
            string name = $"{_nickName}_{value}";
            return name;
        }
    }
}
