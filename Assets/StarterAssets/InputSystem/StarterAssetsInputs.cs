using UnityEngine;


	using System;
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
				print("Smt");
			};
			input.Player.ButtonInput.canceled += context => ButtonInput = false;

			input.Player.CancelInput.performed += context => CancelInput = true;
			input.Player.CancelInput.canceled += context => CancelInput = false;
		}

		private void Update()
		{
			
		}
		
		
		public Ray GetRay()
		{
			Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

			return ray;
		}

	}
	
