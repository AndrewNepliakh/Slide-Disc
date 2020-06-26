using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UserManager", menuName = "Managers/UserManager")]

public class UserManager : BaseInjectable
{
    private List<User> _users = new List<User>();

    public User CreateNewUser(string id = default)
    {
        User user;
        
        if(id == default) user = new User();
        else user = new User(id);
        
        _users.Add(user);

        return user;
    }
    
    public void RemoveUser(User user)
    {
        _users.Remove(user);
    }
    public List<User> GetUserList()
    {
        return new List<User>(_users);
    }
    
}
