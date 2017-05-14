using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all communication with the server. This follows the
/// (Unity) singleton pattern. Server information should be filled out on the
/// login screen.
/// </summary>
public class ServiceController : MonoBehaviour {
  // This is the URL to the service php script. It is where all API calls are
  // sent to.
  string apiUrl = "";

  public delegate void ConnectionCallback(int status);

  /// <summary>
  /// Connect to the specified url with given username and password.
  /// </summary>
  /// <param name="url">URL of the API.</param>
  /// <param name="username">Username.</param>
  /// <param name="password">Password.</param>
  /// <param name = "callback">Callback function.</param>
  public void Connect(string url, string username, string password, ConnectionCallback callback) {
    apiUrl = url;

    // We send in username and password. If successful, we are granted a
    // token which we can use to gain access to editing the map database.
  }
}
