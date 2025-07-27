using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MathQuestionHandler : MonoBehaviour, IMiniGame
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
        DifficultyLevel difficulty = (DifficultyLevel)PlayerPrefs.GetInt("DifficultyLevel", 0);
        SetDifficulty(difficulty);
    }
    public void SetDifficulty(DifficultyLevel difficulty)
    {
        GenerateRandomQuestion(difficulty);
        GenerateAnswerOptions();
    }

    void GenerateRandomQuestion( DifficultyLevel difficultyLevel )
    {
        string operatorSymbol;

        int firstNumber = 0;
        int secondNumber = 0;

        int upRange , downRange;
        
        if (difficultyLevel == DifficultyLevel.Easy)
        {
            string[] operators = { "+", "-" }; // Only addition and subtraction allowed
            operatorSymbol = operators[Random.Range(0, operators.Length)];
            upRange = 11;
            downRange = 1;

        }
        else if (difficultyLevel == DifficultyLevel.Medium)
        {
            string[] operators = { "+", "-" ,"x" , "/"}; 
            operatorSymbol = operators[Random.Range(0, operators.Length)];
            upRange = 30;
            downRange = 11;
        }
        else
        {
            string[] operators = { "+", "-" ,"x" , "/" }; 
            operatorSymbol = operators[Random.Range(0, operators.Length)];
            upRange = 150; downRange = 50;

        }


        // Generate numbers based on the selected operator
        switch (operatorSymbol)
        {
            case "+":
                firstNumber = Random.Range(downRange, upRange); // Numbers between 1 and 10
                secondNumber = Random.Range(downRange, upRange);
                correctAnswer = firstNumber + secondNumber;
                break;

            case "-":
                firstNumber = Random.Range(downRange, upRange);
                secondNumber = Random.Range(downRange, upRange);

                // Ensure positive result
                if (secondNumber > firstNumber)
                {
                    int temp = firstNumber;
                    firstNumber = secondNumber;
                    secondNumber = temp;
                }
                correctAnswer = firstNumber - secondNumber;
                break;
            
            case "x":
                firstNumber = Random.Range(downRange, upRange); // Numbers between 1 and 10
                secondNumber = Random.Range(downRange, upRange);
                correctAnswer = firstNumber * secondNumber;
                break;
            
            case "/":
                correctAnswer = Random.Range(downRange, upRange); 

                secondNumber = Random.Range(downRange, upRange); 

                firstNumber = correctAnswer * secondNumber;

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
        while (options.Count < 3) // Only 3 options now
        {
            int fakeAnswer = correctAnswer + Random.Range(-5, 6); // Smaller range for fake answers
            if (fakeAnswer != correctAnswer && !options.Contains(fakeAnswer) && fakeAnswer >= 0)
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