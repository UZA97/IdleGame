using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    [HideInInspector] public NetworkObject network;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            network = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            if (network.HasStateAuthority)
            {
                ChatManager.instance.SetNetWorkObject(network);
            }
        }
    }
}