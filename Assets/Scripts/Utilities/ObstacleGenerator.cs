using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Utility;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private List<ObstacleChunkInfo> _obstacleChunks = new List<ObstacleChunkInfo>();
    [SerializeField] private ObstacleChunkInfo _freeChunk;

    [SerializeField] private PlayerActionInfo.PlayerAction _generationTheme = PlayerActionInfo.PlayerAction.DODGE;
    [SerializeField] private List<ObstacleChunkInfo.CarGoalPosition> _lastEndPosition = new List<ObstacleChunkInfo.CarGoalPosition>();
    [SerializeField] private float _actionBudget = 20;
    [SerializeField] private float _actionMoney = 20;
    [SerializeField] private float _geometryBudget = 40;
    [SerializeField] private float _geometryMoney = 40;
    [SerializeField] private int _chunksBackInPoolUntilRest = 10;
    [SerializeField] private float _restDuration = 0.5f;
    [SerializeField] private float _spawnCopyCount = 3;
    [SerializeField] private float _freeChunkCopyCount = 5;
    [SerializeField] private int _startingChunks = 10;
    [SerializeField] private float _startingOffset = 0;
    [SerializeField] private float _freeChunkMoney = 10;
    [SerializeField] private float _offset = 10;
    [SerializeField] private float _likelinessChosenThemeSpawning = 0.6f;
    [SerializeField] private float _likelinessOfChunkRecurring = 0.2f;

    private List<ObstacleChunkInfo> _dodgeChunks = new List<ObstacleChunkInfo>();
    private List<ObstacleChunkInfo> _rampChunks = new List<ObstacleChunkInfo>();
    private List<ObstacleChunkInfo> _zigzagChunks = new List<ObstacleChunkInfo>();
    private List<ObstacleChunkInfo> _modChunks = new List<ObstacleChunkInfo>();

    List<GameObject> _freePool = new List<GameObject>();
    List<GameObject> _dodgePool = new List<GameObject>();
    List<GameObject> _dodgeChunkInGame = new List<GameObject>();
    List<GameObject> _rampPool = new List<GameObject>();
    List<GameObject> _rampChunkInGame = new List<GameObject>();
    List<GameObject> _zigzagPool = new List<GameObject>();
    List<GameObject> _zigzagChunksInGame = new List<GameObject>();
    List<GameObject> _chunksInGame = new List<GameObject>();

    List<PlayerActionInfo.PlayerAction> _availableActions = new List<PlayerActionInfo.PlayerAction>();
    private int _chunksBackInPool = 0;
    private float _minimumChunkCost = 999;
    private int _obstaclesSpawned = 0;
    private float _startingExtraOffset = 0;
    private ObstacleChunkInfo _previousChunk;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate chunks for their respective pools
        foreach (ObstacleChunkInfo chunk in _obstacleChunks)
        {
            if (chunk.GeometryCost < _minimumChunkCost)
            {
                _minimumChunkCost = chunk.GeometryCost;
            }

            switch (chunk.GetPlayerActionInfo.GetPlayerAction)
            {
                case PlayerActionInfo.PlayerAction.DODGE:
                    _dodgeChunks.Add(chunk);
                    for (int i = 0; i < _spawnCopyCount; i++)
                    {
                        _dodgePool.Add(SpawnChunkForPool(chunk, false));
                    }
                    break;
                case PlayerActionInfo.PlayerAction.ZIGZAG:
                    _zigzagChunks.Add(chunk);
                    for (int i = 0; i < _spawnCopyCount; i++)
                    {
                        _zigzagPool.Add(SpawnChunkForPool(chunk, false));
                    }
                    break;
                case PlayerActionInfo.PlayerAction.RAMP:
                    _rampChunks.Add(chunk);
                    for (int i = 0; i < _spawnCopyCount; i++)
                    {
                        _rampPool.Add(SpawnChunkForPool(chunk, false));
                    }
                    break;
                default:
                    break;
            }
        }

        //Pool for free chunks, so that it will always spawn something
        for (int i = 0; i < _freeChunkCopyCount; i++)
        {
            _freePool.Add(SpawnChunkForPool(_freeChunk, true));
        }

        SprintStart();

        //Start with more money based on starting chunks
        float startMultiplier = Mathf.Clamp(_startingChunks / _chunksBackInPoolUntilRest, 1, 99);
        _actionMoney *= startMultiplier;
        _geometryMoney *= startMultiplier;

        //Sets chunks in the correct position
        transform.position = new Vector3(transform.position.x, transform.position.y, _obstaclesSpawned * _offset + _startingOffset);

        GenerateChunk();
    }

    //Gives info to every chunk that is spawned
    private GameObject SpawnChunkForPool(ObstacleChunkInfo chunk, bool free)
    {
        GameObject spawnedChunk = Instantiate(chunk.ObstaclePrefab);
        DestroyAfterPos destroyAfterPos = spawnedChunk.GetComponent<DestroyAfterPos>();
        destroyAfterPos._obstacleGenerator = this;
        destroyAfterPos.free = free;
        destroyAfterPos.actionTheme = chunk.GetPlayerActionInfo.GetPlayerAction;
        spawnedChunk.SetActive(false);
        spawnedChunk.transform.GetChild(1).gameObject.SetActive(false);

        //If there are no start or end positions clone one of the ones that does have something and apply that to that the empty one
        if (chunk.StartPositions.Count == 0)
        {
            chunk.StartPositions.AddRange(chunk.EndPositions);
        }
        else if(chunk.EndPositions.Count == 0)
        {
            chunk.EndPositions.AddRange(chunk.StartPositions);
        }

        return spawnedChunk;
    }

    //After an given amount of chunks the money will reset to the given budget
    private void SprintStart()
    {
        _actionMoney = _actionBudget;
        _geometryMoney = _geometryBudget;
        _chunksBackInPool = 0;

        AddAvailableActions();
    }

    //Add every action back so that every action can be taken by the generator
    private void AddAvailableActions()
    {
        _availableActions.Clear();
        int enumLength = System.Enum.GetValues(typeof(PlayerActionInfo.PlayerAction)).Length;

        for (int i = 0; i < enumLength; i++)
        {
            _availableActions.Add((PlayerActionInfo.PlayerAction)i);
        }        
    }

    //After an chunk is done add it to the pool and if that enough chunks are back then start a new sprint
    public void CheckGenerateOrStartSprint(GameObject chunk, PlayerActionInfo.PlayerAction actionTheme, bool free)
    {
        _chunksBackInPool++;
        _chunksInGame.Remove(chunk);

        if (!free)
        {
            switch (actionTheme)
            {
                case PlayerActionInfo.PlayerAction.DODGE:
                    _dodgePool.Add(chunk);
                    _dodgeChunkInGame.Remove(chunk);
                    break;
                case PlayerActionInfo.PlayerAction.ZIGZAG:
                    _zigzagPool.Add(chunk);
                    _zigzagChunksInGame.Remove(chunk);
                    break;
                case PlayerActionInfo.PlayerAction.RAMP:
                    _rampPool.Add(chunk);
                    _rampChunkInGame.Remove(chunk);
                    break;
                default:
                    _dodgePool.Add(chunk);
                    _dodgeChunkInGame.Remove(chunk);
                    break;
            }
        }
        else
        {
            switch (actionTheme)
            {
                case PlayerActionInfo.PlayerAction.DODGE:
                    _freePool.Add(chunk);
                    _dodgeChunkInGame.Remove(chunk);
                    break;
                case PlayerActionInfo.PlayerAction.ZIGZAG:
                    _freePool.Add(chunk);
                    _zigzagChunksInGame.Remove(chunk);
                    break;
                case PlayerActionInfo.PlayerAction.RAMP:
                    _freePool.Add(chunk);
                    _rampChunkInGame.Remove(chunk);
                    break;
                default:
                    _freePool.Add(chunk);
                    _dodgeChunkInGame.Remove(chunk);
                    break;
            }

        }

        if (_chunksBackInPool >= _chunksBackInPoolUntilRest)
        {
            SprintStart();
        }

        if (_obstaclesSpawned >= _startingChunks)
        {
            GenerateChunk(_chunksInGame[_chunksInGame.Count - 1].transform.position + Vector3.forward * (_offset + _previousChunk.ExtraOffset));
        }
    }

    //Generate a chunk on the postion of the generator
    private void GenerateChunk()
    {
        GenerateChunk(transform.position);
    }

    //Generate a chunk on the given position
    private void GenerateChunk(Vector3 pos)
    {
        List<ObstacleChunkInfo> chosenChunkInfos;
        float actionCost = 0;

        float beginActionMoney = _actionMoney;
        float beginGeometryMoney = _geometryMoney;

        if (_availableActions.Count > 1)
        {
            float rand = Random.Range(0f, 1f);

            //Choose an action theme based on the current theme of the generator. The theme of the generator has a higher chance of spawning
            if (rand < _likelinessChosenThemeSpawning && _availableActions.Contains(_generationTheme))
            {
                chosenChunkInfos = ChooseChunks((int)_generationTheme);
                //Debug.Log("First Chunk " + chosenChunkInfos[0]);
                actionCost = chosenChunkInfos[0].GetPlayerActionInfo.HighFrequencyActionCost;
            }
            else
            {
                //If the generator chooses another action then the current theme then choose another theme excluding the current theme of the generator
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

            chosenChunkInfos = ChooseChunks((int)_availableActions[0]);
            actionCost = chosenChunkInfos[0].GetPlayerActionInfo.HighFrequencyActionCost;
        }
        
        //Get the chosen action theme
        PlayerActionInfo.PlayerAction chosenAction = chosenChunkInfos[0].GetPlayerActionInfo.GetPlayerAction;

        //If the action theme costs too much money then try again but removing the chosen action
        if (actionCost > _actionMoney && _actionMoney > 0)
        {
            _availableActions.Remove(chosenAction);
            GenerateChunk();
            return;
        }

        ObstacleChunkInfo chosenChunk;
        _actionMoney -= actionCost;

        chosenChunk = ChooseChunk(chosenChunkInfos);
        chosenAction = chosenChunk.GetPlayerActionInfo.GetPlayerAction;
        
        //Set the last postions the car could have been in after the previous chunk
        _lastEndPosition.Clear();
        _lastEndPosition.AddRange(chosenChunk.EndPositions);

        List<GameObject> chosenChunksPool;

        //Choose a pool
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

        int obstacleIndex = -1;

        //Find gameObject in pool based on the current chunk chosen
        for (int i = 0; i < chosenChunksPool.Count; i++)
        {
            if (chosenChunksPool[i].name == chosenChunk.ObstaclePrefab.name + "(Clone)")
            {
                obstacleIndex = i;
                break;
            }
        }
        
        GameObject chunkToSpawn = null;

        //If there are no chunks available in the pool of the chosen chunk then choose a free chunk
        if (obstacleIndex < 0 || chosenChunksPool.Count == 0)
        {
            chosenAction = PlayerActionInfo.PlayerAction.DODGE;
            chosenChunksPool = _dodgePool;

            for (int i = 0; i < _dodgePool.Count; i++)
            {
                if (_dodgePool[i].name == _freeChunk.ObstaclePrefab.name + "(Clone)")
                {
                    obstacleIndex = i;
                    break;
                }
            }

            _actionMoney = beginActionMoney + _freeChunkMoney;
            _geometryMoney = beginGeometryMoney + _freeChunkMoney;

            //If there are no free chunks anymore make a new one
            if (obstacleIndex < 0)
            {
                chosenChunksPool = _freePool;
                chosenChunk = _freeChunk;
                chunkToSpawn = SpawnChunkForPool(_freeChunk, true);
                _freePool.Add(chunkToSpawn);
            }
        }

        if (obstacleIndex >= 0)
        {
            chunkToSpawn = chosenChunksPool[obstacleIndex];
        }

        //Debug.Log(chunkToSpawn);

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

        _chunksInGame.Add(chunkToSpawn);

        //Make the chunk active on the given position
        chunkToSpawn.transform.position = pos;
        chunkToSpawn.SetActive(true);
        chunkToSpawn.transform.GetChild(1).gameObject.SetActive(false);

        _previousChunk = chosenChunk;

        if (_obstaclesSpawned < int.MaxValue)
        {
            _obstaclesSpawned++;
        }       

        if (_obstaclesSpawned < _startingChunks)
        {
            _startingExtraOffset += _previousChunk.ExtraOffset;
            transform.position = new Vector3(transform.position.x, transform.position.y, _obstaclesSpawned * _offset + _startingExtraOffset + _startingOffset);
            GenerateChunk();
        }
    }

    private ObstacleChunkInfo ChooseChunk(List<ObstacleChunkInfo> obstacleChunkInfos)
    {
        //If the generator has no money then use a free chunk
        if (_geometryMoney < _minimumChunkCost)
        {
            return _freeChunk;
        }

        List<ObstacleChunkInfo> modifiedObstacleChunkInfos = new List<ObstacleChunkInfo>();
        _modChunks = modifiedObstacleChunkInfos;
        modifiedObstacleChunkInfos.AddRange(obstacleChunkInfos);

        //If the previous chunk is availbe then there's a high chance it will be removed as a choice this time
        if (modifiedObstacleChunkInfos.Contains(_previousChunk))
        {
            if (Random.Range(0, 1) > _likelinessOfChunkRecurring)
            {
                modifiedObstacleChunkInfos.Remove(_previousChunk);
            }
        }

        //Remove all chunks that are impossible to clear based on the previous chunk
        if (_lastEndPosition.Count > 0)
        {
            List<ObstacleChunkInfo> incompatibleChunks = new List<ObstacleChunkInfo>();

            foreach (ObstacleChunkInfo chunk in modifiedObstacleChunkInfos)
            {
                List<ObstacleChunkInfo.CarGoalPosition> incompatibleLanes = new List<ObstacleChunkInfo.CarGoalPosition>();

                for (int i = 0; i < _lastEndPosition.Count; i++)
                {
                    int enumIndex = (int)_lastEndPosition[i];
                    bool compatible = false;

                    for (int j = 0; j < chunk.StartPositions.Count; j++)
                    {
                        //if there is 1 possibility then it's possible so it may be used
                        if (Mathf.Abs(enumIndex - (int)chunk.StartPositions[j]) <= chunk.AllowedPreviousChunkEndAndStartPosDifference)
                        {
                            compatible = true;
                            break;
                        }
                    }

                    if (!compatible)
                    {
                        incompatibleLanes.Add(_lastEndPosition[i]);
                    }
                }

                //If there is no way too clear the chunk based on the previous then remove it
                if (incompatibleLanes.Count >= _lastEndPosition.Count)
                {
                    incompatibleChunks.Add(chunk);
                }
            }

            //Remove all incompatible chunks
            foreach (ObstacleChunkInfo item in incompatibleChunks)
            {
                modifiedObstacleChunkInfos.Remove(item);
            }
        }

        //If all chunks are impossible then choose a free chunk
        if (modifiedObstacleChunkInfos.Count == 0)
        {
            return _freeChunk;
        }

        ObstacleChunkInfo chosenChunk = modifiedObstacleChunkInfos[Random.Range(0, modifiedObstacleChunkInfos.Count)];

        float cost = chosenChunk.GeometryCost;

        //If the random chunk's cost is too high for the current money then find another chunk until there's enough money
        while (cost > _geometryMoney)
        {
            int randResult = Random.Range(0, modifiedObstacleChunkInfos.Count);

            chosenChunk = modifiedObstacleChunkInfos[randResult];
            cost = chosenChunk.GeometryCost;

            if (cost > _geometryMoney)
            {
                modifiedObstacleChunkInfos.Remove(chosenChunk);
            }

            //If every chunk costs too much then choose a chunk from the dodge theme (cost generally less)
            if (modifiedObstacleChunkInfos.Count == 0)
            {
                modifiedObstacleChunkInfos.Clear();
                modifiedObstacleChunkInfos.AddRange(_dodgeChunks);
            }
        }

        _geometryMoney -= cost;

        return chosenChunk;
    }

    //Choose list of chunks based on the given action theme
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
