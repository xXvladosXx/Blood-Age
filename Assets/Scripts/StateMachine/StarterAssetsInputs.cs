using UnityEngine;


	using System;
using MouseSystem;
using UI.Stats;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StarterAssetsInputs : MonoBehaviour
	{
		public bool ButtonInput;
		public bool RollInput;
		public bool CancelInput;
		public bool SecondSkillCast;

		private StarterAssets input;
		
		private void Start()
		{
			input = new StarterAssets();

			input.Enable();

			input.Player.ButtonInput.performed += context =>
			{
				ButtonInput = true;
			};
			input.Player.ButtonInput.canceled += context =>
			{
				ButtonInput = false;
			};

			input.Player.CancelInput.performed += context => CancelInput = true;
			input.Player.CancelInput.canceled += context => CancelInput = false;
		}
	}
	
