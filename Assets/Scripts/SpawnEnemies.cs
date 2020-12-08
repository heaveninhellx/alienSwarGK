using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemies : MonoBehaviour
{
    GameObject[] enemies;
    public GameObject enemyPrefab;
    public float fibonacciA = 5;
    public float fibonacciB = 8;
    public float wave = 1;
    public float alreadySpawned = 0f;
    public float spawnRate = 1f;

    public Text text;
    Color originalColor; 
    public float fadeOutTime= 3f;

    private GameObject enemy;
    private float nextTimeToSpawn = 0f;

    void Start()
    {
        text.text = "Fala " + wave;
        originalColor = text.color;
        StartCoroutine(FadeOutRoutine());
    }
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("AI");
        if (alreadySpawned<fibonacciA && Time.time >= nextTimeToSpawn)
        {
            Spawn();
        }        
        if(alreadySpawned==fibonacciA && enemies.Length == 0)
        {
            wave++;
            text.text = "Fala " + wave;
            StartCoroutine(FadeOutRoutine());
            float temp = fibonacciA;
            fibonacciA = fibonacciB;
            fibonacciB += temp;
            alreadySpawned = 0;
        }
    }
    void Spawn()
    {
        int randomChildIdx = Random.Range(0, transform.childCount);
        Transform randomChild = transform.GetChild(randomChildIdx);

        nextTimeToSpawn = Time.time + 1f / spawnRate;
        enemy = Instantiate(enemyPrefab, randomChild.position, Quaternion.identity);
        Debug.Log(randomChild.localPosition.x+" "+ randomChild.localPosition.y +" "+ randomChild.localPosition.z);
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
}