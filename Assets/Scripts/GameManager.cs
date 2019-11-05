using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject slider;
    private GameObject canvas;
    private Text playAgainText;
    public float sliderSpeed = 2.0f;
    private float previousPlankPosX1;
    private float previousPlankPosX2;
    private bool stopSliding = false;
    private int numPlanks = 1;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("Slider");
        canvas = GameObject.Find("Canvas");
        playAgainText = GameObject.Find("PlayAgainText").GetComponent<Text>();

        canvas.SetActive(false);

        previousPlankPosX1 = 0.00f - slider.transform.localScale.x / 2;
        previousPlankPosX2 = 0.00f - slider.transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver) {
            // Mouse click
            if (Input.GetMouseButtonDown(0))
            {
                stopSliding = true;

                float plankX = slider.transform.position.x;
                float plankX1 = plankX - slider.transform.localScale.x / 2;
                float plankX2 = plankX + slider.transform.localScale.x / 2;
                float scaleX = slider.transform.localScale.x;

                // If plank is outside the range
                if((plankX1 < previousPlankPosX1 && plankX2 < previousPlankPosX1) || (plankX1 > previousPlankPosX2))
                {
                    stopSliding = false;
                    GameOver();
                }
                // If plank fits exactly or If plank is at the left
                else if(plankX - scaleX / 2 <= previousPlankPosX1 + 0.05)
                {
                    float newPlankLength = plankX2 - previousPlankPosX1;
                    float newX = previousPlankPosX1 + newPlankLength / 2;

                    // Crop plank
                    slider.transform.position = new Vector3(newX, slider.transform.position.y, slider.transform.position.z);
                    slider.transform.localScale = new Vector3(newPlankLength, slider.transform.localScale.y, slider.transform.localScale.z);
                    previousPlankPosX1 = newX - newPlankLength / 2;
                    previousPlankPosX2 = newX + newPlankLength / 2;

                    // New plank
                    slider = Instantiate(slider, new Vector3(newX, slider.transform.position.y + 1, slider.transform.position.z), Quaternion.identity);

                    numPlanks += 1;
                    stopSliding = false;
                }
                // If plank is at the right
                else if(plankX2 >= previousPlankPosX2 - 0.05)
                {
                    float newPlankLength = previousPlankPosX2 - plankX1;
                    float newX = plankX1 + newPlankLength / 2;

                    // Crop plank
                    slider.transform.position = new Vector3(newX, slider.transform.position.y, slider.transform.position.z);
                    slider.transform.localScale = new Vector3(newPlankLength, slider.transform.localScale.y, slider.transform.localScale.z);
                    previousPlankPosX1 = newX - newPlankLength / 2;
                    previousPlankPosX2 = newX + newPlankLength / 2;

                    // New plank
                    slider = Instantiate(slider, new Vector3(newX, slider.transform.position.y + 1, slider.transform.position.z), Quaternion.identity);
                    
                    numPlanks += 1;
                    stopSliding = false;
                }

                if(numPlanks > 4)
                {
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 1, Camera.main.transform.position.z);
                }
            }
            // Move
            else {
                if(!stopSliding) {
                    float newX  = slider.transform.position.x - (sliderSpeed * Time.deltaTime);
                    slider.transform.position = new Vector3(newX, slider.transform.position.y, slider.transform.position.z);

                    if(slider.transform.position.x - slider.transform.localScale.x / 2 <= -10  || slider.transform.position.x + slider.transform.localScale.x / 2 >= 10) 
                    {
                        sliderSpeed *= -1;
                    }
                }
            }
        }
        
    }

    void GameOver()
    {
        isGameOver = true;
        canvas.SetActive(true);
    }
}
