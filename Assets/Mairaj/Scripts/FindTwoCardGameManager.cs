//Mairaj Muhammad ->2415831
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static System.Action <bool> EnableCardClicking = null;

    public static System.Action<Card> OnCardClickedCallback = null;
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

        OnCardClickedCallback += OnCardClicked;
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
        // Get all possible card types
        CardType[] allTypes = (CardType[])System.Enum.GetValues(typeof(CardType));
        System.Random rng = new System.Random();

        // Pick the repeated card randomly
        CardType repeated = allTypes[rng.Next(allTypes.Length)];

        // Create a list of remaining types excluding the repeated one
        List<CardType> remaining = allTypes.Where(c => c != repeated).ToList();

        // Shuffle the remaining list and pick two unique ones
        for (int i = 0; i < remaining.Count; i++)
        {
            int swapIndex = rng.Next(i, remaining.Count);
            (remaining[i], remaining[swapIndex]) = (remaining[swapIndex], remaining[i]);
        }

        CardType[] result = new CardType[4];
        result[0] = repeated;
        result[1] = repeated;
        result[2] = remaining[0];
        result[3] = remaining[1];

        // Shuffle the final result
        Shuffle(result);

        return result;
    }

    //private void Shuffle(CardType[] array)
    //{
    //    System.Random rand = new System.Random();
    //    int n = array.Length;
    //    while (n > 1)
    //    {
    //        int k = rand.Next(n--);
    //        var temp = array[n];
    //        array[n] = array[k];
    //        array[k] = temp;
    //    }
    //}

    private void Shuffle<T>(T[] array)
    {
        System.Random rng = new System.Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }


    public void OnCardClicked(Card clickedCard)
    {
        if (selectedCards.Contains(clickedCard) || selectedCards.Count >= 2)
            return;

        selectedCards.Add(clickedCard);

        if (selectedCards.Count == 2)
        {
            EnableCardClicking?.Invoke(false);
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

            EnableCardClicking?.Invoke(true);

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

        if (ScoreManager.Instance)
        {
            //scoreManager.GameComplete();
           StartCoroutine( ScoreManager.Instance.GameComplete());
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

    private void OnDestroy()
    {
        OnCardClickedCallback-= OnCardClicked;
    }
}

public enum CardType
{
    mTwoClub, mThreeClub, mFourClub, mFiveClub, mSixClub, mSevenClub, mEightClub, mNineClub, mTenClub, mAClub, mJClub, mKClub, mQClub
    //mTwoDiamond, mThreeDiamond, mFourDiamond, mFiveDiamond, mSixDiamond, mSevenDiamond, mEightDiamond, mNineDiamond, mTenDiamond, mADiamond, mJDiamond, mKDiamond, mQDiamond,
    //mTwoHeart, mThreeHeart, mFourHeart, mFiveHeart, mSixHeart, mSevenHeart, mEightHeart, mNineHeart, mTenHeart, mAHeart, mJHeart, mKHeart, mQHeart,
    //mTwoSpade, mThreeSpade, mFourSpade, mFiveSpade, mSixSpade, mSevenSpade, mEightSpade, mNineSpade, mTenSpade, mASpade, mJSpade, mKSpade, mQSpade
}

public enum FindTwoCardsVariant
{
    mFindTwoCardsNormal,
    mFindTwoCardsSwapMode,
    mFindTwoCardsSwapAndTutorialMode
}
