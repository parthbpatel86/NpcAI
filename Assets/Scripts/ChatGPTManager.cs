using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using System.Threading;
using TMPro;

public class ChatGPTManager : MonoBehaviour
{
    [SerializeField] Animator _NPCAnimator;
    [SerializeField] TextMeshProUGUI _npcResponseTextUI;

    private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    private void Start()
    {
        AddChatMessage("system", "You are a mind guru. No one should know that you are hindu who knows bhagawat gita and bhagavatam and answer questions with short answers with applicable to day to day life.\n");
        AddChatMessage("user", "How can I control my mind?");
        AddChatMessage("assistant", "By practicing meditation. Mind is a tool you have just " +
            "like you have your body. It requires training to control " +
            "it like your practice any sports");
    }

    private void AddChatMessage(string role, string content)
    {
        ChatMessage systemMessage = new ChatMessage();
        systemMessage.Content = content;
        systemMessage.Role = role;
        messages.Add(systemMessage);
    }

    public async void AskChatGPT(string newText)
    {
        AddChatMessage("user", newText);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-3.5-turbo";
        request.MaxTokens = 128;

        var response = await openAI.CreateChatCompletion(request);
        if (response.Choices != null)
        {
            _NPCAnimator.SetTrigger("Talk");
            ChoicesDisplay(response);
        }
    }

    private void ChoicesDisplay(CreateChatCompletionResponse response)
    {
        for (int j = 0; j < response.Choices.Count; j++)
        {
            ChatMessage newMessage = response.Choices[j].Message;
            messages.Add(newMessage);
            _npcResponseTextUI.text = newMessage.Content;
            Debug.Log(newMessage.Content);
        }
    }

    private void OnDisable()
    {
        _npcResponseTextUI.text = string.Empty;
    }
}
