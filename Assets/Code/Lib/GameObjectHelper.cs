using UnityEngine;
using System.Collections;
using System;

public static class GameObjectHelper {
	
	public static GameObject findChildBySubstringInName(GameObject root, string substring){
		if (root.name.IndexOf(substring) != -1){
			return root;	
		}
		else {
			int count = root.transform.childCount;
			
			for (int i = 0; i < count; ++i){
				GameObject child = root.transform.GetChild(i).gameObject;
				
				GameObject searchResult = findChildBySubstringInName(child, substring);
				
				if (searchResult != null){
					return searchResult;
				}
			}
		}
		
		return null;
	}
	
	public static void visitComponentsInDirectChildren<T>(GameObject root, Action<T> callback) where T : Component {
		int count = root.transform.childCount;
		
		for (int i = 0; i < count; ++i){
			GameObject child = root.transform.GetChild(i).gameObject;
			
			T t = child.GetComponent<T>();
			if (t) callback(t);
		}
	}
	
	/**
	 * recursive and expensive
	 */
	public static void visitComponentsDeep<T>(GameObject root, Action<T> callback) where T : Component {
		T t = root.GetComponent<T>();
		if (t) callback(t);
		
		int count = root.transform.childCount;
		
		for (int i = 0; i < count; ++i){
			GameObject child = root.transform.GetChild(i).gameObject;
			
			visitComponentsDeep<T>(child, callback);
		}
	}
}
