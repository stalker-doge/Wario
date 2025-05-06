//Mairaj Muhammad -->2415831
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindTwoCardGameManager : MonoBehaviour
{
    [SerializeField]
    private Card[] cards;

    [SerializeField]
    private FindTwoCardsVariant variant = FindTwoCardsVariant.mFindTwoCardsNormal;

    [SerializeField]
    private float tutorialTimerForVariantMode = 0.0f;

    private List<Card> selectedCards = new List<Card>();
    private Coroutine countDownCoroutine = null;

    public static System.Action SuccessCompletionCallback = null;

    private void Awake()
    {
        TimeAndLifeManager.FindTwoCardsGameEndCallBack += GameEndFailedCallback;
        if (cards != null && cards.Length > 0)
        {
            InitializeCards();
        }
    }

    private void InitializeCards()
    {
        int[] cardNumbers = GenerateCardNumbers();
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].InitializeCardNumber(cardNumbers[i]);
        }

        if (variant == FindTwoCardsVariant.mFindTwoCardsSwapAndTutorialMode)
        {
            StartCoroutine(QuickTutorialCoroutine());
        }
    }

    private IEnumerator QuickTutorialCoroutine()
    {
        Debug.Log("XYZ QuickTutorialCoroutine Called");
        foreach (Card cd in cards)
        {
            Debug.Log("XYZ Cards Rotation");
            cd.Rotate(true, () => { }, true);
        }

        yield return new WaitForSeconds(tutorialTimerForVariantMode);

        foreach (Card cd in cards)
        {
            cd.Rotate(false, () => { }, false);
        }
    }

    private int[] GenerateCardNumbers()
    {
        System.Random rand = new System.Random();
        int[] numbers = new int[4];

        int repeatedDigit = rand.Next(1, 4);
        const int uniqueNumbers = 2;
        int[] uniqueDigits = new int[uniqueNumbers];
        int index = 0;

        while (index < uniqueDigits.Length)
        {
            int randomDigit = rand.Next(1, 4);
            if (Array.IndexOf(uniqueDigits, randomDigit) == -1 && randomDigit != repeatedDigit)
            {
                uniqueDigits[index] = randomDigit;
                index++;
            }
        }

        numbers[0] = repeatedDigit;
        numbers[1] = repeatedDigit;

        int uniqueIndex = 0;
        for (int i = 2; i < 4; i++)
        {
            numbers[i] = uniqueDigits[uniqueIndex++];
        }

        Shuffle(numbers);
        return numbers;
    }

    private void Shuffle(int[] array)
    {
        System.Random rand = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            int value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }

    public void OnCardClicked(Card clickedCard)
    {
        if (selectedCards.Contains(clickedCard) || selectedCards.Count >= 2)
            return;

        selectedCards.Add(clickedCard);

        if (selectedCards.Count == 2)
        {
            CheckForMatch();
        }
    }

    private void CheckForMatch()
    {
        if (selectedCards[0].GetCardNumber() == selectedCards[1].GetCardNumber())
        {
            SoundManager.Instance.CardMatchAudioClip();
            Invoke("GameEndSuccessCallback", selectedCards[1].GetRotateTimer() * 2);
            Invoke("GameCompleteDelayedSound", 0.5f);
        }
        else
        {
            StartCoroutine(ShakeAndResetCards(selectedCards[0], selectedCards[1], selectedCards[0].GetRotateTimer()));
        }
    }

    // Returns all cards except the two specified ones
    private Card[] GetCardsExcluding(Card cardA, Card cardB)
    {
        List<Card> result = new List<Card>();
        foreach (var card in cards)
        {
            if (card != cardA && card != cardB)
            {
                result.Add(card);
            }
        }
        return result.ToArray();
    }



    private void GameEndSuccessCallback()
    {
        Debug.Log("Game Over!");

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
            scoreManager.GameComplete();
        }
        else
        {
            Debug.LogError("ScoreManager not found in the scene.");
        }
    }

    private void GameEndFailedCallback()
    {
        Debug.Log("XYZ FindTwoCardsAllLivesGoneCase Callback");
    }

    private IEnumerator ShakeAndResetCards(Card card1, Card card2, float delay)
    {
        yield return new WaitForSeconds(delay);

        card1.ShakeCardAndReset();
        card2.ShakeCardAndReset();

        yield return new WaitForSeconds(0.5f);

        card1.ResetCard();
        card2.ResetCard(() =>
        {
            // Now deal the code according to variants
            if (variant != FindTwoCardsVariant.mFindTwoCardsNormal)
            {
                Card wrongCard = selectedCards[1];
                Card[] nonSelectedCards = GetCardsExcluding(selectedCards[0], selectedCards[1]);

                if (nonSelectedCards.Length > 0)
                {
                    Card swapTarget = nonSelectedCards[UnityEngine.Random.Range(0, nonSelectedCards.Length)];
                    int temp = wrongCard.GetCardNumber();
                    wrongCard.InitializeCardNumber(swapTarget.GetCardNumber());
                    swapTarget.InitializeCardNumber(temp);
                }
            }

            selectedCards.Clear();
        });
    }

    private void GameCompleteDelayedSound()
    {
        SoundManager.Instance.MiniGameCompleteAudioClip();
    }

    private void OnDestroy()
    {
        TimeAndLifeManager.FindTwoCardsGameEndCallBack -= GameEndFailedCallback;
    }
}

public enum FindTwoCardsVariant
{
    mFindTwoCardsNormal,
    mFindTwoCardsSwapMode,
    mFindTwoCardsSwapAndTutorialMode
}
