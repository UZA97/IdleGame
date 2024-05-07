using Fusion;
using UnityEngine;
using Unity;
using UnityEngine.UI;

public class PlayerColor : NetworkBehaviour
{
    public MeshRenderer MeshRenderer;
    [Networked, OnChangedRender(nameof(ColorChanged))]
    public Color NetworkedColor { get; set; }
    private void Update()
    {
        if (HasStateAuthority && Input.GetKeyDown(KeyCode.E))
        {
            NetworkedColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }
    void ColorChanged()
    {
        Debug.Log("Color Runner : " + Runner + " _ " + NetworkedColor);

        MeshRenderer.material.color = NetworkedColor;
    }
}
