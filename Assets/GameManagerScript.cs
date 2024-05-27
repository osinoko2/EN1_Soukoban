using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == null)
                {
                    continue;
                }
                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        if (map[moveTo.y, moveTo.x] == 4){ return false; }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber("Box", moveTo, moveTo + velocity);
            if (!success) { return false; }
        }
        //field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        Vector3 moveToPosition = new Vector3(
            moveTo.x, field.GetLength(0) - moveTo.y, 0
        );
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo( moveToPosition );
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        Instantiate(
                        ParticlePrefab,
                        new Vector3(moveFrom.x, map.GetLength(0) - moveFrom.y, 0),
                        Quaternion.identity
                    );
        Instantiate(
                        ParticlePrefab,
                        new Vector3(moveFrom.x, map.GetLength(0) - moveFrom.y, 0),
                        Quaternion.identity
                    );
        Instantiate(
                        ParticlePrefab,
                        new Vector3(moveFrom.x, map.GetLength(0) - moveFrom.y, 0),
                        Quaternion.identity
                    );
        return true;
    }

    bool IsCleard()
    {
        // Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                // 格納場所か否かを判断
                if (map[y, x] == 3)
                {
                    // 格納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box") {
                // 一つでも箱が無かったら条件未達成
                return false;
            }
        }
        // 条件未達成でなければ条件達成
        return true;
    }

    public GameObject playerPrefab;
    public GameObject BoxPrefab;
    public GameObject goalPrefab;
    public GameObject clearText;
    public GameObject ParticlePrefab;
    public GameObject NotMoveBoxPrefab;
    int[,] map;
    GameObject[,] field;

    void Start()
    {
        Screen.SetResolution(1280, 720, false);
        // 配列の実態の作成と初期化
        map = new int[,] {
            { 0, 0, 4, 0, 0 },
            { 0, 3, 1, 3, 0 },
            { 0, 0, 2, 4, 0 },
            { 0, 2, 3, 2, 0 },
            { 0, 0, 0, 0, 0 },
        };
        field = new GameObject[
            map.GetLength(0),
            map.GetLength(1)
        ];

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++) {
                //debugText += map[y, x].ToString() + ",";
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                    );
                }

                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        BoxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                    );
                }

                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(
                        goalPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                    );
                }

                if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(
                        NotMoveBoxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                    );
                }
            }
            //debugText += "\n";
        }
        //Debug.Log(debugText);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Box", playerIndex, playerIndex + new Vector2Int(1, 0));
            // もしクリアしていたら
            if (IsCleard())
            {
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Box", playerIndex, playerIndex + new Vector2Int(-1, 0));
            // もしクリアしていたら
            if (IsCleard())
            {
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Box", playerIndex, playerIndex + new Vector2Int(0, -1));
            // もしクリアしていたら
            if (IsCleard())
            {
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Box", playerIndex, playerIndex + new Vector2Int(0, 1));
            // もしクリアしていたら
            if (IsCleard())
            {
                clearText.SetActive(true);
            }
        }
    }
}
