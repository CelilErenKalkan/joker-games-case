using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Utilities
    {
        private static Dictionary<float, WaitForSeconds> waitList;
        
        /// <summary>
        /// Store wfs for next usage for better optimization
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static WaitForSeconds GetWait(this float time)
        {
            if (waitList == null) waitList = new Dictionary<float, WaitForSeconds>();
            if (!waitList.ContainsKey(time))
            {
                WaitForSeconds waitTime = new WaitForSeconds(time);
                waitList.Add(time, waitTime);
                return waitTime;
            }
            else
            {
                return waitList[time];
            }
        }
    }
}
