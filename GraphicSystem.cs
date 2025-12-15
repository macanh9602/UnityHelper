using DP.Utilities;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VTLTools;

namespace RobotGTA.System
{
    public class GraphicSystem : Singleton<GraphicSystem>
    {
        private void OnEnable()
        {
            if (DeviceInfo.IsWeakDevice())
                ChangeGraphicPreset(GraphicPreset.Low);
            else
                ChangeGraphicPreset(GraphicPreset.High);
        }

        public void ChangeGraphicPreset(GraphicPreset _graphicPreset)
        {
            //UserDataManager.CurrentGraphicPreset = _graphicPreset;
            QualitySettings.SetQualityLevel((int)_graphicPreset, true);

            //switch (_graphicPreset)
            //{
            //    case GraphicPreset.High:
            //        Application.targetFrameRate = 60;
            //        break;
            //    case GraphicPreset.Low:
            //        Application.targetFrameRate = 30;
            //        break;
            //} 

            DPDebug.Log("<color=yellow>Quality setting now:</color> " + _graphicPreset);
        }
    }

    public enum GraphicPreset
    {
        None,
        Low,
        High,
    }

}
