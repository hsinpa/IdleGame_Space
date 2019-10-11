using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM.Model {
    public class PMUtility
    {

        public TimeScale currentTimeScale;
        private Dictionary<TimeScale, float> _timeScaleMapTable;
        private int baseHourWidth = 50;

        public enum TimeScale { 
            Hour, Day, Week
        }

        public PMUtility()
        {
            _timeScaleMapTable = new Dictionary<TimeScale, float> {
                {TimeScale.Hour , 1},
                {TimeScale.Day , 24},
                {TimeScale.Week , 168}
            };
        }

        public float GetTimeToWorldValue(float p_value) {

            try {
                return p_value * (baseHourWidth / _timeScaleMapTable[currentTimeScale]);
            }
            catch {
            }
            return 0;
        }


    }
}