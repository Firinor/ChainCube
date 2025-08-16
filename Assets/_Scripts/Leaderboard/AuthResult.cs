using System;

namespace Firestore
{
    [Serializable]
    public struct AuthResult
    {
        public string kind;
        public string localId;
        public string email;
        public string displayName;
        public string idToken;
        public string refreshToken;
    }
    [Serializable]
    public class RefreshAuthResult
    {
        public string expires_in;
        public string token_type;
        public string refresh_token;
        public string id_token;
        public string user_id;
        public string project_id;
    }
}