using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Utilities
    {
        private static Dictionary<float, WaitForSeconds> _waitList;
        
        /// <summary>
        /// Store wfs for next usage for better optimization
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static WaitForSeconds GetWait(this float time)
        {
            if (_waitList == null) _waitList = new Dictionary<float, WaitForSeconds>();
            if (!_waitList.ContainsKey(time))
            {
                WaitForSeconds waitTime = new WaitForSeconds(time);
                _waitList.Add(time, waitTime);
                return waitTime;
            }
            else
            {
                return _waitList[time];
            }
        }
    }
}
