using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Utility;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private List<ObstacleChunkInfo> _obstacleChunks = new List<ObstacleChunkInfo>();
    [SerializeField] private ObstacleChunkInfo freeChunk;

    [SerializeField] private PlayerActionInfo.PlayerAction _generationTheme = PlayerActionInfo.PlayerAction.DODGE;
    [SerializeField] private float _actionBudget = 20;
    [SerializeField] private float _actionMoney = 20;
    [SerializeField] private float _geometryBudget = 40;
    [SerializeField] private float _geometryMoney = 40;
    [SerializeField] private int _chunksBackInPoolUntilRest = 10;
    [SerializeField] private float _restDuration = 0.5f;
    [SerializeField] private float _spawnCopyCount = 3;
    [SerializeField] private int _startingChunks = 10;
    [SerializeField] private float _startingOffset = 0;
    [SerializeField] private float _offset = 10;
    [SerializeField] private float _likelinessChosenThemeSpawning = 0.6f;

    private List<ObstacleChunkInfo> _dodgeChunks = new List<ObstacleChunkInfo>();
    private List<ObstacleChunkInfo> _rampChunks = new List<ObstacleChunkInfo>();
    private List<ObstacleChunkInfo> _zigzagChunks = new List<ObstacleChunkInfo>();

    List<GameObject> _dodgePool = new List<GameObject>();
    List<GameObject> _dodgeChunkInGame = new List<GameObject>();
    List<GameObject> _rampPool = new List<GameObject>();
    List<GameObject> _rampChunkInGame = new List<GameObject>();
    List<GameObject> _zigzagPool = new List<GameObject>();
    List<GameObject> _zigzagChunksInGame = new List<GameObject>();

    List<PlayerActionInfo.PlayerAction> _availableActions = new List<PlayerActionInfo.PlayerAction>();
    private int chunksBackInPool = 0;

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
                        _dodgePool.Add(SpawnChunkForPool(chunk));
                    }
                    break;
                case PlayerActionInfo.PlayerAction.ZIGZAG:
                    for (int i = 0; i < _spawnCopyCount; i++)
                    {
                        _zigzagPool.Add(SpawnChunkForPool(chunk));
                    }
                    break;
                case PlayerActionInfo.PlayerAction.RAMP:
                    for (int i = 0; i < _spawnCopyCount; i++)
                    {
                        _rampPool.Add(SpawnChunkForPool(chunk));
                    }
                    break;
                default:
                    break;
            }
        }

        SprintStart(false);

        for (int i = 0; i < _startingChunks; i++)
        {
            GenerateChunk(new Vector3(transform.position.x, transform.position.y, i * _offset + _startingOffset));
        }
    }

    private GameObject SpawnChunkForPool(ObstacleChunkInfo chunk)
    {
        GameObject spawnedChunk = Instantiate(chunk.ObstaclePrefab);
        DestroyAfterPos destroyAfterPos = spawnedChunk.GetComponent<DestroyAfterPos>();
        destroyAfterPos._obstacleGenerator = this;
        destroyAfterPos.actionTheme = chunk.GetPlayerActionInfo.GetPlayerAction;
        spawnedChunk.SetActive(false);
        spawnedChunk.transform.GetChild(1).gameObject.SetActive(false);

        return spawnedChunk;
    }

    private void SprintStart()
    {
        SprintStart(true);
    }

    private void SprintStart(bool generateChunk)
    {
        _actionMoney = _actionBudget;
        _geometryMoney = _geometryBudget;
        chunksBackInPool = 0;

        AddAvailableActions();

        if (generateChunk)
        {
            Invoke(nameof(GenerateChunk), _restDuration);
        }
    }

    private void AddAvailableActions()
    {
        _availableActions.Clear();
        int enumLength = System.Enum.GetValues(typeof(PlayerActionInfo.PlayerAction)).Length;

        for (int i = 0; i < enumLength; i++)
        {
            _availableActions.Add((PlayerActionInfo.PlayerAction)i);
        }        
    }

    public void CheckGenerateOrStartSprint(GameObject chunk, PlayerActionInfo.PlayerAction actionTheme)
    {
        chunksBackInPool++;

        switch (actionTheme)
        {
            case PlayerActionInfo.PlayerAction.DODGE:
                _dodgePool.Add(chunk);
                break;
            case PlayerActionInfo.PlayerAction.ZIGZAG:
                _zigzagPool.Add(chunk);
                break;
            case PlayerActionInfo.PlayerAction.RAMP:
                _rampPool.Add(chunk);
                break;
        }

        if (chunksBackInPool >= _chunksBackInPoolUntilRest)
        {
            SprintStart();
        }
        else
        {
            GenerateChunk();
        }
    }

    private void GenerateChunk()
    {
        GenerateChunk(transform.position);
    }

    private void GenerateChunk(Vector3 pos)
    {
        List<ObstacleChunkInfo> chosenChunkInfos;
        float actionCost = 0;

        if (_availableActions.Count > 1)
        {
            float rand = Random.Range(0f, 1f);

            if (rand < _likelinessChosenThemeSpawning && _availableActions.Contains(_generationTheme))
            {
                chosenChunkInfos = ChooseChunks((int)_generationTheme);
                actionCost = chosenChunkInfos[0].GetPlayerActionInfo.HighFrequencyActionCost;
            }
            else
            {
                int enumLength = System.Enum.GetValues(typeof(PlayerActionInfo.PlayerAction)).Length;
                int result = Random.Range(0, enumLength);

                if (result == (int)_generationTheme)
                {
                    result += result != enumLength - 1 ? 1 : -1;
                }

                chosenChunkInfos = ChooseChunks(result);
                actionCost = chosenChunkInfos[0].GetPlayerActionInfo.LowFrequencyActionCost;
            }
        }
        else
        {
            chosenChunkInfos = ChooseChunks(_availableActions.Count - 1);
            actionCost = chosenChunkInfos[0].GetPlayerActionInfo.HighFrequencyActionCost;
        }
        
        PlayerActionInfo.PlayerAction chosenAction = chosenChunkInfos[0].GetPlayerActionInfo.GetPlayerAction;

        if (actionCost > _actionBudget)
        {
            _availableActions.Remove(chosenAction);
            GenerateChunk();
            return;
        }

        _actionBudget -= actionCost;

        ObstacleChunkInfo chosenChunk = ChooseChunk(chosenChunkInfos);       

        List<GameObject> chosenChunksPool;

        switch (chosenAction)
        {
            case PlayerActionInfo.PlayerAction.DODGE:
                chosenChunksPool = _dodgePool;
                break;
            case PlayerActionInfo.PlayerAction.ZIGZAG:
                chosenChunksPool = _zigzagPool;
                break;
            case PlayerActionInfo.PlayerAction.RAMP:
                chosenChunksPool = _rampPool;
                break;
            default:
                chosenChunksPool = _dodgePool;
                break;
        }

        int obstacleIndex = chosenChunksPool.IndexOf(chosenChunk.ObstaclePrefab);
        GameObject chunkToSpawn = chosenChunksPool[obstacleIndex];

        bool chunksInPool = true;

        if (obstacleIndex < 0)
        {
            if (chosenChunksPool.Count > 0)
            {
                chunkToSpawn = chosenChunksPool[Random.Range(0, chosenChunksPool.Count)];
            }
            else if(_dodgePool.Count > 0)
            {
                chosenChunksPool = _dodgePool;
                chosenAction = PlayerActionInfo.PlayerAction.DODGE;
                chunkToSpawn = _dodgePool[Random.Range(0, _dodgePool.Count)];
            }
            else
            {
                chunksInPool = false;
                chosenAction = PlayerActionInfo.PlayerAction.DODGE;
                chunkToSpawn = _dodgeChunkInGame[0];
            }
        }

        if (chunksInPool)
        {
            chosenChunksPool.Remove(chunkToSpawn);

            switch (chosenAction)
            {
                case PlayerActionInfo.PlayerAction.DODGE:
                    _dodgeChunkInGame.Add(chunkToSpawn);
                    break;
                case PlayerActionInfo.PlayerAction.ZIGZAG:
                    _zigzagChunksInGame.Add(chunkToSpawn);
                    break;
                case PlayerActionInfo.PlayerAction.RAMP:
                    _rampChunkInGame.Add(chunkToSpawn);
                    break;
                default:
                    break;
            }
        }

        chunkToSpawn.transform.position = pos;
        chunkToSpawn.SetActive(true);
        chunkToSpawn.transform.GetChild(1).gameObject.SetActive(false);
    }   

    private ObstacleChunkInfo ChooseChunk(List<ObstacleChunkInfo> obstacleChunkInfos)
    {
        List<ObstacleChunkInfo> modifiedObstacleChunkInfos = new List<ObstacleChunkInfo>();
        modifiedObstacleChunkInfos.AddRange(obstacleChunkInfos);

        ObstacleChunkInfo chosenChunk = modifiedObstacleChunkInfos[0];
        float cost = chosenChunk.GeometryCost;

        while (cost > _geometryBudget)
        {
            if (_geometryBudget <= 0)
            {
                chosenChunk = freeChunk;
                break;
            }

            int randResult = Random.Range(0, modifiedObstacleChunkInfos.Count - 1);

            chosenChunk = modifiedObstacleChunkInfos[randResult];
            cost = chosenChunk.GeometryCost;

            if (cost > _geometryBudget)
            {
                modifiedObstacleChunkInfos.Remove(chosenChunk);
            }

            if (modifiedObstacleChunkInfos.Count == 0)
            {
                modifiedObstacleChunkInfos.Clear();
                modifiedObstacleChunkInfos.AddRange(_dodgeChunks);
            }
        }        

        return chosenChunk;
    }

    private List<ObstacleChunkInfo> ChooseChunks(int enumIndex)
    {
        List<ObstacleChunkInfo> chosenChunkInfos = new List<ObstacleChunkInfo>();

        switch ((PlayerActionInfo.PlayerAction)enumIndex)
        {
            case PlayerActionInfo.PlayerAction.DODGE:
                chosenChunkInfos = _dodgeChunks;
                break;
            case PlayerActionInfo.PlayerAction.ZIGZAG:
                chosenChunkInfos = _zigzagChunks;
                break;
            case PlayerActionInfo.PlayerAction.RAMP:
                chosenChunkInfos = _rampChunks;
                break;
            default:
                chosenChunkInfos = _dodgeChunks;
                break;
        }

        return chosenChunkInfos;
    }
}
