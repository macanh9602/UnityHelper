using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

namespace VTLTools
{
    public static class IOUtils
    {
        public static void SaveStringToFile(string _filePath, string _textToSave)
        {
            File.WriteAllText("Assets/Resources/" + _filePath + ".txt", _textToSave);
        }
        public static string LoadFileToString(string _filePath)
        {
            TextAsset _textAss = (TextAsset)Resources.Load(_filePath);
            return _textAss.text;
        }

        public static IEnumerator IEGetRequest(string _uri, System.Action<bool, string> _onCompleted)
        {
            UnityWebRequest _webRequest = UnityWebRequest.Get(_uri);
            yield return _webRequest.SendWebRequest();
            //
            if (_webRequest.result == UnityWebRequest.Result.ConnectionError || _webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                _onCompleted.Invoke(false, _webRequest.error);
            }
            else
            {
                string _jsonResponse = _webRequest.downloadHandler.text;
                _onCompleted.Invoke(true, _jsonResponse);
            }
        }

    }
}