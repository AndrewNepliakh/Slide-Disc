using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseController : MonoBehaviour
{
   [SerializeField] private GameObject _baseItemPrefab;

   private BaseItemsData _baseItemsData;

   private void Start()
   {
      PlaceBaseItems();
   }

   private void PlaceBaseItems()
   {
      
   }

}
