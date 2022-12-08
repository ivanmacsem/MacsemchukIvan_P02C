using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardsManager : MonoBehaviour
{
    public static Action PlayerWin = delegate {};
    public static Action PlayerLose = delegate {};
    public List<Card> allCards = new List<Card>();
    public List<Card> playerDeck = new List<Card>();
    public List<Card> enemyDeck = new List<Card>();
    public List<Slot> playerSlots = new List<Slot>();
    public List<Slot> enemySlots = new List<Slot>();
    public List<CardDisplay> enemyHand = new List<CardDisplay>();
    public List<CardDisplay> playerHandCards = new List<CardDisplay>();
    public CardDisplay cardPrefab;

    public HorizontalLayoutGroup playerHand;

    public HorizontalLayoutGroup enemyHandTransform;
    public bool[] availablePlayerSlots;
    public bool[] availableEnemySlots;

    [SerializeField] Text playerDeckCountTxt;
    [SerializeField] Text enemyDeckCountTxt;

    public int buffoons = 0;
    public int chiefs = 0;
    public int elders = 0;
    public int guards = 0;
    public int heroes = 0;
    public int hunters = 0;
    public int shamans = 0;
    public int tribesmen = 0;
    public int warriors = 0;
    public int youths = 0;
    public bool playerTaunt = false;
    public bool enemyTaunt = false;

    public void PlayerDrawCard(bool setup){
        if(playerDeck.Count >= 1){
            Card randCard = playerDeck[UnityEngine.Random.Range(0, playerDeck.Count)];
            cardPrefab.card = randCard;
            cardPrefab.isPlayer = true;
            GameObject newCard = Instantiate(cardPrefab.gameObject, playerHand.transform);
            newCard.GetComponent<Draggable>().firstThree = setup;
            playerDeck.Remove(randCard);
            playerDeckCountTxt.text = "(" + playerDeck.Count + ")";
        }
        else{
            PlayerLose.Invoke();
        }
    }
    public void EnemyDrawCard(){
        if(enemyDeck.Count >= 1){
            Card randCard = enemyDeck[UnityEngine.Random.Range(0, enemyDeck.Count)];
            cardPrefab.card = randCard;
            cardPrefab.isPlayer = false;
            GameObject newCard = Instantiate(cardPrefab.gameObject, enemyHandTransform.transform);
            newCard.GetComponent<Draggable>().enabled = false;
            enemyDeck.Remove(randCard);
            enemyHand.Add(newCard.GetComponent<CardDisplay>());
            enemyDeckCountTxt.text = "(" + enemyDeck.Count + ")";
        }
        else{
            PlayerWin.Invoke();
        }
    }

    public void Spray(bool player){
        if(player){
            for(int i = 0; i<playerSlots.Count; i++){
                if(playerSlots[i].card != null){
                    StartCoroutine(playerSlots[i].card.TakeDamage(1, false));
                }
            }
        }
        else{
            for(int i = 0; i<enemySlots.Count; i++){
                if(enemySlots[i].card != null){
                    StartCoroutine(enemySlots[i].card.TakeDamage(1, false));
                }
            }
        }
    }

    public void Banish(bool player){
        List<int> inSlots = new List<int>();
        if(player){
            for(int i = 0; i<playerSlots.Count; i++){
                if(playerSlots[i].card != null){
                    inSlots.Add(i);
                }
            }
            StartCoroutine(playerSlots[inSlots[UnityEngine.Random.Range(0, inSlots.Count-1)]].card.TakeDamage(99, false));
        }
        else{
            for(int i = 0; i<enemySlots.Count; i++){
                if(enemySlots[i].card != null){
                    inSlots.Add(i);
                }
            }
            StartCoroutine(enemySlots[inSlots[UnityEngine.Random.Range(0, inSlots.Count-1)]].card.TakeDamage(99, false));
        }
    }

    public void Callback(bool player){
        if(player){
            for(int i = 0; i<playerSlots.Count; i++){
                if(playerSlots[i].card != null){
                    availablePlayerSlots[i] = true;
                    playerSlots[i].card.inSlot = false;
                    playerSlots[i].card.GetComponent<RectTransform>().SetParent(playerHand.transform);
                    playerSlots[i].card.Start();
                    playerSlots[i].card = null;
                }
            }
        }
        else{
            for(int i = 0; i<enemySlots.Count; i++){
                if(enemySlots[i].card != null){
                    availableEnemySlots[i] = true;
                    enemyHand.Add(enemySlots[i].card);
                    enemySlots[i].card.inSlot = false;
                    enemySlots[i].card.GetComponent<RectTransform>().SetParent(enemyHandTransform.transform);
                    enemySlots[i].card.Start();
                    enemySlots[i].card = null;
                }
            }
        }
    }

    public bool EmptySlot(Slot s){
        return (availablePlayerSlots[playerSlots.IndexOf(s)]);
    }

    public void CreateDecks(int player, int enemy){
        for(int i = 0; i<buffoons; i++){
            playerDeck.Add(allCards[0+player*10]);
            enemyDeck.Add(allCards[0+enemy*10]);
        }
        for(int i = 0; i<chiefs; i++){
            playerDeck.Add(allCards[1+player*10]);
            enemyDeck.Add(allCards[1+enemy*10]);
        }
        for(int i = 0; i<elders; i++){
            playerDeck.Add(allCards[2+player*10]);
            enemyDeck.Add(allCards[2+enemy*10]);
        }
        for(int i = 0; i<guards; i++){
            playerDeck.Add(allCards[3+player*10]);
            enemyDeck.Add(allCards[3+enemy*10]);
        }
        for(int i = 0; i<heroes; i++){
            playerDeck.Add(allCards[4+player*10]);
            enemyDeck.Add(allCards[4+enemy*10]);
        }
        for(int i = 0; i<hunters; i++){
            playerDeck.Add(allCards[5+player*10]);
            enemyDeck.Add(allCards[5+enemy*10]);
        }
        for(int i = 0; i<shamans; i++){
            playerDeck.Add(allCards[6+player*10]);
            enemyDeck.Add(allCards[6+enemy*10]);
        }
        for(int i = 0; i<tribesmen; i++){
            playerDeck.Add(allCards[7+player*10]);
            enemyDeck.Add(allCards[7+enemy*10]);
        }
        for(int i = 0; i<warriors; i++){
            playerDeck.Add(allCards[8+player*10]);
            enemyDeck.Add(allCards[8+enemy*10]);
        }
        for(int i = 0; i<youths; i++){
            playerDeck.Add(allCards[9+player*10]);
            enemyDeck.Add(allCards[9+enemy*10]);
        }
    }
}
