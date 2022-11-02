using Machina.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Machina.Service
{
    internal static class CognitiveService
    {

        private static readonly string API_KEY = "";

        public static async Task<FaceDetectResult>  FaceDetect( Stream imageStream )
        {
            if ( imageStream == null)
            {
                return null;
            }
            var url = "http://codeavecjonathan.pythonanywhere.com/face";

            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
                    webClient.Headers.Add("Ocp-Apim-Subscripton-Key", API_KEY);

                    
                    var data = ReadStream( imageStream );

                    var result = await Task.Run(() =>  webClient.UploadData(url, data));

                   // var result = webClient.UploadData(url, data);

                    if ( result == null)
                    {
                        return null;
                    }
                    string json = Encoding.UTF8.GetString(result, 0, result.Length);
                    var faceResult = Newtonsoft.Json.JsonConvert.DeserializeObject<FaceDetectResult[]>(json);

                    if (faceResult.Length >= 1)
                    {
                        return faceResult[0] ;
                    }
                    Console.WriteLine("Réponse OK : " + json);
                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception webclient : " + ex.Message);
                }
                return null;    
            }
            

        }

        private static byte[] ReadStream(Stream input)
        {
            BinaryReader b = new BinaryReader(input);
            byte[] data = b.ReadBytes((int)input.Length);
            return data;
        }
    }
}
