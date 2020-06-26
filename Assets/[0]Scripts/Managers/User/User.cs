using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class User
{
    private string _userID;
    private int _scores;

    public string UserID
    {
        get { return _userID; }
        set { if (!String.IsNullOrWhiteSpace(value) && !String.IsNullOrEmpty(value)) _userID = value;}
    }
    public int Scores
    {
        get { return _scores; }
        set { if(value >= 0) _scores = value;}
    }

    public User()
    {
        _userID = Guid.NewGuid().ToString("N").Substring(0, 6);
        _scores = 0;
    }

    public User(string id)
    {
        _userID = id;
        _scores = 0;
    }

    public void Reset()
    {
        _scores = 0;
    }

    public int AddScores(int scores)
    {
        _scores += scores;
        return _scores;
    }
}
