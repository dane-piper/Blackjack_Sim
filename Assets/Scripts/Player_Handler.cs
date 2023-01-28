using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Player_Handler : MonoBehaviour
{
    public GameObject outcome;
    public TextMeshProUGUI basic_strat;
    public GameObject blackjack_trainer;
    public GameObject dealer;
    public GameObject player;
    public GameObject prefab_player;
    public GameObject card_handler;
    public GameObject current_turn;
    public Card top_card;
    public Queue<GameObject> turn;
    public List<Hand> completed_hands;
    public int player_count = 1;
    public List<GameObject> splits;

    // Start is called before the first frame update
    void Start()
    {
        turn = new Queue<GameObject> ();
        splits = new List<GameObject> ();
        turn.Enqueue(player);
        next_turn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Add_Card(GameObject player, bool doubled = false)
    {
        top_card = card_handler.GetComponent<Card_Handler>().Pop();
        player.GetComponent<Hand>().add_to_hand(top_card, doubled);
    }

    public void initialize_wrapper()
    {
        outcome.GetComponent<TextMeshProUGUI>().enabled = false;
        basic_strat.enabled = false;
        StartCoroutine(initialize());
    }

    IEnumerator initialize()
    {
        turn.Enqueue(player);
        current_turn = turn.Dequeue();
        clear();
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Add_Card(player);
            yield return new WaitForSeconds(0.5f);
            Add_Card(dealer);
        }
    }

    public void hit()
    {
        Add_Card(current_turn);
        if (current_turn.GetComponent<Hand>().count > 21)
        {
            next_turn();

        }
    }

    public void stand()
    {
        blackjack_trainer.GetComponent<blackjack_trainer>().basic_strategy(1);
        next_turn();
    }

    public void double_down()
    {
        Add_Card(current_turn);
        next_turn();
    }

    public void split()
    {
        Transform current_transform = current_turn.transform;
        GameObject new_hand = Instantiate(prefab_player, new Vector3(current_transform.position.x - 1f, current_transform.position.y, 0), current_transform.rotation);
        splits.Add(new_hand);
        current_transform.transform.position = new Vector3(current_transform.position.x + 1f, current_transform.position.y, 0);
        Card second_card = current_turn.GetComponent<Hand>().pop_hand();
        new_hand.GetComponent<Hand>().add_to_hand(second_card);
        Add_Card(current_turn);
        Add_Card(new_hand);
        turn.Enqueue(new_hand);
    }

    public void next_turn()
    {
        if (turn.Count == 0)
        {
            current_turn = dealer;
            dealer_turn();

        }
        else
        {
            current_turn = turn.Dequeue();
            completed_hands.Add(current_turn.GetComponent<Hand>());
        }
        
    }

    void clear()
    {
        player.transform.position = new Vector3(-0.43f, -0.73f, 0);
        player.GetComponent<Hand>().Clear();
        dealer.GetComponent<Hand>().Clear();
        if(splits.Count > 0)
        {
            foreach (GameObject split in splits)
            {
                Destroy(split);
            }
        }

    }

    void dealer_turn()
    {
        dealer.GetComponent<Hand>().dealer_reveal();
        bool busted = is_busted();
        Debug.Log("all cards busted: " + busted.ToString());
        while (dealer.GetComponent<Hand>().count < 17 && !busted)
        {
            hit();
        }
        if (dealer.GetComponent<Hand>().count > 21)
        {
            Debug.Log("Dealer Busts");
        }
        foreach(Hand hand in completed_hands)
        {
            hand.win_condition = Compare(hand);
        }
    }

    int Compare(Hand player_hand)
    {
        if (dealer.GetComponent<Hand>().busted)
        {
            Debug.Log("Count: " + player_hand.count.ToString());
            outcome.GetComponent<TextMeshProUGUI>().text = "Player Wins";
            outcome.GetComponent<TextMeshProUGUI>().enabled = true;
            Debug.Log("Dealer Busts");
            return 1;
        }
        if (player_hand.busted)
        {
            Debug.Log("Count: " + player_hand.count.ToString());
            outcome.GetComponent<TextMeshProUGUI>().text = "Player Loses";
            outcome.GetComponent<TextMeshProUGUI>().enabled = true;
            Debug.Log("Player Busts");
            return 2;
        }
        if (dealer.GetComponent<Hand>().count == player_hand.count)
        {
            Debug.Log("Count: " + player_hand.count.ToString());
            outcome.GetComponent<TextMeshProUGUI>().text = "Push";
            outcome.GetComponent<TextMeshProUGUI>().enabled = true;
            Debug.Log("Push");
            return 3;
        }
        if (dealer.GetComponent<Hand>().count > player_hand.count)
        {
            Debug.Log("Count: " + player_hand.count.ToString());
            outcome.GetComponent<TextMeshProUGUI>().text = "Player Loses";
            outcome.GetComponent<TextMeshProUGUI>().enabled = true;
            Debug.Log("Dealer wins");
            return 2;
        }
        else
        {
            Debug.Log("Count: " + player_hand.count.ToString());
            outcome.GetComponent<TextMeshProUGUI>().text = "Player Wins";
            outcome.GetComponent<TextMeshProUGUI>().enabled = true;
            Debug.Log("Player Wins");
            return 1;
        }
    }

    bool is_busted()
    {
        foreach (Hand player in completed_hands)
        {
            if (!player.busted)
            {
                return false;
            }
        }
        return true;
    }
}
