using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    public List<Card> playerDeck = new List<Card>();
    public List<Card> enemyDeck = new List<Card>();
    public List<Slot> playerSlots = new List<Slot>();
    public List<Slot> enemySlots = new List<Slot>();
    public List<CardDisplay> enemyHand = new List<CardDisplay>();
    public List<CardDisplay> enemyCardsInSlots = new List<CardDisplay>();

    public CardDisplay cardPrefab;

    public HorizontalLayoutGroup playerHand;

    public HorizontalLayoutGroup enemyHandTransform;
    public bool[] availablePlayerSlots;
    public bool[] availableEnemySlots;

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

    public void PlayerDrawCard(){
        if(playerDeck.Count >= 1){
            Card randCard = playerDeck[Random.Range(0, playerDeck.Count)];
            cardPrefab.card = randCard;
            cardPrefab.isPlayer = true;
            Instantiate(cardPrefab.gameObject, playerHand.transform);
            playerDeck.Remove(randCard);
        }
    }
    public void EnemyDrawCard(){
        if(enemyDeck.Count >= 1){
            Card randCard = enemyDeck[Random.Range(0, enemyDeck.Count)];
            cardPrefab.card = randCard;
            cardPrefab.isPlayer = false;
            GameObject newCard = Instantiate(cardPrefab.gameObject, enemyHandTransform.transform);
            newCard.GetComponent<Draggable>().enabled = false;
            enemyDeck.Remove(randCard);
            enemyHand.Add(newCard.GetComponent<CardDisplay>());
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
