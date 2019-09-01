using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleServices
{
    public class GoogleSheetsExampleScript : MonoBehaviour
    {
        public GoogleSheetsService service;

        private void Start()
        {
            var data = service.GetData("A2:C8");

            print("This is the link to the sheet. Edit it and see the results! \n" +
                "https://docs.google.com/spreadsheets/d/1mqWbcEp29vkSOVxh7w2gthWR-CjDSQntWVtovM3rXUY/edit?usp=sharing");

            foreach (var item in data)
            {
                print($"Player ID: {item[0]}, Class: {item[1]}, Gold: {item[2]}");
            }
        }
    }
}
