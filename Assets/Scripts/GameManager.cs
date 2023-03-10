using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameOver;
    public float gameSpeed;
    public bool isOnGround;
    public int score = 0;
    public bool playing;
    private bool first;
    private bool check;
    private Vector3 roadPos;
    private int count;
    [SerializeField] List<GameObject> cars;
    [SerializeField] List<GameObject> foods;
    [SerializeField] List<GameObject> trees;
    [SerializeField] List<GameObject> buildings;
    [SerializeField] TMPro.TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject openingPanel;
    [SerializeField] GameObject road;
    [SerializeField] GameObject ratel;
    private List<int> roadway = new List<int> { -5, 5};
    private List<int> side = new List<int> { -1, 1 };
    private List<int> buildingSide = new List<int> { -21, 60 };
    public List<GameObject> carList;
    public List<GameObject> foodList;
    public List<GameObject> treeList;
    public List<GameObject> buildingList;

    void Awake()
    {


    }
    void Start()
    {
        roadPos = road.transform.position;
        gameOverPanel.SetActive(false);
        scoreText.enabled = false;
        openingPanel.SetActive(true);
        playing = false;
        check = true;
        first = true;
    }

    public void StartGame()
    {
        openingPanel.SetActive(false);
        scoreText.enabled = true;
        StartCoroutine(SpawnObjects());
        StartCoroutine(SpawnEnvironments());
    }

    void Update()
    {
        if (first && Input.touchCount >= 1)
        {
            StartGame();
            first = false;
        }

        if (check && gameOver)
        {
            GameOver();
            check = false;
        }

        if (gameOver)
        {
            if (Input.touchCount >= 1)
            {
                TryAgain();
            }
        }
        if (playing)
        {
            StartCoroutine(MoveRoad());
        }
    }

    private IEnumerator SpawnObjects()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(Random.Range(2, 4));
            playing = true;

            SpawnCar();

            int selectedRoad;
            int lastSelectedRoad = 0;
            for (int i = 0; i < Random.Range(3, 5); i++)
            {
                if (i == 0)
                {
                    selectedRoad = roadway[Random.Range(0, 2)];
                    lastSelectedRoad = selectedRoad;
                }
                else
                {
                    selectedRoad = lastSelectedRoad;
                }
                SpawnFood(i, selectedRoad);
            }
        }
    }

    private IEnumerator SpawnEnvironments()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(0.1f);

            SpawnBuildings();
            for (int i = 0; i < Random.Range(3, 5); i++)
            {
                SpawnTree();
            }
        }
    }

    private void GameOver()
    {
        gameSpeed = 0;
        stopObject(carList);
        stopObject(foodList);
        stopObject(treeList);
        stopObject(buildingList);

        StopAllCoroutines();

        gameOverPanel.SetActive(true);
    }

    private void stopObject(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            if (obj != null)
            {
                obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                obj.GetComponent<Rigidbody>().drag = 1000;
            }
        }
    }
    private void SpawnCar()
    {
        if (cars != null)
        {
            int carIndex = Random.Range(0, cars.Count);
            int selectedRoad = roadway[Random.Range(0, 2)];
            Vector3 carPosition = new Vector3(selectedRoad, 10, 500);
            GameObject carHolder = Instantiate(cars[carIndex], carPosition, cars[carIndex].transform.rotation);
            if (carList != null)
            {
                carList.Add(carHolder);
            }
            carHolder.GetComponent<Rigidbody>().drag = 0;
            carHolder.GetComponent<Rigidbody>().AddForce(Vector3.back * gameSpeed * 700 * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    private void SpawnFood(int i, int selectedRoad)
    {
        if (foods != null)
        {
            int foodIndex = Random.Range(0, foods.Count);
            Vector3 foodPosition = new Vector3(selectedRoad, 3, 505 + (i*10));
            GameObject foodHolder = Instantiate(foods[foodIndex], foodPosition, foods[foodIndex].transform.rotation);
            if (foodList != null)
            {
                foodList.Add(foodHolder);
            }
            foodHolder.GetComponent<Rigidbody>().drag = 0;
            foodHolder.GetComponent<Rigidbody>().AddForce(Vector3.back * gameSpeed * 500 * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    private void SpawnTree()
    {
        if (trees != null)
        {
            int treeIndex = Random.Range(0, trees.Count);
            int selectedSide = side[Random.Range(0, 2)];
            Vector3 treePosition = new Vector3((selectedSide* Random.Range(25,125)), 1.25f, 485);
            GameObject treeHolder = Instantiate(trees[treeIndex], treePosition, trees[treeIndex].transform.rotation);
            if (treeList != null)
            {
                treeList.Add(treeHolder);
            }
            treeHolder.GetComponent<Rigidbody>().drag = 0;
            treeHolder.GetComponent<Rigidbody>().AddForce(Vector3.back * gameSpeed * 500 * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    private void SpawnBuildings()
    {
        if (buildings != null)
        {
            int buildingIndex = Random.Range(0, buildings.Count);
            int selectedSide = buildingSide[Random.Range(0, 2)];
            Vector3 buildingPosition = new Vector3(selectedSide, 1.375f, 485);
            GameObject buildingHolder = Instantiate(buildings[buildingIndex], buildingPosition, buildings[buildingIndex].transform.rotation);
            if (buildingList != null)
            {
                buildingList.Add(buildingHolder);
            }
            buildingHolder.GetComponent<Rigidbody>().drag = 0;
            buildingHolder.GetComponent<Rigidbody>().AddForce(Vector3.back * gameSpeed * 500 * Time.deltaTime, ForceMode.VelocityChange);
        }
    }


    private IEnumerator MoveRoad()
    {
        yield return new WaitForSeconds(1.1f);
        if (road.transform.position.z <= (roadPos.z - 15.31f))
        {
            road.transform.position = roadPos;
        }
        road.transform.Translate(Vector3.right * gameSpeed * Time.deltaTime);
    }

    public void RefreshScore()
    {
        scoreText.text = "Score = " + score.ToString();
    } 

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SpeedUp()
    {
        count++;
        if (count == 5)
        {
            if (gameSpeed < 25)
            {
                gameSpeed += 0.001f;
            }
            count = 0;
        }
    }
}
