using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextResponse : MonoBehaviour
{
    public TextMeshProUGUI ChangeColorObject;
    public TextMeshProUGUI playerText;

    private bool isSelected = false;

    public Relationship relationship;

    public int response;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isSelected == false && relationship.startTime == true && relationship.phraseCount >0)
            {
                isSelected = true;
                ChangeColorObject.color = Color.yellow;
                playerText.text = playerText.text + " " + ChangeColorObject.text;
                relationship.companionRelationship += response;
                relationship.phraseCount -= 1;
            }
        }
    }
}
