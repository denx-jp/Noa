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

    [SerializeField] private Text _subjectText;  // CSVの情報を表示するGameObject

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

        // indexを1にセット
        ShowCurrentIndex(1);
        _subjectText.text = GetSubjectText(1);
        currentIndex = 1;
    }

    private void Update()
    {
        // Enterキーでカード名表示
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var index = Random.Range(1, _subjectsList.Count);
            ShowCurrentIndex(index);
            _subjectText.text = GetSubjectText(index);
            currentIndex = index;
        }
    }

    private string GetSubjectText(int index)
    {
        string _str = "";
        int _length;
        foreach (var w in _subjectsList[index])
        {
            _length = (w.Length == Encoding.GetEncoding("shift_jis").GetByteCount(w)) ? (w.Length / 2) : w.Length;

            if (_length > 9) _str += "<size=2>" + w + "</size>" + "\r\n";
            else if (_length > 7) _str += "<size=3>" + w + "</size>" + "\r\n";
            else _str += w + "\r\n";
        }
        return _str;
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
        _subjectText.text = GetSubjectText(index);
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
        _subjectText.text = GetSubjectText(index);
        currentIndex = index;
    }
}
