using BitSplash.AI.GPT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Speaking : MonoBehaviour
{
    [SerializeField] private Text result = null;
    [SerializeField] private InputField question = null;
    public string[] Facts;
    private ChatGPTConversation Conversation;

    private Coroutine writing;
    [SerializeField] private GameObject loader;
    private void Start()
    {
        Conversation = ChatGPTConversation.Start(this)
                .System(string.Join("\n", Facts) + "\n" + "Give information about the city of Grozny") // sets the identity of the chat ai agent
                .MaximumLength(512); // set the maximum length of tokens per request
        result.text = "...";
    }
    public void Answer()
    {
        loader.SetActive(true);
        result.text = "Идет обработка данных...";
        Conversation.Say(question.text);
    }

    void OnConversationResponse(string text)
    {
        if (writing != null)
            StopCoroutine(writing);

        writing = StartCoroutine(TypeText(text));
    }
    void OnConversationError(string text)
    {
        loader.SetActive(false);
        Debug.Log(text);
        result.text = "Произошла ошибка. Попробуйте другой запрос";
        Conversation.RestartConversation();
        
    }
    private IEnumerator TypeText(string text)
    {
        result.text = "Ответ: ";
        loader.SetActive(false);
        foreach (char letter in text.ToCharArray())
        {
            result.text += letter;
            yield return 0;
            yield return new WaitForSeconds(0.05f);
        }
        writing = null;
    }

    public void FakeLoad()
    {
        if (buildings.activeSelf == false)
        {
            StartCoroutine(Loa());
        }
        else
        {
            buildings.SetActive(false);
        }
    }
    public GameObject load;
    public GameObject buildings;
    private IEnumerator Loa()
    {
        buildings.SetActive(false);
        load.SetActive(true);
        yield return new WaitForSeconds(5);
        load.SetActive(false);
        buildings.SetActive(true);
    }
}
