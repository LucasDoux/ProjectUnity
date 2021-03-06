﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

    #region Variables

    public GameObject[] obstacles; //colocando como vetor pois ira ter mais de um objeto
    public Vector2 numberOfObstacles; //armazenar valor minimo e maximo(quantidade de obstaculos)
    public GameObject[] coins;
    public Vector2 numberOfCoins;

    public List<GameObject> newObstacles;
    public List<GameObject> newCoins;

    #endregion

    #region Unity Events

    void Start() {
        int newNumberOfObstacles = (int)Random.Range(numberOfObstacles.x, numberOfObstacles.y); //sorteando valores
        int newNumberOfCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);
        int aux = 0;

        for (int i = 0; i < newNumberOfObstacles; i++) { 
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform)); //instanciando os prefabs(obstaculos)
            newObstacles[i].SetActive(false); //deixando desativado de inicio
        }

        for (int i = 0; i < newNumberOfCoins; i++)  {
            //newCoins.Add(Instantiate(coin, transform));
            aux = Random.Range(1, 101);

            if (aux > 2) {
                newCoins.Add(Instantiate(coins[Random.Range(0, coins.Length - 1)], transform));
            } else {
                newCoins.Add(Instantiate(coins[coins.Length - 1], transform)); //Mask
            }

            newCoins[i].SetActive(false);
        }

        PlaceObstacles();
        PlaceCoins();
    }

    #endregion

    #region Functions

    void PlaceObstacles() {
        for (int i = 0; i < newObstacles.Count; i++) {
            float posZMin = (292f / newObstacles.Count) + (292f / newObstacles.Count) * i; //posicionando os objetos atraves do tamanho da pista(posição minima)
            float posZMax = (292f / newObstacles.Count) + (292f / newObstacles.Count) * i + 1; //posicionando os objetos atraves do tamanho da pista(posição maxima)

            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax)); //posicionando em lugares randomicos em z
            newObstacles[i].SetActive(true); //ativando os obstaculos

            if (newObstacles[i].GetComponent<ChangeLane>() != null)
                newObstacles[i].GetComponent<ChangeLane>().PositionLane();
        }
    }

    void PlaceCoins() {
        /*
        float minZPos = 10f; //Distancia minima para posicionar a moeda

        for (int i = 0; i < newCoins.Count; i++) {
            float maxZPos = minZPos + 10f; //Distancia maxima para posicionar a moeda // TODO -----------------
            float randomZPos = Random.Range(minZPos, maxZPos);

            newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos); //Posicionando em lugares randomicos em z
            newCoins[i].SetActive(true); //Ativando a moeda
            newCoins[i].GetComponent<ChangeLane>().PositionLane(); //Todo coin vai ter um componente ChangeLane que é responsável por escolher uma das 3 lanes para posicioná-lo.

            minZPos = randomZPos + 7.5f; //A proxima moeda vai ter pelo menos X de distancia em Z da moeda anterior
        }
        */
        int aux = 0;

        for (int i = 0; i < newCoins.Count; i++) {
            float posZMin = (292f / newCoins.Count) + (292f / newCoins.Count) * i; //posicionando os objetos atraves do tamanho da pista(posição minima)
            float posZMax = (292f / newCoins.Count) + (292f / newCoins.Count) * i + 1; //posicionando os objetos atraves do tamanho da pista(posição maxima)

            aux = Random.Range(-2, 3);
            newCoins[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax) + aux); //posicionando em lugares randomicos em z
            newCoins[i].SetActive(true); //ativando os obstaculos

            if (newCoins[i].GetComponent<ChangeLane>() != null)
                newCoins[i].GetComponent<ChangeLane>().PositionLane();
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

    #endregion
}
