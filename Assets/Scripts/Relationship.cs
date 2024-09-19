using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Relationship : MonoBehaviour
{
    public float timeStart = 30;
    public TextMeshProUGUI timerText;
    public bool startTime = false;

    public TextMeshProUGUI phraseCountText;
    public int phraseCount = 3;

    public GameObject speakCloud;
    private int seconds;

    public int companionRelationship = 0;

    private void Start()
    {
        timerText.text = timeStart.ToString();
        startTime = true;
    }


    private void Update()
    {
        if (timeStart > 1)
        {
            timeStart -= Time.deltaTime;
            seconds = Mathf.FloorToInt(timeStart % 60F);
        }
        timerText.text = "Time: " + seconds.ToString();
        phraseCountText.text = "Phrase count: " + phraseCount.ToString();

        if (timeStart < 1)
        {
            speakCloud.SetActive(true);
            startTime = false;

            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3);

        if (companionRelationship >= 1)
        {
            Debug.Log("LOAD LEVEL 1");
            SceneManager.LoadScene("Level2");
        }
        else if (companionRelationship == 0)
        {
            Debug.Log("LOAD LEVEL 2");
            SceneManager.LoadScene("Level2");
        }
        else if (companionRelationship <0)
        {
            Debug.Log("LOAD LEVEL 3");
            SceneManager.LoadScene("Level3");
        } 
    }

}
