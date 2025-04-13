using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames {
    internal interface IMinigame {
        void StartGame();
        void EndGame();
        void FailGame();
        void WinGame();
    }
}