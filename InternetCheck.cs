using AntiStress.UI;
using DP.Utilities;
using UnityEngine;
using VTLTools;

public class InternetCheck
{
    public static void CheckInternetConnection()
    {
        NetworkReachability reachability = Application.internetReachability;

        if (reachability == NetworkReachability.NotReachable)
        {
            DPDebug.Log("<color=red>Device is NOT connected to the internet!</color>");
            if (!UIManager.Instance.noInternetPopup.IsShow)
                UIManager.Instance.noInternetPopup.Show();
        }
        else
        {
            DPDebug.Log("<color=yellow>Device is connected to the internet.</color>");
            if (UIManager.Instance.noInternetPopup.IsShow)
                UIManager.Instance.noInternetPopup.Hide();
        }
    }
}
