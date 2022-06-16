using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PauseSystem
{
    public class ProjectContext : MonoBehaviour
    {
        public static ProjectContext Instance { get; private set; }
        public PauseManager PauseManager { get; private set; }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            
            PauseManager = new PauseManager();
        }
    }
}