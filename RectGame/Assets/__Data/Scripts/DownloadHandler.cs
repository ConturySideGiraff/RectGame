using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadHandler : MonoBehaviour
{
    // document
    // https://docs.unity3d.com/kr/2021.3/Manual/UnityWebRequest-CreatingDownloadHandlers.html

    public async UniTask BackSpriteDownloadAsync()
    {
        var savePath = PathCombine("Sprites", "Card", "TestDownload", "test0.png");
        const string downUrl = "https://github.com/ConturySideGiraff/RectGame/blob/main/RectGame/Assets/__Data/Sprites/apple.png";
        
        await Download(downUrl, savePath);
    }
    
    private string PathCombine(params string[] units)
    {
#if UNITY_EDITOR
        var result = units.Aggregate((p0, p1) => PathCombine(PathCombine(Application.dataPath, p0), p1));
#endif
        return result;
    }

    private async UniTask<bool> Download(string downloadUrl, string savePath)
    {
        // file check (un absolute file), remove
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        // request
        var request = new UnityWebRequest(downloadUrl, UnityWebRequest.kHttpVerbGET)
        {
            downloadHandler = new DownloadHandlerFile(savePath)
        };

        // send
        await request.SendWebRequest();

        // result
        return request.result == UnityWebRequest.Result.Success;
    }
}
