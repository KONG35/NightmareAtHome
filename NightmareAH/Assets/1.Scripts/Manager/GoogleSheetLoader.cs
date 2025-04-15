using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GoogleSheetLoader : Singleton<GoogleSheetLoader>
{
    private const string SHEETURL = "https://docs.google.com/spreadsheets/d/1ezhz3PkzM-KgIL1tFaIYw-yYjWiFmQpQnu1YuaTkTBQ/export?format=csv&gid=";
    protected override void Awake()
    {
        base.Awake();
    }

    private Dictionary<Type, IList> dataLists = new Dictionary<Type, IList>();
    private Dictionary<Type, string> csvURLs = new Dictionary<Type, string>
    {
        { typeof(MeleeMonsterSheetData), SHEETURL+"0" },

        { typeof(RangedMonsterSheetData), SHEETURL+"329945714" },

        { typeof(UITextData), SHEETURL+"938497610" },

        { typeof(SpawnSheetData), SHEETURL+"1593328088" }
    };

    private void Start()
    {
        StartCoroutine(LoadAllCSVs());
    }
    public IEnumerator LoadAllCSVs()
    {
        foreach (var entry in csvURLs)
        {
            Type dataType = entry.Key;
            string url = entry.Value;

            // dataType이 IBaseSheetData를 구현하는지 확인
            if (typeof(IBaseSheetData).IsAssignableFrom(dataType))
            {
                // 리플렉션으로 LoadCSV<T> 메서드의 MethodInfo를 가져옵니다.
                var methodInfo = this.GetType().GetMethod("LoadCSV", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                // 제네릭 타입 파라미터를 dataType으로 설정합니다.
                var genericMethod = methodInfo.MakeGenericMethod(dataType);

                // 제네릭 메서드를 호출한 후 yield return을 통해 코루틴을 실행합니다.
                yield return StartCoroutine((IEnumerator)genericMethod.Invoke(this, new object[] { dataType, url }));
            }
            else
            {
                Debug.LogWarning($"{dataType.Name}은(는) IBaseSheetData를 구현하지 않습니다.");
            }
        }
    }

    private IEnumerator LoadCSV<T>(Type type, string url) where T : IBaseSheetData, new()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string csvData = request.downloadHandler.text;
                ParseCSV<T>(type, csvData);
            }
            else
            {
                Debug.LogError($"{type.Name} CSV 불러오기 실패: {request.error}");
            }
        }
    }

    private void ParseCSV<T>(Type type, string csv) where T : IBaseSheetData, new()
    {
        string[] lines = csv.Split('\n');
        List<T> list = new List<T>();

        // 첫 줄은 헤더
        for (int i = 1; i < lines.Length; i++) 
        {
            string[] values = lines[i].Split(',');
            if (values.Length == 0) continue;

            T obj = new T();
            obj.Parse(values);

            Debug.Log($"{string.Concat(lines)}");

            list.Add(obj);
        }

        dataLists[type] = list;
        Debug.Log($"{type.Name} {list.Count}개 로드 완료!");
    }

    public List<T> GetDataList<T>() where T : IBaseSheetData
    {
        Type type = typeof(T);
        return dataLists.ContainsKey(type) ? (List<T>)dataLists[type] : new List<T>();
    }
    [Button]
    private void LoadIngameSceneEdit()
    {
        SceneManager.LoadScene("2.Ingame", LoadSceneMode.Additive);
    }
}
