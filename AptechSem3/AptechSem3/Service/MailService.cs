using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;

namespace AptechSem3.Service
{
    public class MailService
    {
        //path to current application directory
        static String path = System.AppDomain.CurrentDomain.BaseDirectory;
        //google mail scopes
        static string[] Scopes = { GmailService.Scope.GmailSend };
        // my application name
        static string ApplicationName = "Gmail API .NET Quickstart";
        //base 64 encode to send mail
        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
        /// <summary>
        /// send mail
        /// </summary>
        /// <param name="toAddress">destination address</param>
        /// <param name="subject">subject of mail</param>
        /// <param name="Content">type: html,text </param>
        /// <returns></returns>
        public bool sendMail(String toAddress, String subject, String Content)
        {
            try
            {
                UserCredential credential;
                using (var stream =
                    new FileStream( path + "credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = path + "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    var service = new GmailService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName,
                    });
                    string plainText = $"To: {toAddress}\r\n" +
                               $"Subject: {subject}\r\n" +
                               "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                               $"{Content}";
                    var newMsg = new Google.Apis.Gmail.v1.Data.Message();
                    newMsg.Raw = Base64UrlEncode(plainText.ToString());
                    service.Users.Messages.Send(newMsg, "me").Execute();
                }
                
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void changeMail()
        {
            try
            {
                //get path to json directory
                string startupPath = Path.GetFullPath(Path.Combine(path, @"token.json"));
                //delete directory where user google account is saved
                Directory.Delete(startupPath);
                //create json dir again
                UserCredential credential;
                using (var stream =
                    new FileStream(path + "credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = path + "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        } 
    }
}