using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveRightObstacleChunk", menuName = "ScriptableObjects/ObstacleChunkInfo", order = 0)]
public class ObstacleChunkInfo : ScriptableObject
{
    public enum CarGoalPosition { L, ML, MR, R}

    [SerializeField] private float _geometryCost = 0;
    [SerializeField] private float _extraOffset = 0;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private PlayerActionInfo _playerActionInfo;
    [SerializeField] private List<CarGoalPosition> _startPositions;
    [SerializeField] private List<CarGoalPosition> _endPositions;
    [SerializeField] private int _allowedPreviousChunkEndAndStartPosDifference = 2;

    public float GeometryCost { get { return _geometryCost; } }
    public float ExtraOffset { get { return _extraOffset; } }
    public GameObject ObstaclePrefab { get { return _obstaclePrefab; } }
    public PlayerActionInfo GetPlayerActionInfo { get { return _playerActionInfo; } }
    public List<CarGoalPosition> StartPositions { get { return _startPositions; } }
    public List<CarGoalPosition> EndPositions { get { return _endPositions; } }
    public int AllowedPreviousChunkEndAndStartPosDifference { get { return _allowedPreviousChunkEndAndStartPosDifference; } }
}
