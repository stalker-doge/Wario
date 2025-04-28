using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MathQuestionHandler : MonoBehaviour
{
    // UI references for the math question
    public TMP_Text firstNumberText;
    public TMP_Text operatorText;
    public TMP_Text secondNumberText;

    // Prefab for answer options
    public GameObject answerOptionPrefab;

    // Spawn points for answer options
    public Transform[] optionSpawnPoints;

    // Stores the correct answer
    public int correctAnswer;

    void Start()
    {
        GenerateRandomQuestion();
        GenerateAnswerOptions();
    }

    void GenerateRandomQuestion()
    {
        string[] operators = { "+", "-", "*", "/" };
        string operatorSymbol = operators[Random.Range(0, operators.Length)];

        int firstNumber = 0;
        int secondNumber = 0;

        // Generate numbers based on the selected operator
        switch (operatorSymbol)
        {
            case "+":
                firstNumber = Random.Range(10, 100);
                secondNumber = Random.Range(10, 100);
                correctAnswer = firstNumber + secondNumber;
                break;

            case "-":
                firstNumber = Random.Range(10, 100);
                secondNumber = Random.Range(10, firstNumber); // ensure positive result
                correctAnswer = firstNumber - secondNumber;
                break;

            case "*":
                firstNumber = Random.Range(2, 10);
                secondNumber = Random.Range(2, 10);
                correctAnswer = firstNumber * secondNumber;
                break;

            case "/":
                secondNumber = Random.Range(2, 10);
                correctAnswer = Random.Range(2, 10);
                firstNumber = secondNumber * correctAnswer; // ensure integer division
                break;
        }

        // Update question texts
        firstNumberText.text = firstNumber.ToString();
        operatorText.text = operatorSymbol;
        secondNumberText.text = secondNumber.ToString();
    }

    void GenerateAnswerOptions()
    {
        // Initialize answer options with the correct answer
        List<int> options = new List<int> { correctAnswer };

        // Generate additional unique wrong answers
        while (options.Count < 5)
        {
            int fakeAnswer = correctAnswer + Random.Range(-10, 10);
            if (fakeAnswer != correctAnswer && !options.Contains(fakeAnswer))
                options.Add(fakeAnswer);
        }

        ShuffleList(options);

        // Instantiate answer options
        for (int i = 0; i < options.Count; i++)
        {
            GameObject option = Instantiate(answerOptionPrefab, optionSpawnPoints[i].position, Quaternion.identity);
            option.GetComponentInChildren<TMP_Text>().text = options[i].ToString();

            // Set answer option properties
            AnswerOption optionScript = option.AddComponent<AnswerOption>();
            optionScript.value = options[i];
            optionScript.isCorrect = (options[i] == correctAnswer);
        }
    }

    void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
