using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class Loader
{
    //public static List<ShopItem> GetAllItems()
    //{
    //    List<ShopItem> items = new List<ShopItem>();
    //    var allPath = AssetDatabase.FindAssets($"t:{typeof(ShopItem)}").ToList();
    //    Debug.Log(allPath.Count);
    //    allPath.ForEach(p =>
    //    {
    //        var item = AssetDatabase.LoadAssetAtPath<ShopItem>(AssetDatabase.GUIDToAssetPath(p));
    //        items.Add(item);
    //    });

    //    return items;
    //}

    // return all prefabs by path
    public static List<T> LoadItems<T>(string path) where T : UnityEngine.Object
    {
          return Resources.LoadAll<T>(path).ToList();
    }
}
