using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DodgeActionInfo", menuName = "ScriptableObjects/PlayerActionInfo", order = 1)]
public class PlayerActionInfo : ScriptableObject
{
    public enum PlayerAction { DODGE, ZIGZAG, RAMP}
    [SerializeField] private PlayerAction _playerAction;
    public PlayerAction GetPlayerAction { get { return _playerAction; } }

    [SerializeField] private float _highFrequencyActionCost = 0;
    public float HighFrequencyActionCost { get { return _highFrequencyActionCost; } }
    [SerializeField] private float _lowFrequencyActionCost = 0;
    public float LowFrequencyActionCost { get { return _lowFrequencyActionCost; } }
}
