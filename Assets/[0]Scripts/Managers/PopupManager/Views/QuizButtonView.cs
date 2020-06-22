using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizButtonView : MonoEntity
{
    public bool IsCorrectButton { get; set; }
    public bool IsSelectedButton { get; private set; } = false;

    [SerializeField] private Image _foreground;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _selectButton;

    public void SetForegroundColor(Color color)
    {
        _foreground.color = color;
    }

    public void SetButtonText(string text)
    {
        _text.text = text;
    }

    public void SetFlag(bool flag)
    {
        IsCorrectButton = flag;
    }
    

    public void OnClickSelectButton()
    {
        IsSelectedButton = true;
        
        var eventArgs = new OnClickQuizButton {IsCorrect = IsCorrectButton};
        EventManager.TriggerEvent(eventArgs);
    }

    public Button GetSelectButton() => _selectButton;
    
    public void ResetButton()
    {
        _selectButton.enabled = true;
        IsCorrectButton = false;
        IsSelectedButton = false;
    }
}