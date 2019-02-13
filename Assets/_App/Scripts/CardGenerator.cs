using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class CardGenerator : MonoBehaviour
{
    [SerializeField] private Text _currentNumText;

    private int currentIndex;

    [SerializeField] private string csvFileName = "cards_data.csv";

    private List<string[]> _subjectsList = new List<string[]>();

    [SerializeField] private GameObject _subjectNameObject;  // CSVの情報を表示するGameObject

    private Text[] _subjectNameTexts;

    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        // CSVデータの読み込み
        var filePath = $"{Application.streamingAssetsPath}/CSV/{csvFileName}";
        using (var sr = new StreamReader(filePath))
        {
            // ストリームの末尾まで繰り返す
            while (!sr.EndOfStream)
            {
                // ファイルから一行読み込む
                var line = sr.ReadLine();
                // 読み込んだ一行をカンマ毎に分けてリストに格納する
                var values = line.Split(',');
                _subjectsList.Add(values);
            }
        }

        // Textオブジェクトの取得
        _subjectNameTexts = _subjectNameObject.GetComponentsInChildren<Text>();

        // indexを1にセット
        ShowCurrentIndex(1);
        ShowSubjectName(1);
        currentIndex = 1;
    }

    private void Update()
    {
        // Enterキーでカード名表示
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var index = Random.Range(1, _subjectsList.Count);
            ShowCurrentIndex(index);
            ShowSubjectName(index);
            currentIndex = index;
        }
    }

    private void ShowSubjectName(int index)
    {
        for (int i = 0; i < _subjectNameTexts.Length; i++)
        {
            _subjectNameTexts[i].text = _subjectsList[index][i];
        }
    }

    private void ShowCurrentIndex(int index)
    {
        _currentNumText.text = $"{index}/{_subjectsList.Count - 1}";
    }

    /// <summary>
    /// UnityEvent用
    /// </summary>
    public void Next()
    {
        var index = currentIndex + 1;
        if (index >= _subjectsList.Count - 1) index = 0;
        ShowCurrentIndex(index);
        ShowSubjectName(index);
        currentIndex = index;
    }

    /// <summary>
    /// UnityEvent用
    /// </summary>
    public void Back()
    {
        var index = currentIndex - 1;
        if (index < 1) index = _subjectsList.Count - 1;
        ShowCurrentIndex(index);
        ShowSubjectName(index);
        currentIndex = index;
    }
}
