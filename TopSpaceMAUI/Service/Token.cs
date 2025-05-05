using System;
using RestSharp;
using System.Net;
using TopSpaceMAUI.Util;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TopSpaceMAUI.Service
{
    public class Token
    {
        private DAL.Token tokenController;



        public Token()
        {
            tokenController = new DAL.Token();
        }



        public Tuple<LoginStatusCode, string> CreateTokenID(string username, string password)
        {
            RestClient client = null;
            RestRequest request = null;
            RestResponse<Model.Token> response = null;
            string description = string.Empty;

            try
            {
                client = new RestClient(Config.URL_API_BASE);
                request = new RestRequest(Config.URL_API_REQUEST_TOKEN, Method.Post);
                request.AddParameter(Config.URL_API_PARAMETER_USERNAME, username);
                request.AddParameter(Config.URL_API_PARAMETER_PASSWORD, password);

                response = client.Execute<Model.Token>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    description = Description(client, request, response);
                    return new Tuple<LoginStatusCode, string>(LoginStatusCode.UserUnauthorized, description);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        var tokenID = response.Data.TokenID;
                        tokenController.SaveUser(username, tokenID);
                        return new Tuple<LoginStatusCode, string>(LoginStatusCode.UserAuthorized, description);
                    }
                }
            }
            catch (Exception ex)
            {
                description = Description(client, request, response, ex);
                return new Tuple<LoginStatusCode, string>(LoginStatusCode.AuthenticationError, description);
            }

            description = Description(client, request, response);
            return new Tuple<LoginStatusCode, string>(LoginStatusCode.AuthenticationError, description);
        }



        public LoginStatusCode CheckTokenID(string tokenID)
        {
            try
            {
                var client = new RestClient(Config.URL_API_BASE);
                var request = new RestRequest(Config.URL_API_REQUEST_TOKEN);
                request.AddParameter(Config.URL_API_PARAMETER_ID, tokenID);

                RestResponse<Model.Token> response = client.Execute<Model.Token>(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.Gone)
                {
                    return LoginStatusCode.UserUnauthorized;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        if (tokenID == response.Data.TokenID)
                        {
                            return LoginStatusCode.UserAuthorized;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return LoginStatusCode.AuthenticationError;
            }

            return LoginStatusCode.AuthenticationError;
        }

        public string Description(RestClient client, RestRequest request, RestResponse<Model.Token> response, Exception ex = null)
        {
            string description = "-";

            if (response.ErrorException != null)
            {
                try
                {
                    WebException error = (WebException)response.ErrorException;
                    switch (error.Status)
                    {
                        case System.Net.WebExceptionStatus.ConnectFailure:
                            description = Localization.TryTranslateText("LoginErrorConnectFailure");
                            break;
                        case System.Net.WebExceptionStatus.NameResolutionFailure:
                            description = Localization.TryTranslateText("LoginErrorNameResolutionFailure");
                            break;
                        case System.Net.WebExceptionStatus.Timeout:
                            description = Localization.TryTranslateText("LoginErrorTimeout");
                            break;
                        default:
                            description = error.Message;
                            break;
                    }
                } catch (Exception e)
                {
                    description = response.ErrorMessage??"Unknown connection error";
                }
            }

            return string.Concat(Localization.TryTranslateText("LoginErrorDescription"), description);
        }
    }
}

