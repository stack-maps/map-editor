using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace StackMaps {
  /// <summary>
  /// This class handles all communication with the server. This follows the
  /// (Unity) singleton pattern. Server information should be filled out on the
  /// login screen.
  /// </summary>
  public class ServiceController : MonoBehaviour {
    /// <summary>
    /// The shared instance of the service controller.
    /// </summary>
    public static ServiceController shared;

    // This is the URL to the service php script. It is where all API calls are
    // sent to.
    string apiUrl = "http://zhengfu23.us/api.php";

    // This is our token, acting as a pass for making changes to the database.
    string token = "";

    #region Login

    // The coroutine handling network connection.
    Coroutine loginCoroutine;

    /// <summary>
    /// A callback to Login method called after server responds.
    /// </summary>
    /// <param name="success">Whether the request successful.</param>
    /// <param name="authenticated">Whether login is successful.</param>
    public delegate void LoginCallback(bool success,bool authenticated);

    /// <summary>
    /// Connect to the specified url with given username and password. If there is
    /// already a connecting attempt, does nothing.
    /// </summary>
    /// <param name="url">URL of the API.</param>
    /// <param name="username">Username.</param>
    /// <param name="password">Password.</param>
    /// <param name = "callback">Callback function.</param>
    public void Login(string url, string username, string password, LoginCallback callback) {
      if (loginCoroutine != null) {
        return;
      }

      apiUrl = url;

      // We send in username and password. If successful, we are granted a
      // token which we can use to gain access to editing the map database.
      loginCoroutine = StartCoroutine(LoginLoop(url, username, password, callback));
    }

    IEnumerator LoginLoop(string url, string username, string password, LoginCallback callback) {
      WWWForm form = new WWWForm();
      form.AddField("request", "login");
      form.AddField("username", username);
      form.AddField("password", password);

      // Create a download object
      WWW request = new WWW(apiUrl, form);

      // Wait until the download is done
      yield return request;

      if (!string.IsNullOrEmpty(request.error)) {
        Debug.Log("Unable to login: " + request.error);
        callback(false, false);
      } else {
        JSONNode node = JSON.Parse(request.text);

        if (node["success"]) {
          token = node["token"];
          callback(true, true);
        } else {
          callback(true, false);
        }
      }

      loginCoroutine = null;
    }

    /// <summary>
    /// Cancels the connection. This guarantees that the callback passed in from
    /// connect will never be called. If no current connection is underway, does
    /// nothing.
    /// </summary>
    public void CancelLogin() {
      StopCoroutine(loginCoroutine);
      loginCoroutine = null;
    }

    #endregion

    #region GetLibraryList

    // The coroutine handling network connection.
    Coroutine getLibraryListCoroutine;

    /// <summary>
    /// A callback to Login method called after server responds.
    /// </summary>
    /// <param name="success">Whether the request successful.</param>
    /// <param name="authenticated">Whether login is successful.</param>
    public delegate void GetLibraryListCallback(bool success, List<Library> data);

    /// <summary>
    /// Fetches the list of libraries in the database. Does nothing if there is
    /// already a request under way.
    /// </summary>
    /// <param name="callback">Callback function for data handling.</param>
    public void GetLibraryList(GetLibraryListCallback callback) {
      if (getLibraryListCoroutine != null) {
        return;
      }

      getLibraryListCoroutine = StartCoroutine(GetLibraryListLoop(callback));
    }

    IEnumerator GetLibraryListLoop(GetLibraryListCallback callback) {
      WWWForm form = new WWWForm();
      form.AddField("request", "getLibraryList");

      // Create a download object
      WWW request = new WWW(apiUrl, form);

      // Wait until the download is done
      yield return request;

      if (!string.IsNullOrEmpty(request.error)) {
        Debug.Log("Unable to fetch library list: " + request.error);
        callback(false, null);
      } else {
        Debug.Log(request.text);
        JSONNode root = JSON.Parse(request.text);

        // Parse the library list here.
        List<Library> libList = new List<Library>();

        foreach (JSONNode node in root.AsArray) {
          Library lib = new Library();
          lib.libraryName = node["lname"];
          lib.libraryId = node["lid"];
          libList.Add(lib);
        }

        callback(true, libList);
      }

      getLibraryListCoroutine = null;
    }

    /// <summary>
    /// Cancels the connection. This guarantees that the callback passed in from
    /// connect will never be called. If no current connection is underway, does
    /// nothing.
    /// </summary>
    public void CancelGetLibraryList() {
      StopCoroutine(getLibraryListCoroutine);
      getLibraryListCoroutine = null;
    }

    #endregion


    void Awake() {
      if (shared != null) {
        Destroy(gameObject);
      } else {
        shared = this;
        DontDestroyOnLoad(gameObject);
      }
    }

    /// <summary>
    /// This simply clears the token.
    /// </summary>
    public void Logout() {
      token = "";
    }
  }
}