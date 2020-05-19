using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{

    public GameObject[] obstacles; //colocando como vetor pois ira ter mais de um objeto
    public Vector2 numberOfObstacles; //armazenar valor minimo e maximo(quantidade de obstaculos)

    public List<GameObject> newObstacles;

    void Start()
    {
        int newNumberOfObstacles = (int)Random.Range(numberOfObstacles.x, numberOfObstacles.y); //sorteando valores

        for (int i = 0; i < newNumberOfObstacles; i++) //looping
        {
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));//instanciando os prefabs(obstaculos)
            newObstacles[i].SetActive(false);//deixando desativado de inicio
        }

        PositionateObstacles();
    }

    
    
    void PositionateObstacles() // função para posicionar os obstaculos
    {
        for (int i = 0; i < newObstacles.Count; i++)
        {
            float posZMin = (292f / newObstacles.Count) + (297f / newObstacles.Count) * i; //posicionando os objetos atraves do tamanho da pista(posição minima)
            float posZMax = (292f / newObstacles.Count) + (297f / newObstacles.Count) * i + 1; //posicionando os objetos atraves do tamanho da pista(posição maxima)
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax)); //posicionando em lugares randomicos em z
            newObstacles[i].SetActive(true); //ativando os obstaculos
        }

        

    }


    private void OnTriggerEnter(Collider other) //alterando a posição da pista
    {
        if (other.CompareTag("Player")) //se colidir com a tag player
        {
            transform.position = new Vector3(0, 0, transform.position.z + 292 * 2); //alterando apenas a profundidade, pegando a posição atual + tamanho da pista * 2, quando chegar ao final da pista vai pegar a segunda pista e colocar como a atual
           Invoke("PositionateObstacles",5f);//posicionar os obstaculos na nova pista, invocando um tempo para reorganizar os objetos 5f = 5s
        }
    }

}
