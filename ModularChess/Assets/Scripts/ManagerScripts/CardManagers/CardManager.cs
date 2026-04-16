using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    public List<Card> Hand {get; private set;} = new();
    public List<Card> Deck {get; private set;} = new();


    [SerializeField] private GameObject _emptyCardPre;
    [SerializeField] private GameObject _playerHandGroup;

    [SerializeField] private List<CardData> _startingDeck;

    private List<CardBuilder> cardBuilders = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardGA>(DrawCardPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.AttachPerformer<DestroyCardGA>(DestroyCardPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.DetachPerformer<DestroyCardGA>();
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeDeck(_startingDeck);
    }

    public IEnumerator DrawCardPerformer(DrawCardGA drawCardGA)
    {
        Card drawnCard = Deck[0];
        Deck.RemoveAt(0);

        Hand.Add(drawnCard);
        AddCardToHandGroup(drawnCard);

        yield return null;
    }

    public IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {

        if (playCardGA.CardToPlay.Type == CardType.Block)
        {
            SummonBlockGA summonBlockGA = new()
            {
                BlockToSummon = playCardGA.CardToPlay.BlockPre
            };
            
            ActionSystem.Instance.Perform(summonBlockGA);
        }

        yield return null;
    }

    public IEnumerator DestroyCardPerformer(DestroyCardGA destroyCardGA)
    {
        Hand.Remove(destroyCardGA.CardToDestroy);
        DestroyCardInHandGroup(destroyCardGA.CardToDestroy);

        yield return null;
    }



    private void AddCardToHandGroup(Card cardToAdd)
    {
        GameObject newCardObj = Instantiate(_emptyCardPre, _playerHandGroup.transform);
        
        CardBuilder cardBuilder = newCardObj.GetComponent<CardBuilder>();
        cardBuilder.CardRef = cardToAdd;

        cardBuilders.Add(cardBuilder);
    }

    private void DestroyCardInHandGroup(Card cardToDestroy)
    {
        foreach (CardBuilder cardBuilder in cardBuilders)
        {
            if (cardBuilder.CardRef == cardToDestroy)
            {
                Destroy(cardBuilder);
            }
        }

    }

    private void InitializeDeck(List<CardData> cardDatas)
    {
        foreach (CardData cardData in cardDatas)
        {
            Deck.Add(new Card(cardData));
        }
    }
}
