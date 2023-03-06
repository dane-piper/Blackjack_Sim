using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Button_Handler : MonoBehaviour
{
    public GameObject New_Hand;
    public GameObject Split;
    public GameObject Double;
    public GameObject Stand;
    public GameObject Hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player_handler = GameObject.Find("Player_Handler");
        GameObject current_turn = player_handler.GetComponent<Player_Handler>().current_turn;
        //Debug.Log(current_turn.GetComponent<Hand>().get_hand());
        if (current_turn.GetComponent<Hand>().get_hand().Count >= 2)
        {
            if ((current_turn.GetComponent<Hand>().dealer))
            {
                Split.GetComponent<Button>().interactable = false;
                Double.GetComponent<Button>().interactable = false;
                Stand.GetComponent<Button>().interactable = false;
                Hit.GetComponent<Button>().interactable = false;
            }
            else if (current_turn.GetComponent<Hand>().get_hand().Count > 2)
            {
                Stand.GetComponent<Button>().interactable = true;
                Hit.GetComponent<Button>().interactable = true;
                Split.GetComponent<Button>().interactable = false;
                Double.GetComponent<Button>().interactable = false;
            }
            else if (value_transform(current_turn.GetComponent<Hand>().get_hand()[0].Value) != value_transform(current_turn.GetComponent<Hand>().get_hand()[1].Value))
            {
                Double.GetComponent<Button>().interactable = true;
                Stand.GetComponent<Button>().interactable = true;
                Hit.GetComponent<Button>().interactable = true;
                Split.GetComponent<Button>().interactable = false;
            }
            else
            {
                Split.GetComponent<Button>().interactable = true;
                Double.GetComponent<Button>().interactable = true;
                Stand.GetComponent<Button>().interactable = true;
                Hit.GetComponent<Button>().interactable = true;
            }
        }

    }
    private int value_transform(int val)
    {
        if (val > 9 && val < 14)
        {
            return 10;
        }
        else if (val == 14)
        {
            return 11;
        }
        else
        {
            return val;
        }
    }
}
