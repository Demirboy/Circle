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
                new QuestionData { question = "In the computer generated world I had a sense of 'being there'\n(In der computererzeugten Welt hatte ich den Eindruck, dort gewesen zu sein...)", weakScale = "not at all\n(überhaupt nicht)", strongScale = "very much\n(sehr stark)" },

                new QuestionData { question = "Somehow I felt that the virtual world surrounded me.\n(Ich hatte das Gefühl, daß die virtuelle Umgebung hinter mir weitergeht.)", weakScale = "fully disagree\n(trifft gar nicht zu)", strongScale = "fully agree\n(trifft völlig zu)" },

                new QuestionData { question = "I felt like I was just perceiving pictures.\n(Ich hatte das Gefühl, nur Bilder zu sehen.)", weakScale = "fully disagree\n(trifft gar nicht zu)", strongScale = "fully agree\n(trifft völlig zu)" },

                new QuestionData { question = "I did not feel present in the virtual space.\n(Ich hatte nicht das Gefühl, in dem virtuellen Raum zu sein.)", weakScale = "did not feel\n(hatte nicht das Gefühl)", strongScale = "felt present\n(hatte das Gefühl)" },

                new QuestionData { question = "I had a sense of acting in the virtual space, rather than operating something from outside.\n(Ich hatte das Gefühl, in dem virtuellen Raum zu handeln statt etwas von außen zu bedienen.)", weakScale = "fully disagree\n(trifft gar nicht zu)", strongScale = "fully agree\n(trifft völlig zu)" },

                new QuestionData { question = "I felt present in the virtual space.\n(Ich fühlte mich im virtuellen Raum anwesend.)", weakScale = "fully disagree\n(trifft gar nicht zu)", strongScale = "fully agree\n(trifft völlig zu)" },

                new QuestionData { question = "How aware were you of the real world surrounding while navigating in the virtual world? (i.e. sounds, room temperature, other people, etc.)?\n(Wie bewußt war Ihnen die reale Welt, während Sie sich durch die virtuelle Welt bewegten (z.B. Geräusche, Raumtemperatur, andere Personen etc.)?)", weakScale = "extremely aware\n(extrem bewußt)", strongScale = "not aware at all\n(unbewußt)" },

                new QuestionData { question = "I was not aware of my real environment.\n(Meine reale Umgebung war mir nicht mehr bewußt.)", weakScale = "fully disagree\n(trifft gar nicht zu)", strongScale = "fully agree\n(trifft völlig zu)" },

                new QuestionData { question = "I still paid attention to the real environment.\n(Ich achtete noch auf die reale Umgebung.)", weakScale = "fully disagree\n(trifft gar nicht zu)", strongScale = "fully agree\n(trifft völlig zu)" },

                new QuestionData { question = "I was completely captivated by the virtual world.\n(Meine Aufmerksamkeit war von der virtuellen Welt völlig in Bann gezogen.)", weakScale = "fully disagree\n(trifft gar nicht zu)", strongScale = "fully agree\n(trifft völlig zu)" },

                new QuestionData { question = "How real did the virtual world seem to you?\n(Wie real erschien Ihnen die virtuelle Umgebung?)", weakScale = "completely real\n(vollkommen real)", strongScale = "not real at all\n(gar nicht real)" },

                new QuestionData { question = "How much did your experience in the virtual environment seem consistent with your real world experience?\n(Wie sehr glich Ihr Erleben der virtuellen Umgebung dem Erleben einer realen Umgebung?)", weakScale = "not consistent\n(überhaupt nicht)", strongScale = "very consistent\n(vollständig)" },

                new QuestionData { question = "How real did the virtual world seem to you?\n(Wie real erschien Ihnen die virtuelle Welt?)", weakScale = "about as real as an imagined world\n(wie eine vorgestellte Welt)", strongScale = "indistinguishable from the real world\n(nicht zu unterscheiden von der r. Welt)" },

                new QuestionData { question = "The virtual world seemed more realistic than the real world.\n(Die virtuelle Welt erschien mir wirklicher als die reale Welt.)", weakScale = "fully disagree\n(trifft gar nicht zu)", strongScale = "fully agree\n(trifft völlig zu)" }

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
            string answer = PlayerData.playerName + ", " + PlayerData.currentScene + ", " + questions[currentQuestionIndex].question + "\nAnswer:  " + (buttonIndex ).ToString();
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
            questionText.text = "Thank you, please continue\nVielen dank, fahren sie fort.";
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