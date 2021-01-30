using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private float speed;
    private float rotationSpeed;
    private int index;
    public bool isPlayable;
    private bool endReplay;

    List<PointInTime> pointsInTime;
    GameManager gameManager;
    public SpriteRenderer carSprite;

    private void Awake()
    {
        carSprite = gameObject.GetComponent<SpriteRenderer>();
        endReplay = false;
        isPlayable = false;
        index = 0;
        speed = 2f;
        rotationSpeed = 5f;
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        pointsInTime = new List<PointInTime>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    void Update()
    {
        if (gameManager.isTouched)
        {
            if (isPlayable)
            {
                rigidbody2D.velocity = transform.up * speed;
                Drive();
                Record();
            }
            else
            {
                if (!endReplay)
                {
                    Replay();
                }
            }
        }
    }
    private void Drive()
    {
        //Movement
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x > Screen.width / 2)
            {
                rigidbody2D.rotation -= rotationSpeed;
            }
            else if (touch.position.x < Screen.width / 2)
            {

                rigidbody2D.rotation += rotationSpeed;
            }
        }
    }

    //Replay car Position and rotation
    void Replay()
    {
        if (pointsInTime.Count > 0)
        {
            if (pointsInTime.Count <= index)
            {
                endReplay = true;
                rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
                PointInTime pointInTime = pointsInTime[index];
                transform.position = pointInTime.position;
                transform.rotation = pointInTime.rotation;
                index += 1;
            }
        }
    }
    public void RestartCar(int carNum)
    {
        index = 0;
        if (gameManager.playableCar == carNum)
        {
            pointsInTime.Clear();
        }
        endReplay = false;
        rigidbody2D.velocity = Vector2.zero;
    }

    //Record car Position and rotation
    void Record()
    {
        pointsInTime.Add(new PointInTime(transform.position, transform.rotation));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building") || collision.CompareTag("Player"))
        {
            gameManager.LoadCheckPoint();
        }
        else if (collision.CompareTag("Finish"))
        {
            if (isPlayable)
            {
                gameManager.Next();
            }
        }
    }

}
