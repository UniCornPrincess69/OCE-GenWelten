using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private enum ERoomType
    {
        Free = -1,
        Room
    }

    private const int MAP_WIDTH = 9;
    private const int MAP_HEIGHT = 8;

    private int[,] map;


    private void Awake()
    {
        InitMap();
        GenerateMap(1);
        Debug.Log(GetMapLog());
    }

    private void InitMap()
    {
        map = new int[MAP_WIDTH, MAP_HEIGHT];

        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                map[x, y] = -1;
            }
        }
    }

    private void GenerateMap(int levelIdx)
    {
        int neededRoomCount = (int)(5 + levelIdx * 2.6f + Random.Range(0, 2));

        int iterations = 0;
        int maxIterations = 3000;

        bool isMapValid = false;
        while (!isMapValid && iterations < maxIterations)
        {
            InitMap();
            GenerateLevel(neededRoomCount);
            isMapValid = ValidateLevel(neededRoomCount);
            iterations++;
        }
    }

    private int GenerateLevel(int neededRoomCount)
    {
        Vector2Int startPos = new Vector2Int(MAP_WIDTH, MAP_HEIGHT) / 2;
        map[startPos.x, startPos.y] = (int)ERoomType.Room;

        int currentRoomCount = 1;
        Queue<Vector2Int> positionsToExpand = new Queue<Vector2Int>();
        positionsToExpand.Enqueue(startPos);

        while (positionsToExpand.Count > 0)
        {
            Vector2Int positionToExpand = positionsToExpand.Dequeue();

            Vector2Int[] positionsToCheck = new Vector2Int[]
            {
                positionToExpand + Vector2Int.up,
                positionToExpand + Vector2Int.down,
                positionToExpand + Vector2Int.left,
                positionToExpand + Vector2Int.right,
            };

            for (int i = 0; i < positionsToCheck.Length; i++)
            {
                Vector2Int toCheck = positionsToCheck[i];

                if (IsPositionInBounds(toCheck))
                {
                    //Check 1: Wenn alle Räume genertiert sind -> Do nothing
                    if (currentRoomCount >= neededRoomCount) continue;

                    //Check 2: Wenn ein Raum vorhanden ist -> Do nothing
                    if (map[toCheck.x, toCheck.y] != (int)ERoomType.Free) continue;

                    //Check 3: Coinflip -> 50% Chance -> Do nothing
                    float rngPercent = Random.Range(0f, 1f);
                    if (rngPercent > 0.5f) continue;

                    //Check 4: Wenn bereits mehr oder gleich 2 Nachbarn da sind -> Do nothing
                    int neighbourCount = GetNeighbourCount(toCheck);
                    if (neighbourCount >= 2) continue;

                    //Andernfalls -> gnerate room

                    map[toCheck.x, toCheck.y] = (int)ERoomType.Room;
                    positionsToExpand.Enqueue(toCheck);
                    currentRoomCount++;
                }
            }
        }

        return currentRoomCount;
    }

    private bool ValidateLevel(int neededRoomCount)
    {
        int count = 0;
        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                if (map[x, y] != (int)ERoomType.Free) count++;
            }
        }
        return count == neededRoomCount;
    }

    private int GetNeighbourCount(Vector2Int position)
    {
        int count = 0;

        Vector2Int[] positionsToSearch = new Vector2Int[]
        {
                position + Vector2Int.up,
                position + Vector2Int.down,
                position + Vector2Int.left,
                position + Vector2Int.right,
        };

        for (int i = 0; i < positionsToSearch.Length; i++)
        {
            Vector2Int toSearch = positionsToSearch[i];

            if (IsPositionInBounds(toSearch))
            {
                if (map[toSearch.x, toSearch.y] != (int)ERoomType.Free) count++;
            }
        }

        return count;
    }

    private bool IsPositionInBounds(Vector2Int position)
    {
        return (position.x >= 0 && position.x < MAP_WIDTH && position.y >= 0 && position.y < MAP_HEIGHT);
    }

    private string GetMapLog()
    {
        //var outp = "<color=white>██</color><color=red>████</color><color=white>██</color>\n<color=red>██</color><color=white>████</color><color=green>██</color>";
        //return outp;

        string output = string.Empty;

        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            output += $"|";
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                int num = map[x, y];
                if (num >= 0) output += $" ";

                output += $"{num}|";
            }
            output += $"\n";
        }

        output = output
            .Replace("-1", "<color=white>██</color>") // ERoomType.Free = white
            .Replace(" 0", "<color=green>██</color>");// ERoomType.Room = green

        return output;
    }
}
