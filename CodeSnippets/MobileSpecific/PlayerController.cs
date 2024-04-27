using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    GameObject[] fruits;
    GameObject bomb;
    public GameObject splitFruit;
    public Vector3 playerPosition;
    public AudioSource audioSource;
    public AudioClip sliceSound;
    public AudioClip bombSound;

    GameManager gmInfo;

    // Start is called before the first frame update
    void Start()
    {
        gmInfo = GetComponent<GameManager>();
    }



    // Update is called once per frame
    void Update()
    {
        fruits = GameObject.FindGameObjectsWithTag("Fruit");

        for (int i = 0; i < Input.touchCount; i++)
        {
            
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                playerPosition = touchPosition;
                GameObject nearestFruitToTouchPosition = FindNearestFruitToTouchPosition(touchPosition);

                if (nearestFruitToTouchPosition != null && Time.timeScale == 1)
                {
                    
                    if(nearestFruitToTouchPosition.name != "Bomb(Clone)")
                    {
                        
                        Destroy(nearestFruitToTouchPosition);
                        Instantiate(splitFruit, gmInfo.SpawnLocation(), Quaternion.identity);
                        audioSource.PlayOneShot(sliceSound);
                        GameManager.score++;
                    }else if(nearestFruitToTouchPosition.name == "Bomb(Clone)")
                    {
                      
                        Destroy(nearestFruitToTouchPosition);
                        GameManager.lives--;
                        audioSource.PlayOneShot(bombSound);
                    }

                }

            }
            if(touch.phase == TouchPhase.Began && GameManager.gameIsOver == true)
            {
                GameManager.gameIsOver = false;
                SceneManager.LoadScene(0);
            }

        }

    }

    private GameObject FindNearestFruitToTouchPosition(Vector3 touchPosition)
    {
        GameObject nearest = null;
        float distance = .75f;

        for (int i = 0; i < fruits.Length; i++)
        {
            Transform fruitPos = fruits[i].transform;
            float candidateDistance = Vector3.Distance(touchPosition, fruitPos.position);
            if (candidateDistance < distance)
            {
                nearest = fruits[i].gameObject;
                //distance = candidateDistance;
            }
        }

        return nearest;
    }
}
