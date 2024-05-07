using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHealth { get; set; } = 100;

    void HealthChanged()
    {
        Debug.Log($"Health changed to: {NetworkedHealth}");
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damage)
    {
        Debug.Log("Received DealDamageRpc : " + damage);
        NetworkedHealth -= damage;
    }
}