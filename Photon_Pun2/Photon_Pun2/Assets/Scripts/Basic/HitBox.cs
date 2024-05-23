using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] PlayerBattleControl playerBattleControl;

    public PlayerBattleControl BattleControl => playerBattleControl;
}
