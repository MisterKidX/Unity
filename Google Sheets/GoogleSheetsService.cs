using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = System.Object;
using System.Linq;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace GoogleServices
{
    public class GoogleSheetsService : MonoBehaviour
    {

        public string applicationName = "Unity Project";
        public string spreadsheetId = "Your Sheet ID";
        public string activeSheet = "";

        private SheetsService _service;
        private string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private StringBuilder _sb = new StringBuilder();

        [SerializeField] [HideInInspector]
        private string credentials = "credentials.json";
        [SerializeField]
        [HideInInspector]
        private string tokenResponse = "TokenResponse-user";

        private void Awake()
        {
            _service = AuthorizeGoogleApp();
        }

        /// <summary>
        /// Gets data from a given range.
        /// </summary>
        /// <param name="range">The range given as "X1:Y2" </param>
        /// <returns>rows[Columns][Cell]</returns>
        public IList<IList<Object>> GetData(string range)
        {
            range = FullPath(range);

            SpreadsheetsResource.ValuesResource.GetRequest request =
                    _service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            return response.Values;
        }

        /// <summary>
        /// Gets data from a specific cell
        /// </summary>
        /// <param name="cell">given as "X1" </param>
        /// <returns>returns the object contained in the cell</returns>
        public object GetCellData(string cell)
        {
            _sb.Clear();
            _sb.Append(cell);
            _sb.Append(":");
            _sb.Append(cell);
            cell = FullPath(cell);

            SpreadsheetsResource.ValuesResource.GetRequest request =
        _service.Spreadsheets.Values.Get(spreadsheetId, cell);

            ValueRange response = request.Execute();
            return response?.Values?[0][0];
        }

        /// <summary>
        /// Inserts (overwrite) a row anywhere
        /// </summary>
        /// <param name="startingCell">given as "X1"</param>
        /// <param name="rowData"></param>
        public void WriteRow(string startingCell, IList<Object> rowData)
        {
            var val = new List<IList<Object>>();
            val.Add(rowData);

            WriteData(val, startingCell);
        }

        /// <summary>
        /// Writes data starting from a specific cell
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startCell">given as "X1"</param>
        public void WriteData(IList<IList<Object>> values, string startCell)
        {
            startCell = FullPath(startCell);

            SpreadsheetsResource.ValuesResource.UpdateRequest request =
   _service.Spreadsheets.Values.Update(new ValueRange() { Values = values }, spreadsheetId, startCell);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var response = request.Execute();
        }

        private SheetsService AuthorizeGoogleApp()
        {
            UserCredential credential;

#if UNITY_ANDROID && !UNITY_EDITOR

            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
            var credFileName = Path.GetFileNameWithoutExtension(credentials);
            var tokenFileName = Path.GetFileNameWithoutExtension(tokenResponse);

            var objCred = Resources.Load<TextAsset>(credFileName);
            // The file must be saved as json or unity can't load it   
            var verf = Resources.Load<TextAsset>(tokenFileName);

            var partPath = Path.Combine(Application.persistentDataPath, "credentials");
            var pathCred = Path.Combine(partPath, "credentials.json");
            var pathVerf = Path.Combine(partPath, "Google.Apis.Auth.OAuth2.Responses.TokenResponse-user");

            if (!Directory.Exists(partPath))
            {
                Directory.CreateDirectory(partPath);
            }

            if (File.Exists(pathCred) && File.Exists(pathVerf))
            {
                credentials = pathCred;
                print(Application.persistentDataPath);
            }
            // We are doing this because authorizing thorigh web causes unexpected bugs on android
            else
            {
                File.WriteAllText(pathCred, objCred.text);
                File.WriteAllText(pathVerf, verf.text);

                credentials = pathCred;
            }

            using (var stream = new StreamReader(credentials))
            {

                var credPath = Path.Combine(Application.persistentDataPath, "credentials");
                FileDataStore store = new FileDataStore(credPath, true);

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream.BaseStream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    store).Result;
            }

#else
            using (var stream = new FileStream(credentials, FileMode.Open, FileAccess.Read))
            {
                string credPath = Path.Combine(Application.dataPath, "Google Sheets", "Resources");
                var store = new FileDataStore(credPath, true);

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    store).Result;

                var f = Directory.GetFiles(credPath).Where(fi => fi.Contains("Google.Apis")).ToArray()[0];
                var tokenPath = Path.Combine(Application.dataPath, "Google Sheets", "Resources",
                    "TokenResponse.json");

                if (!File.Exists(tokenPath))
                {
                    File.Move(f, tokenPath);
                }

                print("Credential file saved to: " + tokenPath);
            }
#endif

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            return service;
        }

        private string FullPath(string cellRange)
        {
            _sb.Clear();
            _sb.Append(activeSheet);
            _sb.Append("!");
            _sb.Append(cellRange);

            return _sb.ToString();
        }

    }
}
