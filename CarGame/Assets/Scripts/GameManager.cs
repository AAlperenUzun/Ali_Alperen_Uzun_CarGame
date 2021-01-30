using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startPosition;
    public GameObject endPosition;
    public GameObject car;
    public int playableCar;
    public bool isTouched;
    public GameObject finish;
    private void Awake()
    {
        isTouched = false;
        playableCar = 0;
        car.transform.GetChild(playableCar).gameObject.SetActive(true);
        car.transform.GetChild(playableCar).transform.position = startPosition.transform.GetChild(playableCar).transform.position;
        car.transform.GetChild(playableCar).transform.rotation = startPosition.transform.GetChild(playableCar).transform.rotation;
        car.transform.GetChild(playableCar).GetComponent<CarScript>().isPlayable = true;

        finish.transform.position = endPosition.transform.GetChild(playableCar).transform.position;
    }

    public void LoadCheckPoint()
    {
        for (int i = 0; i <= playableCar; i++)
        {
            isTouched = false;
            car.transform.GetChild(i).transform.position = startPosition.transform.GetChild(i).transform.position;
            car.transform.GetChild(i).transform.rotation = startPosition.transform.GetChild(i).transform.rotation;
            car.transform.GetChild(i).GetComponent<CarScript>().RestartCar(i);
        }
    }
    private void Update()
    {
        if (!isTouched && Input.touchCount > 0)
        {
            isTouched = true;
        }
    }

    public void Next()
    {
        playableCar += 1;
        if (playableCar > 7)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            finish.transform.position = endPosition.transform.GetChild(playableCar).transform.position;
            finish.transform.rotation = endPosition.transform.GetChild(playableCar).transform.rotation;
            car.transform.GetChild(playableCar).gameObject.SetActive(true);
            LoadCheckPoint();
            car.transform.GetChild(playableCar).GetComponent<CarScript>().isPlayable = true;
            car.transform.GetChild(playableCar - 1).GetComponent<CarScript>().isPlayable = false;
            car.transform.GetChild(playableCar - 1).GetComponent<CarScript>().carSprite.color = Color.red;
        }
    }
}
