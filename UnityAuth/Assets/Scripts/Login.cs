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
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _registerButton;
    
    [Header("Parameters")]
    [SerializeField] private string _authHost = "http://127.0.0.1";
    [SerializeField] private string _authLoginRoute = "/account/login";
    [SerializeField] private string _authRegisterRoute = "/account/create";
    [SerializeField] private int _authPort = 13756;

    private void Awake() {
        _loginButton.onClick.AddListener(() => {
            StartCoroutine(CoroTrySubmitInfo());
        });
        
        _registerButton.onClick.AddListener(() => {
            StartCoroutine(CoroTryRegister());
        });
    }

    private IEnumerator CoroTrySubmitInfo() {

        _descLabel.text = "Signing in...";
        
        var username = _nameInput.text;
        var password = _passwordInput.text;

        if (username.Length < 3 || password.Length < 3) {
            _descLabel.text = "Invalid credentials";
            yield break;
        }
        
        SetButtonsEnabled(false);
        
        var href = GetFullHref(_authLoginRoute);

        var form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        
        var request = UnityWebRequest.Post(href, form);
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
                
                var account = JsonUtility.FromJson<UserAccount>(request.downloadHandler.text);
                
                _descLabel.text = account._id + "Welcome " + account.username;
                SetButtonsEnabled(true);
                
            } else {
                _descLabel.text = "Login failed";
                _loginButton.interactable = false;
            }
            
            Debug.Log(request.downloadHandler.text);
            
        } else {
            _descLabel.text = "Connection error";
            SetButtonsEnabled(true);
            Debug.Log("Connection error");
        }
    }

    private IEnumerator CoroTryRegister() {
        
        SetButtonsEnabled(false);
        
        _descLabel.text = "Registering in...";
        
        var username = _nameInput.text;
        var password = _passwordInput.text;

        if (username.Length < 3 || password.Length < 3) {
            _descLabel.text = "Invalid credentials";
            yield break;
        }
        
        SetButtonsEnabled(false);
        
        var href = GetFullHref(_authRegisterRoute);

        var form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        
        var request = UnityWebRequest.Post(href, form);
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
                
                Debug.Log(request.downloadHandler);
                
                var account = JsonUtility.FromJson<UserAccount>(request.downloadHandler.text);
                
                Debug.Log(account.ToString());
                
                //_descLabel.text = account._id + "Welcome " + account.username;
                _descLabel.text = "Successfully register " + account._id;
                SetButtonsEnabled(true);
                
            } else {
                _descLabel.text = "Login failed";
                _loginButton.interactable = false;
            }
            
            Debug.Log(request.downloadHandler.text);
            
        } else {
            _descLabel.text = "Connection error";
            SetButtonsEnabled(true);
            Debug.Log("Connection error");
        }
    }

    private void SetButtonsEnabled(bool flag) {
        _loginButton.interactable = flag;
        _registerButton.interactable = flag;
    }
    
    private string GetFullHref(string route) {
        return _authHost + ":" + _authPort + route;
    }
}
