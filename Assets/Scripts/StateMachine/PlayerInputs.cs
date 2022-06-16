using PauseSystem;
using UnityEngine;

namespace StateMachine
{
	public class PlayerInputs : MonoBehaviour
	{
		public bool ButtonInput;
		public bool CancelInput;

		private StarterAssets _input;
		private bool IsPaused => ProjectContext.Instance.PauseManager.IsPaused;

		private void Awake()
		{
			_input = new StarterAssets();

			_input.Enable();

			_input.Player.ButtonInput.performed += context => { ButtonInput = !IsPaused; };
			_input.Player.ButtonInput.canceled += context =>
			{
				ButtonInput = false;
			};

			_input.Player.CancelInput.performed += context => CancelInput = true;
			_input.Player.CancelInput.canceled += context => CancelInput = false;
		}

		public void DisableInput()
		{
			enabled = false;
		}
		
		public void EnableInput()
		{
			enabled = true;
		}
	}
}
	
