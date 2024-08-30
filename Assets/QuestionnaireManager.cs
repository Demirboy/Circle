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

                new QuestionData { question = "To what extent did you feel you were 'inside' the virtual environment rather than just observing it?\n\n(Inwieweit hatten Sie das Gefühl, dass Sie sich 'innerhalb' der virtuellen Umgebung befanden, anstatt sie nur zu beobachten?)", weakScale = "Not at all\n\n(Überhaupt nicht)", strongScale = "Completely\n\n(Vollständig)" },

                new QuestionData { question = "Did you feel as though you were part of the virtual environment?\n\n(Hatten Sie das Gefühl, Teil der virtuellen Umgebung zu sein?)", weakScale = "Not at all\n\n(Überhaupt nicht)", strongScale = "Completely\n\n(Vollständig)" },

                new QuestionData { question = "How immersive did you find the virtual environment?\n\n(Wie immersiv fanden Sie die virtuelle Umgebung?)", weakScale = "Not immersive at all\n\n(Überhaupt nicht immersiv)", strongScale = "Completely immersive\n\n(Vollständig immersiv)" },

                new QuestionData { question = "Did the quality of the graphics and sound in the VR environment contribute to your feeling of immersion?\n\n(Hat die Qualität der Grafik und des Sounds in der VR-Umgebung zu Ihrem Gefühl der Immersion beigetragen?)", weakScale = "Not at all\n\n(Überhaupt nicht)", strongScale = "Completely\n\n(Vollständig)" },

                new QuestionData { question = "Did you experience any discomfort or nausea during the VR experience?\n\n(Haben Sie während der VR-Erfahrung Unbehagen oder Übelkeit verspürt?)", weakScale = "None at all\n\n(Überhaupt nicht)", strongScale = "Severe\n\n(Stark)" },

                new QuestionData { question = "How would you rate your overall comfort level while using the VR system?\n\n(Wie würden Sie Ihr allgemeines Komfortniveau bei der Nutzung des VR-Systems bewerten?)", weakScale = "Very uncomfortable\n\n(Sehr unangenehm)", strongScale = "Very comfortable\n\n(Sehr angenehm)" };
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
            string answer = PlayerData.currentscene + ", " questions[currentQuestionIndex].question + ": " + (buttonIndex + 1).ToString();
            answers.Add(answer);
            PlayerData.questionaireAnswers.Add(answer);
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