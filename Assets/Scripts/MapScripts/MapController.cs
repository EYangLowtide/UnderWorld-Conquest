using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    //public Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    //PlayerMovement pm;
    Vector3 playerLastPos;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist; //Must be greater than the length and width of the tilemap
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDur;



    void Start()
    {
        //pm = FindObjectOfType<PlayerMovement>();
        playerLastPos = player.transform.position;
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimzer();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }
        /*
        if (pm.moveDir.x > 0 && pm.moveDir.y == 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;  //Right
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;    //Left
                SpawnChunk();
            }
        }
        else if (pm.moveDir.y > 0 && pm.moveDir.x == 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position; //Up
                SpawnChunk();
            }
        }
        else if (pm.moveDir.y < 0 && pm.moveDir.x == 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;    //Down
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y > 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Up").position;   //Right up
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y < 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Down").position;  //Right down
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y > 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Up").position;  //Left up
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y < 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Down").position; //Left down
                SpawnChunk();
            }
        }*/

        Vector3 moveDir = player.transform.position - playerLastPos;
        playerLastPos = player.transform.position;

        string directionName = GetDirectionName(moveDir);
        /* 
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(directionName).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(directionName).position);

            //check for unspawned chunks
            if (directionName.Contains("Up") && directionName.Contains("Right"))
            {
                if (Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Up").position);
                }
                if (Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Right").position);
                }
            }
            else if (directionName.Contains("Up") && directionName.Contains("Left"))
            {
                if (Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Up").position);
                }
                if (Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Left").position);
                }
            } 
            else if (directionName.Contains("Down") && directionName.Contains("Right"))
            {
                if (Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Down").position);
                }
                if (Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Right").position);
                }
            }
            else if (directionName.Contains("Down") && directionName.Contains("Left"))
            {
                if (Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Down").position);
                }
                if (Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
                {
                    SpawnChunk(currentChunk.transform.Find("Left").position);
                }
            }
        }*/
        //to get rid of repetetivness
        CheckAndSpawnChunk(directionName);
        
        //to fix diagonal chucks not spawning(incase)
        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
        }
        if (directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
        }
        if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
        }
        if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
        }
    }

    void CheckAndSpawnChunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(direction).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(direction).position);
        }
    }

    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;

        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //horizonatl mov
            if(direction.y > 0.5f)
            {
                //also mov up
                return direction.x > 0 ? "RightUp" : "LeftUp";
            }
            else if (direction.y < -0.5f)
            {
                //also mov down
                return direction.x > 0 ? "RightDown" : "LeftDown";
            }
            else
            {
                //mov horz
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            //vertical mov
            if (direction.x > 0.5f)
            {
                //also mov right
                return direction.y > 0 ? "RightUp" : "RightDown";
            }
            else if (direction.x < -0.5f)
            {
                //also mov left
                return direction.y > 0 ? "LeftUp" : "LeftDown";
            }
            else
            {
                //also mov up
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPos)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], spawnPos, Quaternion.identity); //was noTerrainPosition
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimzer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDur;   //Check every 1 second to save cost, change this value to lower to check more times
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
