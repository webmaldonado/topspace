using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSpaceMAUI
{
    public enum LoginStatusCode
    {
        UserAuthorized,
        UserUnauthorized,
        AuthenticationError
    }

    public enum SyncStatusCode
    {
        RequestOK,
        //		RequestEmpty,
        RequestUnauthorized,
        RequestError,
        RequestConnectionError,
        SaveOK,
        SaveError
    }
}