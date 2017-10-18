﻿namespace Mvc.Controls
{
    public enum HttpVerb
    {
        GET,
        POST        
    }

    public class SourceCallBackOptions
    {
        /// <summary>
        /// Gets or sets the JavaScript function to call after the data is successfully loaded.
        /// This function could be used to modify the loaded data before it's used to fill the drop down. 
        /// The format of the data should remain the same
        /// </summary>
        public string OnSuccessGetData { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript function to call if get data request results with failure.
        /// The function receives the following parameters: responseText, responseStatus, statusText
        /// </summary>
        public string OnFailureGetData { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript function to call when get data request is completed (both in case of success and failure).
        /// </summary>
        public string OnCompleteGetData { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript function to call before data call is made to the server.
        /// This function could be used to modify the data on the form before the data call is triggered.
        /// Function gets as argument an object that is going to be sent to the server. This will be later manipulated in the "ManipulateDataSend" function call
        /// Function returns void; Sub-Routine
        /// </summary>
        public string BeforeDataLoad { get; set; }



        public string OnChange { get; set; }



        /// <summary>
        /// Gets or sets the JavaScript function to call before data is sent to server.
        /// This function could be used to modify the data that is sent to server.
        /// Function gets as argument an object that is going to be sent to the server (By default will have single property with name specified in ajaxActionParamName and the value of parent dropdown)
        /// Function should return an object that is going to be sent to server in order to fetch the data.
        /// </summary>
        public string ManipulateDataSend { get; set; }

        /// <summary>
        /// GET or POST (GET is default)
        /// </summary>
        public HttpVerb HttpMethod { get; set; }
    }



    public class TextCallBackOptions
    {      
        public string OnChange { get; set; } 
    }

    public class CheckCallBackOptions
    {
        public string OnChange { get; set; }
    }
}
