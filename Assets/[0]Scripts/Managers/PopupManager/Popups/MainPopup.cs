using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _scoresText;
    [SerializeField] private TextMeshProUGUI _userIdText;

    protected override void OnShow(object obj = null)
    {
        base.OnShow(obj);
        EventManager.Add<OnScoreGainedEvent>(OnScoreGained);
        EventManager.Add<OnLevelLoadedEvent>(OnLevelLoaded);
    }

    public void UpdateLevelText(int value)
    {
        _levelText.text = "LEVEL " + ++value;
    }

    public void UpdateScoresText(string value)
    {
        _scoresText.text = "SCORES " + value.ToUpper();
    }

    public void UpdateUserIdText(string value)
    {
        _userIdText.text = "user id: " + value;
    }
    
    public void OnScoreGained(OnScoreGainedEvent args)
    {
        UpdateScoresText(args.Score.ToString());
    }

    public void OnLevelLoaded(OnLevelLoadedEvent args)
    {
        UpdateLevelText(args.Level);
        UpdateUserIdText(args.CurrentUser.UserID);
        UpdateScoresText(args.CurrentUser.Scores.ToString());
    }


    protected override void OnClose()
    {
        base.OnClose();
        EventManager.Remove<OnScoreGainedEvent>(OnScoreGained);
        EventManager.Remove<OnLevelLoadedEvent>(OnLevelLoaded);
    }
}