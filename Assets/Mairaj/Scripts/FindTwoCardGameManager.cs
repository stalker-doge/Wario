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

    //[SerializeField]
    //private Text counterText; // Text component to display the countdown

    //private float countdownTime = 10.0f; // Start time for countdown

    private List<Card> selectedCards = new List<Card>();  // Keep track of selected cards

    private Coroutine countDownCoroutine = null;

    // Successful completion of game to stop timer
    public static System.Action SuccessCompletionCallback = null;

    private void Awake()
    {
        TimeAndLifeManager.FindTwoCardsGameEndCallBack += GameEnd;
        if (cards != null && cards.Length > 0)
        {
            InitializeCards();
        }
    }

    /*private void Start()
    {
        countDownCoroutine = StartCoroutine(StartCountdown());
    }*/

    /*private IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            counterText.text = countdownTime.ToString("F1"); // Update the counter text with 1 decimal place
            countdownTime -= Time.deltaTime; // Decrease the countdown time by the time passed since last frame
            yield return null; // Wait for the next frame
        }

        counterText.text = "0.0"; // Ensure it shows 0.0 when finished
        GameEnd(); // Trigger game end when countdown finishes
    }*/
    private void InitializeCards()
    {
        // Generate the card numbers
        int[] cardNumbers = GenerateCardNumbers();

        // Initialize the cards with these numbers
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].InitializeCardNumber(cardNumbers[i]);
        }
    }

    private int[] GenerateCardNumbers()
    {
        System.Random rand = new System.Random();
        int[] numbers = new int[6];

        // Step 1: Choose a random number for repetition (between 1 and 5)
        int repeatedDigit = rand.Next(1, 6);

        // Step 2: Choose 4 unique numbers for the rest (between 1 and 5)
        int[] uniqueDigits = new int[4];
        int index = 0;

        while (index < 4)
        {
            int randomDigit = rand.Next(1, 6);
            // Ensure we don't repeat the repeatedDigit
            if (Array.IndexOf(uniqueDigits, randomDigit) == -1 && randomDigit != repeatedDigit)
            {
                uniqueDigits[index] = randomDigit;
                index++;
            }
        }

        // Step 3: Place the repeated digit in two random positions
        numbers[0] = repeatedDigit;
        numbers[1] = repeatedDigit;

        // Step 4: Place the unique digits in the remaining positions
        int uniqueIndex = 0;
        for (int i = 2; i < 6; i++)
        {
            numbers[i] = uniqueDigits[uniqueIndex++];
        }

        // Step 5: Shuffle the numbers to randomize their order
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

    // Call this method when a card is clicked
    public void OnCardClicked(Card clickedCard)
    {
        selectedCards.Add(clickedCard);

        // If we have selected two cards, check if they match
        if (selectedCards.Count == 2)
        {
            CheckForMatch();
        }
    }

    // Check if the selected cards match
    private void CheckForMatch()
    {
        if (selectedCards[0].GetCardNumber() == selectedCards[1].GetCardNumber())
        {
            // Card match audio clip
            SoundManager.Instance.CardMatchAudioClip();

            // Match found, end the game
            GameEnd();

            Invoke("GameCompleteDelayedSound", 0.5f);
        }
        else
        {
            // No match, shake and reset the cards
            StartCoroutine(ShakeAndResetCards(selectedCards[0], selectedCards[1]));
        }

        // Clear the selected cards for the next round
        selectedCards.Clear();
    }

    // Game end logic
    private void GameEnd()
    {
        Debug.Log("Game Over! Cards matched.");

        SuccessCompletionCallback?.Invoke();

        if (countDownCoroutine != null) { 
            StopCoroutine(countDownCoroutine);
        }
        // Disable buttons for only the selected cards
        foreach (var card in cards)
        {
            Button cardButton = card.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.interactable = false;
            }
        }
    }

    // Coroutine to shake and reset cards
    private IEnumerator ShakeAndResetCards(Card card1, Card card2)
    {
        card1.ShakeCardAndReset();
        card2.ShakeCardAndReset();

        yield return new WaitForSeconds(0.5f);  // Wait for shaking to finish

        // Reset cards after shaking
        card1.ResetCard();
        card2.ResetCard();
    }

    private void GameCompleteDelayedSound() {
        SoundManager.Instance.MiniGameCompleteAudioClip();
    }

    private void OnDestroy() 
    {
        TimeAndLifeManager.FindTwoCardsGameEndCallBack -= GameEnd;
    }

    // Might need for future where we implement one game as one prefab so that it won't get destroyed so above OnDestroy will be useless
    //private void OnDisable()
    //{
    //    TimeAndLifeManager.FindTwoCardsGameEndCallBack -= GameEnd;
    //}
}
