using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private List<ObstacleChunkInfo> _obstacleChunks = new List<ObstacleChunkInfo>();

    [SerializeField] private PlayerActionInfo.PlayerAction _generationTheme = PlayerActionInfo.PlayerAction.DODGE;
    [SerializeField] private float _actionBudget = 20;
    [SerializeField] private float _geometryBudget = 40;
    [SerializeField] private int _chunksDestroyedUntilRest = 10;
    [SerializeField] private float _restDuration = 0.5f;
    [SerializeField] private float _spawnCopyCount = 2;
    [SerializeField] private int _startingChunks = 10;
    [SerializeField] private float _startingOffset = 10;

    private List<ObstacleChunkInfo> _dodgeChunks = new List<ObstacleChunkInfo>();
    private List<ObstacleChunkInfo> _rampChunks = new List<ObstacleChunkInfo>();
    private List<ObstacleChunkInfo> _zigzagChunks = new List<ObstacleChunkInfo>();

    List<GameObject> _dodgePool = new List<GameObject>();
    List<GameObject> _rampPool = new List<GameObject>();
    List<GameObject> _zigzagPool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(ObstacleChunkInfo chunk in _obstacleChunks)
        {
            switch (chunk.GetPlayerActionInfo.GetPlayerAction)
            {
                case PlayerActionInfo.PlayerAction.DODGE:
                    _dodgeChunks.Add(chunk);
                    for (int i = 0; i < _spawnCopyCount; i++)
                    {
                        GameObject spawnedChunk = Instantiate(chunk.ObstaclePrefab);
                        spawnedChunk.SetActive(false);
                        _dodgePool.Add(spawnedChunk);
                    }
                    break;
                case PlayerActionInfo.PlayerAction.ZIGZAG:
                    for (int i = 0; i < _spawnCopyCount; i++)
                    {
                        GameObject spawnedChunk = Instantiate(chunk.ObstaclePrefab);
                        spawnedChunk.SetActive(false);
                        _zigzagPool.Add(spawnedChunk);
                    }
                    break;
                case PlayerActionInfo.PlayerAction.RAMP:
                    for (int i = 0; i < _spawnCopyCount; i++)
                    {
                        GameObject spawnedChunk = Instantiate(chunk.ObstaclePrefab);
                        spawnedChunk.SetActive(false);
                        _rampPool.Add(spawnedChunk);
                    }
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < _startingChunks; i++)
        {
            GenerateChunk(new Vector3(transform.position.x, transform.position.y, i * _startingOffset));
        }
    }

    private void GenerateChunk()
    {

    }

    private void GenerateChunk(Vector3 pos)
    {

    }
}
