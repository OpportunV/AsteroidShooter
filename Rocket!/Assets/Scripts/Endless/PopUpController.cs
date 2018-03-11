using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour {

    public Text text;

    public void SetUp(string _text, Color _color) {
        text.text = _text;
        text.color = _color;
    }

}
