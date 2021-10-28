using UnityEngine;


	using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool ButtonInput;
		
		[Header("Movement Settings")]
		public bool analogMovement;

		private StarterAssets input;
		private void Start()
		{
			input = new StarterAssets();

			input.Enable();
		}

		private void Update()
		{
			ButtonInput = Mouse.current.leftButton.isPressed;
		}
		
		
		public Ray GetRay()
		{
			Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

			return ray;
		}

	}
	
