using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuestionnaireManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestionData
    {
        public string question;
        public string weakScale;
        public string strongScale;
    }

    public List<QuestionData> questions; // List of questions and their scales

    public TMP_Text questionText;   // Reference to the TMP text for the question
    public TMP_Text weakScaleText;  // Reference to the TMP text for the weak scale
    public TMP_Text strongScaleText; // Reference to the TMP text for the strong scale
    public Button[] answerButtons;  // Reference to the answer buttons

    private int currentQuestionIndex = 0; // Tracks the current question
    private List<string> answers = new List<string>(); // Stores the answers

    void Start()
    {
        questions = new List<QuestionData>
            {
                new QuestionData { question = "How natural did your interactions with the virtual environment feel?\n\n(Wie natürlich hat sich Ihre Interaktion mit der virtuellen Umgebung angefühlt?)", weakScale = "Very Unnatural\n\n(Sehr Unnatürlich)", strongScale = "Very Natural\n\n(Sehr Natürlich)" },
                new QuestionData { question = "How immersive was the experience?", weakScale = "Not Immersive", strongScale = "Very Immersive" },
                new QuestionData { question = "How immersive was the experience?", weakScale = "Not Immersive", strongScale = "Very Immersive" },
                new QuestionData { question = "How immersive was the experience?", weakScale = "Not Immersive", strongScale = "Very Immersive" },

            };
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            questionText.text = questions[currentQuestionIndex].question;
            weakScaleText.text = questions[currentQuestionIndex].weakScale;
            strongScaleText.text = questions[currentQuestionIndex].strongScale;
        }
        else
        {
            // No more questions, display a thank you message
            questionText.text = "Thank you, please continue";
            weakScaleText.text = "";
            strongScaleText.text = "";

            // Disable all buttons
            foreach (Button button in answerButtons)
            {
                button.interactable = false;
            }
        }
    }

    public void OnAnswerButtonClicked(int buttonIndex)
    {
        if (currentQuestionIndex < questions.Count)
        {
            // Record the answer
            string answer = questions[currentQuestionIndex].question + ": " + (buttonIndex + 1).ToString();
            answers.Add(answer);
            Debug.Log(answer);

            // Move to the next question
            currentQuestionIndex++;

            // Display the next question after a delay
            StartCoroutine(HandleNextQuestionWithDelay());
        }
    }

    private IEnumerator HandleNextQuestionWithDelay()
    {
        // Disable buttons to prevent double clicks
        SetButtonsInteractable(false);

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Check if there are more questions
        if (currentQuestionIndex < questions.Count)
        {
            // Display the next question
            DisplayQuestion();

            // Enable buttons again
            SetButtonsInteractable(true);
        }
        else
        {
            // No more questions, leave buttons disabled
            questionText.text = "Thank you, please continue";
            weakScaleText.text = "";
            strongScaleText.text = "";
        }
    }

    private void SetButtonsInteractable(bool state)
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = state;
        }
    }
}