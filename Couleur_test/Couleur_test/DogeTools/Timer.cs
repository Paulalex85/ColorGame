using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Couleur_test.DogeTools
{
    public class Timer
    {
        public float _timer_max, _timer;

        public Timer(float timer_max)
        {
            _timer_max = timer_max;
            _timer = 0f;
        }

        public bool IncreaseTimer(float gameTime)
        {
            _timer += gameTime;
            if (_timer > _timer_max)
            {
                _timer = 0f;
                return true;
            }
            return false;
        }
    }
}
