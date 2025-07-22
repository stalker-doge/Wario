using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MathQuestionManager : MiniGameManagerBase
{
    [Header("UI References")]
    public TMP_Text firstNumberText;
    public TMP_Text operatorText;
    public TMP_Text secondNumberText;

    [Header("Answer Options")]
    public GameObject answerOptionPrefab;
    public Transform[] optionSpawnPoints;

    private int correctAnswer;

    public override void InitializeGame()
    {
        GenerateRandomQuestion();
        GenerateAnswerOptions();
    }

    private void GenerateRandomQuestion()
    {
        string[] operators = { "+", "-" };
        string operatorSymbol = operators[Random.Range(0, operators.Length)];

        int firstNumber = 0;
        int secondNumber = 0;

        switch (operatorSymbol)
        {
            case "+":
                firstNumber = Random.Range(1, 11);
                secondNumber = Random.Range(1, 11);
                correctAnswer = firstNumber + secondNumber;
                break;

            case "-":
                firstNumber = Random.Range(1, 11);
                secondNumber = Random.Range(1, 11);

                if (secondNumber > firstNumber)
                    (firstNumber, secondNumber) = (secondNumber, firstNumber);

                correctAnswer = firstNumber - secondNumber;
                break;
        }

        firstNumberText.text = firstNumber.ToString();
        operatorText.text = operatorSymbol;
        secondNumberText.text = secondNumber.ToString();
    }

    private void GenerateAnswerOptions()
    {
        List<int> options = new List<int> { correctAnswer };

        while (options.Count < 3)
        {
            int fakeAnswer = correctAnswer + Random.Range(-5, 6);
            if (fakeAnswer >= 0 && !options.Contains(fakeAnswer))
                options.Add(fakeAnswer);
        }

        ShuffleList(options);

        for (int i = 0; i < options.Count; i++)
        {
            GameObject option = Instantiate(answerOptionPrefab, optionSpawnPoints[i].position, Quaternion.identity);
            option.GetComponentInChildren<TMP_Text>().text = options[i].ToString();

            AnswerOption optionScript = option.AddComponent<AnswerOption>();
            optionScript.value = options[i];
            optionScript.isCorrect = (options[i] == correctAnswer);
        }
    }

    private void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    public override void EndGame()
    {
        if (ScoreManager.Instance)
            StartCoroutine(ScoreManager.Instance.GameComplete());
    }

    protected override void RegisterCallbacks()
    {
        
    }

    protected override void UnregisterCallbacks()
    {
        
    }
}
