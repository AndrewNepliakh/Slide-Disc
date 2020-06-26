using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScorableItemType
{
   BaseItem = 0
}

public interface IScorable
{
   ScorableItemType ScorableItemType { get; set; }
}

[Serializable]
public class ScorableItem
{
   public ScorableItemType scorableItemType;
   public int scoreValue;
}

[CreateAssetMenu(fileName = "ScoreData", menuName = "Data/ScoreData")]
public class ScoreData : BaseInjectable
{
   public List<ScorableItem> scorableItems;

   public bool GetItemScoreValue(Collision collision, out int scoreValue)
   {
      var iScorable = collision.gameObject.GetComponent<IScorable>();

      if (iScorable != null)
      {
         scoreValue = scorableItems.Find(x => x.scorableItemType == iScorable.ScorableItemType).scoreValue;
         return true;
      }

      scoreValue = default;
      return false;
   }
}
