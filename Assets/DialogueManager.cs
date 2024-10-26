using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    // Max length for a single sentence
    private const int maxSentenceLength = 70;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        bool open = true;
        animator.SetBool("isOpen", open);

        Debug.Log("isOpen should be " + open);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            EnqueueSplitSentences(sentence);
        }

        DisplayNextSentence();
    }

    private void EnqueueSplitSentences(string sentence)
    {
        // Split the sentence into chunks if it's longer than the max length
        while (sentence.Length > maxSentenceLength)
        {
            // Find the last space within the max length limit
            int splitIndex = sentence.LastIndexOf(' ', maxSentenceLength);

            // If no space is found, split at maxSentenceLength
            if (splitIndex == -1) splitIndex = maxSentenceLength;

            // Extract the substring and enqueue it
            string chunk = sentence.Substring(0, splitIndex);
            sentences.Enqueue(chunk);

            // Remove the processed chunk from the sentence
            sentence = sentence.Substring(splitIndex).Trim();
        }

        // Enqueue any remaining part of the sentence
        if (sentence.Length > 0)
        {
            sentences.Enqueue(sentence);
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        Debug.Log(sentence);

        // if no more sentences, set isOpen to false


    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }
}
