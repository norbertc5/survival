using System;
using System.Collections.Generic;
using UnityEngine;

namespace norbertcUtilities.ActionOnTime
{
    public class ActionOnTime : MonoBehaviour
    {
        Action action;
        float delay;
        new string name;
        static List<ActionOnTime> workingActions = new List<ActionOnTime>();

        /// <summary>
        /// Create new action. Pass 'name' to make it stopable.
        /// </summary>
        /// <param name="action">aaa</param>
        /// <param name="delay">bbb</param>
        /// <param name="name"></param>
        public static void Create(Action action, float delay, string name = null)
        {
            // create gameObject with ActionOnTime component
            // this gameObject handles one action

            GameObject gameObject = new GameObject($"ActionOnTime_{name}", typeof(ActionOnTime));
            ActionOnTime actionOnTime = gameObject.GetComponent<ActionOnTime>();
            gameObject.hideFlags = HideFlags.HideInInspector;
            actionOnTime.action = action;
            actionOnTime.delay = delay;
            actionOnTime.name = name;
            workingActions.Add(actionOnTime);
        }

        /// <summary>
        /// Stop action with passed name.
        /// </summary>
        /// <param name="name"></param>
        public static void Stop(string name)
        {
            for (int i = 0; i < workingActions.Count; i++)
            {
                if (workingActions[i].name == name)
                {
                    workingActions[i].DestroySelf();
                }
            }
        }

        void DestroySelf()
        {
            workingActions.Remove(gameObject.GetComponent<ActionOnTime>());
            Destroy(gameObject);
        }

        private void Update()
        {
            delay -= Time.deltaTime;

            if (delay <= 0)
            {
                action();
                DestroySelf();
            }
        }
    }
}