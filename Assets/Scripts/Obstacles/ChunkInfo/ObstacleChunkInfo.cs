using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveRightObstacleChunk", menuName = "ScriptableObjects/ObstacleChunkInfo", order = 0)]
public class ObstacleChunkInfo : ScriptableObject
{
    public enum CarGoalPosition { L, ML, MR, R}

    [SerializeField] private float _geometryCost = 0;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private PlayerActionInfo _playerActionInfo;
    [SerializeField] private CarGoalPosition[] _startPositions;
    [SerializeField] private CarGoalPosition[] _endPositions;

    public float GeometryCost { get { return _geometryCost; } }
    public GameObject ObstaclePrefab { get { return _obstaclePrefab; } }
    public PlayerActionInfo GetPlayerActionInfo { get { return _playerActionInfo; } }
    public CarGoalPosition[] StartPositions { get { return _startPositions; } }
    public CarGoalPosition[] EndPositions { get { return _endPositions; } }
}
