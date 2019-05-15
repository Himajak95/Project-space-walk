//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System;
using Mission;
using Character;

namespace Design
{
	public class design
	{
		//static string timerFormatted = "";

		// Constructor
		public design (){}

		private static Font font_futura = null;
		public static Font Font_Futura
		{ 
			get { return font_futura; }
			set { font_futura = value; }
		}

		private static string timerFormatted = "";
		public static string TIMER_FORMATTED
		{
			get { return timerFormatted; }
			set { timerFormatted = value; }
		}


		//************************************************************************
		//Button Style
		public static GUIStyle StyleButton(Font font, int fontSize, Color fontColor, Texture2D bkColor, Texture2D bkColor_hover){
			var _button = new GUIStyle();
			_button = new GUIStyle(GUI.skin.button);
			_button.normal.textColor = fontColor;
			_button.normal.background = bkColor;
			_button.hover.textColor  = fontColor;
			_button.hover.background = bkColor_hover;
			_button.fontSize = fontSize;
			_button.font = font;
			
			return _button;
		}
		
		//************************************************************************
		// Changes the style of the text
		public static GUIStyle StyleText(Font font, int fontSize, TextAnchor text_align, Color fontColor){
			//Text Style
			var style_text = new GUIStyle();
			style_text = new GUIStyle (GUI.skin.label);
			style_text.fontSize = fontSize;
			style_text.normal.textColor = fontColor;
			style_text.font = font;
			style_text.wordWrap = true;
			style_text.alignment = text_align;
			
			return style_text;
		}
		
		//************************************************************************
		// Draw rectangle shap
		public static void DrawRectangle(Rect rect, Color color, float alpha = 0f)
		{
			//Source: http://snipplr.com/view/71903/rectangle-with-alpha-unity/
			Texture2D texture = new Texture2D(1, 1);
			color.a = alpha;
			texture.SetPixel(0, 0, color);
			texture.wrapMode = TextureWrapMode.Repeat;
			texture.Apply();
			GUI.DrawTexture(rect, texture);
		}
		
		//************************************************************************
		// Button
		public static GUIStyle DrawLevelButton(Texture2D button_texture_normal, Texture2D button_texture_hover, float opacity){
			var button_style = new GUIStyle();
			button_style = new GUIStyle(GUI.skin.button);
			button_style.normal.background = button_texture_normal;
			button_style.hover.background = button_texture_hover;
			GUI.color = new Color(1,1,1,opacity);
			return button_style;
		}
		
		//************************************************************************
		// Pause Menu screen
		public static void StatePause(Font futura_font, Texture2D button_blank, Texture2D button_blue, Texture2D button_blue_hover) {
			DrawRectangle (new Rect (0,0,Screen.width, Screen.height), Color.black, 0.9f);
			DrawRectangle (new Rect (Screen.width/2,40,1, Screen.height*0.9f), Color.white, 0.4f);
			
			GUI.Label (new Rect (20, 30, Screen.width/2, 70), "MISSION OBJECTIVES", StyleText(futura_font, 35, TextAnchor.MiddleLeft, Color.white));
			
			int counter = 0;
			//Displays objectives

			for (int i = 0; i < mission.OBJ_LEVEL01.Length; i++)
			{
				GUI.Label(new Rect(20, 100 + counter, Screen.width / 2, 70), "- " + mission.OBJ_LEVEL01[i], StyleText(futura_font, 20, TextAnchor.MiddleLeft, Color.white));
				counter+= 35;
			}
			

			GUI.Label (new Rect ((Screen.width/2)+20, 30, Screen.width*0.4f, 70), "PAUSED", StyleText(futura_font, 35, TextAnchor.MiddleRight, Color.white));
			
			var menu_buttons = new GUIStyle();
			menu_buttons = new GUIStyle(GUI.skin.button);
			menu_buttons.normal.textColor = Color.white;
			menu_buttons.normal.background = button_blank;
			menu_buttons.hover.textColor  = Color.cyan;
			menu_buttons.hover.background = button_blue;
			menu_buttons.font = futura_font;
			menu_buttons.fontSize = 20;
			menu_buttons.alignment = TextAnchor.MiddleRight;
			
			//Return game
			if(GUI.Button (new Rect ((Screen.width/2)+20, 120, Screen.width*0.4f, 30), new GUIContent("Resume", "RESUME"), menu_buttons)){
				mission.ISMENU = false;
			}
			
			//Return Main Menu
			if(GUI.Button (new Rect ((Screen.width/2)+20, 160, Screen.width*0.4f, 30), new GUIContent("Quit", "QUIT"), menu_buttons)){
				mission.STATEPIVOT = 2; // Set to Main menu
				Application.LoadLevel("main_menu"); // Go to the main_menu.unity
			}
		}

		//************************************************************************
		// Mission fail screen
		public static void MissionAbort(Font futura_font, Texture2D button_blank, Texture2D button_blue, Texture2D button_blue_hover)
		{
			var menu_buttons = new GUIStyle();
			menu_buttons = new GUIStyle(GUI.skin.button);
			menu_buttons.normal.textColor = Color.white;
			menu_buttons.normal.background = button_blank;
			menu_buttons.hover.textColor  = Color.cyan;
			menu_buttons.hover.background = button_blue;
			menu_buttons.font = futura_font;
			menu_buttons.fontSize = 20;
			menu_buttons.alignment = TextAnchor.MiddleCenter;
			
			DrawRectangle (new Rect (0,0,Screen.width, Screen.height), new Color(94/255f,0f,0f, 2f),0.9f);
			GUI.Label (new Rect (0, Screen.height/2, Screen.width, 70), "MISSION FAILED", StyleText(futura_font, 35, TextAnchor.MiddleCenter, Color.white));
			
			//Restart game
			if(GUI.Button (new Rect ((Screen.width/2)-150, (Screen.height/2)+50, 100, 80), new GUIContent("Play Again", "PLAY AGAIN"), menu_buttons)){
				character.TRACK_OBJECTIVE = 0;
				//character.HEALTH = 100;
				mission.ISFAIL = !mission.ISFAIL;
				Application.LoadLevel("level_01");
			}
			
			//Return Main Menu
			if(GUI.Button (new Rect ((Screen.width/2)+50, (Screen.height/2)+50, 100, 70), new GUIContent("Quit", "QUIT"), menu_buttons)){
				mission.STATEPIVOT = 2; // Set to Main menu
				//character.HEALTH = 100;
				mission.ISFAIL = !mission.ISFAIL;
				Application.LoadLevel("main_menu"); // Go to the main_menu.unity
			}
		}

		//************************************************************************
		public static void MissionSuccess(Font futura_font, Texture2D button_blank, Texture2D button_blue, Texture2D button_blue_hover)
		{
			var menu_buttons = new GUIStyle();
			menu_buttons = new GUIStyle(GUI.skin.button);
			menu_buttons.normal.textColor = Color.white;
			menu_buttons.normal.background = button_blank;
			menu_buttons.hover.textColor  = Color.cyan;
			menu_buttons.hover.background = button_blue;
			menu_buttons.font = futura_font;
			menu_buttons.fontSize = 20;
			menu_buttons.alignment = TextAnchor.MiddleCenter;
			
			DrawRectangle (new Rect (0,0,Screen.width, Screen.height), new Color(0/255f,58/255f,81/255f, 1f),0.9f);
			GUI.Label (new Rect (0, Screen.height/2, Screen.width, 70), "MISSION ACCOMPLISHED", StyleText(futura_font, 35, TextAnchor.MiddleCenter, Color.white));
			
			//Restart game
			if(GUI.Button (new Rect ((Screen.width/2)-150, (Screen.height/2)+50, 100, 80), new GUIContent("Play Again", "PLAY AGAIN"), menu_buttons)){
				character.TRACK_OBJECTIVE = 0;
				//character.HEALTH = 100;
				mission.ISFAIL = !mission.ISFAIL;
				Application.LoadLevel("level_01");
			}
			
			//Return Main Menu
			if(GUI.Button (new Rect ((Screen.width/2)+50, (Screen.height/2)+50, 100, 70), new GUIContent("Quit", "QUIT"), menu_buttons)){
				mission.STATEPIVOT = 2; // Set to Main menu
				//character.HEALTH = 100;
				mission.ISFAIL = !mission.ISFAIL;
				Application.LoadLevel("main_menu"); // Go to the main_menu.unity
			}
		}


		//************************************************************************
		// Displays the gameplay UI
		public static void GamePlayUI()
		{
			GUI.Label (new Rect (20, 10, Screen.width/2, 40), "HEALH: "+character.health, StyleText(font_futura, 18, TextAnchor.MiddleLeft, Color.white));
			//GUI.Label (new Rect (20, 35, Screen.width/2, 40), "METABOLISM RATE", StyleText(font_futura, 18, TextAnchor.MiddleLeft, Color.white));
			
			GUI.Label (new Rect ((Screen.width/2) - 20, 10, Screen.width/2, 40), "MISSION DURATION", StyleText(font_futura, 18, TextAnchor.MiddleRight, Color.white));
			GUI.Label (new Rect ((Screen.width/2) - 20, 35, Screen.width/2, 40), timerFormatted, design.StyleText(design.Font_Futura, 18, TextAnchor.MiddleRight, Color.white));
		}

		//************************************************************************
		public static void LevelStatusUI()
		{
			//LEVEL 01 OBJECTIVES

			switch(mission.IS_OBJ_DONE)
			{
			case true:
				DrawRectangle(new Rect(Screen.width/2, 85, Screen.width/2, 50), new Color(41/255.0f, 159/255.0f, 22/255.0f, 1f), 0.8f);
				GUI.Label (new Rect ((Screen.width/2) + 20, 90, Screen.width/2, 40), "OBJECTIVE COMPLETED!", StyleText(font_futura, 30, TextAnchor.MiddleLeft, Color.white));
				break;
			case false:
				DrawRectangle(new Rect(Screen.width/2, 75, Screen.width/2, 100), new Color(22/255.0f, 77/255.0f, 159/255.0f, 1f), 0.8f);
				GUI.Label (new Rect ((Screen.width/2) + 20, 90, Screen.width/2, 40), "NEW OBJECTIVE", StyleText(font_futura, 30, TextAnchor.MiddleLeft, Color.white));
				GUI.Label (new Rect ((Screen.width/2) + 20, 125, Screen.width/2, 30), mission.OBJ_LEVEL01[mission.LEVELPIVOT], StyleText(font_futura, 18, TextAnchor.MiddleLeft, Color.white));
				break;
			}
		}
	}
}
