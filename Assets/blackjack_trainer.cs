using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class blackjack_trainer: MonoBehaviour
{
    public GameObject player_handler;
    public GameObject dealer;
    public TextMeshProUGUI basic_strat;
    public int decision = 0;
    private int[] dealer_Upcard = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    private int[] splits = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    private int[] softs = { 13, 14, 15, 16, 17, 18, 19, 20 };
    private int[] hards = { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
    private int[,] hard_action = {  {2,2,2,2,2,2,2,2,2,2},
                                {2,3,3,3,3,2,2,2,2,2},
                                {3,3,3,3,3,3,3,3,2,2},
                                {3,3,3,3,3,3,3,3,3,3},
                                {2,2,1,1,1,2,2,2,2,2},
                                {1,1,1,1,1,2,2,2,2,2},
                                {1,1,1,1,1,2,2,2,2,2},
                                {1,1,1,1,1,2,2,2,2,2},
                                {1,1,1,1,1,2,2,2,2,2},
                                {1,1,1,1,1,1,1,1,1,1} };
    private int[,] soft_action = { {2,2,2,3,3,2,2,2,2,2 },
                                {2,2,2,3,3,2,2,2,2,2 },
                                {2,2,3,3,3,2,2,2,2,2 },
                                {2,2,3,3,3,2,2,2,2,2 },
                                {2,3,3,3,3,2,2,2,2,2 },
                                {3,3,3,3,3,1,1,2,2,2 },
                                {1,1,1,1,3,1,1,1,1,1 },
                                {1,1,1,1,1,1,1,1,1,1 } };
    private int[,] split_action = { {4,4,4,4,4,4,5,5,5,5 },
                                {4,4,4,4,4,4,5,5,5,5 },
                                {5,5,5,4,4,5,5,5,5,5 },
                                {5,5,5,5,5,5,5,5,5,5 },
                                {4,4,4,4,4,5,5,5,5,5 },
                                {4,4,4,4,4,4,5,5,5,5 },
                                {4,4,4,4,4,4,4,4,4,4 },
                                {4,4,4,4,4,5,4,4,5,5 },
                                {5,5,5,5,5,5,5,5,5,5 },
                                {4,4,4,4,4,4,4,4,4,4 } };
    public enum action
    {
        Stand = 1,
        Hit = 2,
        Double = 3,
        Split = 4,
        No_Split = 5
    }
    public void basic_strategy(int player_action)
    {
        GameObject player = player_handler.GetComponent<Player_Handler>().current_turn;
        int player_count = player.GetComponent<Hand>().count;
        bool is_soft = player.GetComponent<Hand>().soft;
        bool splitable = player.GetComponent<Hand>().splitable;
        int up_card = dealer.GetComponent<Hand>().get_hand()[1].Value;

        if (up_card > 9 && up_card < 14){
            up_card = 10;
        }
        else if (up_card == 14)
        {
            up_card = 11;
        }
        try
        {
            if (splitable)
            {
                int indexer1 = Array.IndexOf(dealer_Upcard, up_card);
                int indexer2 = Array.IndexOf(splits, player_count);
                Debug.Log("indexer1: " + indexer1.ToString() + " indexer2: " + indexer2.ToString());
                decision = split_action[indexer2, indexer1];
                Debug.Log("Action: " + player_action.ToString() + " Correct decision: " + decision.ToString());
                if (decision != player_action)
                {
                    basic_strat.text = "Wrong, Correct decision: " + (action)decision;
                    basic_strat.enabled = true;
                }
            }
            else if (is_soft)
            {
                int indexer1 = Array.IndexOf(dealer_Upcard, up_card);
                int indexer2 = Array.IndexOf(softs, player_count);
                Debug.Log("indexer1: " + indexer1.ToString() + " indexer2: " + indexer2.ToString());
                decision = soft_action[indexer2, indexer1];
                Debug.Log("Action: " + player_action.ToString() + " Correct decision: " + decision.ToString());
                if (decision != player_action)
                {
                    basic_strat.text = "Wrong, Correct decision: " + (action)decision;
                    basic_strat.enabled = true;
                }
            }
            else
            {
                int indexer1 = Array.IndexOf(dealer_Upcard, up_card);
                int indexer2 = Array.IndexOf(hards, player_count);
                Debug.Log("indexer1: " + indexer1.ToString() + " indexer2: " + indexer2.ToString());
                decision = hard_action[indexer2, indexer1];
                Debug.Log("Action: " + player_action.ToString() + " Correct decision: " + decision.ToString());
                if (decision == 3 && player.GetComponent<Hand>().get_hand().Count > 2)
                {
                    decision = 2;
                }
                if (decision != player_action)
                {
                    basic_strat.text = "Wrong, Correct decision: " + (action)decision;
                    basic_strat.enabled = true;
                }
            }

        }
        catch (IndexOutOfRangeException e)
        {
            if (player_count < 8 && player_action != 2)
            {
                basic_strat.text = "Wrong, Correct decision: " + (action)2;
                basic_strat.enabled = true;
            }
            else if (player_count > 17 && player_action != 1)
            {
                basic_strat.text = "Wrong, Correct decision: " + (action)1;
                basic_strat.enabled = true;
            }
        }
        
    }
}
