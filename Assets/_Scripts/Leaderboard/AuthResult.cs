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
}