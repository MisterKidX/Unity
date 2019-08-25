using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using UnityEngine;
using Object = System.Object;

namespace GoogleServices
{
    class GoogleSheetsService : MonoBehaviour
    {

        public string applicationName = "Unity Project";
        public string spreadsheetId = "Your Sheet ID";
        public string activeSheet = "";

        private SheetsService _service;
        private string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private StringBuilder _sb = new StringBuilder();

        private void Awake()
        {
            _service = AuthorizeGoogleApp();
        }

        private SheetsService AuthorizeGoogleApp()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                print("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            return service;
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
            return response?.Values[0][0];
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