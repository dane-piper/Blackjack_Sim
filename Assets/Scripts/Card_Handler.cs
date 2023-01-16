using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Card_Handler : MonoBehaviour
{
    public int size;
    public List<Card> shoe;
    public Deck deck;
    public int deck_count = 0;
    public TextMeshProUGUI count_text;

    // Start is called before the first frame update

    void Start()
    {
        shoe = new List<Card>();
        deck = new Deck();
        deck.FillDeck();
        Create_Shoe();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Create_Shoe();
        }
    }

    void Create_Shoe()
    {
        for (int i = 0; i < size; i++)
        {
            shoe.AddRange(deck.Cards);
        }
        Shuffle(shoe);
    }

    void Shuffle(List<Card> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public Card Pop()
    {
        Card top_card = shoe[0];
        count(top_card);
        shoe.RemoveAt(0);
        return top_card;
    }

    public void count(Card top)
    {
        if (top.Value > 9)
        {
            deck_count -= 1;
            Debug.Log("Count -1");
        }
        else if ((top.Value < 10) && (top.Value > 6))
        {
            deck_count += 0;
            Debug.Log("Count +0");
        }
        else
        {
            deck_count += 1;
            Debug.Log("Count +1");
        }
        count_text.text = deck_count.ToString();

    }

}
