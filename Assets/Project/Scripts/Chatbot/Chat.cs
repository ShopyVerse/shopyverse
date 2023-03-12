using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour {
    [SerializeField] private RectTransform content;
    [SerializeField] private Text chatLine;
    [SerializeField] private InputField input;

    public void Start() {
        input.onSubmit.AddListener(s => {
            var newLine = ((GameObject) Instantiate(chatLine.gameObject)).GetComponent<Text>();
            newLine.text = s;
            newLine.gameObject.SetActive(true);
            newLine.rectTransform.parent = content;

            //input.value = "";
        });
    }
}
