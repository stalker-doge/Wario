//Mairaj Muhammad ->2415831
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindTwoCardGameManager : MonoBehaviour
{
    [SerializeField] private Card[] cards;
    [SerializeField] private Sprite backCardSprite;

    [System.Serializable]
    public struct CardSpriteMap
    {
        public CardType cardType;
        public Sprite cardSprite;
    }

    public static System.Action SuccessCompletionCallback = null;

    [SerializeField] private CardSpriteMap[] cardSpriteMappings;

    [SerializeField] private FindTwoCardsVariant variant = FindTwoCardsVariant.mFindTwoCardsNormal;
    [SerializeField] private float tutorialTimerForVariantMode = 0.0f;

    private Dictionary<CardType, Sprite> cardSpriteDict;
    private List<Card> selectedCards = new List<Card>();
    private Coroutine countDownCoroutine = null;

    private void Awake()
    {
        TimeAndLifeManager.FindTwoCardsGameEndCallBack += GameEndFailedCallback;

        // Map sprites
        cardSpriteDict = new Dictionary<CardType, Sprite>();
        foreach (var map in cardSpriteMappings)
        {
            if (!cardSpriteDict.ContainsKey(map.cardType))
                cardSpriteDict.Add(map.cardType, map.cardSprite);
        }

        if (cards != null && cards.Length > 0)
        {
            InitializeCards();
        }
    }

    private void InitializeCards()
    {
        CardType[] types = GenerateCardTypes();

        for (int i = 0; i < cards.Length; i++)
        {
            var type = types[i];
            cards[i].InitializeCard(type, cardSpriteDict[type], backCardSprite);
        }

        if (variant == FindTwoCardsVariant.mFindTwoCardsSwapAndTutorialMode)
            StartCoroutine(QuickTutorialCoroutine());
    }

    private CardType[] GenerateCardTypes()
    {
        CardType repeated = CardType.mTwoClub;
        CardType[] unique = { CardType.mFiveDiamond, CardType.mQueenSpade };

        CardType[] result = new CardType[4];
        result[0] = repeated;
        result[1] = repeated;
        result[2] = unique[0];
        result[3] = unique[1];

        Shuffle(result);
        return result;
    }

    private void Shuffle(CardType[] array)
    {
        System.Random rand = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            int k = rand.Next(n--);
            var temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    public void OnCardClicked(Card clickedCard)
    {
        if (selectedCards.Contains(clickedCard) || selectedCards.Count >= 2)
            return;

        selectedCards.Add(clickedCard);

        if (selectedCards.Count == 2)
        {
            StartCoroutine(CheckForMatch());
        }
    }

    private IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(cards[0].GetRotateTimer() + 0.1f);

        Card card1 = selectedCards[0];
        Card card2 = selectedCards[1];

        bool isMatch = card1.GetCardType() == card2.GetCardType();

        if (isMatch)
        {
            SoundManager.Instance.CardMatchAudioClip();
            card1.GetComponent<Button>().interactable = false;
            card2.GetComponent<Button>().interactable = false;
            SuccessCompletionCallback?.Invoke();
            Invoke("GameEndSuccessCallback", selectedCards[1].GetRotateTimer() * 2);
            Invoke("GameCompleteDelayedSound", 0.5f);
        }
        else
        {
            SoundManager.Instance.CardMismatchAudioClip();
            card1.ShakeCardAndReset();
            card2.ShakeCardAndReset();

            yield return new WaitForSeconds(0.3f);

            card1.ResetCard();
            card2.ResetCard();

            if (variant != FindTwoCardsVariant.mFindTwoCardsNormal)
            {
                Card wrongCard = card2;
                List<Card> nonSelected = new List<Card>();

                foreach (var card in cards)
                {
                    if (card != card1 && card != card2)
                        nonSelected.Add(card);
                }

                if (nonSelected.Count > 0)
                {
                    Card swapTarget = nonSelected[Random.Range(0, nonSelected.Count)];

                    var tempType = wrongCard.GetCardType();
                    var tempSprite = wrongCard.GetFrontSprite();

                    wrongCard.InitializeCard(swapTarget.GetCardType(), swapTarget.GetFrontSprite(), backCardSprite);
                    swapTarget.InitializeCard(tempType, tempSprite, backCardSprite);
                }
            }
        }

        selectedCards.Clear();
    }

    private void GameCompleteDelayedSound()
    {
        SoundManager.Instance.MiniGameCompleteAudioClip();
    }

    private void GameEndSuccessCallback()
    {
        // Debug.Log("XYZ Game Over!");

        SuccessCompletionCallback?.Invoke();

        if (countDownCoroutine != null)
        {
            StopCoroutine(countDownCoroutine);
        }

        foreach (var card in cards)
        {
            Button cardButton = card.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.interactable = false;
            }
        }

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            StartCoroutine(scoreManager.GameComplete());
        }
        else
        {
            Debug.LogError("ScoreManager not found in the scene.");
        }
    }

    private IEnumerator QuickTutorialCoroutine()
    {
        //Debug.Log("XYZ QuickTutorialCoroutine Called");
        foreach (Card cd in cards)
        {
            //Debug.Log("XYZ Cards Rotation");
            cd.Rotate(true, () => { }, true);
        }

        yield return new WaitForSeconds(tutorialTimerForVariantMode);

        foreach (Card cd in cards)
        {
            cd.Rotate(false, () => { }, false);
        }
    }

    private void GameEndFailedCallback()
    {
        StopAllCoroutines();
    }
}

public enum CardType
{
    mTwoClub,
    mTwoDiamond,
    mFiveClub,
    mFiveDiamond,
    mAceClub,
    mAceDiamond,
    mKingDiamond,
    mKingHeart,
    mQueenDiamond,
    mQueenSpade
}

public enum FindTwoCardsVariant
{
    mFindTwoCardsNormal,
    mFindTwoCardsSwapMode,
    mFindTwoCardsSwapAndTutorialMode
}
