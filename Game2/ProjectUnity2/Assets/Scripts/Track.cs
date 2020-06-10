using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{

    public GameObject[] obstacles; //colocando como vetor pois ira ter mais de um objeto
    public Vector2 numberOfObstacles; //armazenar valor minimo e maximo(quantidade de obstaculos)
    public GameObject coin;
    public Vector2 numberOfCoins;

    public List<GameObject> newObstacles;
    public List<GameObject> newCoins;

    void Start() {
        int newNumberOfObstacles = (int)Random.Range(numberOfObstacles.x, numberOfObstacles.y); //sorteando valores
        int newNumberOfCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);

        for (int i = 0; i < newNumberOfObstacles; i++) {
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));//instanciando os prefabs(obstaculos)
            newObstacles[i].SetActive(false);//deixando desativado de inicio
        }

        for (int i = 0; i < newNumberOfCoins; i++)  {
            newCoins.Add(Instantiate(coin, transform));
            newCoins[i].SetActive(false);
        }

        PlaceObstacles();
        PlaceCoins();
    }

    void PlaceObstacles() {
        for (int i = 0; i < newObstacles.Count; i++) {
            float posZMin = (292f / newObstacles.Count) + (297f / newObstacles.Count) * i; //posicionando os objetos atraves do tamanho da pista(posição minima)
            float posZMax = (292f / newObstacles.Count) + (297f / newObstacles.Count) * i + 1; //posicionando os objetos atraves do tamanho da pista(posição maxima)
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax)); //posicionando em lugares randomicos em z
            newObstacles[i].SetActive(true); //ativando os obstaculos
            if (newObstacles[i].GetComponent<ChangeLane>() != null)
                newObstacles[i].GetComponent<ChangeLane>().PositionLane();
        }
    }

    void PlaceCoins() {
        float minZPos = 10f; //Distancia minima para posicionar a moeda

        for (int i = 0; i < newCoins.Count; i++) {
            float maxZPos = minZPos + 5f; //Distancia maxima para posicionar a moeda // TODO -----------------
            float randomZPos = Random.Range(minZPos, maxZPos);
            newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos); //Posicionando em lugares randomicos em z
            newCoins[i].SetActive(true); //Ativando a moeda
            newCoins[i].GetComponent<ChangeLane>().PositionLane(); //Todo coin vai ter um componente ChangeLane que é responsável por escolher uma das 3 lanes para posicioná-lo.
            minZPos = randomZPos + 1; //A proxima moeda vai ter pelo menos 1 de distancia em Z da moeda anterior
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {//se colidir com a tag player
            other.GetComponent<Player>().IncreaseSpeed();
            transform.position = new Vector3(0, 0, transform.position.z + 292 * 2); //alterando apenas a profundidade, pegando a posição atual + tamanho da pista * 2, quando chegar ao final da pista vai pegar a segunda pista e colocar como a atual
            PlaceObstacles();
            PlaceCoins();
        }
    }

}
