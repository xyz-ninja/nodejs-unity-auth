using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour {

    [Header("Components")] 
    [SerializeField] private TextMeshProUGUI _descLabel;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Button _submitButton;
    
    [Header("Parameters")]
    [SerializeField] private string _authHost = "http://127.0.0.1";
    [SerializeField] private string _authRoute = "/account";
    [SerializeField] private int _authPort = 13756;

    private void Awake() {
        _submitButton.onClick.AddListener(() => {
            StartCoroutine(TrySubmitInfoCoro());
        });
    }

    private IEnumerator TrySubmitInfoCoro() {

        _descLabel.text = "Signing in...";
        
        var username = _nameInput.text;
        var password = _passwordInput.text;

        if (username.Length < 3 || password.Length < 3) {
            _descLabel.text = "Invalid credentials";
            yield break;
        }
        
        _submitButton.interactable = false;
        
        var href = _authHost + ":" + _authPort + _authRoute;
        
        var request = UnityWebRequest.Get($"{href}?username={username}&password={password}");
        var handler = request.SendWebRequest();

        var handlerTime = 0f;
        while (handler.isDone == false) {
            handlerTime += Time.deltaTime;

            if (handlerTime > 3) {
                break;
            } 
            
            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success) {
            if (true) {
                
                Debug.Log(request.downloadHandler.text);
                
                UserAccount account = JsonUtility.FromJson<UserAccount>(request.downloadHandler.text);
                
                _descLabel.text = account._id + "Welcome " + account.username;
                _submitButton.interactable = true;
                
            } else {
                _descLabel.text = "Login failed";
                _submitButton.interactable = false;
            }
            
            Debug.Log(request.downloadHandler.text);
            
        } else {
            _descLabel.text = "Connection error";
            _submitButton.interactable = true;
            Debug.Log("Connection error");
        }
    }
}
