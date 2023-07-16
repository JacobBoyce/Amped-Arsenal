using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class FontReplacer : MonoBehaviour
{
    public TextMeshProUGUI[] GetText;
	public TMP_FontAsset numberFont, textFont;

	// Use this for initialization
	void Start () {
		GetText = FindObjectsOfType<TextMeshProUGUI>(true); // .FindObjectsOfType<TextMeshProUGUI> ();

		foreach (TextMeshProUGUI go in GetText)
		{
			if(go.tag == "TextFont")
			{
				go.font = textFont;
			}
			else if(go.tag == "NumberFont")
			{
				go.font = numberFont;
			}
		}
			
	}
}
