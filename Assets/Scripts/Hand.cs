using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private List<Card> hand;
    public int count = 0;
    private List<GameObject> sprite_list;
    public GameObject card;
    public bool busted;
    public int win_condition = 0;
    public bool dealer;
    public bool doubled_down = false;
    public bool soft = false;
    public bool splitable = false;


    // Start is called before the first frame update
    void Awake()
    {
        hand = new List<Card>();
        sprite_list = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void add_to_hand(Card card, bool doubled = false)
    {
        hand.Add(card);
        count_hand();
        if (dealer && hand.Count == 1)
        {
            dealer_hand_to_sprite(card);
        }
        else
        {
            hand_to_sprite(card, doubled);
        }
    }

    public Card pop_hand()
    {
        Card last = hand.Last();
        hand.RemoveAt(1);
        Destroy(sprite_list.Last());
        return last;
    }

    void count_hand()
    {
        count = 0;
        int ace_count = 0;
        soft = false;
        foreach (Card card in hand)
        {
            if (card.Value >= 10 && card.Value < 14)
            {
                count += 10;
            }
            else if (card.Value == 14)
            {
                ace_count++;
            }
            else
            {
                count += card.Value;
            }
        }
        // Counts Aces last
        for (int i = 0; i < ace_count; i++)
        {
            if ((count + ace_count) > 11)
            {
                count++;
            }
            else if (count > 10)
            {
                count += 1;
            }
            else
            {
                count += 11;
                soft = true;
            }
        }
        if (count > 21)
        {
            busted = true;
        }
        if (hand.Count == 2 && value_transform(hand[0].Value) == value_transform(hand[1].Value))
        {
            splitable = true;
        }
        else
        {
            splitable = false;
        }
       
    }

    public void Clear()
    {
        hand.Clear();
        foreach (GameObject card in sprite_list)
        {
            Destroy(card);
        }
        sprite_list.Clear();
        busted = false;
        win_condition = 0;
    }

    void hand_to_sprite(Card top_card, bool doubled = false)
    {
        Quaternion rotation = transform.rotation;
        float offset = (hand.Count - 1) * 0.4f;
        if (doubled)
        {
            rotation = Quaternion.Euler(0, 0, 90f);
            GameObject doubled_card = Instantiate(card, new Vector3(transform.position.x + offset, transform.position.y - offset + 20, 0), rotation, transform);
            doubled_card.GetComponent<attributes>().cards = top_card;
            doubled_card.GetComponent<attributes>().initialize();
            doubled_card.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(top_card.Name);
            doubled_card.GetComponent<SpriteRenderer>().sortingOrder = (hand.Count - 1);
            sprite_list.Add(doubled_card);
        }
        else
        {
            GameObject current_card = Instantiate(card, new Vector3(transform.position.x + offset, transform.position.y + offset + 20, 0), rotation, transform);
            current_card.GetComponent<attributes>().cards = top_card;
            current_card.GetComponent<attributes>().initialize();
            current_card.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(top_card.Name);
            current_card.GetComponent<SpriteRenderer>().sortingOrder = (hand.Count - 1);
            sprite_list.Add(current_card);
        }

    }

    void dealer_hand_to_sprite(Card top_card)
    {
        float offset = (hand.Count - 1) * 0.4f;
        GameObject current_card = Instantiate(card, new Vector3(transform.position.x + offset, transform.position.y + offset + 20, 0), transform.rotation, transform);
        current_card.GetComponent<attributes>().cards = top_card;
        current_card.GetComponent<attributes>().initialize();
        current_card.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("card_back");
        current_card.GetComponent<SpriteRenderer>().sortingOrder = -1;
        sprite_list.Add(current_card);
    }

    public void dealer_reveal()
    {
        GameObject flipped_card = sprite_list[0];
        flipped_card.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flipped_card.GetComponent<attributes>().cards.Name);
        flipped_card.GetComponent<SpriteRenderer>().sortingLayerID = 0;

    }

    public List<Card> get_hand()
    {
        return hand;
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