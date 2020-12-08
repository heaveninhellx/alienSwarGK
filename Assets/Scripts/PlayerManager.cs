using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    GameObject[] lights;
    public float lightsSwitch = 4f;
    bool changeLights = true;

    public GameObject inputField;
    public Button ok;
    string userName;

    GameObject[] enemies;
    public GameObject enemyPrefab;
    public int fibonacciA = 5;
    public int fibonacciB = 8;
    public int wave = 1;
    public float alreadySpawned = 0f;
    public float spawnRate = 1f;
    int[] weights= new int[2];

    public Text text;
    Color originalColor;
    public float fadeOutTime = 3f;

    private GameObject enemy;
    private float nextTimeToSpawn = 0f;

    public GameObject player;
    public float restartDelay = 1f;

    void Start()
    {
        lights = GameObject.FindGameObjectsWithTag("Light");
        text.text = "Wave " + wave;
        originalColor = text.color;
        weights[1] = fibonacciA;
        weights[0] = fibonacciB;
        StartCoroutine(FadeOutRoutine());
    }
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("AI");
        if (alreadySpawned < fibonacciA && Time.time >= nextTimeToSpawn)
        {
            Spawn();
        }
        if (alreadySpawned == fibonacciA && enemies.Length == 0)
        {
            wave++;
            text.text = "Wave " + wave;
            StartCoroutine(FadeOutRoutine());
            int temp = fibonacciA;
            fibonacciA = fibonacciB;
            fibonacciB += temp;
            weights[1] = fibonacciA;
            weights[0] = fibonacciB;
            alreadySpawned = 0;
        }
        if(changeLights)
        StartCoroutine(manageLights());
    }
    IEnumerator manageLights()
    {
        changeLights = false;
        foreach (GameObject go in lights) {
            int id = GetRandomWeightedIndex(weights);
            if (id == 1)
                go.GetComponent<Light>().intensity = 3f;
            else
                go.GetComponent<Light>().intensity = 0f;
                }
        yield return new WaitForSeconds(lightsSwitch);
        changeLights = true;
    }
    int GetRandomWeightedIndex(int[] weights)
    {
        int weightSum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }

        int index = 0;
        int lastIndex = weights.Length - 1;
        while (index < lastIndex)
        {
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }

            weightSum -= weights[index++];
        }

        return index;
    }

    void Spawn()
    {
        int randomChildIdx = Random.Range(0, transform.childCount);
        Transform randomChild = transform.GetChild(randomChildIdx);

        nextTimeToSpawn = Time.time + 1f / spawnRate;
        enemy = Instantiate(enemyPrefab, randomChild.position, Quaternion.identity);
        Debug.Log(randomChild.localPosition.x + " " + randomChild.localPosition.y + " " + randomChild.localPosition.z);
        float angle = Random.Range(0, 360);
        enemy.transform.Rotate(0, angle, 0);
        alreadySpawned++;
    }
    IEnumerator FadeOutRoutine()
    {
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
    }

    public void GameOver()
    {        
        Invoke("EndScreen", restartDelay);        
    }
    public void StoreName()
    {
        userName = inputField.GetComponent<Text>().text;
        if (userName.Length>0)
            AddHighscoreEntry(wave - 1, inputField.GetComponent<Text>().text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    void EndScreen()
    {
        text.text = "You died, input your name to save your score";
        text.color = originalColor;
        ok.gameObject.SetActive(true);
        inputField.transform.parent.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;       
    }

    public void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            highscores = new Highscores()
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        highscores.highscoreEntryList.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }


    [System.Serializable]
    public class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
